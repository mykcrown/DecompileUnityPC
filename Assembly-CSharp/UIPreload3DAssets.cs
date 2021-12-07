using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008A7 RID: 2215
public class UIPreload3DAssets : IStartupLoader
{
	// Token: 0x17000D77 RID: 3447
	// (get) Token: 0x06003770 RID: 14192 RVA: 0x00102B5F File Offset: 0x00100F5F
	// (set) Token: 0x06003771 RID: 14193 RVA: 0x00102B67 File Offset: 0x00100F67
	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager { private get; set; }

	// Token: 0x17000D78 RID: 3448
	// (get) Token: 0x06003772 RID: 14194 RVA: 0x00102B70 File Offset: 0x00100F70
	// (set) Token: 0x06003773 RID: 14195 RVA: 0x00102B78 File Offset: 0x00100F78
	[Inject]
	public ICharacterLists characterManager { private get; set; }

	// Token: 0x17000D79 RID: 3449
	// (get) Token: 0x06003774 RID: 14196 RVA: 0x00102B81 File Offset: 0x00100F81
	// (set) Token: 0x06003775 RID: 14197 RVA: 0x00102B89 File Offset: 0x00100F89
	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel { private get; set; }

	// Token: 0x17000D7A RID: 3450
	// (get) Token: 0x06003776 RID: 14198 RVA: 0x00102B92 File Offset: 0x00100F92
	// (set) Token: 0x06003777 RID: 14199 RVA: 0x00102B9A File Offset: 0x00100F9A
	[Inject]
	public GameDataManager gameDataManager { private get; set; }

	// Token: 0x17000D7B RID: 3451
	// (get) Token: 0x06003778 RID: 14200 RVA: 0x00102BA3 File Offset: 0x00100FA3
	// (set) Token: 0x06003779 RID: 14201 RVA: 0x00102BAB File Offset: 0x00100FAB
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x0600377A RID: 14202 RVA: 0x00102BB4 File Offset: 0x00100FB4
	public void Reset()
	{
		this.uiSceneCharacterManager.ClearPreload();
	}

	// Token: 0x0600377B RID: 14203 RVA: 0x00102BC4 File Offset: 0x00100FC4
	public List<SkinDefinition> GetDefaultSkins()
	{
		CharacterDefinition[] nonRandomCharacters = this.characterManager.GetNonRandomCharacters();
		List<SkinDefinition> list = new List<SkinDefinition>();
		foreach (CharacterDefinition characterDefinition in nonRandomCharacters)
		{
			List<SkinDefinition> mostLikelySkins = this.getMostLikelySkins(characterDefinition.characterID);
			foreach (SkinDefinition item in mostLikelySkins)
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x00102C60 File Offset: 0x00101060
	public void PreloadForScene(Action callback)
	{
		CharacterDefinition[] nonRandomCharacters = this.characterManager.GetNonRandomCharacters();
		List<UISceneCharacterManager.CharacterWithSkin> list = new List<UISceneCharacterManager.CharacterWithSkin>();
		foreach (CharacterDefinition characterDefinition in nonRandomCharacters)
		{
			List<SkinDefinition> mostLikelySkins = this.getMostLikelySkins(characterDefinition.characterID);
			foreach (SkinDefinition skinDefinition in mostLikelySkins)
			{
				list.Add(new UISceneCharacterManager.CharacterWithSkin
				{
					characterDef = characterDefinition,
					skinDefinition = skinDefinition
				});
			}
		}
		this.uiSceneCharacterManager.VRAMPreload(list, callback);
	}

	// Token: 0x0600377D RID: 14205 RVA: 0x00102D20 File Offset: 0x00101120
	private List<SkinDefinition> getMostLikelySkins(CharacterID characterId)
	{
		List<SkinDefinition> list = new List<SkinDefinition>();
		int num = 1;
		SkinDefinition equippedSkin = this.userCharacterEquippedModel.GetEquippedSkin(characterId, 100);
		if (equippedSkin != null)
		{
			list.Add(equippedSkin);
		}
		SkinDefinition[] skins = this.skinDataManager.GetSkins(characterId);
		SkinDefinition defaultSkin = this.skinDataManager.GetDefaultSkin(characterId);
		if (!list.Contains(defaultSkin) && list.Count < num)
		{
			if (defaultSkin == null)
			{
				throw new UnityException("No Default Skin for " + characterId);
			}
			list.Add(defaultSkin);
		}
		foreach (SkinDefinition item in skins)
		{
			if (list.Count >= num)
			{
				break;
			}
			if (!list.Contains(item))
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x00102E00 File Offset: 0x00101200
	public void StartupLoad(Action callback)
	{
		if (SystemBoot.mode == SystemBoot.Mode.StagePreview)
		{
			callback();
		}
		else
		{
			this.PreloadForScene(callback);
		}
	}
}
