using System;
using UnityEngine;

// Token: 0x0200047D RID: 1149
public abstract class HostedGameBehavior : MonoBehaviour, IPooledComponent, IResetable
{
	// Token: 0x060018DE RID: 6366 RVA: 0x00067A59 File Offset: 0x00065E59
	public virtual void Reset()
	{
		this.hostObject = null;
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x00067A62 File Offset: 0x00065E62
	public virtual void SetHost(GameObject hostObject)
	{
		this.hostObject = hostObject;
	}

	// Token: 0x040012BF RID: 4799
	protected GameObject hostObject;
}
