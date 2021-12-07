// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterDataLoader : ICharacterDataLoader
{
	private sealed class _Preload_c__AnonStorey0
	{
		internal string name;

		internal CharacterDataLoader _this;

		internal void __m__0(CharacterData result)
		{
			this._this.onAsyncCharacterDataReady(this.name, result);
		}
	}

	private Dictionary<string, CharacterData> indexByName = new Dictionary<string, CharacterData>();

	private Dictionary<CharacterID, CharacterData> indexGameDataById = new Dictionary<CharacterID, CharacterData>(default(CharacterIDComparer));

	private Dictionary<string, List<Action>> pendingCallbacks = new Dictionary<string, List<Action>>();

	[Inject]
	public ILocalization localization
	{
		private get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		private get;
		set;
	}

	[Inject]
	public IResourceLoader asyncResourceLoader
	{
		private get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
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

	public void Preload(CharacterDefinition characterDef, Action callback)
	{
		CharacterDataLoader._Preload_c__AnonStorey0 _Preload_c__AnonStorey = new CharacterDataLoader._Preload_c__AnonStorey0();
		_Preload_c__AnonStorey._this = this;
		_Preload_c__AnonStorey.name = characterDef.characterName;
		if (this.indexByName.ContainsKey(_Preload_c__AnonStorey.name))
		{
			this.timer.UnblockThread(callback);
		}
		else
		{
			List<Action> list;
			if (!this.pendingCallbacks.TryGetValue(_Preload_c__AnonStorey.name, out list))
			{
				list = new List<Action>();
				this.pendingCallbacks[_Preload_c__AnonStorey.name] = list;
			}
			list.Add(callback);
			this.asyncResourceLoader.Load<CharacterData>("Character/" + _Preload_c__AnonStorey.name, new Action<CharacterData>(_Preload_c__AnonStorey.__m__0));
		}
	}

	private void onAsyncCharacterDataReady(string name, CharacterData result)
	{
		this.storeLoadedData(name, result);
		List<Action> list;
		if (this.pendingCallbacks.TryGetValue(name, out list))
		{
			this.pendingCallbacks.Remove(name);
			foreach (Action current in list)
			{
				current();
			}
		}
	}

	public CharacterData GetData(CharacterDefinition characterDef)
	{
		return this.loadByName(characterDef.characterName);
	}

	public CharacterData GetData(string characterName)
	{
		return this.loadByName(characterName);
	}

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

	private CharacterDefinition findDefById(CharacterID characterId)
	{
		return this.characterLists.GetCharacterDefinition(characterId);
	}

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

	private void initializeCharacterData(CharacterData characterData)
	{
		CharacterMoveSetData[] moveSets = characterData.moveSets;
		for (int i = 0; i < moveSets.Length; i++)
		{
			CharacterMoveSetData characterMoveSetData = moveSets[i];
			this.addGlobalMoves(characterMoveSetData);
			MoveData[] moves = characterMoveSetData.moves;
			for (int j = 0; j < moves.Length; j++)
			{
				MoveData moveData = moves[j];
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

	private void addGlobalMoves(CharacterMoveSetData moveSetData)
	{
		Dictionary<MoveLabel, bool> dictionary = new Dictionary<MoveLabel, bool>();
		List<MoveData> list = new List<MoveData>();
		MoveData[] moves = moveSetData.moves;
		for (int i = 0; i < moves.Length; i++)
		{
			MoveData moveData = moves[i];
			if (!(moveData == null))
			{
				list.Add(moveData);
				dictionary[moveData.label] = true;
			}
		}
		foreach (MoveData current in this.config.globalMoves.moves)
		{
			if (!(current == null))
			{
				if (!dictionary.ContainsKey(current.label))
				{
					list.Add(current);
				}
			}
		}
		moveSetData.moves = list.ToArray();
	}
}
