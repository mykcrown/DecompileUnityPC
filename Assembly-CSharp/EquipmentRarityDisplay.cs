using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009F4 RID: 2548
public class EquipmentRarityDisplay : MonoBehaviour
{
	// Token: 0x060048C1 RID: 18625 RVA: 0x0013A7DC File Offset: 0x00138BDC
	private void Awake()
	{
		this.raritySprites[EquipmentRarity.COMMON] = this.CommonSprite;
		this.raritySprites[EquipmentRarity.UNCOMMON] = this.UncommonSprite;
		this.raritySprites[EquipmentRarity.RARE] = this.RareSprite;
		this.raritySprites[EquipmentRarity.LEGENDARY] = this.LegendarySprite;
		this.theImage = base.GetComponent<Image>();
	}

	// Token: 0x060048C2 RID: 18626 RVA: 0x0013A83D File Offset: 0x00138C3D
	public void SetRarity(EquipmentRarity rarity)
	{
		this.theImage.overrideSprite = this.raritySprites[rarity];
	}

	// Token: 0x0400300F RID: 12303
	public Sprite CommonSprite;

	// Token: 0x04003010 RID: 12304
	public Sprite UncommonSprite;

	// Token: 0x04003011 RID: 12305
	public Sprite RareSprite;

	// Token: 0x04003012 RID: 12306
	public Sprite LegendarySprite;

	// Token: 0x04003013 RID: 12307
	public Dictionary<EquipmentRarity, Sprite> raritySprites = new Dictionary<EquipmentRarity, Sprite>();

	// Token: 0x04003014 RID: 12308
	private Image theImage;
}
