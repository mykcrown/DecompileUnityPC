using System;
using UnityEngine;

// Token: 0x02000B86 RID: 2950
public class VFXAutoRotate : VFXBehavior
{
	// Token: 0x0600552B RID: 21803 RVA: 0x001B4B60 File Offset: 0x001B2F60
	public override void OnVFXInit()
	{
	}

	// Token: 0x0600552C RID: 21804 RVA: 0x001B4B64 File Offset: 0x001B2F64
	private void Awake()
	{
		this.initialRotation = base.transform.localRotation.eulerAngles;
	}

	// Token: 0x0600552D RID: 21805 RVA: 0x001B4B8C File Offset: 0x001B2F8C
	public void Reset()
	{
		this.applyRotation = default(Vector3);
	}

	// Token: 0x0600552E RID: 21806 RVA: 0x001B4BA8 File Offset: 0x001B2FA8
	public override void OnVFXStart()
	{
		this.updateRotation();
	}

	// Token: 0x170013A4 RID: 5028
	// (get) Token: 0x0600552F RID: 21807 RVA: 0x001B4BB0 File Offset: 0x001B2FB0
	// (set) Token: 0x06005530 RID: 21808 RVA: 0x001B4BB8 File Offset: 0x001B2FB8
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

	// Token: 0x06005531 RID: 21809 RVA: 0x001B4BC7 File Offset: 0x001B2FC7
	private void Update()
	{
		if (this.lookAtCam)
		{
			this.updateRotation();
		}
	}

	// Token: 0x06005532 RID: 21810 RVA: 0x001B4BDC File Offset: 0x001B2FDC
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

	// Token: 0x04003621 RID: 13857
	private Vector3 applyRotation = default(Vector3);

	// Token: 0x04003622 RID: 13858
	private Vector3 initialRotation = default(Vector3);

	// Token: 0x04003623 RID: 13859
	public bool rotationMirror;

	// Token: 0x04003624 RID: 13860
	public bool useScaleInvertForMirror;

	// Token: 0x04003625 RID: 13861
	public bool lookAtCam = true;
}
