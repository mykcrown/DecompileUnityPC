using System;
using FixedPoint;

// Token: 0x020005D1 RID: 1489
[Serializable]
public class GrabData : CloneableObject, ICopyable<GrabData>, ICopyable
{
	// Token: 0x0600216C RID: 8556 RVA: 0x000A6FEC File Offset: 0x000A53EC
	public void CopyTo(GrabData target)
	{
		target.grabbedOpponent = this.grabbedOpponent;
		target.grabbingOpponent = this.grabbingOpponent;
		target.grabbedStartFrame = this.grabbedStartFrame;
		target.grabDurationFrames = this.grabDurationFrames;
		target.grabType = this.grabType;
		target.grabbedBodyPart = this.grabbedBodyPart;
		target.grabbedWithMoveName = this.grabbedWithMoveName;
		target.initialGrabDisplacement = this.initialGrabDisplacement;
		target.ignoreThrowBoneRotation = this.ignoreThrowBoneRotation;
		target.victimUnderChainGrabPrevention = this.victimUnderChainGrabPrevention;
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x000A7074 File Offset: 0x000A5474
	public override object Clone()
	{
		GrabData grabData = new GrabData();
		this.CopyTo(grabData);
		return grabData;
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x000A7090 File Offset: 0x000A5490
	public void Clear()
	{
		this.grabbedOpponent = PlayerNum.None;
		this.grabbedStartFrame = 0;
		this.grabDurationFrames = 0;
		this.grabType = GrabType.None;
		this.grabbedWithMoveName = null;
		this.initialGrabDisplacement = default(Vector3F);
		this.ignoreThrowBoneRotation = false;
		this.victimUnderChainGrabPrevention = false;
	}

	// Token: 0x04001A49 RID: 6729
	public PlayerNum grabbedOpponent = PlayerNum.None;

	// Token: 0x04001A4A RID: 6730
	public PlayerNum grabbingOpponent = PlayerNum.None;

	// Token: 0x04001A4B RID: 6731
	public int grabbedStartFrame;

	// Token: 0x04001A4C RID: 6732
	public int grabDurationFrames;

	// Token: 0x04001A4D RID: 6733
	public GrabType grabType;

	// Token: 0x04001A4E RID: 6734
	public BodyPart grabbedBodyPart;

	// Token: 0x04001A4F RID: 6735
	public string grabbedWithMoveName;

	// Token: 0x04001A50 RID: 6736
	public Vector3F initialGrabDisplacement = default(Vector3F);

	// Token: 0x04001A51 RID: 6737
	public bool ignoreThrowBoneRotation;

	// Token: 0x04001A52 RID: 6738
	public bool victimUnderChainGrabPrevention;
}
