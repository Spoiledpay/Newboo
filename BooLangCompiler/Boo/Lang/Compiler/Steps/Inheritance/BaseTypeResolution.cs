using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Compiler.Util;

namespace Boo.Lang.Compiler.Steps.Inheritance
{
	internal class BaseTypeResolution : AbstractCompilerComponent
	{
		private readonly TypeDefinition _typeDefinition;

		private readonly List<TypeDefinition> _visited;

		private int _removed;

		private int _index;

		private Set<TypeDefinition> _ancestors;

		public BaseTypeResolution(CompilerContext context, TypeDefinition typeDefinition, List<TypeDefinition> visited)
			: base(context)
		{
			_typeDefinition = typeDefinition;
			_visited = visited;
			_visited.Add(_typeDefinition);
			_removed = 0;
			_index = -1;
			NameResolutionService nameResolutionService = base.NameResolutionService;
			INamespace currentNamespace = nameResolutionService.CurrentNamespace;
			nameResolutionService.EnterNamespace(ParentNamespaceOf(_typeDefinition));
			try
			{
				Run();
			}
			finally
			{
				nameResolutionService.EnterNamespace(currentNamespace);
			}
		}

		private INamespace ParentNamespaceOf(TypeDefinition typeDefinition)
		{
			return (INamespace)GetEntity(typeDefinition.ParentNode);
		}

		private void Run()
		{
			IType type = (IType)TypeSystemServices.GetEntity(_typeDefinition);
			EnterGenericParametersNamespace(type);
			List<TypeDefinition> visited = null;
			List<TypeDefinition> visited2;
			if (_typeDefinition is InterfaceDefinition)
			{
				visited2 = _visited;
			}
			else
			{
				visited = _visited;
				visited2 = new List<TypeDefinition>();
			}
			TypeReference[] array = _typeDefinition.BaseTypes.ToArray();
			foreach (TypeReference typeReference in array)
			{
				base.NameResolutionService.ResolveTypeReference(typeReference);
				_index++;
				AbstractInternalType abstractInternalType = typeReference.Entity as AbstractInternalType;
				if (null != abstractInternalType)
				{
					if (IsEnclosingType(abstractInternalType.TypeDefinition))
					{
						BaseTypeError(CompilerErrorFactory.NestedTypeCannotExtendEnclosingType(typeReference, type, abstractInternalType));
					}
					else if (abstractInternalType is InternalInterface)
					{
						CheckForCycles(typeReference, abstractInternalType, visited2);
					}
					else
					{
						CheckForCycles(typeReference, abstractInternalType, visited);
					}
				}
			}
			LeaveGenericParametersNamespace(type);
		}

		private bool IsEnclosingType(TypeDefinition node)
		{
			return GetAncestors().Contains(node);
		}

		private Set<TypeDefinition> GetAncestors()
		{
			return _ancestors ?? (_ancestors = new Set<TypeDefinition>(_typeDefinition.GetAncestors<TypeDefinition>()));
		}

		private void LeaveGenericParametersNamespace(IType type)
		{
			if (type.GenericInfo != null)
			{
				base.NameResolutionService.LeaveNamespace();
			}
		}

		private void EnterGenericParametersNamespace(IType type)
		{
			if (type.GenericInfo != null)
			{
				base.NameResolutionService.EnterNamespace(new GenericParametersNamespaceExtender(type, base.NameResolutionService.CurrentNamespace));
			}
		}

		private void CheckForCycles(TypeReference baseTypeRef, AbstractInternalType baseType, List<TypeDefinition> visited)
		{
			if (visited.Contains(baseType.TypeDefinition))
			{
				BaseTypeError(CompilerErrorFactory.InheritanceCycle(baseTypeRef, baseType));
			}
			else
			{
				new BaseTypeResolution(base.Context, baseType.TypeDefinition, visited);
			}
		}

		private void BaseTypeError(CompilerError error)
		{
			base.Errors.Add(error);
			_typeDefinition.BaseTypes.RemoveAt(_index - _removed);
			_removed++;
		}
	}
}
