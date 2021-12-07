// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectionManager : ISelectionManager
{
	public void Select(GameObject obj)
	{
		this.Select(obj, null);
	}

	public void Validate()
	{
		if (!this.isValidSelection(EventSystem.current.currentSelectedGameObject))
		{
			EventSystem.current.SetSelectedGameObject(null, null);
		}
	}

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
