using System;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public class CheckMemberTypes : AbstractVisitorCompilerStep
	{
		public override void Run()
		{
			if (!base.Parameters.DisabledWarnings.Contains("BCW0024"))
			{
				Visit(base.CompileUnit.Modules);
			}
		}

		public override void OnModule(Module node)
		{
			Visit(node.Members);
		}

		public override void LeaveField(Field node)
		{
			LeaveMember(node);
		}

		public override void LeaveProperty(Property node)
		{
			ValidateSetter(node.Setter);
			LeaveMember(node);
		}

		private void ValidateSetter(Method setter)
		{
			if (setter != null && setter.ReturnType != null)
			{
				throw new InvalidOperationException();
			}
		}

		public override void LeaveEvent(Event node)
		{
			LeaveMember(node);
		}

		public override void OnMethod(Method node)
		{
			LeaveMethod(node);
		}

		public override void LeaveMethod(Method node)
		{
			LeaveMember(node);
		}

		public override void LeaveConstructor(Constructor node)
		{
			LeaveMember(node);
		}

		private void LeaveMember(TypeMember node)
		{
			CheckExplicitTypeForVisibleMember(node);
		}

		private void CheckExplicitTypeForVisibleMember(TypeMember node)
		{
			if (node.IsSynthetic || !node.IsVisible)
			{
				return;
			}
			switch (node.NodeType)
			{
			default:
				return;
			case NodeType.Constructor:
				CheckExplicitParametersType(node);
				return;
			case NodeType.Method:
			{
				Method method = (Method)node;
				if (method.IsPropertyAccessor())
				{
					return;
				}
				CheckExplicitParametersType(node);
				if (method.ReturnType != null || (method.Entity != null && ((IMethod)method.Entity).ReturnType == base.TypeSystemServices.VoidType))
				{
					return;
				}
				break;
			}
			case NodeType.Property:
				if (null != ((Property)node).Type)
				{
					return;
				}
				break;
			case NodeType.Event:
				if (null != ((Event)node).Type)
				{
					return;
				}
				break;
			case NodeType.Local:
			case NodeType.BlockExpression:
				return;
			}
			base.Warnings.Add(CompilerWarningFactory.VisibleMemberDoesNotDeclareTypeExplicitely(node));
		}

		private void CheckExplicitParametersType(TypeMember node)
		{
			INodeWithParameters nodeWithParameters = node as INodeWithParameters;
			if (null == nodeWithParameters)
			{
				return;
			}
			foreach (ParameterDeclaration parameter in nodeWithParameters.Parameters)
			{
				if (null == parameter.Type)
				{
					base.Warnings.Add(CompilerWarningFactory.VisibleMemberDoesNotDeclareTypeExplicitely(node, parameter.Name));
				}
			}
		}
	}
}
