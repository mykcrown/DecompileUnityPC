using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A2A RID: 2602
public class StoreIntroPanel : MonoBehaviour
{
	// Token: 0x1700120B RID: 4619
	// (get) Token: 0x06004BD2 RID: 19410 RVA: 0x00142DBA File Offset: 0x001411BA
	// (set) Token: 0x06004BD3 RID: 19411 RVA: 0x00142DC2 File Offset: 0x001411C2
	public StoreIntroPanel.ItemLayoutData itemData { get; private set; }

	// Token: 0x06004BD4 RID: 19412 RVA: 0x00142DCC File Offset: 0x001411CC
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

	// Token: 0x06004BD5 RID: 19413 RVA: 0x00142F10 File Offset: 0x00141310
	private void syncPortrait()
	{
		if (this.theImage != null && this.itemData.isDynamicAlignment)
		{
			CharacterMenusData.UIPortraitData uiportraitData = null;
			if (this.itemData is StoreIntroPanel.CharacterItemLayoutData)
			{
				uiportraitData = (this.itemData as StoreIntroPanel.CharacterItemLayoutData).portraitData;
			}
			if (uiportraitData != null)
			{
				this.theImage.transform.localPosition = uiportraitData.offset;
				this.theImage.transform.localScale = new Vector3(uiportraitData.scale, uiportraitData.scale, uiportraitData.scale);
			}
			else
			{
				this.theImage.transform.localPosition = Vector3.zero;
				this.theImage.transform.localScale = Vector3.one;
			}
		}
	}

	// Token: 0x040031C4 RID: 12740
	public Image DisplayImageCentered;

	// Token: 0x040031C5 RID: 12741
	public Image DisplayImage;

	// Token: 0x040031C6 RID: 12742
	public TextMeshProUGUI NameText;

	// Token: 0x040031C7 RID: 12743
	public TextMeshProUGUI AmountText;

	// Token: 0x040031C8 RID: 12744
	public GameObject IsUnlockedGroup;

	// Token: 0x040031C9 RID: 12745
	public GameObject IsUnlockedCheck;

	// Token: 0x040031CA RID: 12746
	public GameObject NewFlag;

	// Token: 0x040031CB RID: 12747
	public MenuItemButton Button;

	// Token: 0x040031CD RID: 12749
	private Image theImage;

	// Token: 0x02000A2B RID: 2603
	public class ItemLayoutData
	{
		// Token: 0x040031CE RID: 12750
		public Sprite image;

		// Token: 0x040031CF RID: 12751
		public string itemName;

		// Token: 0x040031D0 RID: 12752
		public int amount;

		// Token: 0x040031D1 RID: 12753
		public int maxAmount;

		// Token: 0x040031D2 RID: 12754
		public bool useUnlocked;

		// Token: 0x040031D3 RID: 12755
		public bool isUnlocked;

		// Token: 0x040031D4 RID: 12756
		public bool isNew;

		// Token: 0x040031D5 RID: 12757
		public bool isDynamicAlignment;
	}

	// Token: 0x02000A2C RID: 2604
	public class CharacterItemLayoutData : StoreIntroPanel.ItemLayoutData
	{
		// Token: 0x040031D6 RID: 12758
		public CharacterID characterID;

		// Token: 0x040031D7 RID: 12759
		public CharacterMenusData.UIPortraitData portraitData;
	}

	// Token: 0x02000A2D RID: 2605
	public class CollectibleItemLayoutData : StoreIntroPanel.ItemLayoutData
	{
		// Token: 0x040031D8 RID: 12760
		public EquipmentTypes equipmentType;
	}
}
