// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class CameraShakeData
{
	public int minFrames;

	public int extraFrames;

	public float amplitude = 3f;

	public float wavelength = 1f;

	public float shakeRandomizer = 0.75f;

	public float lateralMotion = 0.1f;

	public bool scale;

	public bool softenScale;

	public bool useOverrideAngle;

	public float overrideAngle;

	public float maxAmplitude;
}
