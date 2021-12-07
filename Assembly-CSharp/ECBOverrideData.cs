using System;

// Token: 0x02000503 RID: 1283
[Serializable]
public class ECBOverrideData : ICloneable
{
	// Token: 0x06001BDF RID: 7135 RVA: 0x0008D1AA File Offset: 0x0008B5AA
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x0400163D RID: 5693
	public int startFrame;

	// Token: 0x0400163E RID: 5694
	public int endFrame;

	// Token: 0x0400163F RID: 5695
	public BodyPart[] boneList = new BodyPart[]
	{
		BodyPart.leftUpperArm,
		BodyPart.rightUpperArm,
		BodyPart.leftCalf,
		BodyPart.rightCalf
	};

	// Token: 0x04001640 RID: 5696
	public bool addHeadToVerticalOnly = true;
}
