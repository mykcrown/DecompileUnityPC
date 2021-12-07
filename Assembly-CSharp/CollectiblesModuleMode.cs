using System;
using TMPro;
using UnityEngine;

// Token: 0x020009EF RID: 2543
public class CollectiblesModuleMode : MonoBehaviour
{
	// Token: 0x06004893 RID: 18579 RVA: 0x0013A310 File Offset: 0x00138710
	public void Init(Action<MenuItemButton, InputEventData> previewClick, StoreScene3D storeScene)
	{
		this.storeScene = storeScene;
		this.CanvasGroup = base.GetComponent<CanvasGroup>();
		this.PreviewClickButton.Submit = previewClick;
		this.PreviewClickButton.InteractableButton.Unselectable = true;
	}

	// Token: 0x06004894 RID: 18580 RVA: 0x0013A342 File Offset: 0x00138742
	public void Activate()
	{
		this.storeScene.AttachCollectiblesTo(this.ItemDisplay);
	}

	// Token: 0x04002FF7 RID: 12279
	private StoreScene3D storeScene;

	// Token: 0x04002FF8 RID: 12280
	public MenuItemButton PreviewClickButton;

	// Token: 0x04002FF9 RID: 12281
	public Transform ItemDisplay;

	// Token: 0x04002FFA RID: 12282
	public CanvasGroup CanvasGroup;

	// Token: 0x04002FFB RID: 12283
	public TextMeshProUGUI ItemTitle;
}
