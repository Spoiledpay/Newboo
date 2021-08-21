using System.Collections.Generic;

namespace Boo.Lang.Compiler.TypeSystem.Generics
{
	internal class TypeInferrer : AbstractCompilerComponent
	{
		private Dictionary<IGenericParameter, InferredType> _inferredTypes = new Dictionary<IGenericParameter, InferredType>();

		protected IDictionary<IGenericParameter, InferredType> InferredTypes => _inferredTypes;

		public TypeInferrer()
		{
		}

		public TypeInferrer(IEnumerable<IGenericParameter> typeParameters)
		{
			InitializeTypeParameters(typeParameters);
		}

		public void InitializeTypeParameters(IEnumerable<IGenericParameter> typeParameters)
		{
			foreach (IGenericParameter typeParameter in typeParameters)
			{
				InferredTypes.Add(typeParameter, new InferredType());
			}
		}

		public bool FinalizeInference()
		{
			foreach (InferredType value in InferredTypes.Values)
			{
				if (!value.Fixed)
				{
					value.Fix();
				}
			}
			foreach (InferredType value2 in InferredTypes.Values)
			{
				if (!value2.Fixed)
				{
					return false;
				}
			}
			return true;
		}

		public IType GetInferredType(IGenericParameter gp)
		{
			if (InferredTypes.ContainsKey(gp))
			{
				return InferredTypes[gp].ResultingType;
			}
			return null;
		}

		public bool Infer(IType formalType, IType actualType)
		{
			return Infer(formalType, actualType, TypeInference.AllowCovariance);
		}

		protected bool Infer(IType formalType, IType actualType, TypeInference inference)
		{
			if (actualType == null)
			{
				return true;
			}
			IGenericParameter genericParameter = formalType as IGenericParameter;
			if (null != genericParameter)
			{
				return InferGenericParameter(genericParameter, actualType, inference);
			}
			ICallableType callableType = formalType as ICallableType;
			if (null != callableType)
			{
				return InferCallableType(callableType, actualType, inference);
			}
			if (formalType.ConstructedInfo != null)
			{
				return InferConstructedType(formalType, actualType, inference);
			}
			IArrayType arrayType = formalType as IArrayType;
			if (null != arrayType)
			{
				return InferArrayType(arrayType, actualType, inference);
			}
			return InferSimpleType(formalType, actualType, inference);
		}

		protected virtual bool InferGenericParameter(IGenericParameter formalType, IType actualType, TypeInference inference)
		{
			if (InferredTypes.ContainsKey(formalType))
			{
				InferredType inferredType = InferredTypes[formalType];
				if ((inference & TypeInference.AllowContravariance) != TypeInference.AllowContravariance)
				{
					inferredType.ApplyLowerBound(actualType);
				}
				if ((inference & TypeInference.AllowCovariance) != TypeInference.AllowCovariance)
				{
					inferredType.ApplyUpperBound(actualType);
				}
			}
			return true;
		}

		private bool InferCallableType(ICallableType formalType, IType actualType, TypeInference inference)
		{
			ICallableType callableType = actualType as ICallableType;
			if (callableType == null)
			{
				return false;
			}
			CallableSignature signature = formalType.GetSignature();
			CallableSignature signature2 = callableType.GetSignature();
			if (signature.AcceptVarArgs)
			{
				if (signature2.Parameters.Length < signature.Parameters.Length)
				{
					return false;
				}
			}
			else if (signature.Parameters.Length != signature2.Parameters.Length)
			{
				return false;
			}
			if (!Infer(signature.ReturnType, signature2.ReturnType, inference))
			{
				return false;
			}
			for (int i = 0; i < signature.Parameters.Length; i++)
			{
				if (!Infer(signature.Parameters[i].Type, signature2.Parameters[i].Type, Invert(inference)))
				{
					return false;
				}
			}
			return true;
		}

		private bool InferConstructedType(IType formalType, IType actualType, TypeInference inference)
		{
			IType type = GenericsServices.FindConstructedType(actualType, formalType.ConstructedInfo.GenericDefinition);
			if (type == null)
			{
				return false;
			}
			if (inference == TypeInference.Exact && actualType != type)
			{
				return false;
			}
			for (int i = 0; i < formalType.ConstructedInfo.GenericArguments.Length; i++)
			{
				if (!Infer(formalType.ConstructedInfo.GenericArguments[i], type.ConstructedInfo.GenericArguments[i], TypeInference.Exact))
				{
					return false;
				}
			}
			return true;
		}

		private bool InferArrayType(IArrayType formalType, IType actualType, TypeInference inference)
		{
			IArrayType arrayType = actualType as IArrayType;
			return arrayType != null && arrayType.Rank == formalType.Rank && Infer(formalType.ElementType, actualType.ElementType, inference);
		}

		private bool InferSimpleType(IType formalType, IType actualType, TypeInference inference)
		{
			return true;
		}

		private TypeInference Invert(TypeInference inference)
		{
			return inference switch
			{
				TypeInference.AllowCovariance => TypeInference.AllowContravariance, 
				TypeInference.AllowContravariance => TypeInference.AllowCovariance, 
				_ => TypeInference.Exact, 
			};
		}
	}
}
