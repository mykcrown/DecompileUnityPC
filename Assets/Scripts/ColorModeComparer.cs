// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ColorModeComparer : IEqualityComparer<ColorMode>
{
	public bool Equals(ColorMode x, ColorMode y)
	{
		return x == y;
	}

	public int GetHashCode(ColorMode obj)
	{
		return (int)obj;
	}
}
