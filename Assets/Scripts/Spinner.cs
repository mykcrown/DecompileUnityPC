// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class Spinner : VFXBehavior
{
	public Vector3 SpinAxis;

	public AnimationCurve SpeedTimeCurve;

	public float Speed;

	public Space RotateSpace;

	private float mTime;

	public override void OnVFXInit()
	{
	}

	public override void OnVFXStart()
	{
		this.mTime = 0f;
	}

	private void Update()
	{
		this.mTime += WTime.deltaTime;
		float num = this.Speed * WTime.deltaTime;
		if (this.SpeedTimeCurve.keys.Length != 0)
		{
			num *= this.SpeedTimeCurve.Evaluate(this.mTime);
		}
		base.transform.Rotate(this.SpinAxis, num, this.RotateSpace);
	}
}
