using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler.Steps
{
	public class AstAnnotations
	{
		public const string RawArrayIndexing = "rawarrayindexing";

		public const string Checked = "checked";

		private static object TryBlockDepthKey = new object();

		public static void MarkChecked(Node node)
		{
			node["checked"] = true;
		}

		public static void MarkUnchecked(Node node)
		{
			node["checked"] = false;
		}

		public static bool IsChecked(Node node, bool defaultValue)
		{
			return GetBoolAnnotationValue(node, "checked", defaultValue);
		}

		public static bool IsRawIndexing(Node node)
		{
			return GetBoolAnnotationValue(node, "rawarrayindexing", defaultValue: false);
		}

		public static bool GetBoolAnnotationValue(Node node, string annotation, bool defaultValue)
		{
			object obj = node[annotation];
			return (obj is bool) ? ((bool)obj) : defaultValue;
		}

		public static void MarkRawArrayIndexing(Node node)
		{
			node["rawarrayindexing"] = true;
		}

		public static void SetTryBlockDepth(Node node, int depth)
		{
			node[TryBlockDepthKey] = depth;
		}

		public static int GetTryBlockDepth(Node node)
		{
			return (int)node[TryBlockDepthKey];
		}
	}
}
