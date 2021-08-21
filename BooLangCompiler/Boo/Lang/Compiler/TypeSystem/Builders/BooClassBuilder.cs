using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Builders
{
	public class BooClassBuilder
	{
		private readonly BooCodeBuilder _codeBuilder;

		private readonly ClassDefinition _cd;

		private InternalTypeSystemProvider _internalTypeSystemProvider;

		public ClassDefinition ClassDefinition => _cd;

		public IType Entity => (IType)_cd.Entity;

		public TypeMemberModifiers Modifiers
		{
			get
			{
				return _cd.Modifiers;
			}
			set
			{
				_cd.Modifiers = value;
			}
		}

		public LexicalInfo LexicalInfo
		{
			get
			{
				return _cd.LexicalInfo;
			}
			set
			{
				_cd.LexicalInfo = value;
			}
		}

		public BooClassBuilder(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			_internalTypeSystemProvider = My<InternalTypeSystemProvider>.Instance;
			_codeBuilder = My<BooCodeBuilder>.Instance;
			_cd = new ClassDefinition
			{
				Name = name,
				IsSynthetic = true
			};
			EnsureEntityFor(_cd);
		}

		public void AddAttribute(Boo.Lang.Compiler.Ast.Attribute attribute)
		{
			_cd.Attributes.Add(attribute);
		}

		public void AddBaseType(IType type)
		{
			_cd.BaseTypes.Add(_codeBuilder.CreateTypeReference(type));
		}

		public BooMethodBuilder AddConstructor()
		{
			Constructor constructor = _codeBuilder.CreateConstructor(TypeMemberModifiers.Public);
			_cd.Members.Add(constructor);
			return new BooMethodBuilder(_codeBuilder, constructor);
		}

		private void EnsureEntityFor(TypeMember constructor)
		{
			_internalTypeSystemProvider.EntityFor(constructor);
		}

		public BooMethodBuilder AddMethod(string name, IType returnType)
		{
			return AddMethod(name, returnType, TypeMemberModifiers.Public);
		}

		public BooMethodBuilder AddVirtualMethod(string name, IType returnType)
		{
			return AddMethod(name, returnType, TypeMemberModifiers.Public | TypeMemberModifiers.Virtual);
		}

		public BooMethodBuilder AddMethod(string name, IType returnType, TypeMemberModifiers modifiers)
		{
			BooMethodBuilder booMethodBuilder = new BooMethodBuilder(_codeBuilder, name, returnType, modifiers);
			_cd.Members.Add(booMethodBuilder.Method);
			return booMethodBuilder;
		}

		public Property AddReadOnlyProperty(string name, IType type)
		{
			Property property = _codeBuilder.CreateProperty(name, type);
			property.Getter = _codeBuilder.CreateMethod("get_" + name, type, TypeMemberModifiers.Public);
			_cd.Members.Add(property);
			return property;
		}

		public Field AddField(string name, IType type)
		{
			Field field = _codeBuilder.CreateField(name, type);
			_cd.Members.Add(field);
			return field;
		}

		public Field AddPublicField(string name, IType type)
		{
			return AddField(name, type, TypeMemberModifiers.Public);
		}

		public Field AddInternalField(string name, IType type)
		{
			return AddField(name, type, TypeMemberModifiers.Internal);
		}

		public Field AddField(string name, IType type, TypeMemberModifiers modifiers)
		{
			Field field = AddField(name, type);
			field.Modifiers = modifiers;
			return field;
		}

		public GenericParameterDeclaration AddGenericParameter(string name)
		{
			GenericParameterDeclarationCollection genericParameters = ClassDefinition.GenericParameters;
			GenericParameterDeclaration genericParameterDeclaration = _codeBuilder.CreateGenericParameterDeclaration(genericParameters.Count, name);
			genericParameters.Add(genericParameterDeclaration);
			return genericParameterDeclaration;
		}
	}
}
