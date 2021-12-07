using System;
using UnityEngine;

// Token: 0x02000B90 RID: 2960
public class VFXProjectileOrigin : MonoBehaviour
{
	// Token: 0x06005558 RID: 21848 RVA: 0x001B5531 File Offset: 0x001B3931
	private void start()
	{
	}

	// Token: 0x06005559 RID: 21849 RVA: 0x001B5534 File Offset: 0x001B3934
	private void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Space) && this.Projectile != null)
		{
			this.pos = base.transform.position;
			this.Projectile.transform.position = this.pos;
			this.MuzzleEffect.transform.position = this.pos + this.MuzzleOffset;
		}
	}

	// Token: 0x04003640 RID: 13888
	public GameObject SourceTransform;

	// Token: 0x04003641 RID: 13889
	public GameObject Projectile;

	// Token: 0x04003642 RID: 13890
	public GameObject MuzzleEffect;

	// Token: 0x04003643 RID: 13891
	private Vector3 pos;

	// Token: 0x04003644 RID: 13892
	public Vector3 MuzzleOffset;
}
