using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200058B RID: 1419
public class CharacterMenusDataLoader : ICharacterMenusDataLoader
{
	// Token: 0x170006FA RID: 1786
	// (get) Token: 0x06001FFC RID: 8188 RVA: 0x000A241B File Offset: 0x000A081B
	// (set) Token: 0x06001FFD RID: 8189 RVA: 0x000A2423 File Offset: 0x000A0823
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x170006FB RID: 1787
	// (get) Token: 0x06001FFE RID: 8190 RVA: 0x000A242C File Offset: 0x000A082C
	// (set) Token: 0x06001FFF RID: 8191 RVA: 0x000A2434 File Offset: 0x000A0834
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x06002000 RID: 8192 RVA: 0x000A243D File Offset: 0x000A083D
	public CharacterMenusData GetData(CharacterDefinition characterDef)
	{
		return this.loadByName(characterDef.characterName);
	}

	// Token: 0x06002001 RID: 8193 RVA: 0x000A244C File Offset: 0x000A084C
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

	// Token: 0x06002002 RID: 8194 RVA: 0x000A2494 File Offset: 0x000A0894
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

	// Token: 0x06002003 RID: 8195 RVA: 0x000A254D File Offset: 0x000A094D
	private CharacterDefinition findDefById(CharacterID characterId)
	{
		return this.characterLists.GetCharacterDefinition(characterId);
	}

	// Token: 0x04001971 RID: 6513
	private Dictionary<string, CharacterMenusData> indexByName = new Dictionary<string, CharacterMenusData>();

	// Token: 0x04001972 RID: 6514
	private Dictionary<CharacterID, CharacterMenusData> indexByID = new Dictionary<CharacterID, CharacterMenusData>(default(CharacterIDComparer));
}
