// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class ECBOverrideData : ICloneable
{
	public int startFrame;

	public int endFrame;

	public BodyPart[] boneList = new BodyPart[]
	{
		BodyPart.leftUpperArm,
		BodyPart.rightUpperArm,
		BodyPart.leftCalf,
		BodyPart.rightCalf
	};

	public bool addHeadToVerticalOnly = true;

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
