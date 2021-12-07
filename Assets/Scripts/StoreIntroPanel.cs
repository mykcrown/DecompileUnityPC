// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreIntroPanel : MonoBehaviour
{
	public class ItemLayoutData
	{
		public Sprite image;

		public string itemName;

		public int amount;

		public int maxAmount;

		public bool useUnlocked;

		public bool isUnlocked;

		public bool isNew;

		public bool isDynamicAlignment;
	}

	public class CharacterItemLayoutData : StoreIntroPanel.ItemLayoutData
	{
		public CharacterID characterID;

		public CharacterMenusData.UIPortraitData portraitData;
	}

	public class CollectibleItemLayoutData : StoreIntroPanel.ItemLayoutData
	{
		public EquipmentTypes equipmentType;
	}

	public Image DisplayImageCentered;

	public Image DisplayImage;

	public TextMeshProUGUI NameText;

	public TextMeshProUGUI AmountText;

	public GameObject IsUnlockedGroup;

	public GameObject IsUnlockedCheck;

	public GameObject NewFlag;

	public MenuItemButton Button;

	private Image theImage;

	public StoreIntroPanel.ItemLayoutData itemData
	{
		get;
		private set;
	}

	public void Initialize(StoreIntroPanel.ItemLayoutData itemData)
	{
		this.itemData = itemData;
		this.DisplayImageCentered.gameObject.SetActive(false);
		this.DisplayImage.gameObject.SetActive(false);
		if (itemData.isDynamicAlignment)
		{
			this.theImage = this.DisplayImageCentered;
		}
		else
		{
			this.theImage = this.DisplayImage;
		}
		this.theImage.gameObject.SetActive(true);
		this.theImage.sprite = itemData.image;
		this.theImage.color = ((!(this.theImage.sprite == null)) ? Color.white : Color.clear);
		this.theImage.preserveAspect = true;
		this.NameText.text = itemData.itemName;
		string text = string.Format("{0}/{1}", itemData.amount, itemData.maxAmount);
		this.AmountText.text = text;
		this.AmountText.gameObject.SetActive(false);
		this.IsUnlockedGroup.SetActive(itemData.useUnlocked);
		this.IsUnlockedCheck.SetActive(itemData.isUnlocked);
		this.NewFlag.SetActive(itemData.isNew);
		this.syncPortrait();
	}

	private void syncPortrait()
	{
		if (this.theImage != null && this.itemData.isDynamicAlignment)
		{
			CharacterMenusData.UIPortraitData uIPortraitData = null;
			if (this.itemData is StoreIntroPanel.CharacterItemLayoutData)
			{
				uIPortraitData = (this.itemData as StoreIntroPanel.CharacterItemLayoutData).portraitData;
			}
			if (uIPortraitData != null)
			{
				this.theImage.transform.localPosition = uIPortraitData.offset;
				this.theImage.transform.localScale = new Vector3(uIPortraitData.scale, uIPortraitData.scale, uIPortraitData.scale);
			}
			else
			{
				this.theImage.transform.localPosition = Vector3.zero;
				this.theImage.transform.localScale = Vector3.one;
			}
		}
	}
}
