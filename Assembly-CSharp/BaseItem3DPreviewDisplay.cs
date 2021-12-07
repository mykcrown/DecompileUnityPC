using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020006D6 RID: 1750
public class BaseItem3DPreviewDisplay : MonoBehaviour
{
	// Token: 0x17000AC9 RID: 2761
	// (get) Token: 0x06002BF0 RID: 11248 RVA: 0x000E49B0 File Offset: 0x000E2DB0
	// (set) Token: 0x06002BF1 RID: 11249 RVA: 0x000E49B8 File Offset: 0x000E2DB8
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06002BF2 RID: 11250 RVA: 0x000E49C4 File Offset: 0x000E2DC4
	// (remove) Token: 0x06002BF3 RID: 11251 RVA: 0x000E49FC File Offset: 0x000E2DFC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action onClick;

	// Token: 0x06002BF4 RID: 11252 RVA: 0x000E4A32 File Offset: 0x000E2E32
	public virtual void Attach(Transform attachTo, Camera usingCamera)
	{
		this.attachToUI = attachTo;
		this.attachToUICamera = usingCamera;
		this.updatePosition();
	}

	// Token: 0x06002BF5 RID: 11253 RVA: 0x000E4A48 File Offset: 0x000E2E48
	protected virtual void Update()
	{
		this.updatePosition();
	}

	// Token: 0x06002BF6 RID: 11254 RVA: 0x000E4A50 File Offset: 0x000E2E50
	protected virtual void updatePosition()
	{
		if (this.attachToUI != null)
		{
			Vector3 position = this.attachToUI.position;
			position.z = Mathf.Abs(this.attachToUICamera.transform.position.z - base.transform.position.z);
			Vector3 position2 = this.attachToUICamera.ScreenToWorldPoint(position);
			position2.z = base.transform.position.z;
			base.transform.position = position2;
		}
	}

	// Token: 0x06002BF7 RID: 11255 RVA: 0x000E4AE6 File Offset: 0x000E2EE6
	public void OnClick()
	{
		if (this.onClick != null)
		{
			this.onClick();
		}
	}

	// Token: 0x04001F53 RID: 8019
	protected Transform attachToUI;

	// Token: 0x04001F54 RID: 8020
	protected Camera attachToUICamera;
}
