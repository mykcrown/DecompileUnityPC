// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct DebugDrawChannelComparer : IEqualityComparer<DebugDrawChannel>
{
	public bool Equals(DebugDrawChannel x, DebugDrawChannel y)
	{
		return x == y;
	}

	public int GetHashCode(DebugDrawChannel obj)
	{
		return (int)obj;
	}
}
