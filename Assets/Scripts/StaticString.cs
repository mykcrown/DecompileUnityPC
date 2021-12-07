// Decompile from assembly: Assembly-CSharp.dll

using System;

public struct StaticString
{
	public char[] arr;

	public int len;

	public int writeIndex;

	public StaticString(int capacity)
	{
		this.arr = new char[capacity];
		this.len = 0;
		this.writeIndex = 0;
	}

	public void Reset()
	{
		this.writeIndex = 0;
		this.len = 0;
	}

	public void Append(char value)
	{
		this.arr[this.writeIndex] = value;
		this.writeIndex++;
		this.len = this.writeIndex;
	}

	public void Append(string value)
	{
		int length = value.Length;
		for (int i = 0; i < value.Length; i++)
		{
			this.arr[this.writeIndex] = value[i];
			this.writeIndex++;
		}
		this.len = this.writeIndex;
	}

	public void AppendInt0to999(int value)
	{
		if (value >= 0)
		{
			this.Append(StringUtil.intToString[value]);
		}
		else
		{
			this.Append('-');
			this.Append(StringUtil.intToString[-value]);
		}
	}
}
