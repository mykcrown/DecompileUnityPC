// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct FeatureIDComparer : IEqualityComparer<FeatureID>
{
	public bool Equals(FeatureID x, FeatureID y)
	{
		return x == y;
	}

	public int GetHashCode(FeatureID obj)
	{
		return (int)obj;
	}
}
