// Decompile from assembly: Assembly-CSharp.dll

using System;

public class Pair<T1, T2>
{
	public T1 First
	{
		get;
		private set;
	}

	public T2 Second
	{
		get;
		private set;
	}

	internal Pair(T1 first, T2 second)
	{
		this.First = first;
		this.Second = second;
	}
}
