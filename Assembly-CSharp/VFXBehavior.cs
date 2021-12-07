using System;
using UnityEngine;

// Token: 0x02000B87 RID: 2951
public abstract class VFXBehavior : MonoBehaviour
{
	// Token: 0x06005534 RID: 21812 RVA: 0x001B48BF File Offset: 0x001B2CBF
	private void Start()
	{
		this.OnVFXInit();
	}

	// Token: 0x06005535 RID: 21813
	public abstract void OnVFXInit();

	// Token: 0x06005536 RID: 21814
	public abstract void OnVFXStart();
}
