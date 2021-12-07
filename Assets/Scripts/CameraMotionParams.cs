// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class CameraMotionParams
{
	public CameraMotionFunction function;

	public float baseSpeed;

	public bool editorToggle;

	public bool quadraticScaling;

	public float maxSpeed;

	public float quadraticScalingDistance;

	public float lazySpeed;

	public bool debugSpeed;

	public bool syncToDolly;

	public CameraMotionParams(float baseSpeed)
	{
		this.baseSpeed = baseSpeed;
	}
}
