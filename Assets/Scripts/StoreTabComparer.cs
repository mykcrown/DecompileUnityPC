// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct StoreTabComparer : IEqualityComparer<StoreTab>
{
	public bool Equals(StoreTab x, StoreTab y)
	{
		return x == y;
	}

	public int GetHashCode(StoreTab obj)
	{
		return (int)obj;
	}
}
