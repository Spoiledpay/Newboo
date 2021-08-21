using System;
using System.Text;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Reflection;
using Boo.Lang.Compiler.TypeSystem.Services;

namespace Boo.Lang.Compiler.Steps.MacroProcessing
{
	public class BindAndApplyAttributes : AbstractNamespaceSensitiveTransformerCompilerStep
	{
		private readonly List<ApplyAttributeTask> _pendingApplications = new List<ApplyAttributeTask>();

		private StringBuilder _buffer = new StringBuilder();

		private IType _astAttributeInterface;

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			_astAttributeInterface = base.TypeSystemServices.Map(typeof(IAstAttribute));
		}

		public override void Run()
		{
			for (int i = 0; i < base.Parameters.MaxExpansionIterations; i++)
			{
				if (!BindAndApply())
				{
					break;
				}
			}
		}

		public bool BindAndApply()
		{
			return BindAndApply(base.CompileUnit);
		}

		public bool BindAndApply(Node node)
		{
			Visit(node);
			return FlushPendingApplications();
		}

		private bool FlushPendingApplications()
		{
			if (_pendingApplications.Count == 0)
			{
				return false;
			}
			foreach (ApplyAttributeTask pendingApplication in _pendingApplications)
			{
				pendingApplication.Execute();
			}
			_pendingApplications.Clear();
			return true;
		}

		public override void OnModule(Module module)
		{
			EnterNamespace(InternalModule.ScopeFor(module));
			try
			{
				Visit(module.Members);
				Visit(module.Globals);
				Visit(module.Attributes);
				Visit(module.AssemblyAttributes);
			}
			finally
			{
				LeaveNamespace();
			}
		}

		private void VisitTypeDefinition(TypeDefinition node)
		{
			Visit(node.Members);
			Visit(node.Attributes);
		}

		public override void OnClassDefinition(ClassDefinition node)
		{
			VisitTypeDefinition(node);
		}

		public override void OnInterfaceDefinition(InterfaceDefinition node)
		{
			VisitTypeDefinition(node);
		}

		public override void OnStructDefinition(StructDefinition node)
		{
			VisitTypeDefinition(node);
		}

		public override void OnEnumDefinition(EnumDefinition node)
		{
			VisitTypeDefinition(node);
		}

		public override void OnBlock(Block node)
		{
		}

		public override void OnAttribute(Boo.Lang.Compiler.Ast.Attribute attribute)
		{
			if (null != attribute.Entity)
			{
				return;
			}
			IEntity entity = base.NameResolutionService.ResolveQualifiedName(BuildAttributeName(attribute.Name, forcePascalNaming: true)) ?? base.NameResolutionService.ResolveQualifiedName(BuildAttributeName(attribute.Name, forcePascalNaming: false)) ?? base.NameResolutionService.ResolveQualifiedName(attribute.Name);
			if (entity == null)
			{
				string suggestion = base.NameResolutionService.GetMostSimilarTypeName(BuildAttributeName(attribute.Name, forcePascalNaming: true)) ?? base.NameResolutionService.GetMostSimilarTypeName(BuildAttributeName(attribute.Name, forcePascalNaming: false));
				Error(attribute, CompilerErrorFactory.UnknownAttribute(attribute, attribute.Name, suggestion));
				return;
			}
			if (entity.IsAmbiguous())
			{
				Error(attribute, CompilerErrorFactory.AmbiguousReference(attribute, attribute.Name, ((Ambiguous)entity).Entities));
				return;
			}
			if (EntityType.Type != entity.EntityType)
			{
				Error(attribute, CompilerErrorFactory.NameNotType(attribute, attribute.Name, entity, null));
				return;
			}
			IType type = ((ITypedEntity)entity).Type;
			if (IsAstAttribute(type))
			{
				ExternalType externalType = type as ExternalType;
				if (null == externalType)
				{
					Error(attribute, CompilerErrorFactory.AstAttributeMustBeExternal(attribute, type));
					return;
				}
				ScheduleAttributeApplication(attribute, externalType.ActualType);
				RemoveCurrentNode();
			}
			else if (!IsSystemAttribute(type))
			{
				Error(attribute, CompilerErrorFactory.TypeNotAttribute(attribute, type));
			}
			else
			{
				attribute.Name = type.FullName;
				attribute.Entity = entity;
				CheckAttributeParameters(attribute);
			}
		}

		private void CheckAttributeParameters(Boo.Lang.Compiler.Ast.Attribute node)
		{
			foreach (Expression argument in node.Arguments)
			{
				if (argument.NodeType == NodeType.BinaryExpression && ((BinaryExpression)argument).Operator == BinaryOperatorType.Assign)
				{
					Error(node, CompilerErrorFactory.ColonInsteadOfEquals(node));
				}
			}
		}

		private void Error(Boo.Lang.Compiler.Ast.Attribute node, CompilerError error)
		{
			node.Entity = TypeSystemServices.ErrorEntity;
			base.Errors.Add(error);
		}

		private void ScheduleAttributeApplication(Boo.Lang.Compiler.Ast.Attribute attribute, Type type)
		{
			_pendingApplications.Add(new ApplyAttributeTask(base.Context, attribute, type));
		}

		private string BuildAttributeName(string name, bool forcePascalNaming)
		{
			_buffer.Length = 0;
			if (forcePascalNaming && !char.IsUpper(name[0]))
			{
				_buffer.Append(char.ToUpper(name[0]));
				_buffer.Append(name.Substring(1));
				_buffer.Append("Attribute");
			}
			else
			{
				_buffer.Append(name);
				_buffer.Append("Attribute");
			}
			return _buffer.ToString();
		}

		private bool IsSystemAttribute(IType type)
		{
			return base.TypeSystemServices.IsAttribute(type);
		}

		private bool IsAstAttribute(IType type)
		{
			return TypeCompatibilityRules.IsAssignableFrom(_astAttributeInterface, type);
		}
	}
}
