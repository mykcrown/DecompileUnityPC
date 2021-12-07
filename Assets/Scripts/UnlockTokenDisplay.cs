// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class UnlockTokenDisplay : BaseItem3DPreviewDisplay
{
	public Transform visualsRoot;

	public float maxSpinRate = 720f;

	public float minSpinRate = 45f;

	public float spinDownTime = 1f;

	public float spinDownDelay = 4f;

	private float spinRate;

	private float spinRateVelocity;

	private float delay;

	private void Awake()
	{
		this.delay = this.spinDownDelay;
		this.Preview();
	}

	protected override void Update()
	{
		base.Update();
		if (this.delay > 0f)
		{
			this.delay -= Time.deltaTime;
			return;
		}
		this.spinRate = Mathf.SmoothDamp(this.spinRate, this.minSpinRate, ref this.spinRateVelocity, this.spinDownTime);
		if (this.visualsRoot != null)
		{
			this.visualsRoot.localRotation *= Quaternion.AngleAxis(this.spinRate * Time.deltaTime, Vector3.up);
		}
	}

	public void Preview()
	{
		this.spinRate = this.maxSpinRate;
	}
}
