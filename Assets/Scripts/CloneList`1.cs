// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class CloneList<T> : List<T>
{
	public CloneList()
	{
	}

	public CloneList(List<T> other) : base(other)
	{
	}

	public object ShallowClone()
	{
		return base.MemberwiseClone();
	}
}
