using System;
using System.Runtime.CompilerServices;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Builders
{
	public class CallableTypeBuilder : AbstractCompilerComponent
	{
		public ClassDefinition ForCallableDefinition(CallableDefinition node)
		{
			ClassDefinition classDefinition = CreateEmptyCallableDefinition(node.Name);
			classDefinition.LexicalInfo = node.LexicalInfo;
			classDefinition.GenericParameters = node.GenericParameters;
			classDefinition.Members.Add(CreateInvokeMethod(node));
			classDefinition.Members.Add(CreateBeginInvokeMethod(node));
			classDefinition.Members.Add(CreateEndInvokeMethod(node));
			return classDefinition;
		}

		public ClassDefinition CreateEmptyCallableDefinition(string name)
		{
			ClassDefinition classDefinition = new ClassDefinition();
			classDefinition.IsSynthetic = true;
			classDefinition.Name = name;
			classDefinition.Modifiers = TypeMemberModifiers.Final;
			ClassDefinition classDefinition2 = classDefinition;
			classDefinition2.Entity = new InternalCallableType(My<InternalTypeSystemProvider>.Instance, classDefinition2);
			classDefinition2.BaseTypes.Add(base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.MulticastDelegateType));
			classDefinition2.BaseTypes.Add(base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.ICallableType));
			classDefinition2.Attributes.Add(base.CodeBuilder.CreateAttribute(typeof(CompilerGeneratedAttribute)));
			classDefinition2.Members.Add(CreateCallableConstructor());
			classDefinition2.Members.Add(CreateCallMethod());
			return classDefinition2;
		}

		private Method CreateCallMethod()
		{
			Method method = base.CodeBuilder.CreateMethod("Call", base.TypeSystemServices.ObjectType, TypeMemberModifiers.Public | TypeMemberModifiers.Virtual);
			method.Parameters.Add(base.CodeBuilder.CreateParameterDeclaration(1, "args", base.TypeSystemServices.ObjectArrayType));
			return method;
		}

		private Constructor CreateCallableConstructor()
		{
			Constructor constructor = base.CodeBuilder.CreateConstructor(TypeMemberModifiers.Public);
			constructor.ImplementationFlags = MethodImplementationFlags.Runtime;
			constructor.Parameters.Add(base.CodeBuilder.CreateParameterDeclaration(1, "instance", base.TypeSystemServices.ObjectType));
			constructor.Parameters.Add(base.CodeBuilder.CreateParameterDeclaration(2, "method", base.TypeSystemServices.IntPtrType));
			return constructor;
		}

		private Method CreateInvokeMethod(CallableDefinition node)
		{
			Method method = CreateRuntimeMethod("Invoke", node.ReturnType);
			method.Parameters = node.Parameters;
			return method;
		}

		private Method CreateBeginInvokeMethod(CallableDefinition node)
		{
			Method method = CreateRuntimeMethod("BeginInvoke", base.CodeBuilder.CreateTypeReference(node.LexicalInfo, typeof(IAsyncResult)));
			method.Parameters.ExtendWithClones(node.Parameters);
			method.Parameters.Add(new ParameterDeclaration("callback", base.CodeBuilder.CreateTypeReference(node.LexicalInfo, typeof(AsyncCallback))));
			method.Parameters.Add(new ParameterDeclaration("asyncState", base.CodeBuilder.CreateTypeReference(node.LexicalInfo, base.TypeSystemServices.ObjectType)));
			return method;
		}

		private Method CreateEndInvokeMethod(CallableDefinition node)
		{
			Method method = CreateRuntimeMethod("EndInvoke", node.ReturnType);
			foreach (ParameterDeclaration parameter in node.Parameters)
			{
				if (parameter.IsByRef)
				{
					method.Parameters.Add(parameter.CloneNode());
				}
			}
			method.Parameters.Add(new ParameterDeclaration("asyncResult", base.CodeBuilder.CreateTypeReference(node.LexicalInfo, typeof(IAsyncResult))));
			return method;
		}

		private Method CreateRuntimeMethod(string name, TypeReference returnType)
		{
			Method method = new Method();
			method.Name = name;
			method.ReturnType = returnType;
			method.Modifiers = TypeMemberModifiers.Public | TypeMemberModifiers.Virtual;
			method.ImplementationFlags = MethodImplementationFlags.Runtime;
			return method;
		}
	}
}
