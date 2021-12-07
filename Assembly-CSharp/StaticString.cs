using System;

// Token: 0x02000B63 RID: 2915
public struct StaticString
{
	// Token: 0x0600545E RID: 21598 RVA: 0x001B1DB8 File Offset: 0x001B01B8
	public StaticString(int capacity)
	{
		this.arr = new char[capacity];
		this.len = 0;
		this.writeIndex = 0;
	}

	// Token: 0x0600545F RID: 21599 RVA: 0x001B1DD4 File Offset: 0x001B01D4
	public void Reset()
	{
		this.writeIndex = 0;
		this.len = 0;
	}

	// Token: 0x06005460 RID: 21600 RVA: 0x001B1DE4 File Offset: 0x001B01E4
	public void Append(char value)
	{
		this.arr[this.writeIndex] = value;
		this.writeIndex++;
		this.len = this.writeIndex;
	}

	// Token: 0x06005461 RID: 21601 RVA: 0x001B1E10 File Offset: 0x001B0210
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

	// Token: 0x06005462 RID: 21602 RVA: 0x001B1E69 File Offset: 0x001B0269
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

	// Token: 0x04003569 RID: 13673
	public char[] arr;

	// Token: 0x0400356A RID: 13674
	public int len;

	// Token: 0x0400356B RID: 13675
	public int writeIndex;
}
