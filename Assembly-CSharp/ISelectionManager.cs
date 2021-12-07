using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000A7A RID: 2682
public interface ISelectionManager
{
	// Token: 0x06004E6B RID: 20075
	void Select(GameObject obj);

	// Token: 0x06004E6C RID: 20076
	void Select(GameObject obj, BaseEventData eventData);

	// Token: 0x06004E6D RID: 20077
	void Validate();
}
