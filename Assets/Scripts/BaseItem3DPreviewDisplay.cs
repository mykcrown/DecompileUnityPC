// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Threading;
using UnityEngine;

public class BaseItem3DPreviewDisplay : MonoBehaviour
{
	protected Transform attachToUI;

	protected Camera attachToUICamera;

	public event Action onClick;

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	public virtual void Attach(Transform attachTo, Camera usingCamera)
	{
		this.attachToUI = attachTo;
		this.attachToUICamera = usingCamera;
		this.updatePosition();
	}

	protected virtual void Update()
	{
		this.updatePosition();
	}

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

	public void OnClick()
	{
		if (this.onClick != null)
		{
			this.onClick();
		}
	}
}
