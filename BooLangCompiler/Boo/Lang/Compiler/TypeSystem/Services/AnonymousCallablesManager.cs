using System;
using System.Collections.Generic;
using System.IO;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem.Builders;
using Boo.Lang.Compiler.TypeSystem.Core;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	internal class AnonymousCallablesManager
	{
		private TypeSystemServices _tss;

		private IDictionary<CallableSignature, AnonymousCallableType> _cache = new Dictionary<CallableSignature, AnonymousCallableType>();

		public static readonly object AnonymousCallableTypeAnnotation = new object();

		protected TypeSystemServices TypeSystemServices => _tss;

		protected BooCodeBuilder CodeBuilder => _tss.CodeBuilder;

		public AnonymousCallablesManager(TypeSystemServices tss)
		{
			_tss = tss;
		}

		public ICallableType GetCallableType(CallableSignature signature)
		{
			AnonymousCallableType anonymousCallableType = GetCachedCallableType(signature);
			if (anonymousCallableType == null)
			{
				anonymousCallableType = new AnonymousCallableType(TypeSystemServices, signature);
				_cache.Add(signature, anonymousCallableType);
			}
			return anonymousCallableType;
		}

		private AnonymousCallableType GetCachedCallableType(CallableSignature signature)
		{
			_cache.TryGetValue(signature, out var value);
			return value;
		}

		public IType GetConcreteCallableType(Node sourceNode, CallableSignature signature)
		{
			ICallableType callableType = GetCallableType(signature);
			AnonymousCallableType anonymousCallableType = callableType as AnonymousCallableType;
			return (anonymousCallableType != null) ? GetConcreteCallableType(sourceNode, anonymousCallableType) : callableType;
		}

		public IType GetConcreteCallableType(Node sourceNode, AnonymousCallableType anonymousType)
		{
			return anonymousType.ConcreteType ?? (anonymousType.ConcreteType = CreateConcreteCallableType(sourceNode, anonymousType));
		}

		private IType CreateConcreteCallableType(Node sourceNode, AnonymousCallableType anonymousType)
		{
			Module compilerGeneratedTypesModule = TypeSystemServices.GetCompilerGeneratedTypesModule();
			string name = GenerateCallableTypeNameFrom(sourceNode, compilerGeneratedTypesModule);
			ClassDefinition classDefinition = My<CallableTypeBuilder>.Instance.CreateEmptyCallableDefinition(name);
			classDefinition.Annotate(AnonymousCallableTypeAnnotation);
			classDefinition.Modifiers |= TypeMemberModifiers.Public;
			classDefinition.LexicalInfo = sourceNode.LexicalInfo;
			classDefinition.Members.Add(CreateInvokeMethod(anonymousType));
			Method item = CreateBeginInvokeMethod(anonymousType);
			classDefinition.Members.Add(item);
			classDefinition.Members.Add(CreateEndInvokeMethod(anonymousType));
			compilerGeneratedTypesModule.Members.Add(classDefinition);
			return (IType)classDefinition.Entity;
		}

		private string GenerateCallableTypeNameFrom(Node sourceNode, Module module)
		{
			TypeMember typeMember = (sourceNode.GetAncestor(NodeType.ClassDefinition) ?? sourceNode.GetAncestor(NodeType.InterfaceDefinition) ?? sourceNode.GetAncestor(NodeType.EnumDefinition) ?? sourceNode.GetAncestor(NodeType.Module)) as TypeMember;
			string text = "";
			string text2 = "";
			if (typeMember != null)
			{
				text += typeMember.Name;
				typeMember = (sourceNode.GetAncestor(NodeType.Method) ?? sourceNode.GetAncestor(NodeType.Property) ?? sourceNode.GetAncestor(NodeType.Event) ?? sourceNode.GetAncestor(NodeType.Field)) as TypeMember;
				if (typeMember != null)
				{
					text = text + "_" + typeMember.Name;
				}
				text += "$";
			}
			else if (!sourceNode.LexicalInfo.Equals(LexicalInfo.Empty))
			{
				text = text + Path.GetFileNameWithoutExtension(sourceNode.LexicalInfo.FileName) + "$";
			}
			if (!sourceNode.LexicalInfo.Equals(LexicalInfo.Empty))
			{
				text2 = "$" + sourceNode.LexicalInfo.Line + "_" + sourceNode.LexicalInfo.Column + text2;
			}
			return "__" + text + "callable" + module.Members.Count + text2 + "__";
		}

		private Method CreateBeginInvokeMethod(ICallableType anonymousType)
		{
			Method method = CodeBuilder.CreateRuntimeMethod("BeginInvoke", TypeSystemServices.Map(typeof(IAsyncResult)), anonymousType.GetSignature().Parameters, hasParamArray: false);
			int count = method.Parameters.Count;
			method.Parameters.Add(CodeBuilder.CreateParameterDeclaration(count + 1, "callback", TypeSystemServices.Map(typeof(AsyncCallback))));
			method.Parameters.Add(CodeBuilder.CreateParameterDeclaration(count + 1, "asyncState", TypeSystemServices.ObjectType));
			return method;
		}

		public Method CreateEndInvokeMethod(ICallableType anonymousType)
		{
			CallableSignature signature = anonymousType.GetSignature();
			Method method = CodeBuilder.CreateRuntimeMethod("EndInvoke", signature.ReturnType);
			int num = 1;
			IParameter[] parameters = signature.Parameters;
			foreach (IParameter parameter in parameters)
			{
				if (parameter.IsByRef)
				{
					method.Parameters.Add(CodeBuilder.CreateParameterDeclaration(++num, parameter.Name, parameter.Type, byref: true));
				}
			}
			num = method.Parameters.Count;
			method.Parameters.Add(CodeBuilder.CreateParameterDeclaration(num + 1, "result", TypeSystemServices.Map(typeof(IAsyncResult))));
			return method;
		}

		private Method CreateInvokeMethod(AnonymousCallableType anonymousType)
		{
			CallableSignature signature = anonymousType.GetSignature();
			return CodeBuilder.CreateRuntimeMethod("Invoke", signature.ReturnType, signature.Parameters, signature.AcceptVarArgs);
		}
	}
}
