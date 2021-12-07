using System;
using UnityEngine;

// Token: 0x02000425 RID: 1061
[Serializable]
public class UnityFileLink
{
	// Token: 0x060015FE RID: 5630 RVA: 0x00077599 File Offset: 0x00075999
	public virtual void SyncData(string ownerFilePath = null)
	{
	}

	// Token: 0x060015FF RID: 5631 RVA: 0x0007759B File Offset: 0x0007599B
	public virtual string GetFilePath()
	{
		return null;
	}

	// Token: 0x06001600 RID: 5632 RVA: 0x0007759E File Offset: 0x0007599E
	public virtual UnityEngine.Object GetObject()
	{
		return null;
	}
}
