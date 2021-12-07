// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentRarityDisplay : MonoBehaviour
{
	public Sprite CommonSprite;

	public Sprite UncommonSprite;

	public Sprite RareSprite;

	public Sprite LegendarySprite;

	public Dictionary<EquipmentRarity, Sprite> raritySprites = new Dictionary<EquipmentRarity, Sprite>();

	private Image theImage;

	private void Awake()
	{
		this.raritySprites[EquipmentRarity.COMMON] = this.CommonSprite;
		this.raritySprites[EquipmentRarity.UNCOMMON] = this.UncommonSprite;
		this.raritySprites[EquipmentRarity.RARE] = this.RareSprite;
		this.raritySprites[EquipmentRarity.LEGENDARY] = this.LegendarySprite;
		this.theImage = base.GetComponent<Image>();
	}

	public void SetRarity(EquipmentRarity rarity)
	{
		this.theImage.overrideSprite = this.raritySprites[rarity];
	}
}
