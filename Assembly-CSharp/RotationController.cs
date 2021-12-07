using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020005FF RID: 1535
public class RotationController : GameBehavior, IRotationController, IRollbackStateOwner
{
	// Token: 0x1700092A RID: 2346
	// (get) Token: 0x0600257E RID: 9598 RVA: 0x000B9BB2 File Offset: 0x000B7FB2
	// (set) Token: 0x0600257F RID: 9599 RVA: 0x000B9BBA File Offset: 0x000B7FBA
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x06002580 RID: 9600 RVA: 0x000B9BC3 File Offset: 0x000B7FC3
	public override void Awake()
	{
		base.Awake();
		this.model = new RotationState();
	}

	// Token: 0x17000928 RID: 2344
	// (get) Token: 0x06002581 RID: 9601 RVA: 0x000B9BD6 File Offset: 0x000B7FD6
	Vector3F IRotationController.EulerAngles
	{
		get
		{
			return this.model.eulerAngles;
		}
	}

	// Token: 0x17000929 RID: 2345
	// (get) Token: 0x06002582 RID: 9602 RVA: 0x000B9BE3 File Offset: 0x000B7FE3
	QuaternionF IRotationController.Rotation
	{
		get
		{
			return this.model.quaternion;
		}
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x000B9BF0 File Offset: 0x000B7FF0
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

	// Token: 0x06002584 RID: 9604 RVA: 0x000B9D01 File Offset: 0x000B8101
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<RotationState>(this.model));
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x000B9D1B File Offset: 0x000B811B
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<RotationState>(ref this.model);
		base.transform.rotation = (Quaternion)this.model.quaternion;
		return true;
	}

	// Token: 0x04001BAB RID: 7083
	private RotationState model;
}
