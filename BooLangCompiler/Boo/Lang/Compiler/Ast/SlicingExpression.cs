using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Xml.Serialization;

namespace Boo.Lang.Compiler.Ast
{
	[Serializable]
	public class SlicingExpression : Expression
	{
		protected Expression _target;

		protected SliceCollection _indices;

		[GeneratedCode("astgen.boo", "1")]
		public override NodeType NodeType => NodeType.SlicingExpression;

		[XmlElement]
		[GeneratedCode("astgen.boo", "1")]
		public Expression Target
		{
			get
			{
				return _target;
			}
			set
			{
				if (_target != value)
				{
					_target = value;
					if (null != _target)
					{
						_target.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		[XmlArray]
		[XmlArrayItem(typeof(Slice))]
		public SliceCollection Indices
		{
			get
			{
				return _indices ?? (_indices = new SliceCollection(this));
			}
			set
			{
				if (_indices != value)
				{
					_indices = value;
					if (null != _indices)
					{
						_indices.InitializeParent(this);
					}
				}
			}
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SlicingExpression CloneNode()
		{
			return (SlicingExpression)Clone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public new SlicingExpression CleanClone()
		{
			return (SlicingExpression)base.CleanClone();
		}

		[GeneratedCode("astgen.boo", "1")]
		public override void Accept(IAstVisitor visitor)
		{
			visitor.OnSlicingExpression(this);
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
			SlicingExpression slicingExpression = (SlicingExpression)node;
			if (!Node.Matches(_target, slicingExpression._target))
			{
				return NoMatch("SlicingExpression._target");
			}
			if (!Node.AllMatch(_indices, slicingExpression._indices))
			{
				return NoMatch("SlicingExpression._indices");
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
			if (_target == existing)
			{
				Target = (Expression)newNode;
				return true;
			}
			if (_indices != null)
			{
				Slice slice = existing as Slice;
				if (null != slice)
				{
					Slice newItem = (Slice)newNode;
					if (_indices.Replace(slice, newItem))
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
			SlicingExpression slicingExpression = new SlicingExpression();
			slicingExpression._lexicalInfo = _lexicalInfo;
			slicingExpression._endSourceLocation = _endSourceLocation;
			slicingExpression._documentation = _documentation;
			slicingExpression._isSynthetic = _isSynthetic;
			slicingExpression._entity = _entity;
			if (_annotations != null)
			{
				slicingExpression._annotations = (Hashtable)_annotations.Clone();
			}
			slicingExpression._expressionType = _expressionType;
			if (null != _target)
			{
				slicingExpression._target = _target.Clone() as Expression;
				slicingExpression._target.InitializeParent(slicingExpression);
			}
			if (null != _indices)
			{
				slicingExpression._indices = _indices.Clone() as SliceCollection;
				slicingExpression._indices.InitializeParent(slicingExpression);
			}
			return slicingExpression;
		}

		[GeneratedCode("astgen.boo", "1")]
		internal override void ClearTypeSystemBindings()
		{
			_annotations = null;
			_entity = null;
			_expressionType = null;
			if (null != _target)
			{
				_target.ClearTypeSystemBindings();
			}
			if (null != _indices)
			{
				_indices.ClearTypeSystemBindings();
			}
		}

		public SlicingExpression()
		{
		}

		public SlicingExpression(LexicalInfo lexicalInfo)
			: base(lexicalInfo)
		{
		}

		public SlicingExpression(Expression target, Expression begin)
			: base(target.LexicalInfo)
		{
			Target = target;
			Indices.Add(new Slice(begin));
		}

		public SlicingExpression(Expression target, params Slice[] indices)
			: base(target.LexicalInfo)
		{
			Target = target;
			Indices.AddRange(indices);
		}
	}
}
