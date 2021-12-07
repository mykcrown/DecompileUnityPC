// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct PlayerNumComparer : IEqualityComparer<PlayerNum>
{
	public bool Equals(PlayerNum x, PlayerNum y)
	{
		return x == y;
	}

	public int GetHashCode(PlayerNum obj)
	{
		return (int)obj;
	}
}
