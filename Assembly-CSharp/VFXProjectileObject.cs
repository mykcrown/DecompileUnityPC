using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B8F RID: 2959
public class VFXProjectileObject : MonoBehaviour
{
	// Token: 0x06005555 RID: 21845 RVA: 0x001B5440 File Offset: 0x001B3840
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(this.ProjectileLife);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06005556 RID: 21846 RVA: 0x001B545B File Offset: 0x001B385B
	private void Update()
	{
		base.transform.Translate(-Vector3.left * this.ProjectileSpeed * Time.deltaTime, Space.Self);
	}

	// Token: 0x0400363E RID: 13886
	public float ProjectileLife;

	// Token: 0x0400363F RID: 13887
	public float ProjectileSpeed;
}
