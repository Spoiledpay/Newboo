namespace Boo.Lang.Compiler.TypeSystem
{
	public abstract class TypeVisitor
	{
		public virtual void Visit(IType type)
		{
			IArrayType arrayType = type as IArrayType;
			if (arrayType != null)
			{
				VisitArrayType(arrayType);
			}
			if (type.IsByRef)
			{
				VisitByRefType(type);
			}
			if (type.ConstructedInfo != null)
			{
				VisitConstructedType(type);
			}
			ICallableType callableType = type as ICallableType;
			if (callableType != null)
			{
				VisitCallableType(callableType);
			}
		}

		public virtual void VisitArrayType(IArrayType arrayType)
		{
			Visit(arrayType.ElementType);
		}

		public virtual void VisitByRefType(IType type)
		{
			Visit(type.ElementType);
		}

		public virtual void VisitConstructedType(IType constructedType)
		{
			Visit(constructedType.ConstructedInfo.GenericDefinition);
			IType[] genericArguments = constructedType.ConstructedInfo.GenericArguments;
			foreach (IType type in genericArguments)
			{
				Visit(type);
			}
		}

		public virtual void VisitCallableType(ICallableType callableType)
		{
			CallableSignature signature = callableType.GetSignature();
			IParameter[] parameters = signature.Parameters;
			foreach (IParameter parameter in parameters)
			{
				Visit(parameter.Type);
			}
			Visit(signature.ReturnType);
		}
	}
}
