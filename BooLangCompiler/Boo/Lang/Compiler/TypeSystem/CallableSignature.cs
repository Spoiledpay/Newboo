using System;
using System.Text;

namespace Boo.Lang.Compiler.TypeSystem
{
	public class CallableSignature : IEquatable<CallableSignature>
	{
		private IParameter[] _parameters;

		private IType _returnType;

		private int _hashCode;

		private bool _acceptVarArgs;

		public IParameter[] Parameters => _parameters;

		public IType ReturnType => _returnType;

		public bool AcceptVarArgs => _acceptVarArgs;

		public CallableSignature(IMethodBase method)
		{
			if (null == method)
			{
				throw new ArgumentNullException("method");
			}
			Initialize(method.GetParameters(), method.ReturnType, method.AcceptVarArgs);
		}

		public CallableSignature(IParameter[] parameters, IType returnType)
		{
			Initialize(parameters, returnType, acceptVarArgs: false);
		}

		public CallableSignature(IParameter[] parameters, IType returnType, bool acceptVarArgs)
		{
			Initialize(parameters, returnType, acceptVarArgs);
		}

		private void Initialize(IParameter[] parameters, IType returnType, bool acceptVarArgs)
		{
			if (null == parameters)
			{
				throw new ArgumentNullException("parameters");
			}
			if (null == returnType)
			{
				throw new ArgumentNullException("returnType");
			}
			_parameters = parameters;
			_returnType = returnType;
			_acceptVarArgs = acceptVarArgs;
		}

		public override int GetHashCode()
		{
			if (0 == _hashCode)
			{
				InitializeHashCode();
			}
			return _hashCode;
		}

		public override bool Equals(object other)
		{
			if (null == other)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			CallableSignature other2 = other as CallableSignature;
			return Equals(other2);
		}

		public bool Equals(CallableSignature other)
		{
			if (null == other)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			return _returnType.Equals(other._returnType) && _acceptVarArgs == other._acceptVarArgs && AreSameParameters(_parameters, other._parameters);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("callable(");
			for (int i = 0; i < _parameters.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				if (_parameters[i].IsByRef)
				{
					stringBuilder.Append("ref ");
				}
				if (_acceptVarArgs && i == _parameters.Length - 1)
				{
					stringBuilder.Append('*');
				}
				stringBuilder.Append(_parameters[i].Type.DisplayName());
			}
			stringBuilder.Append(") as ");
			stringBuilder.Append(_returnType.DisplayName());
			return stringBuilder.ToString();
		}

		public static bool AreSameParameters(IParameter[] lhs, IParameter[] rhs)
		{
			int num = lhs.Length;
			if (num != rhs.Length)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				IParameter parameter = lhs[i];
				IParameter parameter2 = rhs[i];
				if (parameter.IsByRef != parameter2.IsByRef)
				{
					return false;
				}
				IType type = (parameter.IsByRef ? (parameter.Type.ElementType ?? parameter.Type) : parameter.Type);
				IType obj = (parameter2.IsByRef ? (parameter2.Type.ElementType ?? parameter2.Type) : parameter2.Type);
				if (!type.Equals(obj))
				{
					return false;
				}
			}
			return true;
		}

		private void InitializeHashCode()
		{
			_hashCode = (_acceptVarArgs ? 1 : 2);
			IParameter[] parameters = _parameters;
			foreach (IParameter parameter in parameters)
			{
				_hashCode ^= parameter.Type.GetHashCode();
			}
			_hashCode ^= _returnType.GetHashCode();
		}
	}
}
