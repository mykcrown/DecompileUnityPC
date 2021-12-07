// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentLine : MonoBehaviour, IHighlightMode
{
	public Image Background;

	public TextMeshProUGUI Title;

	public TextMeshProUGUI Price;

	public Image Icon;

	public RawImage RawIcon;

	public GameObject PriceDisplay;

	public string LocalizedName;

	public EquipmentRarityDisplay Rarity1;

	public EquipmentRarityDisplay Rarity2;

	public Sprite LockIconHighlightSprite;

	public Sprite EquipmentLineHighlight;

	public Image LockIcon;

	public GameObject NewFlag;

	public GameObject EquippedFlag;

	public GameObject DefaultToken;

	public MenuItemButton MenuItemButton;

	public float LockOffsetX;

	private bool isDirty;

	private float prevTitleRenderedWidth;

	private bool highlightMode;

	private List<GameObject> hasItemGroup = new List<GameObject>();

	private List<GameObject> unownedGroup = new List<GameObject>();

	private IEquipModuleAPI api;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IItemLoader itemLoader
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	public EquippableItem Item
	{
		get;
		private set;
	}

	private void Awake()
	{
		this.hasItemGroup.Add(this.EquippedFlag);
		this.hasItemGroup.Add(this.NewFlag);
		this.unownedGroup.Add(this.PriceDisplay);
		this.unownedGroup.Add(this.Price.gameObject);
		this.unownedGroup.Add(this.LockIcon.gameObject);
	}

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
			UnityEngine.Debug.LogError("unamed asset");
		}
	}

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

	private void setHasItem(bool value)
	{
		foreach (GameObject current in this.hasItemGroup)
		{
			current.SetActive(value);
		}
		foreach (GameObject current2 in this.unownedGroup)
		{
			current2.SetActive(!value);
		}
		if (!value && !this.api.HasPrice(this.Item))
		{
			this.Price.gameObject.SetActive(false);
			this.PriceDisplay.SetActive(false);
		}
	}

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
}
