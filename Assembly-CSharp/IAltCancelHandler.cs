using System;
using UnityEngine.EventSystems;

// Token: 0x02000963 RID: 2403
public interface IAltCancelHandler : IEventSystemHandler
{
	// Token: 0x06004022 RID: 16418
	void OnAltCancel(BaseEventData eventData);
}
