// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct TauntSlotComparer : IEqualityComparer<TauntSlot>
{
	public bool Equals(TauntSlot x, TauntSlot y)
	{
		return x == y;
	}

	public int GetHashCode(TauntSlot obj)
	{
		return (int)obj;
	}
}
