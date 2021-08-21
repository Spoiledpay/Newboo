using System;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.TypeSystem.Services
{
	public class CodeReifier : AbstractCompilerComponent
	{
		public Statement Reify(Statement node)
		{
			return ReifyNode(node);
		}

		public Expression Reify(Expression node)
		{
			return ReifyNode(node);
		}

		public void ReifyInto(TypeDefinition parentType, TypeMember member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			if (parentType == null)
			{
				throw new ArgumentNullException("parentType");
			}
			if (member.ParentNode != null)
			{
				throw new ArgumentException("ParentNode must be null for member to be reified.", "member");
			}
			parentType.Members.Add(member);
			ReifyNode(member);
		}

		public void MergeInto(TypeDefinition targetType, TypeDefinition mixin)
		{
			if (null == targetType)
			{
				throw new ArgumentNullException("targetType");
			}
			if (null == mixin)
			{
				throw new ArgumentNullException("mixin");
			}
			targetType.BaseTypes.AddRange(mixin.BaseTypes);
			foreach (TypeReference baseType in mixin.BaseTypes)
			{
				Reify(baseType);
			}
			targetType.Members.AddRange(mixin.Members);
			foreach (TypeMember member in mixin.Members)
			{
				ReifyNode(member);
			}
		}

		private void Reify(TypeReference node)
		{
			ForEachReifier(delegate(ITypeReferenceReifier r)
			{
				r.Reify(node);
			});
		}

		private T ReifyNode<T>(T node) where T : Node
		{
			T original = node;
			Node originalParent = original.ParentNode;
			if (null == originalParent)
			{
				throw new ArgumentException($"ParentNode must be set on {typeof(T).Name}.", "node");
			}
			ForEachReifier(delegate(INodeReifier<T> r)
			{
				node = r.Reify(node);
				if (node != original)
				{
					originalParent.Replace(original, node);
					original = node;
					originalParent = original.ParentNode;
				}
			});
			return node;
		}

		private void ForEachReifier<T>(Action<T> action) where T : class
		{
			INamespace currentNamespace = base.NameResolutionService.CurrentNamespace;
			try
			{
				CompilerPipeline pipeline = base.Parameters.Pipeline;
				ICompilerStep currentStep = pipeline.CurrentStep;
				foreach (ICompilerStep item in pipeline)
				{
					if (item == currentStep)
					{
						break;
					}
					T val = item as T;
					if (val != null)
					{
						action(val);
					}
				}
			}
			finally
			{
				base.NameResolutionService.EnterNamespace(currentNamespace);
			}
		}
	}
}
