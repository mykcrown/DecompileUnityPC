// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class LedgeGrabEnableData : ICloneable
{
	public int startFrame;

	public int endFrame;

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
