using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace InControl.Internal
{
	// Token: 0x020001D2 RID: 466
	internal class CodeWriter
	{
		// Token: 0x06000814 RID: 2068 RVA: 0x0004A9D3 File Offset: 0x00048DD3
		public CodeWriter()
		{
			this.indent = 0;
			this.stringBuilder = new StringBuilder(4096);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0004A9F2 File Offset: 0x00048DF2
		public void IncreaseIndent()
		{
			this.indent++;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0004AA02 File Offset: 0x00048E02
		public void DecreaseIndent()
		{
			this.indent--;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0004AA12 File Offset: 0x00048E12
		public void Append(string code)
		{
			this.Append(false, code);
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0004AA1C File Offset: 0x00048E1C
		public void Append(bool trim, string code)
		{
			if (trim)
			{
				code = code.Trim();
			}
			string[] array = Regex.Split(code, "\\r?\\n|\\n");
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				string text = array[i];
				IEnumerable<char> source = text;
				if (CodeWriter.f__mg_cache0 == null)
				{
					CodeWriter.f__mg_cache0 = new Func<char, bool>(char.IsWhiteSpace);
				}
				if (!source.All(CodeWriter.f__mg_cache0))
				{
					this.stringBuilder.Append('\t', this.indent);
					this.stringBuilder.Append(text);
				}
				if (i < num - 1)
				{
					this.stringBuilder.Append('\n');
				}
			}
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0004AABD File Offset: 0x00048EBD
		public void AppendLine(string code)
		{
			this.Append(code);
			this.stringBuilder.Append('\n');
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0004AAD4 File Offset: 0x00048ED4
		public void AppendLine(int count)
		{
			this.stringBuilder.Append('\n', count);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0004AAE5 File Offset: 0x00048EE5
		public void AppendFormat(string format, params object[] args)
		{
			this.Append(string.Format(format, args));
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0004AAF4 File Offset: 0x00048EF4
		public void AppendLineFormat(string format, params object[] args)
		{
			this.AppendLine(string.Format(format, args));
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0004AB03 File Offset: 0x00048F03
		public override string ToString()
		{
			return this.stringBuilder.ToString();
		}

		// Token: 0x040005D8 RID: 1496
		private const char NewLine = '\n';

		// Token: 0x040005D9 RID: 1497
		private int indent;

		// Token: 0x040005DA RID: 1498
		private StringBuilder stringBuilder;

		// Token: 0x040005DB RID: 1499
		[CompilerGenerated]
		private static Func<char, bool> f__mg_cache0;
	}
}
