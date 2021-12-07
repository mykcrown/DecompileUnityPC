// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class VFXAutoRotate : VFXBehavior
{
	private Vector3 applyRotation = default(Vector3);

	private Vector3 initialRotation = default(Vector3);

	public bool rotationMirror;

	public bool useScaleInvertForMirror;

	public bool lookAtCam = true;

	public Vector3 ApplyRotation
	{
		get
		{
			return this.applyRotation;
		}
		set
		{
			this.applyRotation = value;
			this.updateRotation();
		}
	}

	public override void OnVFXInit()
	{
	}

	private void Awake()
	{
		this.initialRotation = base.transform.localRotation.eulerAngles;
	}

	public void Reset()
	{
		this.applyRotation = default(Vector3);
	}

	public override void OnVFXStart()
	{
		this.updateRotation();
	}

	private void Update()
	{
		if (this.lookAtCam)
		{
			this.updateRotation();
		}
	}

	private void updateRotation()
	{
		Vector3 vector;
		if (this.lookAtCam)
		{
			Vector3 up = base.transform.up;
			base.transform.LookAt(2f * base.transform.position - Camera.main.transform.position, up);
			vector = base.transform.localRotation.eulerAngles;
		}
		else
		{
			vector = this.initialRotation;
		}
		if (this.applyRotation != Vector3.zero)
		{
			vector += this.applyRotation;
		}
		base.transform.localRotation = Quaternion.Euler(vector);
	}
}
