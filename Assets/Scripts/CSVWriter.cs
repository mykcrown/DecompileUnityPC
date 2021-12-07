// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class CSVWriter
{
	private bool stream;

	private StreamWriter writer;

	private bool autoFlush;

	private List<string> unwrittenLines = new List<string>();

	public CSVWriter(string filePath, bool stream)
	{
		this.stream = stream;
		this.writer = new StreamWriter(filePath, true);
		this.writer.AutoFlush = stream;
	}

	public void Close()
	{
		if (!this.stream)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in this.unwrittenLines)
			{
				stringBuilder.AppendLine(current);
			}
			this.unwrittenLines.Clear();
			this.writer.Write(stringBuilder.ToString());
		}
		this.writer.Close();
	}

	private string toString(IEnumerable line)
	{
		StringBuilder stringBuilder = new StringBuilder();
		IEnumerator enumerator = line.GetEnumerator();
		while (enumerator != null && enumerator.Current != null)
		{
			object current = enumerator.Current;
			stringBuilder.Append(current.ToString());
			if (enumerator.MoveNext())
			{
				stringBuilder.Append(",");
			}
		}
		return stringBuilder.ToString();
	}

	public void AppendLine(string commaSeparatedString)
	{
		this.writer.WriteLine(commaSeparatedString);
	}

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
}
