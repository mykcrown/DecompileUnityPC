using System;
using TMPro;
using UnityEngine;

// Token: 0x020009AD RID: 2477
public class MainMenuStoreButton : MonoBehaviour
{
	// Token: 0x06004479 RID: 17529 RVA: 0x0012D8C5 File Offset: 0x0012BCC5
	private void Awake()
	{
		this.lootboxNotifierBgRectTransform = this.LootboxesCountBg.GetComponent<RectTransform>();
	}

	// Token: 0x0600447A RID: 17530 RVA: 0x0012D8D8 File Offset: 0x0012BCD8
	public void SetLootboxCount(int count)
	{
		if (this.LootboxesCountFlag != null)
		{
			this.LootboxesCountFlag.SetActive(count > 0);
			this.LootboxCountText.text = count + string.Empty;
			this.isDirty = true;
		}
	}

	// Token: 0x0600447B RID: 17531 RVA: 0x0012D928 File Offset: 0x0012BD28
	private void Update()
	{
		if (this.isDirty)
		{
			if (this.LootboxCountText == null)
			{
				this.isDirty = false;
			}
			else if (this.LootboxCountText.renderedWidth > 1f && this.LootboxCountText.renderedWidth != this.prevRenderedWidth)
			{
				this.isDirty = false;
				this.prevRenderedWidth = this.LootboxCountText.renderedWidth;
				Vector2 sizeDelta = this.lootboxNotifierBgRectTransform.sizeDelta;
				sizeDelta.x = this.prevRenderedWidth + this.LootBoxBGBorder * 2f;
				this.lootboxNotifierBgRectTransform.sizeDelta = sizeDelta;
			}
		}
	}

	// Token: 0x04002D8A RID: 11658
	public MenuItemButton StoreButton;

	// Token: 0x04002D8B RID: 11659
	public MenuItemButton LootBoxesButton;

	// Token: 0x04002D8C RID: 11660
	public MenuItemButton GalleryButton;

	// Token: 0x04002D8D RID: 11661
	public GameObject GalleryNewFlag;

	// Token: 0x04002D8E RID: 11662
	public GameObject LootboxesCountFlag;

	// Token: 0x04002D8F RID: 11663
	public GameObject LootboxesCountBg;

	// Token: 0x04002D90 RID: 11664
	public TextMeshProUGUI LootboxCountText;

	// Token: 0x04002D91 RID: 11665
	public float LootBoxBGBorder = 14f;

	// Token: 0x04002D92 RID: 11666
	private bool isDirty;

	// Token: 0x04002D93 RID: 11667
	private float prevRenderedWidth;

	// Token: 0x04002D94 RID: 11668
	private RectTransform lootboxNotifierBgRectTransform;
}
