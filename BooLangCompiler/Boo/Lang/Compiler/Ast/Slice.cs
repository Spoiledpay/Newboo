using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class Slice : Node
	{
		protected Expression _begin;

		protected Expression _end;

		protected Expression _step;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.Slice;

		[GeneratedCode("astgen.boo", "1")]
		[XmlElement]
		public Expression Begin
		{
			get
			{
				return _begin;
			}
			set
			{
				if (_begin != value)
				{
					_begin = value;
					if (null != _begin)
					{
						_begin.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression End
		{
			get
			{
				return _end;
			}
			set
			{
				if (_end != value)
				{
					_end = value;
					if (null != _end)
					{
						_end.InitializeParent(this);
					}
				}
			}
		}

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression Step
		{
			get
			{
				return _step;
			}
			set
			{
				if (_step != value)
				{
					_step = value;
					if (null != _step)
					{
						_step.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Slice CloneNode()
		{
			return (Slice)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new Slice CleanClone()
		{
			return (Slice)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnSlice(this);
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
			Slice slice = (Slice)node;
			if (!Node.Matches(_begin, slice._begin))
			{
				return NoMatch("Slice._begin");
			}
			if (!Node.Matches(_end, slice._end))
			{
				return NoMatch("Slice._end");
			}
			if (!Node.Matches(_step, slice._step))
			{
				return NoMatch("Slice._step");
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
			if (_begin == existing)
			{
				Begin = (Expression)newNode;
				return true;
			}
			if (_end == existing)
			{
				End = (Expression)newNode;
				return true;
			}
			if (_step == existing)
			{
				Step = (Expression)newNode;
				return true;
			}
			return false;
		}

		[GeneratedCode("astgen.boo", "1")]
		public override object Clone()
		{
			Slice slice = new Slice();
			slice._lexicalInfo = _lexicalInfo;
			slice._endSourceLocation = _endSourceLocation;
			slice._documentation = _documentation;
			slice._isSynthetic = _isSynthetic;
			slice._entity = _entity;
			if (_annotations != null)
			{
				slice._annotations = (Hashtable)_annotations.Clone();
			}
			if (null != _begin)
			{
				slice._begin = _begin.Clone() as Expression;
				slice._begin.InitializeParent(slice);
			}
			if (null != _end)
			{
				slice._end = _end.Clone() as Expression;
				slice._end.InitializeParent(slice);
			}
			if (null != _step)
			{
				slice._step = _step.Clone() as Expression;
				slice._step.InitializeParent(slice);
			}
			return slice;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			if (null != _begin)
			{
				_begin.ClearTypeSystemBindings();
			}
			if (null != _end)
			{
				_end.ClearTypeSystemBindings();
			}
			if (null != _step)
			{
				_step.ClearTypeSystemBindings();
			}
		}

		public Slice()
		{
		}

		public Slice(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public Slice(LexicalInfo lexicalInfo, Expression begin, Expression end, Expression step)
			: base(lexicalInfo)
		{
			Begin = begin;
			End = end;
			Step = step;
		}

		public Slice(Expression begin, Expression end, Expression step)
		{
			Begin = begin;
			End = end;
			Step = step;
		}

		public Slice(Expression begin)
			: base(begin.LexicalInfo)
		{
			Begin = begin;
		}
	}
}
