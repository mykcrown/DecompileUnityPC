using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000583 RID: 1411
public class CharacterDataLoader : ICharacterDataLoader
{
	// Token: 0x170006EF RID: 1775
	// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x000A1D82 File Offset: 0x000A0182
	// (set) Token: 0x06001FD5 RID: 8149 RVA: 0x000A1D8A File Offset: 0x000A018A
	[Inject]
	public ILocalization localization { private get; set; }

	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x000A1D93 File Offset: 0x000A0193
	// (set) Token: 0x06001FD7 RID: 8151 RVA: 0x000A1D9B File Offset: 0x000A019B
	[Inject]
	public ConfigData config { private get; set; }

	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x000A1DA4 File Offset: 0x000A01A4
	// (set) Token: 0x06001FD9 RID: 8153 RVA: 0x000A1DAC File Offset: 0x000A01AC
	[Inject]
	public ICharacterLists characterLists { private get; set; }

	// Token: 0x170006F2 RID: 1778
	// (get) Token: 0x06001FDA RID: 8154 RVA: 0x000A1DB5 File Offset: 0x000A01B5
	// (set) Token: 0x06001FDB RID: 8155 RVA: 0x000A1DBD File Offset: 0x000A01BD
	[Inject]
	public IResourceLoader asyncResourceLoader { private get; set; }

	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x06001FDC RID: 8156 RVA: 0x000A1DC6 File Offset: 0x000A01C6
	// (set) Token: 0x06001FDD RID: 8157 RVA: 0x000A1DCE File Offset: 0x000A01CE
	[Inject]
	public IMainThreadTimer timer { private get; set; }

	// Token: 0x170006F4 RID: 1780
	// (get) Token: 0x06001FDE RID: 8158 RVA: 0x000A1DD7 File Offset: 0x000A01D7
	// (set) Token: 0x06001FDF RID: 8159 RVA: 0x000A1DDF File Offset: 0x000A01DF
	[Inject]
	public GameDataManager gameDataManager { private get; set; }

	// Token: 0x06001FE0 RID: 8160 RVA: 0x000A1DE8 File Offset: 0x000A01E8
	public void Preload(CharacterDefinition characterDef, Action callback)
	{
		string name = characterDef.characterName;
		if (this.indexByName.ContainsKey(name))
		{
			this.timer.UnblockThread(callback);
		}
		else
		{
			List<Action> list;
			if (!this.pendingCallbacks.TryGetValue(name, out list))
			{
				list = new List<Action>();
				this.pendingCallbacks[name] = list;
			}
			list.Add(callback);
			this.asyncResourceLoader.Load<CharacterData>("Character/" + name, delegate(CharacterData result)
			{
				this.onAsyncCharacterDataReady(name, result);
			});
		}
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000A1E94 File Offset: 0x000A0294
	private void onAsyncCharacterDataReady(string name, CharacterData result)
	{
		this.storeLoadedData(name, result);
		List<Action> list;
		if (this.pendingCallbacks.TryGetValue(name, out list))
		{
			this.pendingCallbacks.Remove(name);
			foreach (Action action in list)
			{
				action();
			}
		}
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x000A1F14 File Offset: 0x000A0314
	public CharacterData GetData(CharacterDefinition characterDef)
	{
		return this.loadByName(characterDef.characterName);
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x000A1F22 File Offset: 0x000A0322
	public CharacterData GetData(string characterName)
	{
		return this.loadByName(characterName);
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x000A1F2C File Offset: 0x000A032C
	public CharacterData GetData(CharacterID id)
	{
		if (id == CharacterID.None)
		{
			return null;
		}
		if (this.indexGameDataById.ContainsKey(id))
		{
			return this.indexGameDataById[id];
		}
		CharacterDefinition characterDefinition = this.findDefById(id);
		return this.loadByName(characterDefinition.characterName);
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x000A1F73 File Offset: 0x000A0373
	private CharacterDefinition findDefById(CharacterID characterId)
	{
		return this.characterLists.GetCharacterDefinition(characterId);
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x000A1F84 File Offset: 0x000A0384
	private CharacterData loadByName(string name)
	{
		if (!this.indexByName.ContainsKey(name))
		{
			ProfilingUtil.BeginTimer();
			CharacterData characterData = Resources.Load<CharacterData>("Character/" + name);
			ProfilingUtil.EndTimer("LOAD CHARACTER DATA");
			this.storeLoadedData(name, characterData);
		}
		return this.indexByName[name];
	}

	// Token: 0x06001FE7 RID: 8167 RVA: 0x000A1FD8 File Offset: 0x000A03D8
	private void storeLoadedData(string name, CharacterData characterData)
	{
		if (!this.indexByName.ContainsKey(name))
		{
			this.indexByName[name] = characterData;
			this.initializeCharacterData(this.indexByName[name]);
			if (!this.indexByName[name].isPartner)
			{
				this.indexGameDataById[this.indexByName[name].characterID] = this.indexByName[name];
			}
		}
	}

	// Token: 0x06001FE8 RID: 8168 RVA: 0x000A2054 File Offset: 0x000A0454
	private void initializeCharacterData(CharacterData characterData)
	{
		foreach (CharacterMoveSetData characterMoveSetData in characterData.moveSets)
		{
			this.addGlobalMoves(characterMoveSetData);
			foreach (MoveData moveData in characterMoveSetData.moves)
			{
				if (moveData != null && moveData.label != MoveLabel.None)
				{
					if (moveData.inputProfile == null)
					{
					}
					if (moveData.inputProfile == null)
					{
						moveData.inputProfile = this.config.inputConfig.inputProfileMap.Find(MoveLabel.None);
					}
					moveData.activeInputProfile = moveData.inputProfile;
				}
			}
		}
	}

	// Token: 0x06001FE9 RID: 8169 RVA: 0x000A2110 File Offset: 0x000A0510
	private void addGlobalMoves(CharacterMoveSetData moveSetData)
	{
		Dictionary<MoveLabel, bool> dictionary = new Dictionary<MoveLabel, bool>();
		List<MoveData> list = new List<MoveData>();
		foreach (MoveData moveData in moveSetData.moves)
		{
			if (!(moveData == null))
			{
				list.Add(moveData);
				dictionary[moveData.label] = true;
			}
		}
		foreach (MoveData moveData2 in this.config.globalMoves.moves)
		{
			if (!(moveData2 == null))
			{
				if (!dictionary.ContainsKey(moveData2.label))
				{
					list.Add(moveData2);
				}
			}
		}
		moveSetData.moves = list.ToArray();
	}

	// Token: 0x04001941 RID: 6465
	private Dictionary<string, CharacterData> indexByName = new Dictionary<string, CharacterData>();

	// Token: 0x04001942 RID: 6466
	private Dictionary<CharacterID, CharacterData> indexGameDataById = new Dictionary<CharacterID, CharacterData>(default(CharacterIDComparer));

	// Token: 0x04001943 RID: 6467
	private Dictionary<string, List<Action>> pendingCallbacks = new Dictionary<string, List<Action>>();
}
