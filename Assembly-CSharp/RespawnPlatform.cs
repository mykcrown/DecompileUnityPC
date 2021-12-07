using System;
using UnityEngine;

// Token: 0x02000612 RID: 1554
public class RespawnPlatform : MonoBehaviour
{
	// Token: 0x0600264E RID: 9806 RVA: 0x000BC888 File Offset: 0x000BAC88
	public void AttachCustom(GameObject obj)
	{
		while (this.CustomContainer.transform.childCount > 0)
		{
			Transform child = this.CustomContainer.transform.GetChild(0);
			child.gameObject.SetActive(false);
			child.SetParent(null, false);
		}
		if (obj == null)
		{
			this.DefaultContainer.SetActive(true);
			this.CustomContainer.SetActive(false);
		}
		else
		{
			this.DefaultContainer.SetActive(false);
			this.CustomContainer.SetActive(true);
			obj.SetActive(true);
			obj.transform.SetParent(this.CustomContainer.transform, false);
		}
	}

	// Token: 0x0600264F RID: 9807 RVA: 0x000BC935 File Offset: 0x000BAD35
	public void Attach(Transform attachTo, Camera usingCamera)
	{
		this.attachToUI = attachTo;
		this.attachToUICamera = usingCamera;
		this.updatePosition();
	}

	// Token: 0x06002650 RID: 9808 RVA: 0x000BC94B File Offset: 0x000BAD4B
	private void Update()
	{
		this.updatePosition();
	}

	// Token: 0x06002651 RID: 9809 RVA: 0x000BC954 File Offset: 0x000BAD54
	private void updatePosition()
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

	// Token: 0x04001C0B RID: 7179
	public GameObject DefaultContainer;

	// Token: 0x04001C0C RID: 7180
	public GameObject CustomContainer;

	// Token: 0x04001C0D RID: 7181
	private Transform attachToUI;

	// Token: 0x04001C0E RID: 7182
	private Camera attachToUICamera;
}
