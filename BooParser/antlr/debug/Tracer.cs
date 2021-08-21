using System;

namespace antlr.debug
{
	public class Tracer : TraceListenerBase, TraceListener, Listener
	{
		protected string indentString = "";

		protected internal virtual void dedent()
		{
			if (indentString.Length < 2)
			{
				indentString = "";
			}
			else
			{
				indentString = indentString.Substring(2);
			}
		}

		public override void enterRule(object source, TraceEventArgs e)
		{
			Console.Out.WriteLine(indentString + e);
			indent();
		}

		public override void exitRule(object source, TraceEventArgs e)
		{
			dedent();
			Console.Out.WriteLine(indentString + e);
		}

		protected internal virtual void indent()
		{
			indentString += "  ";
		}
	}
}
