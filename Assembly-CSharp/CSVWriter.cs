using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Token: 0x02000B5E RID: 2910
public class CSVWriter
{
	// Token: 0x06005441 RID: 21569 RVA: 0x001B1859 File Offset: 0x001AFC59
	public CSVWriter(string filePath, bool stream)
	{
		this.stream = stream;
		this.writer = new StreamWriter(filePath, true);
		this.writer.AutoFlush = stream;
	}

	// Token: 0x06005442 RID: 21570 RVA: 0x001B188C File Offset: 0x001AFC8C
	public void Close()
	{
		if (!this.stream)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in this.unwrittenLines)
			{
				stringBuilder.AppendLine(value);
			}
			this.unwrittenLines.Clear();
			this.writer.Write(stringBuilder.ToString());
		}
		this.writer.Close();
	}

	// Token: 0x06005443 RID: 21571 RVA: 0x001B1924 File Offset: 0x001AFD24
	private string toString(IEnumerable line)
	{
		StringBuilder stringBuilder = new StringBuilder();
		IEnumerator enumerator = line.GetEnumerator();
		while (enumerator != null && enumerator.Current != null)
		{
			object obj = enumerator.Current;
			stringBuilder.Append(obj.ToString());
			if (enumerator.MoveNext())
			{
				stringBuilder.Append(",");
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06005444 RID: 21572 RVA: 0x001B198A File Offset: 0x001AFD8A
	public void AppendLine(string commaSeparatedString)
	{
		this.writer.WriteLine(commaSeparatedString);
	}

	// Token: 0x06005445 RID: 21573 RVA: 0x001B1998 File Offset: 0x001AFD98
	public void AppendLine(IEnumerable line)
	{
		string text = this.toString(line);
		if (this.stream)
		{
			this.writer.WriteLine(text);
		}
		else
		{
			this.unwrittenLines.Add(text);
		}
	}

	// Token: 0x0400355F RID: 13663
	private bool stream;

	// Token: 0x04003560 RID: 13664
	private StreamWriter writer;

	// Token: 0x04003561 RID: 13665
	private bool autoFlush;

	// Token: 0x04003562 RID: 13666
	private List<string> unwrittenLines = new List<string>();
}
