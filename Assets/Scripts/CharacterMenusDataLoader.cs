// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenusDataLoader : ICharacterMenusDataLoader
{
	private Dictionary<string, CharacterMenusData> indexByName = new Dictionary<string, CharacterMenusData>();

	private Dictionary<CharacterID, CharacterMenusData> indexByID = new Dictionary<CharacterID, CharacterMenusData>(default(CharacterIDComparer));

	[Inject]
	public ICharacterLists characterLists
	{
		private get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	public CharacterMenusData GetData(CharacterDefinition characterDef)
	{
		return this.loadByName(characterDef.characterName);
	}

	public CharacterMenusData GetData(CharacterID id)
	{
		if (id == CharacterID.None)
		{
			return null;
		}
		if (this.indexByID.ContainsKey(id))
		{
			return this.indexByID[id];
		}
		CharacterDefinition characterDefinition = this.findDefById(id);
		return this.loadByName(characterDefinition.characterName);
	}

	private CharacterMenusData loadByName(string name)
	{
		if (!this.indexByName.ContainsKey(name))
		{
			ProfilingUtil.BeginTimer();
			this.indexByName[name] = Resources.Load<CharacterMenusData>("Character/" + name + "-MenuData");
			ProfilingUtil.EndTimer("LOAD CHARACTER MENUS DATA");
			if (!this.indexByName[name].characterDefinition.isPartner)
			{
				this.indexByID[this.indexByName[name].characterID] = this.indexByName[name];
			}
			this.localization.AddLocalizationData(this.indexByName[name].localization);
		}
		return this.indexByName[name];
	}

	private CharacterDefinition findDefById(CharacterID characterId)
	{
		return this.characterLists.GetCharacterDefinition(characterId);
	}
}
