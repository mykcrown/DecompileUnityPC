using System;
using UnityEngine;

// Token: 0x02000426 RID: 1062
[Serializable]
public class UnityFileLink<T> : UnityFileLink where T : UnityEngine.Object
{
	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06001602 RID: 5634 RVA: 0x000775A9 File Offset: 0x000759A9
	public T obj
	{
		get
		{
			return (!(this.reference == null)) ? ((T)((object)this.reference)) : ((T)((object)null));
		}
	}

	// Token: 0x06001603 RID: 5635 RVA: 0x000775D2 File Offset: 0x000759D2
	public void RuntimeOverrideWithMemoryObject(T obj)
	{
		this.reference = obj;
	}

	// Token: 0x040010F3 RID: 4339
	[SerializeField]
	private UnityEngine.Object reference;

	// Token: 0x040010F4 RID: 4340
	[SerializeField]
	private string _FILE_PATH;
}
