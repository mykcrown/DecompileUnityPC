// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine.Serialization;

[Serializable]
public class IntangibleBodyParts : ICloneable
{
	public BodyPart[] bodyParts;

	[FormerlySerializedAs("completelyInvincible")]
	public bool completelyIntangible = true;

	[FormerlySerializedAs("activeFramesBegin")]
	public int startFrame;

	[FormerlySerializedAs("activeFramesEnds")]
	public int endFrame;

	public object Clone()
	{
		return CloneUtil.SlowDeepClone<IntangibleBodyParts>(this);
	}
}
