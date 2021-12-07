// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct BodyPartComparer : IEqualityComparer<BodyPart>
{
	public bool Equals(BodyPart x, BodyPart y)
	{
		return x == y;
	}

	public int GetHashCode(BodyPart obj)
	{
		return (int)obj;
	}
}
