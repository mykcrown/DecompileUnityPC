// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct CharacterIDComparer : IEqualityComparer<CharacterID>
{
	public bool Equals(CharacterID x, CharacterID y)
	{
		return x == y;
	}

	public int GetHashCode(CharacterID obj)
	{
		return (int)obj;
	}
}
