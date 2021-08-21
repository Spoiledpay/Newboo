using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler.TypeSystem;
using Boo.Lang.Compiler.TypeSystem.Internal;

namespace Boo.Lang.Compiler.Steps
{
	public class IntroduceModuleClasses : AbstractFastVisitorCompilerStep
	{
		public const string ModuleAttributeName = "System.Runtime.CompilerServices.CompilerGlobalScopeAttribute";

		public const string EntryPointMethodName = "Main";

		private IType _booModuleAttributeType;

		public bool ForceModuleClass { get; set; }

		public static bool IsModuleClass(TypeMember member)
		{
			return NodeType.ClassDefinition == member.NodeType && member.Attributes.Contains("System.Runtime.CompilerServices.CompilerGlobalScopeAttribute");
		}

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			_booModuleAttributeType = base.TypeSystemServices.Map(typeof(CompilerGlobalScopeAttribute));
		}

		public override void Run()
		{
			Visit(base.CompileUnit.Modules);
			DetectOutputType();
		}

		private void DetectOutputType()
		{
			if (base.Parameters.OutputType == CompilerOutputType.Auto)
			{
				base.Parameters.OutputType = ((!HasEntryPoint()) ? CompilerOutputType.Library : CompilerOutputType.ConsoleApplication);
			}
		}

		private bool HasEntryPoint()
		{
			return ContextAnnotations.GetEntryPoint(base.Context) != null;
		}

		public override void Dispose()
		{
			_booModuleAttributeType = null;
			base.Dispose();
		}

		public override void OnModule(Module module)
		{
			ClassDefinition classDefinition = ExistingModuleClassFor(module);
			ClassDefinition classDefinition2 = classDefinition ?? NewModuleClassFor(module);
			MoveModuleMembersToModuleClass(module, classDefinition2);
			MoveModuleAttributesToModuleClass(module, classDefinition2);
			DetectEntryPoint(module, classDefinition2);
			if (classDefinition != null || ForceModuleClass || classDefinition2.Members.Count > 0)
			{
				if (classDefinition2 != classDefinition)
				{
					classDefinition2.Members.Add(AstUtil.CreateConstructor(module, TypeMemberModifiers.Private));
					module.Members.Add(classDefinition2);
				}
				InitializeModuleClassEntity(module, classDefinition2);
			}
		}

		private static void MoveModuleAttributesToModuleClass(Module module, ClassDefinition moduleClass)
		{
			moduleClass.Attributes.AddRange(module.Attributes);
			module.Attributes.Clear();
		}

		private void DetectEntryPoint(Module module, ClassDefinition moduleClass)
		{
			Method method = (module.Globals.IsEmpty ? (moduleClass.Members["Main"] as Method) : TransformModuleGlobalsIntoEntryPoint(module, moduleClass));
			if (method != null && base.Parameters.OutputType != CompilerOutputType.Library)
			{
				ContextAnnotations.SetEntryPoint(base.Context, method);
			}
		}

		private Method TransformModuleGlobalsIntoEntryPoint(Module node, ClassDefinition moduleClass)
		{
			Method method = new Method();
			method.Name = "Main";
			method.IsSynthetic = true;
			method.Body = node.Globals;
			method.ReturnType = base.CodeBuilder.CreateTypeReference(base.TypeSystemServices.VoidType);
			method.Modifiers = TypeMemberModifiers.Private | TypeMemberModifiers.Static;
			method.LexicalInfo = node.Globals.Statements[0].LexicalInfo;
			method.EndSourceLocation = node.EndSourceLocation;
			method.Parameters.Add(new ParameterDeclaration("argv", new ArrayTypeReference(new SimpleTypeReference("string"))));
			Method method2 = method;
			moduleClass.Members.Add(method2);
			node.Globals = null;
			return method2;
		}

		private static void MoveModuleMembersToModuleClass(Module node, ClassDefinition moduleClass)
		{
			int num = 0;
			TypeMember[] array = node.Members.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				TypeMember typeMember = array[i];
				if (!(typeMember is TypeDefinition))
				{
					node.Members.RemoveAt(i - num);
					typeMember.Modifiers |= TypeMemberModifiers.Static;
					moduleClass.Members.Add(typeMember);
					num++;
				}
			}
		}

		private ClassDefinition NewModuleClassFor(Module node)
		{
			ClassDefinition classDefinition = new ClassDefinition(node.LexicalInfo);
			classDefinition.IsSynthetic = true;
			classDefinition.Modifiers = TypeMemberModifiers.Public | TypeMemberModifiers.Transient | TypeMemberModifiers.Final;
			classDefinition.EndSourceLocation = node.EndSourceLocation;
			classDefinition.Name = BuildModuleClassName(node);
			ClassDefinition classDefinition2 = classDefinition;
			classDefinition2.Attributes.Add(CreateBooModuleAttribute());
			return classDefinition2;
		}

		private static void InitializeModuleClassEntity(Module node, ClassDefinition moduleClass)
		{
			InternalModule internalModule = (InternalModule)node.Entity;
			if (null != internalModule)
			{
				internalModule.InitializeModuleClass(moduleClass);
			}
		}

		private static ClassDefinition ExistingModuleClassFor(Module node)
		{
			return node.Members.OfType<ClassDefinition>().Where(IsModuleClass).SingleOrDefault();
		}

		private Attribute CreateBooModuleAttribute()
		{
			Attribute attribute = new Attribute("System.Runtime.CompilerServices.CompilerGlobalScopeAttribute");
			attribute.Entity = _booModuleAttributeType;
			return attribute;
		}

		private string BuildModuleClassName(Module module)
		{
			string name = module.Name;
			if (string.IsNullOrEmpty(name))
			{
				module.Name = base.Context.GetUniqueName("Module");
				return module.Name;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (!char.IsLetter(name[0]) && name[0] != '_')
			{
				stringBuilder.Append('_');
			}
			stringBuilder.Append(char.ToUpper(name[0]));
			for (int i = 1; i < name.Length; i++)
			{
				char c = name[i];
				stringBuilder.Append(char.IsLetterOrDigit(c) ? c : '_');
			}
			return stringBuilder.Append("Module").ToString();
		}
	}
}
