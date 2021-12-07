// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct MoveLabelComparer : IEqualityComparer<MoveLabel>
{
	public bool Equals(MoveLabel x, MoveLabel y)
	{
		return x == y;
	}

	public int GetHashCode(MoveLabel obj)
	{
		return (int)obj;
	}
}
