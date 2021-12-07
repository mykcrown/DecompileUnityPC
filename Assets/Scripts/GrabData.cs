// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class GrabData : CloneableObject, ICopyable<GrabData>, ICopyable
{
	public PlayerNum grabbedOpponent = PlayerNum.None;

	public PlayerNum grabbingOpponent = PlayerNum.None;

	public int grabbedStartFrame;

	public int grabDurationFrames;

	public GrabType grabType;

	public BodyPart grabbedBodyPart;

	public string grabbedWithMoveName;

	public Vector3F initialGrabDisplacement = default(Vector3F);

	public bool ignoreThrowBoneRotation;

	public bool victimUnderChainGrabPrevention;

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

	public override object Clone()
	{
		GrabData grabData = new GrabData();
		this.CopyTo(grabData);
		return grabData;
	}

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
}
