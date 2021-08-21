using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class CompileUnit : Node
	{
		protected ModuleCollection _modules;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.CompileUnit;

		[XmlArray]
		[XmlArrayItem(typeof(Module))]
		[GeneratedCode("astgen.boo", "1")]
		public ModuleCollection Modules
		{
			get
			{
				return _modules ?? (_modules = new ModuleCollection(this));
			}
			set
			{
				if (_modules != value)
				{
					_modules = value;
					if (null != _modules)
					{
						_modules.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new CompileUnit CloneNode()
		{
			return (CompileUnit)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new CompileUnit CleanClone()
		{
			return (CompileUnit)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnCompileUnit(this);
		}

		[GeneratedCode("astgen.boo", "1")]
		public override bool Matches(Node node)
		{
			if (node == null)
			{
				return false;
			}
			if (NodeType != node.NodeType)
			{
				return false;
			}
			CompileUnit compileUnit = (CompileUnit)node;
			if (!Node.AllMatch(_modules, compileUnit._modules))
			{
				return NoMatch("CompileUnit._modules");
			}
			return true;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override bool Replace(Node existing, Node newNode)
		{
			if (base.Replace(existing, newNode))
			{
				return true;
			}
			if (_modules != null)
			{
				Module module = existing as Module;
				if (null != module)
				{
					Module newItem = (Module)newNode;
					if (_modules.Replace(module, newItem))
					{
						return true;
					}
				}
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			CompileUnit compileUnit = new CompileUnit();
			compileUnit._lexicalInfo = _lexicalInfo;
			compileUnit._endSourceLocation = _endSourceLocation;
			compileUnit._documentation = _documentation;
			compileUnit._isSynthetic = _isSynthetic;
			compileUnit._entity = _entity;
			if (_annotations != null)
			{
				compileUnit._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _modules)
			{
				compileUnit._modules = _modules.Clone() as ModuleCollection;
				compileUnit._modules.InitializeParent(compileUnit);
			}
			return compileUnit;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _modules)
			{
				_modules.ClearTypeSystemBindings();
			}
		}

		public CompileUnit()
		{
		}

		public CompileUnit(params Module[] modules)
		{
			Modules.AddRange(modules);
		}

		internal CompileUnit(LexicalInfo lexicalInfoProvider)
			: base(lexicalInfoProvider)
		{
		}
	}
}
