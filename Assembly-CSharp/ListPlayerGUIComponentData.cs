using System;
using UnityEngine;

// Token: 0x020008CC RID: 2252
[Serializable]
public class ListPlayerGUIComponentData
{
	// Token: 0x0400270E RID: 9998
	public GameObject itemPrefab;

	// Token: 0x0400270F RID: 9999
	public GameObject maxPrefab;

	// Token: 0x04002710 RID: 10000
	public int spacing = 10;

	// Token: 0x04002711 RID: 10001
	public int totalCount = 1;

	// Token: 0x04002712 RID: 10002
	public int defaultCount;

	// Token: 0x04002713 RID: 10003
	public string itemCountChangedEventType;
}
