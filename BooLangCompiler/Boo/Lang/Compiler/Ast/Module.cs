using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Module : TypeDefinition
	{
		protected NamespaceDeclaration _namespace;

		protected ImportCollection _imports;

		protected Block _globals;

		protected AttributeCollection _assemblyAttributes;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Module;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public NamespaceDeclaration Namespace
		{
			get
			{
				return _namespace;
			}
			set
			{
				if (_namespace != value)
				{
					_namespace = value;
					if (null != _namespace)
					{
						_namespace.InitializeParent(this);
					}
				}
			}
		}

		[XmlArrayItem(typeof(Import))]
		[XmlArray]
		[GeneratedCode("astgen.boo", "1")]
		public ImportCollection Imports
		{
			get
			{
				return _imports ?? (_imports = new ImportCollection(this));
			}
			set
			{
				if (_imports != value)
				{
					_imports = value;
					if (null != _imports)
					{
						_imports.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Block Globals
		{
			get
			{
				if (_globals == null)
				{
					_globals = new Block();
					_globals.InitializeParent(this);
				}
				return _globals;
			}
			set
			{
				if (_globals != value)
				{
					_globals = value;
					if (null != _globals)
					{
						_globals.InitializeParent(this);
					}
				}
			}
		}

		[XmlArray]
		[XmlArrayItem(typeof(Attribute))]
		[GeneratedCode("astgen.boo", "1")]
		public AttributeCollection AssemblyAttributes
		{
			get
			{
				return _assemblyAttributes ?? (_assemblyAttributes = new AttributeCollection(this));
			}
			set
			{
				if (_assemblyAttributes != value)
				{
					_assemblyAttributes = value;
					if (null != _assemblyAttributes)
					{
						_assemblyAttributes.InitializeParent(this);
					}
				}
			}
		}

		public override NamespaceDeclaration EnclosingNamespace => _namespace;

		public override string FullName
		{
			get
			{
				if (null != _namespace)
				{
					return _namespace.Name + "." + base.Name;
				}
				return base.Name;
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Module CloneNode()
		{
			return (Module)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Module CleanClone()
		{
			return (Module)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnModule(this);
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
			Module module = (Module)node;
			if (_modifiers != module._modifiers)
			{
				return NoMatch("Module._modifiers");
			}
			if (_name != module._name)
			{
				return NoMatch("Module._name");
			}
			if (!Node.AllMatch(_attributes, module._attributes))
			{
				return NoMatch("Module._attributes");
			}
			if (!Node.AllMatch(_members, module._members))
			{
				return NoMatch("Module._members");
			}
			if (!Node.AllMatch(_baseTypes, module._baseTypes))
			{
				return NoMatch("Module._baseTypes");
			}
			if (!Node.AllMatch(_genericParameters, module._genericParameters))
			{
				return NoMatch("Module._genericParameters");
			}
			if (!Node.Matches(_namespace, module._namespace))
			{
				return NoMatch("Module._namespace");
			}
			if (!Node.AllMatch(_imports, module._imports))
			{
				return NoMatch("Module._imports");
			}
			if (!Node.Matches(_globals, module._globals))
			{
				return NoMatch("Module._globals");
			}
			if (!Node.AllMatch(_assemblyAttributes, module._assemblyAttributes))
			{
				return NoMatch("Module._assemblyAttributes");
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
			if (_attributes != null)
			{
				Attribute attribute = existing as Attribute;
				if (null != attribute)
				{
					Attribute newItem = (Attribute)newNode;
					if (_attributes.Replace(attribute, newItem))
					{
						return true;
					}
				}
			}
			if (_members != null)
			{
				TypeMember typeMember = existing as TypeMember;
				if (null != typeMember)
				{
					TypeMember newItem2 = (TypeMember)newNode;
					if (_members.Replace(typeMember, newItem2))
					{
						return true;
					}
				}
			}
			if (_baseTypes != null)
			{
				TypeReference typeReference = existing as TypeReference;
				if (null != typeReference)
				{
					TypeReference newItem3 = (TypeReference)newNode;
					if (_baseTypes.Replace(typeReference, newItem3))
					{
						return true;
					}
				}
			}
			if (_genericParameters != null)
			{
				GenericParameterDeclaration genericParameterDeclaration = existing as GenericParameterDeclaration;
				if (null != genericParameterDeclaration)
				{
					GenericParameterDeclaration newItem4 = (GenericParameterDeclaration)newNode;
					if (_genericParameters.Replace(genericParameterDeclaration, newItem4))
					{
						return true;
					}
				}
			}
			if (_namespace == existing)
			{
				Namespace = (NamespaceDeclaration)newNode;
				return true;
			}
			if (_imports != null)
			{
				Import import = existing as Import;
				if (null != import)
				{
					Import newItem5 = (Import)newNode;
					if (_imports.Replace(import, newItem5))
					{
						return true;
					}
				}
			}
			if (_globals == existing)
			{
				Globals = (Block)newNode;
				return true;
			}
			if (_assemblyAttributes != null)
			{
				Attribute attribute = existing as Attribute;
				if (null != attribute)
				{
					Attribute newItem = (Attribute)newNode;
					if (_assemblyAttributes.Replace(attribute, newItem))
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
			Module module = new Module();
			module._lexicalInfo = _lexicalInfo;
			module._endSourceLocation = _endSourceLocation;
			module._documentation = _documentation;
			module._isSynthetic = _isSynthetic;
			module._entity = _entity;
			if (_annotations != null)
			{
				module._annotations = (Hashtable)_annotations.Clone();
			}
			module._modifiers = _modifiers;
			module._name = _name;
			if (null != _attributes)
			{
				module._attributes = _attributes.Clone() as AttributeCollection;
				module._attributes.InitializeParent(module);
			}
			if (null != _members)
			{
				module._members = _members.Clone() as TypeMemberCollection;
				module._members.InitializeParent(module);
			}
			if (null != _baseTypes)
			{
				module._baseTypes = _baseTypes.Clone() as TypeReferenceCollection;
				module._baseTypes.InitializeParent(module);
			}
			if (null != _genericParameters)
			{
				module._genericParameters = _genericParameters.Clone() as GenericParameterDeclarationCollection;
				module._genericParameters.InitializeParent(module);
			}
			if (null != _namespace)
			{
				module._namespace = _namespace.Clone() as NamespaceDeclaration;
				module._namespace.InitializeParent(module);
			}
			if (null != _imports)
			{
				module._imports = _imports.Clone() as ImportCollection;
				module._imports.InitializeParent(module);
			}
			if (null != _globals)
			{
				module._globals = _globals.Clone() as Block;
				module._globals.InitializeParent(module);
			}
			if (null != _assemblyAttributes)
			{
				module._assemblyAttributes = _assemblyAttributes.Clone() as AttributeCollection;
				module._assemblyAttributes.InitializeParent(module);
			}
			return module;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _attributes)
			{
				_attributes.ClearTypeSystemBindings();
			}
			if (null != _members)
			{
				_members.ClearTypeSystemBindings();
			}
			if (null != _baseTypes)
			{
				_baseTypes.ClearTypeSystemBindings();
			}
			if (null != _genericParameters)
			{
				_genericParameters.ClearTypeSystemBindings();
			}
			if (null != _namespace)
			{
				_namespace.ClearTypeSystemBindings();
			}
			if (null != _imports)
			{
				_imports.ClearTypeSystemBindings();
			}
			if (null != _globals)
			{
				_globals.ClearTypeSystemBindings();
			}
			if (null != _assemblyAttributes)
			{
				_assemblyAttributes.ClearTypeSystemBindings();
			}
		}

		public Module()
		{
		}

		public Module(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public Module(LexicalInfo lexicalInfo, string name)
			: base(lexicalInfo)
		{
			base.Name = name;
		}

		public Module(Block globals)
		{
			if (null == globals)
			{
				throw new ArgumentNullException("globals");
			}
			Globals = globals;
		}
	}
}
