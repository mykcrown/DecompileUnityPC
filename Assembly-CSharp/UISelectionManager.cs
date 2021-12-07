using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000A79 RID: 2681
public class UISelectionManager : ISelectionManager
{
	// Token: 0x06004E67 RID: 20071 RVA: 0x001498E1 File Offset: 0x00147CE1
	public void Select(GameObject obj)
	{
		this.Select(obj, null);
	}

	// Token: 0x06004E68 RID: 20072 RVA: 0x001498EB File Offset: 0x00147CEB
	public void Validate()
	{
		if (!this.isValidSelection(EventSystem.current.currentSelectedGameObject))
		{
			EventSystem.current.SetSelectedGameObject(null, null);
		}
	}

	// Token: 0x06004E69 RID: 20073 RVA: 0x00149910 File Offset: 0x00147D10
	public void Select(GameObject obj, BaseEventData eventData)
	{
		if (this.isValidSelection(obj))
		{
			if (EventSystem.current.currentSelectedGameObject != obj)
			{
				EventSystem.current.SetSelectedGameObject(obj, eventData);
			}
		}
		else if (EventSystem.current.currentSelectedGameObject != null)
		{
			EventSystem.current.SetSelectedGameObject(null, eventData);
		}
	}

	// Token: 0x06004E6A RID: 20074 RVA: 0x00149970 File Offset: 0x00147D70
	private bool isValidSelection(GameObject obj)
	{
		if (obj != null)
		{
			WavedashUIButton component = obj.GetComponent<WavedashUIButton>();
			if (component != null && component.Unselectable)
			{
				return false;
			}
		}
		return true;
	}
}
