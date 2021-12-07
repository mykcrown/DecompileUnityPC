using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B8E RID: 2958
public class VFXMuzzleObject : MonoBehaviour
{
	// Token: 0x06005553 RID: 21843 RVA: 0x001B5378 File Offset: 0x001B3778
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(this.MuzzleLife);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400363D RID: 13885
	public float MuzzleLife;
}
