// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class GlobalCameraShakeData
{
	public HitCameraShakeMethod hitShakeMethod;

	public int hitShakeDamageMin;

	public int hitShakeDamageMax;

	public int hitShakeDamageThreshold;

	public int hitShakeKnockbackThreshold;

	public int hitShakeKnockbackMin = -1;

	public int hitShakeKnockbackMax;

	public bool debug;

	public CameraShakeData deathShake;

	public CameraShakeData hitShake;

	public CameraShakeData downedShake;
}
