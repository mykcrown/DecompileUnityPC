using System;
using UnityEngine;

// Token: 0x02000B0F RID: 2831
public class AsyncResourceMono : MonoBehaviour
{
	// Token: 0x0600514A RID: 20810 RVA: 0x00151ED7 File Offset: 0x001502D7
	protected void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x0600514B RID: 20811 RVA: 0x00151EDF File Offset: 0x001502DF
	private void Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}

	// Token: 0x04003462 RID: 13410
	public Action OnUpdate;
}
