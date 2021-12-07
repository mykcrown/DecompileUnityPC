// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class LedgeGrabBox
{
	public FixedRect rect = new FixedRect(-1, 3, 1, (Fixed)1.5);

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
