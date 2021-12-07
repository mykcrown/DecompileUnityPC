// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct StatTypeComparer : IEqualityComparer<StatType>
{
	public bool Equals(StatType x, StatType y)
	{
		return x == y;
	}

	public int GetHashCode(StatType obj)
	{
		return (int)obj;
	}
}
