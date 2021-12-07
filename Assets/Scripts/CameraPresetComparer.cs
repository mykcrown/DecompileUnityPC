// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class CameraPresetComparer : IEqualityComparer<CameraPreset>
{
	public bool Equals(CameraPreset x, CameraPreset y)
	{
		return x == y;
	}

	public int GetHashCode(CameraPreset obj)
	{
		return (int)obj;
	}
}
