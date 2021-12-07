using System;
using UnityEngine;

// Token: 0x02000928 RID: 2344
public class DebugUIElement : MonoBehaviour
{
	// Token: 0x06003D25 RID: 15653 RVA: 0x0011AA92 File Offset: 0x00118E92
	private void Start()
	{
	}

	// Token: 0x06003D26 RID: 15654 RVA: 0x0011AA94 File Offset: 0x00118E94
	public void Init(float duration)
	{
		UnityEngine.Object.Destroy(base.gameObject, duration);
	}
}
