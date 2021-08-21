#define DEBUG
using System.Diagnostics;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;

namespace Boo.Lang.Compiler.Steps
{
	public class ImplementICallableOnCallableDefinitions : AbstractVisitorCompilerStep
	{
		private IMethod _RuntimeServices_GetRange1;

		private IMethod RuntimeServices_GetRange1
		{
			get
			{
				if (null == _RuntimeServices_GetRange1)
				{
					_RuntimeServices_GetRange1 = base.NameResolutionService.ResolveMethod(base.TypeSystemServices.RuntimeServicesType, "GetRange1");
				}
				return _RuntimeServices_GetRange1;
			}
		}

		public override void OnModule(Module node)
		{
			Visit(node.Members, NodeType.ClassDefinition);
		}

		public override void OnClassDefinition(ClassDefinition node)
		{
			Visit(node.Members, NodeType.ClassDefinition);
			InternalCallableType internalCallableType = node.Entity as InternalCallableType;
			if (null != internalCallableType)
			{
				ImplementICallableCall(internalCallableType, node);
			}
		}

		private int GetByRefParamCount(CallableSignature signature)
		{
			int num = 0;
			IParameter[] parameters = signature.Parameters;
			foreach (IParameter parameter in parameters)
			{
				if (parameter.IsByRef)
				{
					num++;
				}
			}
			return num;
		}

		private void ImplementICallableCall(InternalCallableType type, ClassDefinition node)
		{
			Method method = (Method)node.Members["Call"];
			Debug.Assert(null != method);
			Debug.Assert(method.Body.IsEmpty);
			CallableSignature signature = type.GetSignature();
			int byRefParamCount = GetByRefParamCount(signature);
			if (byRefParamCount > 0)
			{
				ImplementByRefICallableCall(method, type, node, signature, byRefParamCount);
			}
			else
			{
				ImplementRegularICallableCall(method, type, node, signature);
			}
		}

		private void ImplementByRefICallableCall(Method call, InternalCallableType type, ClassDefinition node, CallableSignature signature, int byRefCount)
		{
			MethodInvocationExpression methodInvocationExpression = CreateInvokeInvocation(type);
			IParameter[] parameters = signature.Parameters;
			ReferenceExpression referenceExpression = base.CodeBuilder.CreateReference(call.Parameters[0]);
			InternalLocal[] array = new InternalLocal[byRefCount];
			int num = 0;
			for (int i = 0; i < parameters.Length; i++)
			{
				SlicingExpression slicingExpression = base.CodeBuilder.CreateSlicing(referenceExpression.CloneNode(), i);
				IParameter parameter = parameters[i];
				if (parameter.IsByRef)
				{
					IType type2 = parameter.Type;
					if (type2.IsByRef)
					{
						type2 = type2.ElementType;
					}
					array[num] = base.CodeBuilder.DeclareLocal(call, "__temp_" + parameter.Name, type2);
					call.Body.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(array[num]), base.CodeBuilder.CreateCast(type2, slicingExpression)));
					methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateReference(array[num]));
					num++;
				}
				else
				{
					methodInvocationExpression.Arguments.Add(slicingExpression);
				}
			}
			if (base.TypeSystemServices.VoidType == signature.ReturnType)
			{
				call.Body.Add(methodInvocationExpression);
				PropagateByRefParameterChanges(call, parameters, array);
				return;
			}
			InternalLocal local = base.CodeBuilder.DeclareLocal(call, "__returnValue", signature.ReturnType);
			call.Body.Add(base.CodeBuilder.CreateAssignment(base.CodeBuilder.CreateReference(local), methodInvocationExpression));
			PropagateByRefParameterChanges(call, parameters, array);
			call.Body.Add(new ReturnStatement(base.CodeBuilder.CreateReference(local)));
		}

		private void PropagateByRefParameterChanges(Method call, IParameter[] parameters, InternalLocal[] temporaries)
		{
			int num = 0;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].IsByRef)
				{
					SlicingExpression lhs = base.CodeBuilder.CreateSlicing(base.CodeBuilder.CreateReference(call.Parameters[0]), i);
					call.Body.Add(base.CodeBuilder.CreateAssignment(lhs, base.CodeBuilder.CreateReference(temporaries[num])));
					num++;
				}
			}
		}

		private void ImplementRegularICallableCall(Method call, InternalCallableType type, ClassDefinition node, CallableSignature signature)
		{
			MethodInvocationExpression methodInvocationExpression = CreateInvokeInvocation(type);
			IParameter[] parameters = signature.Parameters;
			int num = (signature.AcceptVarArgs ? (parameters.Length - 1) : parameters.Length);
			for (int i = 0; i < num; i++)
			{
				SlicingExpression item = base.CodeBuilder.CreateSlicing(base.CodeBuilder.CreateReference(call.Parameters[0]), i);
				methodInvocationExpression.Arguments.Add(item);
			}
			if (signature.AcceptVarArgs)
			{
				if (parameters.Length == 1)
				{
					methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateReference(call.Parameters[0]));
				}
				else
				{
					methodInvocationExpression.Arguments.Add(base.CodeBuilder.CreateMethodInvocation(RuntimeServices_GetRange1, base.CodeBuilder.CreateReference(call.Parameters[0]), base.CodeBuilder.CreateIntegerLiteral(num)));
				}
			}
			if (base.TypeSystemServices.VoidType == signature.ReturnType)
			{
				call.Body.Add(methodInvocationExpression);
			}
			else
			{
				call.Body.Add(new ReturnStatement(methodInvocationExpression));
			}
		}

		private MethodInvocationExpression CreateInvokeInvocation(InternalCallableType type)
		{
			return base.CodeBuilder.CreateMethodInvocation(base.CodeBuilder.CreateSelfReference(type), type.GetInvokeMethod());
		}
	}
}
