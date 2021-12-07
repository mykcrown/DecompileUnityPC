using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009E6 RID: 2534
public class EquipmentLine : MonoBehaviour, IHighlightMode
{
	// Token: 0x1700113B RID: 4411
	// (get) Token: 0x060047F2 RID: 18418 RVA: 0x001380C8 File Offset: 0x001364C8
	// (set) Token: 0x060047F3 RID: 18419 RVA: 0x001380D0 File Offset: 0x001364D0
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x1700113C RID: 4412
	// (get) Token: 0x060047F4 RID: 18420 RVA: 0x001380D9 File Offset: 0x001364D9
	// (set) Token: 0x060047F5 RID: 18421 RVA: 0x001380E1 File Offset: 0x001364E1
	[Inject]
	public IItemLoader itemLoader { get; set; }

	// Token: 0x1700113D RID: 4413
	// (get) Token: 0x060047F6 RID: 18422 RVA: 0x001380EA File Offset: 0x001364EA
	// (set) Token: 0x060047F7 RID: 18423 RVA: 0x001380F2 File Offset: 0x001364F2
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x1700113E RID: 4414
	// (get) Token: 0x060047F8 RID: 18424 RVA: 0x001380FB File Offset: 0x001364FB
	// (set) Token: 0x060047F9 RID: 18425 RVA: 0x00138103 File Offset: 0x00136503
	public EquippableItem Item { get; private set; }

	// Token: 0x060047FA RID: 18426 RVA: 0x0013810C File Offset: 0x0013650C
	private void Awake()
	{
		this.hasItemGroup.Add(this.EquippedFlag);
		this.hasItemGroup.Add(this.NewFlag);
		this.unownedGroup.Add(this.PriceDisplay);
		this.unownedGroup.Add(this.Price.gameObject);
		this.unownedGroup.Add(this.LockIcon.gameObject);
	}

	// Token: 0x060047FB RID: 18427 RVA: 0x00138178 File Offset: 0x00136578
	public void SetItem(EquippableItem item, IEquipModuleAPI api)
	{
		this.api = api;
		this.Item = item;
		if (this.Icon != null)
		{
			this.Icon.enabled = false;
		}
		if (this.RawIcon != null)
		{
			this.RawIcon.enabled = false;
		}
		if (this.DefaultToken != null)
		{
			this.DefaultToken.gameObject.SetActive(false);
		}
		if (this.Title != null)
		{
			string text = api.GetLocalizedItemName(item);
			if (!string.IsNullOrEmpty(text))
			{
				text = text.ToUpper();
			}
			this.Title.text = text;
			this.isDirty = true;
		}
		if (item.type == EquipmentTypes.NETSUKE && this.Icon != null)
		{
			Netsuke netsuke = this.itemLoader.LoadPrefab<Netsuke>(item);
			this.Icon.enabled = true;
			this.Icon.sprite = netsuke.Icon;
		}
		else if (item.type == EquipmentTypes.HOLOGRAM && this.RawIcon != null)
		{
			HologramData hologramData = this.itemLoader.LoadAsset<HologramData>(item);
			this.RawIcon.enabled = true;
			this.RawIcon.texture = hologramData.texture;
		}
		else if (item.type == EquipmentTypes.TOKEN)
		{
			if (!item.isDefault && this.RawIcon != null)
			{
				PlayerToken playerToken = this.itemLoader.LoadPrefab<PlayerToken>(item);
				if (playerToken != null)
				{
					this.RawIcon.enabled = true;
					this.RawIcon.texture = playerToken.Image.sprite.texture;
				}
			}
			else if (this.DefaultToken != null)
			{
				this.DefaultToken.gameObject.SetActive(true);
			}
		}
		else if (item.type == EquipmentTypes.PLAYER_ICON && this.RawIcon != null)
		{
			PlayerCardIconData playerCardIconData = this.itemLoader.LoadAsset<PlayerCardIconData>(item);
			if (playerCardIconData != null)
			{
				this.RawIcon.enabled = true;
				this.RawIcon.texture = playerCardIconData.sprite.texture;
			}
		}
		if (api.HasPrice(item))
		{
			this.PriceDisplay.SetActive(true);
			this.Price.gameObject.SetActive(true);
			this.Price.text = this.localization.GetSoftPriceString(item.price);
		}
		else
		{
			this.Price.gameObject.SetActive(false);
			this.PriceDisplay.SetActive(false);
		}
		this.Rarity1.SetRarity(item.rarity);
		if (this.Rarity2 != null)
		{
			this.Rarity2.SetRarity(item.rarity);
		}
		this.LocalizedName = this.equipmentModel.GetLocalizedItemName(item);
		if (this.LocalizedName == null)
		{
			Debug.LogError("unamed asset");
		}
	}

	// Token: 0x060047FC RID: 18428 RVA: 0x00138478 File Offset: 0x00136878
	private void Update()
	{
		if (this.isDirty)
		{
			if (this.Title == null)
			{
				this.isDirty = false;
			}
			else if (this.Title.renderedWidth > 1f && this.Title.renderedWidth != this.prevTitleRenderedWidth)
			{
				this.isDirty = false;
				this.prevTitleRenderedWidth = this.Title.renderedWidth;
				this.syncPositions();
			}
		}
	}

	// Token: 0x060047FD RID: 18429 RVA: 0x001384F8 File Offset: 0x001368F8
	private void syncPositions()
	{
		if (this.Title != null && !this.isDirty)
		{
			float num = this.Title.renderedWidth + this.LockOffsetX;
			float x = this.Title.transform.localPosition.x + num;
			Vector3 localPosition = this.LockIcon.transform.localPosition;
			localPosition.x = x;
			this.LockIcon.transform.localPosition = localPosition;
			localPosition = this.NewFlag.transform.localPosition;
			localPosition.x = x;
			this.NewFlag.transform.localPosition = localPosition;
		}
	}

	// Token: 0x060047FE RID: 18430 RVA: 0x001385A4 File Offset: 0x001369A4
	public void SetHighlightMode(bool value)
	{
		if (this.highlightMode != value)
		{
			this.highlightMode = value;
			if (this.highlightMode)
			{
				this.LockIcon.overrideSprite = this.LockIconHighlightSprite;
				this.Background.overrideSprite = this.EquipmentLineHighlight;
			}
			else
			{
				this.LockIcon.overrideSprite = null;
				this.Background.overrideSprite = null;
			}
		}
	}

	// Token: 0x060047FF RID: 18431 RVA: 0x00138610 File Offset: 0x00136A10
	private void setHasItem(bool value)
	{
		foreach (GameObject gameObject in this.hasItemGroup)
		{
			gameObject.SetActive(value);
		}
		foreach (GameObject gameObject2 in this.unownedGroup)
		{
			gameObject2.SetActive(!value);
		}
		if (!value && !this.api.HasPrice(this.Item))
		{
			this.Price.gameObject.SetActive(false);
			this.PriceDisplay.SetActive(false);
		}
	}

	// Token: 0x06004800 RID: 18432 RVA: 0x001386F4 File Offset: 0x00136AF4
	public void UpdateDynamicInfo()
	{
		if (this.api.HasItem(this.Item.id))
		{
			this.setHasItem(true);
			if (this.api.IsEquipped(this.Item))
			{
				this.EquippedFlag.SetActive(true);
			}
			else
			{
				this.EquippedFlag.SetActive(false);
			}
		}
		else
		{
			this.setHasItem(false);
		}
		if (this.api.IsNew(this.Item.id))
		{
			this.NewFlag.SetActive(true);
		}
		else
		{
			this.NewFlag.SetActive(false);
		}
		this.syncPositions();
	}

	// Token: 0x04002F93 RID: 12179
	public Image Background;

	// Token: 0x04002F94 RID: 12180
	public TextMeshProUGUI Title;

	// Token: 0x04002F95 RID: 12181
	public TextMeshProUGUI Price;

	// Token: 0x04002F96 RID: 12182
	public Image Icon;

	// Token: 0x04002F97 RID: 12183
	public RawImage RawIcon;

	// Token: 0x04002F98 RID: 12184
	public GameObject PriceDisplay;

	// Token: 0x04002F99 RID: 12185
	public string LocalizedName;

	// Token: 0x04002F9A RID: 12186
	public EquipmentRarityDisplay Rarity1;

	// Token: 0x04002F9B RID: 12187
	public EquipmentRarityDisplay Rarity2;

	// Token: 0x04002F9C RID: 12188
	public Sprite LockIconHighlightSprite;

	// Token: 0x04002F9D RID: 12189
	public Sprite EquipmentLineHighlight;

	// Token: 0x04002F9E RID: 12190
	public Image LockIcon;

	// Token: 0x04002F9F RID: 12191
	public GameObject NewFlag;

	// Token: 0x04002FA0 RID: 12192
	public GameObject EquippedFlag;

	// Token: 0x04002FA1 RID: 12193
	public GameObject DefaultToken;

	// Token: 0x04002FA2 RID: 12194
	public MenuItemButton MenuItemButton;

	// Token: 0x04002FA3 RID: 12195
	public float LockOffsetX;

	// Token: 0x04002FA5 RID: 12197
	private bool isDirty;

	// Token: 0x04002FA6 RID: 12198
	private float prevTitleRenderedWidth;

	// Token: 0x04002FA7 RID: 12199
	private bool highlightMode;

	// Token: 0x04002FA8 RID: 12200
	private List<GameObject> hasItemGroup = new List<GameObject>();

	// Token: 0x04002FA9 RID: 12201
	private List<GameObject> unownedGroup = new List<GameObject>();

	// Token: 0x04002FAA RID: 12202
	private IEquipModuleAPI api;
}
