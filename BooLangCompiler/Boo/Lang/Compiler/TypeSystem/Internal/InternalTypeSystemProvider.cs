using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Internal
{
	public class InternalTypeSystemProvider
	{
		public IType VoidType => CoreTypeSystemServices().VoidType;

		public IType ValueTypeType => CoreTypeSystemServices().ValueTypeType;

		public IType DuckType => CoreTypeSystemServices().DuckType;

		public ICompileUnit EntityFor(CompileUnit unit)
		{
			ICompileUnit compileUnit = (ICompileUnit)unit.Entity;
			if (null != compileUnit)
			{
				return compileUnit;
			}
			return Bind(unit, new InternalCompileUnit(unit));
		}

		public INamespace EntityFor(Module module)
		{
			INamespace @namespace = (INamespace)module.Entity;
			if (null != @namespace)
			{
				return @namespace;
			}
			return Bind(module, new InternalModule(this, module));
		}

		public IEntity EntityFor(TypeMember member)
		{
			IEntity entity = member.Entity;
			if (entity != null)
			{
				return entity;
			}
			return Bind(member, CreateEntityForMember(member));
		}

		private IEntity CreateEntityForMember(TypeMember member)
		{
			return member.NodeType switch
			{
				NodeType.Module => EntityFor((Module)member), 
				NodeType.InterfaceDefinition => new InternalInterface(this, (TypeDefinition)member), 
				NodeType.ClassDefinition => new InternalClass(this, (ClassDefinition)member), 
				NodeType.Field => new InternalField((Field)member), 
				NodeType.EnumDefinition => new InternalEnum(this, (EnumDefinition)member), 
				NodeType.EnumMember => new InternalEnumMember((EnumMember)member), 
				NodeType.Method => CreateEntityFor((Method)member), 
				NodeType.Constructor => new InternalConstructor(this, (Constructor)member), 
				NodeType.Property => new InternalProperty(this, (Property)member), 
				NodeType.Event => new InternalEvent((Event)member), 
				_ => throw new ArgumentException("Member type not supported: " + member.GetType()), 
			};
		}

		private IEntity CreateEntityFor(Method node)
		{
			return (node.GenericParameters.Count == 0) ? new InternalMethod(this, node) : new InternalGenericMethod(this, node);
		}

		private static TEntity Bind<TNode, TEntity>(TNode node, TEntity entity) where TNode : Node where TEntity : IEntity
		{
			node.Entity = entity;
			return entity;
		}

		public IParameter[] Map(ParameterDeclarationCollection parameters)
		{
			IParameter[] array = new IParameter[parameters.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (IParameter)parameters[i].Entity;
			}
			return array;
		}

		private TypeSystemServices CoreTypeSystemServices()
		{
			return My<TypeSystemServices>.Instance;
		}

		public bool IsSystemObject(IType type)
		{
			return CoreTypeSystemServices().IsSystemObject(type);
		}

		public bool IsCallableTypeAssignableFrom(InternalCallableType type, IType other)
		{
			return CoreTypeSystemServices().IsCallableTypeAssignableFrom(type, other);
		}

		public IType Map(Type type)
		{
			return CoreTypeSystemServices().Map(type);
		}
	}
}
