// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct TeamNumComparer : IEqualityComparer<TeamNum>
{
	public bool Equals(TeamNum x, TeamNum y)
	{
		return x == y;
	}

	public int GetHashCode(TeamNum obj)
	{
		return (int)obj;
	}
}
