#define DEBUG
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using antlr.collections;
using antlr.collections.impl;

namespace antlr
{
	public class ASTFactory
	{
		protected class FactoryEntry
		{
			public Type NodeTypeObject;

			public ASTNodeCreator Creator;

			public FactoryEntry(Type typeObj, ASTNodeCreator creator)
			{
				NodeTypeObject = typeObj;
				Creator = creator;
			}

			public FactoryEntry(Type typeObj)
			{
				NodeTypeObject = typeObj;
			}

			public FactoryEntry(ASTNodeCreator creator)
			{
				Creator = creator;
			}
		}

		protected Type defaultASTNodeTypeObject_;

		protected ASTNodeCreator defaultCreator_;

		protected FactoryEntry[] heteroList_;

		protected Hashtable typename2creator_;

		public ASTFactory()
			: this(typeof(CommonAST))
		{
		}

		public ASTFactory(string nodeTypeName)
			: this(loadNodeTypeObject(nodeTypeName))
		{
		}

		public ASTFactory(Type defaultASTNodeType)
		{
			heteroList_ = new FactoryEntry[5];
			defaultASTNodeTypeObject_ = defaultASTNodeType;
			defaultCreator_ = null;
			typename2creator_ = new Hashtable(32, 0.3f);
			typename2creator_["antlr.CommonAST"] = CommonAST.Creator;
			typename2creator_["antlr.CommonASTWithHiddenTokens"] = CommonASTWithHiddenTokens.Creator;
		}

		public void setTokenTypeASTNodeType(int tokenType, string NodeTypeName)
		{
			if (tokenType < 4)
			{
				throw new ANTLRException("Internal parser error: Cannot change AST Node Type for Token ID '" + tokenType + "'");
			}
			if (tokenType > heteroList_.Length + 1)
			{
				setMaxNodeType(tokenType);
			}
			if (heteroList_[tokenType] == null)
			{
				heteroList_[tokenType] = new FactoryEntry(loadNodeTypeObject(NodeTypeName));
			}
			else
			{
				heteroList_[tokenType].NodeTypeObject = loadNodeTypeObject(NodeTypeName);
			}
		}

		[Obsolete("Replaced by setTokenTypeASTNodeType(int, string) since version 2.7.2.6", true)]
		public void registerFactory(int NodeType, string NodeTypeName)
		{
			setTokenTypeASTNodeType(NodeType, NodeTypeName);
		}

		public void setTokenTypeASTNodeCreator(int NodeType, ASTNodeCreator creator)
		{
			if (NodeType < 4)
			{
				throw new ANTLRException("Internal parser error: Cannot change AST Node Type for Token ID '" + NodeType + "'");
			}
			if (NodeType > heteroList_.Length + 1)
			{
				setMaxNodeType(NodeType);
			}
			if (heteroList_[NodeType] == null)
			{
				heteroList_[NodeType] = new FactoryEntry(creator);
			}
			else
			{
				heteroList_[NodeType].Creator = creator;
			}
			typename2creator_[creator.ASTNodeTypeName] = creator;
		}

		public void setASTNodeCreator(ASTNodeCreator creator)
		{
			defaultCreator_ = creator;
		}

		public void setMaxNodeType(int NodeType)
		{
			if (heteroList_ == null)
			{
				heteroList_ = new FactoryEntry[NodeType + 1];
				return;
			}
			int num = heteroList_.Length;
			if (NodeType > num + 1)
			{
				FactoryEntry[] destinationArray = new FactoryEntry[NodeType + 1];
				Array.Copy(heteroList_, 0, destinationArray, 0, heteroList_.Length);
				heteroList_ = destinationArray;
			}
			else if (NodeType < num + 1)
			{
				FactoryEntry[] destinationArray = new FactoryEntry[NodeType + 1];
				Array.Copy(heteroList_, 0, destinationArray, 0, NodeType + 1);
				heteroList_ = destinationArray;
			}
		}

		public virtual void addASTChild(ASTPair currentAST, AST child)
		{
			if (child != null)
			{
				if (currentAST.root == null)
				{
					currentAST.root = child;
				}
				else if (currentAST.child == null)
				{
					currentAST.root.setFirstChild(child);
				}
				else
				{
					currentAST.child.setNextSibling(child);
				}
				currentAST.child = child;
				currentAST.advanceChildToEnd();
			}
		}

		public virtual AST create()
		{
			if (defaultCreator_ == null)
			{
				return createFromNodeTypeObject(defaultASTNodeTypeObject_);
			}
			return defaultCreator_.Create();
		}

		public virtual AST create(int type)
		{
			AST aST = createFromNodeType(type);
			aST.initialize(type, "");
			return aST;
		}

		public virtual AST create(int type, string txt)
		{
			AST aST = createFromNodeType(type);
			aST.initialize(type, txt);
			return aST;
		}

		public virtual AST create(int type, string txt, string ASTNodeTypeName)
		{
			AST aST = createFromNodeName(ASTNodeTypeName);
			aST.initialize(type, txt);
			return aST;
		}

		public virtual AST create(IToken tok, string ASTNodeTypeName)
		{
			AST aST = createFromNodeName(ASTNodeTypeName);
			aST.initialize(tok);
			return aST;
		}

		public virtual AST create(AST aNode)
		{
			AST aST;
			if (aNode == null)
			{
				aST = null;
			}
			else
			{
				aST = createFromAST(aNode);
				aST.initialize(aNode);
			}
			return aST;
		}

		public virtual AST create(IToken tok)
		{
			AST aST;
			if (tok == null)
			{
				aST = null;
			}
			else
			{
				aST = createFromNodeType(tok.Type);
				aST.initialize(tok);
			}
			return aST;
		}

		public virtual AST dup(AST t)
		{
			if (t == null)
			{
				return null;
			}
			AST aST = createFromAST(t);
			aST.initialize(t);
			return aST;
		}

		public virtual AST dupList(AST t)
		{
			AST aST = dupTree(t);
			AST aST2 = aST;
			while (t != null)
			{
				t = t.getNextSibling();
				aST2.setNextSibling(dupTree(t));
				aST2 = aST2.getNextSibling();
			}
			return aST;
		}

		public virtual AST dupTree(AST t)
		{
			AST aST = dup(t);
			if (t != null)
			{
				aST.setFirstChild(dupList(t.getFirstChild()));
			}
			return aST;
		}

		public virtual AST make(params AST[] nodes)
		{
			if (nodes == null || nodes.Length == 0)
			{
				return null;
			}
			AST aST = nodes[0];
			AST aST2 = null;
			aST?.setFirstChild(null);
			for (int i = 1; i < nodes.Length; i++)
			{
				if (nodes[i] != null)
				{
					if (aST == null)
					{
						aST = (aST2 = nodes[i]);
					}
					else if (aST2 == null)
					{
						aST.setFirstChild(nodes[i]);
						aST2 = aST.getFirstChild();
					}
					else
					{
						aST2.setNextSibling(nodes[i]);
						aST2 = aST2.getNextSibling();
					}
					while (aST2.getNextSibling() != null)
					{
						aST2 = aST2.getNextSibling();
					}
				}
			}
			return aST;
		}

		public virtual AST make(ASTArray nodes)
		{
			return make(nodes.array);
		}

		public virtual void makeASTRoot(ASTPair currentAST, AST root)
		{
			if (root != null)
			{
				root.addChild(currentAST.root);
				currentAST.child = currentAST.root;
				currentAST.advanceChildToEnd();
				currentAST.root = root;
			}
		}

		public virtual void setASTNodeType(string t)
		{
			if (defaultCreator_ != null && t != defaultCreator_.ASTNodeTypeName)
			{
				defaultCreator_ = null;
			}
			defaultASTNodeTypeObject_ = loadNodeTypeObject(t);
		}

		public virtual void error(string e)
		{
			Console.Error.WriteLine(e);
		}

		private static Type loadNodeTypeObject(string nodeTypeName)
		{
			Type type = null;
			bool flag = false;
			if (nodeTypeName != null)
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly assembly in assemblies)
				{
					try
					{
						type = assembly.GetType(nodeTypeName, throwOnError: false);
						if (type != null)
						{
							flag = true;
							goto IL_006a;
						}
					}
					catch
					{
						flag = false;
					}
				}
			}
			goto IL_006a;
			IL_006a:
			if (!flag)
			{
				throw new TypeLoadException("Unable to load AST Node Type: '" + nodeTypeName + "'");
			}
			return type;
		}

		private AST createFromAST(AST node)
		{
			AST aST = null;
			Type type = node.GetType();
			ASTNodeCreator aSTNodeCreator = (ASTNodeCreator)typename2creator_[type.FullName];
			if (aSTNodeCreator != null)
			{
				aST = aSTNodeCreator.Create();
				if (aST == null)
				{
					throw new ArgumentException("Unable to create AST Node Type: '" + type.FullName + "'");
				}
			}
			else
			{
				aST = createFromNodeTypeObject(type);
			}
			return aST;
		}

		private AST createFromNodeName(string nodeTypeName)
		{
			AST aST = null;
			ASTNodeCreator aSTNodeCreator = (ASTNodeCreator)typename2creator_[nodeTypeName];
			if (aSTNodeCreator != null)
			{
				aST = aSTNodeCreator.Create();
				if (aST == null)
				{
					throw new ArgumentException("Unable to create AST Node Type: '" + nodeTypeName + "'");
				}
			}
			else
			{
				aST = createFromNodeTypeObject(loadNodeTypeObject(nodeTypeName));
			}
			return aST;
		}

		private AST createFromNodeType(int nodeTypeIndex)
		{
			Debug.Assert(nodeTypeIndex >= 0 && nodeTypeIndex <= heteroList_.Length, "Invalid AST node type!");
			AST aST = null;
			FactoryEntry factoryEntry = heteroList_[nodeTypeIndex];
			if (factoryEntry != null && factoryEntry.Creator != null)
			{
				return factoryEntry.Creator.Create();
			}
			if (factoryEntry == null || factoryEntry.NodeTypeObject == null)
			{
				if (defaultCreator_ == null)
				{
					return createFromNodeTypeObject(defaultASTNodeTypeObject_);
				}
				return defaultCreator_.Create();
			}
			return createFromNodeTypeObject(factoryEntry.NodeTypeObject);
		}

		private AST createFromNodeTypeObject(Type nodeTypeObject)
		{
			AST aST = null;
			try
			{
				aST = (AST)Activator.CreateInstance(nodeTypeObject);
				if (aST == null)
				{
					throw new ArgumentException("Unable to create AST Node Type: '" + nodeTypeObject.FullName + "'");
				}
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("Unable to create AST Node Type: '" + nodeTypeObject.FullName + "'", innerException);
			}
			return aST;
		}
	}
}
