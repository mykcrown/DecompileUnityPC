using System;
using UnityEngine;

// Token: 0x02000BA2 RID: 2978
public class PlanetRotate : MonoBehaviour
{
	// Token: 0x060055F6 RID: 22006 RVA: 0x001B7BCE File Offset: 0x001B5FCE
	private void Start()
	{
	}

	// Token: 0x060055F7 RID: 22007 RVA: 0x001B7BD0 File Offset: 0x001B5FD0
	private void Update()
	{
		this.CurrentOrientation += this.RotationOrientation * Time.deltaTime * PlanetRotate.GlobalSpeedup;
		base.transform.localRotation = Quaternion.Euler(this.InitialOrientation) * Quaternion.Euler(this.CurrentOrientation);
	}

	// Token: 0x040036A5 RID: 13989
	public static float GlobalSpeedup = 1f;

	// Token: 0x040036A6 RID: 13990
	public Vector3 InitialOrientation = Vector3.zero;

	// Token: 0x040036A7 RID: 13991
	public Vector3 RotationOrientation = Vector3.zero;

	// Token: 0x040036A8 RID: 13992
	private Vector3 CurrentOrientation = Vector3.zero;
}
