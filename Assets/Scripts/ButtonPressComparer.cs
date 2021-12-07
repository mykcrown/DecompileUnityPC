// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct ButtonPressComparer : IEqualityComparer<ButtonPress>
{
	public bool Equals(ButtonPress x, ButtonPress y)
	{
		return x == y;
	}

	public int GetHashCode(ButtonPress obj)
	{
		return (int)obj;
	}
}
