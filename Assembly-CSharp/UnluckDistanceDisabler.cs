using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class UnluckDistanceDisabler : MonoBehaviour
{
	// Token: 0x06000005 RID: 5 RVA: 0x00002228 File Offset: 0x00000628
	public void Start()
	{
		if (this._distanceFromMainCam)
		{
			this._distanceFrom = Camera.main.transform;
		}
		base.InvokeRepeating("CheckDisable", this._disableCheckInterval + UnityEngine.Random.value * this._disableCheckInterval, this._disableCheckInterval);
		base.InvokeRepeating("CheckEnable", this._enableCheckInterval + UnityEngine.Random.value * this._enableCheckInterval, this._enableCheckInterval);
		base.Invoke("DisableOnStart", 0.01f);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000022A8 File Offset: 0x000006A8
	public void DisableOnStart()
	{
		if (this._disableOnStart)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000022C4 File Offset: 0x000006C4
	public void CheckDisable()
	{
		if (base.gameObject.activeInHierarchy && (base.transform.position - this._distanceFrom.position).sqrMagnitude > (float)(this._distanceDisable * this._distanceDisable))
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002324 File Offset: 0x00000724
	public void CheckEnable()
	{
		if (!base.gameObject.activeInHierarchy && (base.transform.position - this._distanceFrom.position).sqrMagnitude < (float)(this._distanceDisable * this._distanceDisable))
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x04000003 RID: 3
	public int _distanceDisable = 1000;

	// Token: 0x04000004 RID: 4
	public Transform _distanceFrom;

	// Token: 0x04000005 RID: 5
	public bool _distanceFromMainCam;

	// Token: 0x04000006 RID: 6
	public float _disableCheckInterval = 10f;

	// Token: 0x04000007 RID: 7
	public float _enableCheckInterval = 1f;

	// Token: 0x04000008 RID: 8
	public bool _disableOnStart;
}
