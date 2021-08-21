using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Compiler.Util;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps.MacroProcessing
{
	public sealed class MacroExpander : AbstractNamespaceSensitiveTransformerCompilerStep
	{
		private int _expanded;

		private Set<Node> _visited = new Set<Node>();

		private Queue<Statement> _pendingExpansions = new Queue<Statement>();

		private DynamicVariable<bool> _ignoringUnknownMacros = new DynamicVariable<bool>(initialValue: false);

		private bool _referenced;

		private int _expansionDepth;

		private StringBuilder _buffer = new StringBuilder();

		public bool ExpandAll()
		{
			Reset();
			Run();
			BubbleUpPendingTypeMembers();
			return _expanded > 0;
		}

		private void Reset()
		{
			_pendingExpansions.Clear();
			_visited.Clear();
			_expanded = 0;
		}

		public override void Run()
		{
			ExpandModuleGlobalsIgnoringUnknownMacros();
			ExpandModules();
		}

		private void ExpandModules()
		{
			foreach (Module module in base.CompileUnit.Modules)
			{
				ExpandOnModuleNamespace(module, VisitModule);
			}
		}

		private void ExpandModuleGlobalsIgnoringUnknownMacros()
		{
			foreach (Module module in base.CompileUnit.Modules)
			{
				ExpandModuleGlobalsIgnoringUnknownMacros(module);
			}
		}

		private void ExpandModuleGlobalsIgnoringUnknownMacros(Module current)
		{
			_ignoringUnknownMacros.With(value: true, (Procedure)delegate
			{
				ExpandOnModuleNamespace(current, VisitGlobalsAllowingCancellation);
			});
		}

		private void VisitGlobalsAllowingCancellation(Module module)
		{
			StatementCollection statements = module.Globals.Statements;
			Statement[] array = statements.ToArray();
			foreach (Statement statement in array)
			{
				if (VisitAllowingCancellation(statement, out var resultingNode) && resultingNode != statement)
				{
					statements.Replace(statement, (Statement)resultingNode);
				}
				BubbleUpPendingTypeMembers();
			}
		}

		private void VisitModule(Module module)
		{
			Visit(module.Globals);
			Visit(module.Members);
		}

		private void ExpandOnModuleNamespace(Module module, Action<Module> action)
		{
			EnterModuleNamespace(module);
			try
			{
				action(module);
			}
			finally
			{
				LeaveNamespace();
			}
		}

		private void EnterModuleNamespace(Module module)
		{
			EnterNamespace(InternalModule.ScopeFor(module));
		}

		public override bool EnterClassDefinition(ClassDefinition node)
		{
			if (WasVisited(node))
			{
				return false;
			}
			_visited.Add(node);
			return true;
		}

		internal void EnsureCompilerAssemblyReference(CompilerContext context)
		{
			if (!_referenced)
			{
				if (null != context.References.Find("Boo.Lang.Compiler"))
				{
					_referenced = true;
					return;
				}
				context.References.Add(typeof(CompilerContext).Assembly);
				_referenced = true;
			}
		}

		public override void OnMacroStatement(MacroStatement node)
		{
			EnsureCompilerAssemblyReference(base.Context);
			IType type = ResolveMacroName(node) as IType;
			if (null != type)
			{
				ExpandKnownMacro(node, type);
				return;
			}
			if (_ignoringUnknownMacros.Value)
			{
				Cancel();
			}
			ExpandUnknownMacro(node);
		}

		private bool IsTopLevelExpansion()
		{
			return _expansionDepth == 0;
		}

		private void BubbleUpPendingTypeMembers()
		{
			while (_pendingExpansions.Count > 0)
			{
				TypeMemberStatementBubbler.BubbleTypeMemberStatementsUp(_pendingExpansions.Dequeue());
			}
		}

		private void ExpandKnownMacro(MacroStatement node, IType macroType)
		{
			ExpandChildrenOfMacroOnMacroNamespace(node, macroType);
			ProcessMacro(macroType, node);
		}

		private void ExpandChildrenOfMacroOnMacroNamespace(MacroStatement node, IType macroType)
		{
			EnterMacroNamespace(macroType);
			try
			{
				ExpandChildrenOf(node);
			}
			finally
			{
				LeaveNamespace();
			}
		}

		private void EnterMacroNamespace(IType macroType)
		{
			EnsureNestedMacrosCanBeSeenAsMembers(macroType);
			EnterNamespace(new NamespaceDelegator(base.CurrentNamespace, macroType));
		}

		private static void EnsureNestedMacrosCanBeSeenAsMembers(IType macroType)
		{
			InternalClass internalClass = macroType as InternalClass;
			if (null != internalClass)
			{
				TypeMemberStatementBubbler.BubbleTypeMemberStatementsUp(internalClass.TypeDefinition);
			}
		}

		private void ExpandUnknownMacro(MacroStatement node)
		{
			ExpandChildrenOf(node);
			if (IsTypeMemberMacro(node))
			{
				UnknownTypeMemberMacro(node);
			}
			else
			{
				TreatMacroAsMethodInvocation(node);
			}
		}

		private static bool IsTypeMemberMacro(MacroStatement node)
		{
			return node.ParentNode.NodeType == NodeType.StatementTypeMember;
		}

		private void UnknownTypeMemberMacro(MacroStatement node)
		{
			CompilerError error = (LooksLikeOldStyleFieldDeclaration(node) ? CompilerErrorFactory.UnknownClassMacroWithFieldHint(node, node.Name) : CompilerErrorFactory.UnknownMacro(node, node.Name));
			ProcessingError(error);
		}

		private static bool LooksLikeOldStyleFieldDeclaration(MacroStatement node)
		{
			return node.Arguments.Count == 0 && node.Body.IsEmpty;
		}

		private void ExpandChildrenOf(MacroStatement node)
		{
			EnterExpansion();
			try
			{
				Visit(node.Body);
				Visit(node.Arguments);
			}
			finally
			{
				LeaveExpansion();
			}
		}

		private void LeaveExpansion()
		{
			_expansionDepth--;
		}

		private void EnterExpansion()
		{
			_expansionDepth++;
		}

		private void ProcessMacro(IType macroType, MacroStatement node)
		{
			ExternalType externalType = macroType as ExternalType;
			if (externalType == null)
			{
				InternalClass klass = (InternalClass)macroType;
				ProcessInternalMacro(klass, node);
			}
			else
			{
				ProcessMacro(externalType.ActualType, node);
			}
		}

		private void ProcessInternalMacro(InternalClass klass, MacroStatement node)
		{
			TypeDefinition typeDefinition = klass.TypeDefinition;
			if (MacroDefinitionContainsMacroApplication(typeDefinition, node))
			{
				ProcessingError(CompilerErrorFactory.InvalidMacro(node, klass));
				return;
			}
			MacroCompiler instance = My<MacroCompiler>.Instance;
			bool flag = !instance.AlreadyCompiled(typeDefinition);
			Type type = instance.Compile(typeDefinition);
			if (type == null)
			{
				if (flag)
				{
					ProcessingError(CompilerErrorFactory.AstMacroMustBeExternal(node, klass));
				}
				else
				{
					RemoveCurrentNode();
				}
			}
			else
			{
				ProcessMacro(type, node);
			}
		}

		private static bool MacroDefinitionContainsMacroApplication(TypeDefinition definition, MacroStatement statement)
		{
			return statement.GetAncestors<TypeDefinition>().Any((TypeDefinition ancestor) => ancestor == definition);
		}

		private bool WasVisited(TypeDefinition node)
		{
			return _visited.Contains(node);
		}

		private void ProcessingError(CompilerError error)
		{
			base.Errors.Add(error);
			RemoveCurrentNode();
		}

		private void ProcessMacro(Type actualType, MacroStatement node)
		{
			if (!typeof(IAstMacro).IsAssignableFrom(actualType))
			{
				ProcessingError(CompilerErrorFactory.InvalidMacro(node, Map(actualType)));
				return;
			}
			_expanded++;
			try
			{
				Statement expansion = ExpandMacro(actualType, node);
				Statement statement = ExpandMacroExpansion(node, expansion);
				ReplaceCurrentNode(statement);
				if (statement != null && IsTopLevelExpansion())
				{
					_pendingExpansions.Enqueue(statement);
				}
			}
			catch (LongJumpException)
			{
				throw;
			}
			catch (Exception error)
			{
				ProcessingError(CompilerErrorFactory.MacroExpansionError(node, error));
			}
		}

		private IType Map(Type actualType)
		{
			return base.TypeSystemServices.Map(actualType);
		}

		private Statement ExpandMacroExpansion(MacroStatement node, Statement expansion)
		{
			if (null == expansion)
			{
				return null;
			}
			Statement statement = ApplyMacroModifierToExpansion(node, expansion);
			statement.InitializeParent(node.ParentNode);
			return Visit(statement);
		}

		private static Statement ApplyMacroModifierToExpansion(MacroStatement node, Statement expansion)
		{
			if (node.Modifier == null)
			{
				return expansion;
			}
			return NormalizeStatementModifiers.CreateModifiedStatement(node.Modifier, expansion);
		}

		private void TreatMacroAsMethodInvocation(MacroStatement node)
		{
			MethodInvocationExpression methodInvocationExpression = new MethodInvocationExpression(node.LexicalInfo, new ReferenceExpression(node.LexicalInfo, node.Name));
			methodInvocationExpression.Arguments = node.Arguments;
			MethodInvocationExpression methodInvocationExpression2 = methodInvocationExpression;
			if (node.ContainsAnnotation("compound") || !IsNullOrEmpty(node.Body))
			{
				methodInvocationExpression2.Arguments.Add(new BlockExpression(node.Body));
			}
			ReplaceCurrentNode(new ExpressionStatement(node.LexicalInfo, methodInvocationExpression2, node.Modifier));
		}

		private static bool IsNullOrEmpty(Block block)
		{
			return block?.IsEmpty ?? true;
		}

		private Statement ExpandMacro(Type macroType, MacroStatement node)
		{
			IAstMacro astMacro = (IAstMacro)Activator.CreateInstance(macroType);
			astMacro.Initialize(base.Context);
			IAstGeneratorMacro astGeneratorMacro = astMacro as IAstGeneratorMacro;
			if (astGeneratorMacro != null)
			{
				return ExpandGeneratorMacro(astGeneratorMacro, node);
			}
			return astMacro.Expand(node);
		}

		private static Statement ExpandGeneratorMacro(IAstGeneratorMacro macroType, MacroStatement node)
		{
			IEnumerable<Node> enumerable = macroType.ExpandGenerator(node);
			if (null == enumerable)
			{
				return null;
			}
			return new NodeGeneratorExpander(node).Expand(enumerable);
		}

		private IEntity ResolveMacroName(MacroStatement node)
		{
			string macroTypeName = BuildMacroTypeName(node.Name);
			IEntity entity = ResolvePreferringInternalMacros(macroTypeName) ?? ResolvePreferringInternalMacros(node.Name);
			if (entity is IType)
			{
				return entity;
			}
			if (entity == null)
			{
				return null;
			}
			return ResolveMacroExtensionType(node, entity as IMethod) ?? ResolveMacroExtensionType(node, entity as Ambiguous) ?? entity;
		}

		private IEntity ResolvePreferringInternalMacros(string macroTypeName)
		{
			IEntity entity = base.NameResolutionService.ResolveQualifiedName(macroTypeName);
			Ambiguous ambiguous = entity as Ambiguous;
			if (ambiguous != null && ambiguous.AllEntitiesAre(EntityType.Type))
			{
				return Entities.PreferInternalEntitiesOverExternalOnes(ambiguous);
			}
			return entity;
		}

		private IEntity ResolveMacroExtensionType(MacroStatement node, Ambiguous extensions)
		{
			if (null == extensions)
			{
				return null;
			}
			IEntity[] entities = extensions.Entities;
			foreach (IEntity entity in entities)
			{
				IEntity entity2 = ResolveMacroExtensionType(node, entity as IMethod);
				if (null != entity2)
				{
					return entity2;
				}
			}
			return null;
		}

		private IEntity ResolveMacroExtensionType(MacroStatement node, IMethod extension)
		{
			if (null == extension)
			{
				return null;
			}
			IType extendedMacroType = GetExtendedMacroType(extension);
			if (null == extendedMacroType)
			{
				return null;
			}
			foreach (MacroStatement ancestor in node.GetAncestors<MacroStatement>())
			{
				if (ResolveMacroName(ancestor) == extendedMacroType)
				{
					return GetExtensionMacroType(extension);
				}
			}
			return null;
		}

		private IType GetExtendedMacroType(IMethod method)
		{
			InternalMethod internalMethod = method as InternalMethod;
			if (null != internalMethod)
			{
				Method method2 = internalMethod.Method;
				if (!method2.Attributes.Contains(Types.CompilerGeneratedAttribute.FullName))
				{
					return null;
				}
				SimpleTypeReference simpleTypeReference = method2.Parameters[0].Type as SimpleTypeReference;
				if (simpleTypeReference != null && method2.Parameters.Count == 2)
				{
					IType type = base.NameResolutionService.ResolveQualifiedName(simpleTypeReference.Name) as IType;
					if (type != null && type.Name.EndsWith("Macro"))
					{
						return type;
					}
				}
			}
			else if (method is ExternalMethod && method.IsExtension)
			{
				IParameter[] parameters = method.GetParameters();
				if (parameters.Length == 2 && base.TypeSystemServices.IsMacro(parameters[0].Type))
				{
					return parameters[0].Type;
				}
			}
			return null;
		}

		private IType GetExtensionMacroType(IMethod method)
		{
			InternalMethod internalMethod = method as InternalMethod;
			if (null != internalMethod)
			{
				Method method2 = internalMethod.Method;
				SimpleTypeReference simpleTypeReference = method2.ReturnType as SimpleTypeReference;
				if (null != simpleTypeReference)
				{
					IType type = base.NameResolutionService.ResolveQualifiedName(simpleTypeReference.Name) as IType;
					if (type != null && type.Name.EndsWith("Macro"))
					{
						return type;
					}
				}
			}
			else if (method is ExternalMethod)
			{
				return method.ReturnType;
			}
			return null;
		}

		private string BuildMacroTypeName(string name)
		{
			_buffer.Length = 0;
			if (!char.IsUpper(name[0]))
			{
				_buffer.Append(char.ToUpper(name[0]));
				_buffer.Append(name.Substring(1));
				_buffer.Append("Macro");
			}
			else
			{
				_buffer.Append(name);
				_buffer.Append("Macro");
			}
			return _buffer.ToString();
		}
	}
}
