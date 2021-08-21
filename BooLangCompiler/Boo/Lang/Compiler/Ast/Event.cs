using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Event : TypeMember
	{
		protected Method _add;

		protected Method _remove;

		protected Method _raise;

		protected TypeReference _type;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Event;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Method Add
		{
			get
			{
				return _add;
			}
			set
			{
				if (_add != value)
				{
					_add = value;
					if (null != _add)
					{
						_add.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Method Remove
		{
			get
			{
				return _remove;
			}
			set
			{
				if (_remove != value)
				{
					_remove = value;
					if (null != _remove)
					{
						_remove.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Method Raise
		{
			get
			{
				return _raise;
			}
			set
			{
				if (_raise != value)
				{
					_raise = value;
					if (null != _raise)
					{
						_raise.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public TypeReference Type
		{
			get
			{
				return _type;
			}
			set
			{
				if (_type != value)
				{
					_type = value;
					if (null != _type)
					{
						_type.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Event CloneNode()
		{
			return (Event)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Event CleanClone()
		{
			return (Event)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnEvent(this);
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
			Event @event = (Event)node;
			if (_modifiers != @event._modifiers)
			{
				return NoMatch("Event._modifiers");
			}
			if (_name != @event._name)
			{
				return NoMatch("Event._name");
			}
			if (!Node.AllMatch(_attributes, @event._attributes))
			{
				return NoMatch("Event._attributes");
			}
			if (!Node.Matches(_add, @event._add))
			{
				return NoMatch("Event._add");
			}
			if (!Node.Matches(_remove, @event._remove))
			{
				return NoMatch("Event._remove");
			}
			if (!Node.Matches(_raise, @event._raise))
			{
				return NoMatch("Event._raise");
			}
			if (!Node.Matches(_type, @event._type))
			{
				return NoMatch("Event._type");
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
			if (_add == existing)
			{
				Add = (Method)newNode;
				return true;
			}
			if (_remove == existing)
			{
				Remove = (Method)newNode;
				return true;
			}
			if (_raise == existing)
			{
				Raise = (Method)newNode;
				return true;
			}
			if (_type == existing)
			{
				Type = (TypeReference)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			Event @event = new Event();
			@event._lexicalInfo = _lexicalInfo;
			@event._endSourceLocation = _endSourceLocation;
			@event._documentation = _documentation;
			@event._isSynthetic = _isSynthetic;
			@event._entity = _entity;
			if (_annotations != null)
			{
				@event._annotations = (Hashtable)_annotations.Clone();
			}
			@event._modifiers = _modifiers;
			@event._name = _name;
			if (null != _attributes)
			{
				@event._attributes = _attributes.Clone() as AttributeCollection;
				@event._attributes.InitializeParent(@event);
			}
			if (null != _add)
			{
				@event._add = _add.Clone() as Method;
				@event._add.InitializeParent(@event);
			}
			if (null != _remove)
			{
				@event._remove = _remove.Clone() as Method;
				@event._remove.InitializeParent(@event);
			}
			if (null != _raise)
			{
				@event._raise = _raise.Clone() as Method;
				@event._raise.InitializeParent(@event);
			}
			if (null != _type)
			{
				@event._type = _type.Clone() as TypeReference;
				@event._type.InitializeParent(@event);
			}
			return @event;
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
			if (null != _add)
			{
				_add.ClearTypeSystemBindings();
			}
			if (null != _remove)
			{
				_remove.ClearTypeSystemBindings();
			}
			if (null != _raise)
			{
				_raise.ClearTypeSystemBindings();
			}
			if (null != _type)
			{
				_type.ClearTypeSystemBindings();
			}
		}

		public Event()
		{
		}

		public Event(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public Event(LexicalInfo lexicalInfo, string name, TypeReference type)
			: base(lexicalInfo)
		{
			base.Name = name;
			Type = type;
		}
	}
}
