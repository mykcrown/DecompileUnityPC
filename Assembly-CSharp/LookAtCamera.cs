using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class LookAtCamera : MonoBehaviour
{
	// Token: 0x0600000A RID: 10 RVA: 0x0000238B File Offset: 0x0000078B
	public void Start()
	{
		if (this.lookAtCamera == null)
		{
			this.lookAtCamera = Camera.main;
		}
		if (this.lookOnlyOnAwake)
		{
			this.LookCam();
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000023BA File Offset: 0x000007BA
	public void Update()
	{
		if (!this.lookOnlyOnAwake)
		{
			this.LookCam();
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000023CD File Offset: 0x000007CD
	public void LookCam()
	{
		base.transform.LookAt(this.lookAtCamera.transform);
	}

	// Token: 0x04000009 RID: 9
	public Camera lookAtCamera;

	// Token: 0x0400000A RID: 10
	public bool lookOnlyOnAwake;
}
