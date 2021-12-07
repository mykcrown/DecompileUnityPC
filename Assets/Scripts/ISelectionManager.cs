// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISelectionManager
{
	void Select(GameObject obj);

	void Select(GameObject obj, BaseEventData eventData);

	void Validate();
}
