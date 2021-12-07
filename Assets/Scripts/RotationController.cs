// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class RotationController : GameBehavior, IRotationController, IRollbackStateOwner
{
	private RotationState model;

	Vector3F IRotationController.EulerAngles
	{
		get
		{
			return this.model.eulerAngles;
		}
	}

	QuaternionF IRotationController.Rotation
	{
		get
		{
			return this.model.quaternion;
		}
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public override void Awake()
	{
		base.Awake();
		this.model = new RotationState();
	}

	public void Rotate(Vector3F rotation)
	{
		if (this.model.eulerAngles.y == rotation.y && this.model.eulerAngles.x == rotation.x && this.model.eulerAngles.z == rotation.z)
		{
			return;
		}
		this.model.eulerAngles.x = rotation.x;
		this.model.eulerAngles.y = rotation.y;
		this.model.eulerAngles.z = rotation.z;
		this.model.quaternion = QuaternionF.Euler(this.model.eulerAngles.x, this.model.eulerAngles.y, this.model.eulerAngles.z);
		base.transform.rotation = (Quaternion)this.model.quaternion;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<RotationState>(this.model));
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<RotationState>(ref this.model);
		base.transform.rotation = (Quaternion)this.model.quaternion;
		return true;
	}
}
