// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class RespawnPlatform : MonoBehaviour
{
	public GameObject DefaultContainer;

	public GameObject CustomContainer;

	private Transform attachToUI;

	private Camera attachToUICamera;

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

	public void Attach(Transform attachTo, Camera usingCamera)
	{
		this.attachToUI = attachTo;
		this.attachToUICamera = usingCamera;
		this.updatePosition();
	}

	private void Update()
	{
		this.updatePosition();
	}

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
}
