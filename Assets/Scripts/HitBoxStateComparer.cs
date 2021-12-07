// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct HitBoxStateComparer : IEqualityComparer<HitBoxState>
{
	public bool Equals(HitBoxState x, HitBoxState y)
	{
		return x == y;
	}

	public int GetHashCode(HitBoxState obj)
	{
		return obj.GetHashCode();
	}
}
