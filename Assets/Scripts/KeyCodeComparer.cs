// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct KeyCodeComparer : IEqualityComparer<KeyCode>
{
	public bool Equals(KeyCode x, KeyCode y)
	{
		return x == y;
	}

	public int GetHashCode(KeyCode obj)
	{
		return (int)obj;
	}
}
