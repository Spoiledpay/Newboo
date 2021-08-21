using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	public class GenericConstructionChecker
	{
		private Node _constructionNode;

		private IType[] _typeArguments;

		private CompilerErrorCollection _errors = new CompilerErrorCollection();

		private TypeSystemServices _typeSystemServices = My<TypeSystemServices>.Instance;

		private IType _definition;

		public IType[] TypeArguments => _typeArguments;

		public Node ConstructionNode
		{
			get
			{
				return _constructionNode;
			}
			set
			{
				_constructionNode = value;
			}
		}

		public CompilerErrorCollection Errors => _errors;

		public IEnumerable<Predicate<IEntity>> Checks
		{
			get
			{
				yield return IsGenericDefinition;
				yield return HasCorrectGenerity;
				yield return MaintainsParameterConstraints;
			}
		}

		public GenericConstructionChecker(IType[] typeArguments, Node constructionNode)
		{
			_constructionNode = constructionNode;
			_typeArguments = typeArguments;
		}

		private bool IsGenericDefinition(IEntity definition)
		{
			if (GenericsServices.IsGenericMethod(definition) || GenericsServices.IsGenericType(definition))
			{
				return true;
			}
			Errors.Add(CompilerErrorFactory.NotAGenericDefinition(ConstructionNode, definition.FullName));
			return false;
		}

		private bool HasCorrectGenerity(IEntity definition)
		{
			IGenericParameter[] genericParameters = GenericsServices.GetGenericParameters(definition);
			if (genericParameters.Length != TypeArguments.Length)
			{
				Errors.Add(CompilerErrorFactory.GenericDefinitionArgumentCount(ConstructionNode, definition, genericParameters.Length));
				return false;
			}
			return true;
		}

		private bool MaintainsParameterConstraints(IEntity definition)
		{
			IGenericParameter[] genericParameters = GenericsServices.GetGenericParameters(definition);
			_definition = definition as IType;
			bool flag = true;
			for (int i = 0; i < genericParameters.Length; i++)
			{
				flag &= MaintainsParameterConstraints(genericParameters[i], TypeArguments[i]);
			}
			return flag;
		}

		private bool MaintainsParameterConstraints(IGenericParameter parameter, IType argument)
		{
			if (argument == null || TypeSystemServices.IsError(argument))
			{
				return true;
			}
			if (argument == parameter)
			{
				return true;
			}
			if (argument == _typeSystemServices.VoidType)
			{
				Errors.Add(CompilerErrorFactory.InvalidGenericParameterType(ConstructionNode, argument));
				return false;
			}
			bool result = true;
			if (parameter.IsClass && !argument.IsClass && !argument.IsInterface)
			{
				Errors.Add(CompilerErrorFactory.GenericArgumentMustBeReferenceType(ConstructionNode, parameter, argument));
				result = false;
			}
			if (parameter.IsValueType && !argument.IsValueType)
			{
				Errors.Add(CompilerErrorFactory.GenericArgumentMustBeValueType(ConstructionNode, parameter, argument));
				result = false;
			}
			else if (parameter.MustHaveDefaultConstructor && !HasDefaultConstructor(argument))
			{
				Errors.Add(CompilerErrorFactory.GenericArgumentMustHaveDefaultConstructor(ConstructionNode, parameter, argument));
				result = false;
			}
			IType[] typeConstraints = parameter.GetTypeConstraints();
			if (typeConstraints != null)
			{
				IType[] array = typeConstraints;
				foreach (IType type in array)
				{
					if ((_definition == null || !TypeCompatibilityRules.IsAssignableFrom(type, _definition) || argument != _constructionNode.ParentNode.Entity) && (type != _typeSystemServices.ValueTypeType || !parameter.IsValueType) && !TypeCompatibilityRules.IsAssignableFrom(type, argument))
					{
						Errors.Add(CompilerErrorFactory.GenericArgumentMustHaveBaseType(ConstructionNode, parameter, argument, type));
						result = false;
					}
				}
			}
			return result;
		}

		private bool HasDefaultConstructor(IType argument)
		{
			if (argument.IsValueType)
			{
				return true;
			}
			return argument.GetConstructors().Any((IConstructor ctor) => ctor.GetParameters().Length == 0);
		}

		public void ReportErrors(CompilerErrorCollection targetErrorCollection)
		{
			targetErrorCollection.Extend(Errors);
		}

		public void DiscardErrors()
		{
			Errors.Clear();
		}
	}
}
