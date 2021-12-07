// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class SelfHitData
{
	public KnockbackType knockbackType = KnockbackType.AwayUp;

	public float damage;

	public float baseKnockback = 20f;

	public bool interruptMove;

	public bool applyHitStun;

	public int angleGranularity = 360;
}
