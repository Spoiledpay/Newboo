using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class ExplicitMemberInfo : Node
	{
		protected SimpleTypeReference _interfaceType;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.ExplicitMemberInfo;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public SimpleTypeReference InterfaceType
		{
			get
			{
				return _interfaceType;
			}
			set
			{
				if (_interfaceType != value)
				{
					_interfaceType = value;
					if (null != _interfaceType)
					{
						_interfaceType.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ExplicitMemberInfo CloneNode()
		{
			return (ExplicitMemberInfo)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new ExplicitMemberInfo CleanClone()
		{
			return (ExplicitMemberInfo)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnExplicitMemberInfo(this);
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
			ExplicitMemberInfo explicitMemberInfo = (ExplicitMemberInfo)node;
			if (!Node.Matches(_interfaceType, explicitMemberInfo._interfaceType))
			{
				return NoMatch("ExplicitMemberInfo._interfaceType");
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
			if (_interfaceType == existing)
			{
				InterfaceType = (SimpleTypeReference)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			ExplicitMemberInfo explicitMemberInfo = new ExplicitMemberInfo();
			explicitMemberInfo._lexicalInfo = _lexicalInfo;
			explicitMemberInfo._endSourceLocation = _endSourceLocation;
			explicitMemberInfo._documentation = _documentation;
			explicitMemberInfo._isSynthetic = _isSynthetic;
			explicitMemberInfo._entity = _entity;
			if (_annotations != null)
			{
				explicitMemberInfo._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _interfaceType)
			{
				explicitMemberInfo._interfaceType = _interfaceType.Clone() as SimpleTypeReference;
				explicitMemberInfo._interfaceType.InitializeParent(explicitMemberInfo);
			}
			return explicitMemberInfo;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _interfaceType)
			{
				_interfaceType.ClearTypeSystemBindings();
			}
		}

		public ExplicitMemberInfo()
		{
		}

		public ExplicitMemberInfo(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}
	}
}
