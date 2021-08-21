#define TRACE
using System.Collections.Generic;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Reflection;

namespace Boo.Lang.Compiler.Steps
{
	public class ResolveImports : AbstractTransformerCompilerStep
	{
		private readonly Dictionary<string, Import> _namespaces = new Dictionary<string, Import>();

		public override void Run()
		{
			base.NameResolutionService.Reset();
			Visit(base.CompileUnit.Modules);
		}

		public override void OnModule(Module module)
		{
			Visit(module.Imports);
			_namespaces.Clear();
		}

		public override void OnImport(Import import)
		{
			if (IsAlreadyBound(import))
			{
				return;
			}
			if (import.AssemblyReference != null)
			{
				ImportFromAssemblyReference(import);
				return;
			}
			IEntity entity = ResolveImport(import);
			if (!HandledAsImportError(import, entity) && !HandledAsDuplicatedNamespace(import))
			{
				base.Context.TraceInfo("{1}: import reference '{0}' bound to {2}.", import, import.LexicalInfo, entity);
				import.Entity = ImportedNamespaceFor(import, entity);
			}
		}

		private bool HandledAsImportError(Import import, IEntity entity)
		{
			if (entity == null)
			{
				ImportError(import, CompilerErrorFactory.InvalidNamespace(import));
				return true;
			}
			if (!IsValidImportTarget(entity))
			{
				ImportError(import, CompilerErrorFactory.NotANamespace(import, entity));
				return true;
			}
			return false;
		}

		private void ImportError(Import import, CompilerError error)
		{
			base.Errors.Add(error);
			BindError(import);
		}

		private void BindError(Import import)
		{
			Bind(import, Error.Default);
		}

		private static string EffectiveNameForImportedNamespace(Import import)
		{
			return (import.Alias != null) ? import.Alias.Name : import.Namespace;
		}

		private bool HandledAsDuplicatedNamespace(Import import)
		{
			string key = EffectiveNameForImportedNamespace(import);
			if (!_namespaces.TryGetValue(key, out var value))
			{
				_namespaces[key] = import;
				return false;
			}
			if (value.LexicalInfo.FileName == import.LexicalInfo.FileName)
			{
				base.Warnings.Add(CompilerWarningFactory.DuplicateNamespace(import, import.Namespace));
			}
			BindError(import);
			return true;
		}

		private IEntity ImportedNamespaceFor(Import import, IEntity entity)
		{
			INamespace @namespace = entity as INamespace;
			if (@namespace == null)
			{
				return entity;
			}
			MethodInvocationExpression methodInvocationExpression = import.Expression as MethodInvocationExpression;
			INamespace namespace2 = ((methodInvocationExpression != null) ? SelectiveImportFor(@namespace, methodInvocationExpression) : @namespace);
			INamespace namespace3 = ((import.Alias != null) ? AliasedNamespaceFor(namespace2, import) : namespace2);
			return new ImportedNamespace(import, namespace3);
		}

		private INamespace SelectiveImportFor(INamespace ns, MethodInvocationExpression selectiveImportSpec)
		{
			ExpressionCollection arguments = selectiveImportSpec.Arguments;
			List<IEntity> list = new List<IEntity>(arguments.Count);
			Dictionary<string, string> dictionary = new Dictionary<string, string>(arguments.Count);
			foreach (Expression item in arguments)
			{
				string text;
				if (item is ReferenceExpression)
				{
					text = (item as ReferenceExpression).Name;
					dictionary[text] = text;
				}
				else
				{
					TryCastExpression tryCastExpression = item as TryCastExpression;
					string name = (tryCastExpression.Type as SimpleTypeReference).Name;
					text = (dictionary[name] = (tryCastExpression.Target as ReferenceExpression).Name);
					arguments.Replace(item, tryCastExpression.Target);
				}
				if (!ns.Resolve(list, text, EntityType.Any))
				{
					base.Errors.Add(CompilerErrorFactory.MemberNotFound(item, text, ns, base.NameResolutionService.GetMostSimilarMemberName(ns, text, EntityType.Any)));
				}
			}
			return new SimpleNamespace(null, list, dictionary);
		}

		private static INamespace AliasedNamespaceFor(IEntity entity, Import import)
		{
			AliasedNamespace aliasedNamespace = new AliasedNamespace(import.Alias.Name, entity);
			import.Alias.Entity = aliasedNamespace;
			return aliasedNamespace;
		}

		private void ImportFromAssemblyReference(Import import)
		{
			IEntity entity = ResolveImportAgainstReferencedAssembly(import);
			if (!HandledAsImportError(import, entity) && !HandledAsDuplicatedNamespace(import))
			{
				import.Entity = ImportedNamespaceFor(import, entity);
			}
		}

		private IEntity ResolveImportAgainstReferencedAssembly(Import import)
		{
			return base.NameResolutionService.ResolveQualifiedName(GetBoundReference(import.AssemblyReference).RootNamespace, import.Namespace);
		}

		private static bool IsAlreadyBound(Import import)
		{
			return import.Entity != null;
		}

		private IEntity ResolveImport(Import import)
		{
			IEntity entity = ResolveImportOnParentNamespace(import) ?? base.NameResolutionService.ResolveQualifiedName(import.Namespace);
			if (null != entity)
			{
				return entity;
			}
			if (!TryAutoAddAssemblyReference(import))
			{
				return null;
			}
			return base.NameResolutionService.ResolveQualifiedName(import.Namespace);
		}

		private IEntity ResolveImportOnParentNamespace(Import import)
		{
			INamespace currentNamespace = base.NameResolutionService.CurrentNamespace;
			try
			{
				INamespace parentNamespace = base.NameResolutionService.CurrentNamespace.ParentNamespace;
				if (parentNamespace != null)
				{
					base.NameResolutionService.EnterNamespace(parentNamespace);
					return base.NameResolutionService.ResolveQualifiedName(import.Namespace);
				}
			}
			finally
			{
				base.NameResolutionService.EnterNamespace(currentNamespace);
			}
			return null;
		}

		private bool TryAutoAddAssemblyReference(Import import)
		{
			ICompileUnit compileUnit = base.Parameters.FindAssembly(import.Namespace);
			if (compileUnit != null)
			{
				return false;
			}
			ICompileUnit compileUnit2 = TryToLoadAssemblyContainingNamespace(import.Namespace);
			if (compileUnit2 == null)
			{
				return false;
			}
			base.Parameters.References.Add(compileUnit2);
			import.AssemblyReference = new ReferenceExpression(import.LexicalInfo, compileUnit2.FullName).WithEntity(compileUnit2);
			base.NameResolutionService.ClearResolutionCacheFor(compileUnit2.Name);
			return true;
		}

		private ICompileUnit TryToLoadAssemblyContainingNamespace(string @namespace)
		{
			ICompileUnit compileUnit = base.Parameters.LoadAssembly(@namespace, throwOnError: false);
			if (compileUnit != null)
			{
				return compileUnit;
			}
			string[] array = @namespace.Split('.');
			if (array.Length == 1)
			{
				return null;
			}
			for (int num = array.Length - 1; num > 0; num--)
			{
				string text = string.Join(".", array, 0, num);
				ICompileUnit compileUnit2 = base.Parameters.FindAssembly(text);
				if (compileUnit2 != null)
				{
					return null;
				}
				IAssemblyReference assemblyReference = base.Parameters.LoadAssembly(text, throwOnError: false);
				if (assemblyReference != null)
				{
					return assemblyReference;
				}
			}
			return null;
		}

		private static bool IsValidImportTarget(IEntity entity)
		{
			EntityType entityType = entity.EntityType;
			return entityType == EntityType.Namespace || entityType == EntityType.Type;
		}

		private static ICompileUnit GetBoundReference(ReferenceExpression reference)
		{
			return (ICompileUnit)TypeSystemServices.GetEntity(reference);
		}
	}
}
