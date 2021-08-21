using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Internal;

namespace Boo.Lang.Compiler.TypeSystem.Builders
{
	public class BooMethodBuilder
	{
		private BooCodeBuilder _codeBuilder;

		private Method _method;

		public Method Method => _method;

		public InternalMethod Entity => (InternalMethod)_method.Entity;

		public Block Body => _method.Body;

		public ParameterDeclarationCollection Parameters => _method.Parameters;

		public LocalCollection Locals => _method.Locals;

		public TypeMemberModifiers Modifiers
		{
			get
			{
				return _method.Modifiers;
			}
			set
			{
				_method.Modifiers = value;
			}
		}

		public BooMethodBuilder(BooCodeBuilder codeBuilder, string name, IType returnType)
			: this(codeBuilder, name, returnType, TypeMemberModifiers.Public)
		{
		}

		public BooMethodBuilder(BooCodeBuilder codeBuilder, string name, IType returnType, TypeMemberModifiers modifiers)
		{
			if (null == codeBuilder)
			{
				throw new ArgumentNullException("codeBuilder");
			}
			if (null == name)
			{
				throw new ArgumentNullException("name");
			}
			_codeBuilder = codeBuilder;
			_method = _codeBuilder.CreateMethod(name, returnType, modifiers);
		}

		public BooMethodBuilder(BooCodeBuilder codeBuilder, Method method)
		{
			if (null == codeBuilder)
			{
				throw new ArgumentNullException("codeBuilder");
			}
			if (null == method)
			{
				throw new ArgumentNullException("method");
			}
			_codeBuilder = codeBuilder;
			_method = method;
		}

		public ParameterDeclaration AddParameter(string name, IType type)
		{
			return AddParameter(name, type, byref: false);
		}

		public ParameterDeclaration AddParameter(string name, IType type, bool byref)
		{
			ParameterDeclaration parameterDeclaration = _codeBuilder.CreateParameterDeclaration(GetNextParameterIndex(), name, type, byref);
			_method.Parameters.Add(parameterDeclaration);
			return parameterDeclaration;
		}

		private int GetNextParameterIndex()
		{
			return ((!_method.IsStatic) ? 1 : 0) + _method.Parameters.Count;
		}
	}
}
