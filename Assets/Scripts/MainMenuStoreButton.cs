// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class MainMenuStoreButton : MonoBehaviour
{
	public MenuItemButton StoreButton;

	public MenuItemButton LootBoxesButton;

	public MenuItemButton GalleryButton;

	public GameObject GalleryNewFlag;

	public GameObject LootboxesCountFlag;

	public GameObject LootboxesCountBg;

	public TextMeshProUGUI LootboxCountText;

	public float LootBoxBGBorder = 14f;

	private bool isDirty;

	private float prevRenderedWidth;

	private RectTransform lootboxNotifierBgRectTransform;

	private void Awake()
	{
		this.lootboxNotifierBgRectTransform = this.LootboxesCountBg.GetComponent<RectTransform>();
	}

	public void SetLootboxCount(int count)
	{
		if (this.LootboxesCountFlag != null)
		{
			this.LootboxesCountFlag.SetActive(count > 0);
			this.LootboxCountText.text = count + string.Empty;
			this.isDirty = true;
		}
	}

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
}
