// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class UIPreload3DAssets : IStartupLoader
{
	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterLists characterManager
	{
		private get;
		set;
	}

	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel
	{
		private get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		private get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	public void Reset()
	{
		this.uiSceneCharacterManager.ClearPreload();
	}

	public List<SkinDefinition> GetDefaultSkins()
	{
		CharacterDefinition[] nonRandomCharacters = this.characterManager.GetNonRandomCharacters();
		List<SkinDefinition> list = new List<SkinDefinition>();
		CharacterDefinition[] array = nonRandomCharacters;
		for (int i = 0; i < array.Length; i++)
		{
			CharacterDefinition characterDefinition = array[i];
			List<SkinDefinition> mostLikelySkins = this.getMostLikelySkins(characterDefinition.characterID);
			foreach (SkinDefinition current in mostLikelySkins)
			{
				list.Add(current);
			}
		}
		return list;
	}

	public void PreloadForScene(Action callback)
	{
		CharacterDefinition[] nonRandomCharacters = this.characterManager.GetNonRandomCharacters();
		List<UISceneCharacterManager.CharacterWithSkin> list = new List<UISceneCharacterManager.CharacterWithSkin>();
		CharacterDefinition[] array = nonRandomCharacters;
		for (int i = 0; i < array.Length; i++)
		{
			CharacterDefinition characterDefinition = array[i];
			List<SkinDefinition> mostLikelySkins = this.getMostLikelySkins(characterDefinition.characterID);
			foreach (SkinDefinition current in mostLikelySkins)
			{
				list.Add(new UISceneCharacterManager.CharacterWithSkin
				{
					characterDef = characterDefinition,
					skinDefinition = current
				});
			}
		}
		this.uiSceneCharacterManager.VRAMPreload(list, callback);
	}

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
		SkinDefinition[] array = skins;
		for (int i = 0; i < array.Length; i++)
		{
			SkinDefinition item = array[i];
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
