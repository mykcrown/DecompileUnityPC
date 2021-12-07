// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class CollectiblesModuleMode : MonoBehaviour
{
	private StoreScene3D storeScene;

	public MenuItemButton PreviewClickButton;

	public Transform ItemDisplay;

	public CanvasGroup CanvasGroup;

	public TextMeshProUGUI ItemTitle;

	public void Init(Action<MenuItemButton, InputEventData> previewClick, StoreScene3D storeScene)
	{
		this.storeScene = storeScene;
		this.CanvasGroup = base.GetComponent<CanvasGroup>();
		this.PreviewClickButton.Submit = previewClick;
		this.PreviewClickButton.InteractableButton.Unselectable = true;
	}

	public void Activate()
	{
		this.storeScene.AttachCollectiblesTo(this.ItemDisplay);
	}
}
