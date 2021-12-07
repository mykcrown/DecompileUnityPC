// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ActionStateComparer : IEqualityComparer<ActionState>
{
	public bool Equals(ActionState x, ActionState y)
	{
		return x == y;
	}

	public int GetHashCode(ActionState obj)
	{
		return (int)obj;
	}
}
