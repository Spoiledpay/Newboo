#define TRACE
using System;
using System.Reflection;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps.MacroProcessing
{
	internal sealed class ApplyAttributeTask
	{
		private CompilerContext _context;

		private Boo.Lang.Compiler.Ast.Attribute _attribute;

		private Type _type;

		private Node _targetNode;

		public ApplyAttributeTask(CompilerContext context, Boo.Lang.Compiler.Ast.Attribute attribute, Type type)
		{
			_context = context;
			_attribute = attribute;
			_type = type;
			_targetNode = TargetNode();
		}

		private Node TargetNode()
		{
			return IsAssemblyAttribute() ? _context.CompileUnit : _attribute.ParentNode;
		}

		private bool IsAssemblyAttribute()
		{
			return (_attribute.ParentNode as Boo.Lang.Compiler.Ast.Module)?.AssemblyAttributes.Contains(_attribute) ?? false;
		}

		public void Execute()
		{
			try
			{
				IAstAttribute astAttribute = CreateAstAttributeInstance();
				if (astAttribute != null)
				{
					astAttribute.Initialize(_context);
					astAttribute.Apply(_targetNode);
				}
			}
			catch (Exception ex)
			{
				_context.TraceError(ex);
				_context.Errors.Add(CompilerErrorFactory.AttributeApplicationError(ex, _attribute, _type));
			}
		}

		public IAstAttribute CreateAstAttributeInstance()
		{
			IAstAttribute astAttribute = CreateInstance(ConstructorArguments());
			if (astAttribute == null)
			{
				return null;
			}
			return (_attribute.NamedArguments.Count > 0) ? InitializeProperties(astAttribute) : astAttribute;
		}

		private object[] ConstructorArguments()
		{
			return (_attribute.Arguments.Count > 0) ? _attribute.Arguments.ToArray() : new object[0];
		}

		private IAstAttribute CreateInstance(object[] arguments)
		{
			try
			{
				IAstAttribute astAttribute = (IAstAttribute)Activator.CreateInstance(_type, arguments);
				astAttribute.Attribute = _attribute;
				return astAttribute;
			}
			catch (MissingMethodException error)
			{
				_context.Errors.Add(CompilerErrorFactory.MissingConstructor(error, _attribute, _type, arguments));
				return null;
			}
		}

		private IAstAttribute InitializeProperties(IAstAttribute aa)
		{
			bool flag = true;
			foreach (ExpressionPair namedArgument in _attribute.NamedArguments)
			{
				bool flag2 = SetFieldOrProperty(aa, namedArgument);
				flag = flag && flag2;
			}
			return flag ? aa : null;
		}

		private bool SetFieldOrProperty(IAstAttribute aa, ExpressionPair p)
		{
			ReferenceExpression referenceExpression = p.First as ReferenceExpression;
			if (referenceExpression == null)
			{
				_context.Errors.Add(CompilerErrorFactory.NamedParameterMustBeIdentifier(p));
				return false;
			}
			MemberInfo[] array = FindMembers(referenceExpression);
			if (array.Length <= 0)
			{
				_context.Errors.Add(CompilerErrorFactory.NotAPublicFieldOrProperty(referenceExpression, referenceExpression.Name, Type()));
				return false;
			}
			if (array.Length > 1)
			{
				_context.Errors.Add(CompilerErrorFactory.AmbiguousReference(referenceExpression, array));
				return false;
			}
			MemberInfo memberInfo = array[0];
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(aa, p.Second, null);
				return true;
			}
			FieldInfo fieldInfo = (FieldInfo)memberInfo;
			fieldInfo.SetValue(aa, p.Second);
			return true;
		}

		private IType Type()
		{
			return My<TypeSystemServices>.Instance.Map(_type);
		}

		private MemberInfo[] FindMembers(ReferenceExpression name)
		{
			return _type.FindMembers(MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public, System.Type.FilterName, name.Name);
		}
	}
}
