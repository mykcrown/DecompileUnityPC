// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using MemberwiseEquality;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Xft;

public class GameAssetPreloader : IDestroyable
{
	private sealed class _Preload_c__AnonStorey0
	{
		internal List<SkinDefinition> skinDefs;

		internal List<CharacterDefinition> characterDefs;

		internal PreloadContext context;

		internal GameAssetPreloader _this;

		internal void __m__0()
		{
			this._this.recursiveLoadSkins(this.skinDefs, 0, new Action(this.__m__1));
		}

		internal void __m__1()
		{
			this._this.onCharactersLoaded(this.characterDefs, this.context);
		}
	}

	private sealed class _recursiveLoadCharacters_c__AnonStorey1
	{
		internal List<CharacterDefinition> characterDefs;

		internal int i;

		internal Action callback;

		internal GameAssetPreloader _this;

		internal void __m__0()
		{
			this._this.recursiveLoadCharacters(this.characterDefs, this.i + 1, this.callback);
		}
	}

	private sealed class _recursiveLoadSkins_c__AnonStorey2
	{
		internal List<SkinDefinition> skinDefs;

		internal int i;

		internal Action callback;

		internal GameAssetPreloader _this;

		internal void __m__0()
		{
			this._this.recursiveLoadSkins(this.skinDefs, this.i + 1, this.callback);
		}
	}

	private sealed class _finishAssetPreload_c__AnonStorey3
	{
		internal GameObject temp;

		internal PreloadContext context;

		internal void __m__0()
		{
			if (this.temp != null)
			{
				this.temp.DestroySafe();
			}
			this.context.OneLoadComplete();
		}
	}

	private static int CREW_BATTLE_PLAYERS = 4;

	private PreloadContext currentContext;

	private GameLoadPayload payload;

	private static Stopwatch inProgressTimer = new Stopwatch();

	private static bool inProgress;

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IGameItemsPreloader itemsPreloader
	{
		get;
		set;
	}

	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel
	{
		get;
		set;
	}

	[Inject]
	public IPlayerTauntsFinder tauntsFinder
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataLoader characterDataLoader
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	public static bool InProgress
	{
		get
		{
			return GameAssetPreloader.inProgress;
		}
		set
		{
			if (value)
			{
				GameAssetPreloader.inProgressTimer.Reset();
				GameAssetPreloader.inProgressTimer.Start();
				if (!GameAssetPreloader.inProgress)
				{
					GameClient.Log(LogLevel.InfoVerbose, new object[]
					{
						"GameAssetPreloader::InProgress::Begin"
					});
				}
			}
			else if (GameAssetPreloader.inProgress && !value)
			{
				GameClient.Log(LogLevel.InfoBasic, new object[]
				{
					"GameAssetPreloader::InProgress::Complete: ",
					GameAssetPreloader.inProgressTimer.Elapsed.TotalSeconds
				});
			}
			GameAssetPreloader.inProgress = value;
		}
	}

	private DynamicObjectContainer dynamicObjects
	{
		get
		{
			return this.gameController.currentGame.DynamicObjects;
		}
	}

	public float Progress
	{
		get
		{
			if (this.currentContext != null)
			{
				return this.currentContext.GetProgress();
			}
			return 0f;
		}
	}

	public bool IsComplete
	{
		get
		{
			return this.currentContext != null && this.currentContext.IsComplete();
		}
	}

	public void Destroy()
	{
		if (this.currentContext != null)
		{
			this.currentContext.Cancel();
			this.currentContext = null;
		}
		GameAssetPreloader.InProgress = false;
	}

	public void Preload(GameLoadPayload payload)
	{
		GameAssetPreloader._Preload_c__AnonStorey0 _Preload_c__AnonStorey = new GameAssetPreloader._Preload_c__AnonStorey0();
		_Preload_c__AnonStorey._this = this;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		GameAssetPreloader.InProgress = true;
		_Preload_c__AnonStorey.context = new PreloadContext();
		_Preload_c__AnonStorey.context.particleQuality = this.userVideoSettingsModel.ParticleQuality;
		this.currentContext = _Preload_c__AnonStorey.context;
		this.payload = payload;
		_Preload_c__AnonStorey.context.multiplier = this.getCharacterMultiplier(payload);
		if (this.battleServerAPI.IsConnected)
		{
			_Preload_c__AnonStorey.context.multiplier *= 2;
		}
		this.preloadMemberwiseEquality();
		this.config.RegisterPreload(_Preload_c__AnonStorey.context);
		GameModeData dataByType = this.gameDataManager.GameModeData.GetDataByType(payload.battleConfig.mode);
		if (dataByType != null)
		{
			dataByType.RegisterPreload(_Preload_c__AnonStorey.context);
		}
		_Preload_c__AnonStorey.characterDefs = new List<CharacterDefinition>();
		_Preload_c__AnonStorey.skinDefs = new List<SkinDefinition>();
		IEnumerator enumerator = ((IEnumerable)payload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
				CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(playerSelectionInfo.characterID);
				if (characterDefinition != null)
				{
					CharacterDefinition[] linkedCharacters = this.characterDataHelper.GetLinkedCharacters(characterDefinition);
					for (int i = 0; i < linkedCharacters.Length; i++)
					{
						CharacterDefinition item = linkedCharacters[i];
						if (!_Preload_c__AnonStorey.characterDefs.Contains(item))
						{
							_Preload_c__AnonStorey.characterDefs.Add(item);
						}
					}
					SkinDefinition skinDefinition = this.characterDataHelper.GetSkinDefinition(playerSelectionInfo.characterID, playerSelectionInfo.skinKey);
					if (skinDefinition != null && !_Preload_c__AnonStorey.skinDefs.Contains(skinDefinition))
					{
						_Preload_c__AnonStorey.skinDefs.Add(skinDefinition);
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		this.recursiveLoadCharacters(_Preload_c__AnonStorey.characterDefs, 0, new Action(_Preload_c__AnonStorey.__m__0));
	}

	private void recursiveLoadCharacters(List<CharacterDefinition> characterDefs, int i, Action callback)
	{
		GameAssetPreloader._recursiveLoadCharacters_c__AnonStorey1 _recursiveLoadCharacters_c__AnonStorey = new GameAssetPreloader._recursiveLoadCharacters_c__AnonStorey1();
		_recursiveLoadCharacters_c__AnonStorey.characterDefs = characterDefs;
		_recursiveLoadCharacters_c__AnonStorey.i = i;
		_recursiveLoadCharacters_c__AnonStorey.callback = callback;
		_recursiveLoadCharacters_c__AnonStorey._this = this;
		if (_recursiveLoadCharacters_c__AnonStorey.i >= _recursiveLoadCharacters_c__AnonStorey.characterDefs.Count)
		{
			_recursiveLoadCharacters_c__AnonStorey.callback();
		}
		else
		{
			this.characterDataLoader.Preload(_recursiveLoadCharacters_c__AnonStorey.characterDefs[_recursiveLoadCharacters_c__AnonStorey.i], new Action(_recursiveLoadCharacters_c__AnonStorey.__m__0));
		}
	}

	private void recursiveLoadSkins(List<SkinDefinition> skinDefs, int i, Action callback)
	{
		GameAssetPreloader._recursiveLoadSkins_c__AnonStorey2 _recursiveLoadSkins_c__AnonStorey = new GameAssetPreloader._recursiveLoadSkins_c__AnonStorey2();
		_recursiveLoadSkins_c__AnonStorey.skinDefs = skinDefs;
		_recursiveLoadSkins_c__AnonStorey.i = i;
		_recursiveLoadSkins_c__AnonStorey.callback = callback;
		_recursiveLoadSkins_c__AnonStorey._this = this;
		if (_recursiveLoadSkins_c__AnonStorey.i >= _recursiveLoadSkins_c__AnonStorey.skinDefs.Count)
		{
			_recursiveLoadSkins_c__AnonStorey.callback();
		}
		else
		{
			this.skinDataManager.Preload(_recursiveLoadSkins_c__AnonStorey.skinDefs[_recursiveLoadSkins_c__AnonStorey.i], new Action(_recursiveLoadSkins_c__AnonStorey.__m__0));
		}
	}

	private void onCharactersLoaded(List<CharacterDefinition> characterDefs, PreloadContext context)
	{
		List<CharacterData> list = new List<CharacterData>();
		foreach (CharacterDefinition current in characterDefs)
		{
			CharacterData data = this.characterDataLoader.GetData(current);
			if (data != null)
			{
				data.RegisterPreload(context);
				if (!list.Contains(data))
				{
					list.Add(data);
				}
			}
		}
		for (int i = 0; i < this.payload.players.Length; i++)
		{
			PlayerNum playerNumFromInt = PlayerUtil.GetPlayerNumFromInt(i, true);
			PlayerSelectionInfo player = this.payload.players.GetPlayer(playerNumFromInt);
			if (!player.isSpectator)
			{
				List<MoveData> playerEmoteMoveData = this.tauntsFinder.GetPlayerEmoteMoveData(playerNumFromInt, player.characterID);
				foreach (MoveData current2 in playerEmoteMoveData)
				{
					current2.RegisterPreload(context);
				}
				List<HologramData> playerHologramData = this.tauntsFinder.GetPlayerHologramData(playerNumFromInt, player.characterID);
				foreach (HologramData current3 in playerHologramData)
				{
					current3.RegisterPreload(context);
				}
			}
		}
		if (this.config.warmAllShaders)
		{
			foreach (CharacterData current4 in list)
			{
				if (current4.shaderVariantFile.obj != null)
				{
					current4.shaderVariantFile.obj.WarmUp();
				}
			}
		}
		Util.StaticInitialize<Fixed[]>(SineTableData.SINE_TABLE);
		this.preloadAll(context, new Action(this.onGamePreloadComplete));
	}

	private int getCharacterMultiplier(GameLoadPayload payload)
	{
		if (payload.battleConfig.mode == GameMode.CrewBattle)
		{
			return GameAssetPreloader.CREW_BATTLE_PLAYERS;
		}
		int num = 0;
		IEnumerator enumerator = ((IEnumerable)payload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
				if (playerSelectionInfo.type != PlayerType.None && !playerSelectionInfo.isSpectator)
				{
					num++;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		if (num == 0)
		{
			throw new Exception("Multiplier calculated as 0.  Is the GameLoadPayload valid?");
		}
		return num;
	}

	private void onGamePreloadComplete()
	{
		this.preloadItems();
	}

	private void preloadItems()
	{
		this.itemsPreloader.Preload(this.payload, new Action(this.onPreloadItemsComplete));
	}

	private void onPreloadItemsComplete()
	{
		GameAssetPreloader.InProgress = false;
	}

	private void preloadMemberwiseEquality()
	{
		Type[] typesInNamespace = this.getTypesInNamespace(Assembly.GetExecutingAssembly());
		for (int i = 0; i < typesInNamespace.Length; i++)
		{
			Type type = typesInNamespace[i];
			if (!type.IsAbstract && type.IsSubclassOf(typeof(MemberwiseEqualityObject)))
			{
				Activator.CreateInstance(type);
			}
		}
	}

	private Type[] getTypesInNamespace(Assembly assembly)
	{
		return assembly.GetTypes();
	}

	private void addCharacters(CharacterData character, PreloadContext context)
	{
		if (character != null)
		{
			CharacterDefinition[] linkedCharacters = this.characterDataHelper.GetLinkedCharacters(character.characterDefinition);
			for (int i = 0; i < linkedCharacters.Length; i++)
			{
				CharacterDefinition characterDef = linkedCharacters[i];
				this.characterDataLoader.GetData(characterDef).RegisterPreload(context);
			}
		}
	}

	private void preloadAll(PreloadContext context, Action callback)
	{
		context.StartLoad(callback);
		foreach (KeyValuePair<PreloadDef, int> current in context.list)
		{
			this.preloadFromDef(current.Key, current.Value, context);
		}
	}

	private void preloadFromDef(PreloadDef def, int poolSize, PreloadContext context)
	{
		if (def.obj == null)
		{
			context.OneLoadComplete();
		}
		else
		{
			GameObject temp = null;
			switch (def.type)
			{
			case PreloadType.EFFECT:
				temp = this.dynamicObjects.InstantiateDynamicObject<Effect>(def.obj, poolSize * context.multiplier, true).gameObject;
				break;
			case PreloadType.ARTICLE:
				temp = this.dynamicObjects.PreloadDynamicObject<ArticleController>(def.obj, poolSize * context.multiplier).gameObject;
				break;
			case PreloadType.PROJECTILE:
				temp = this.dynamicObjects.PreloadDynamicObject<ProjectileController>(def.obj, poolSize * context.multiplier).gameObject;
				break;
			case PreloadType.WEAPON_TRAIL:
				temp = this.dynamicObjects.InstantiateDynamicObject<XWeaponTrail>(def.obj, poolSize * context.multiplier, true).gameObject;
				break;
			}
			this.finishAssetPreload(temp, context);
		}
	}

	private void finishAssetPreload(GameObject temp, PreloadContext context)
	{
		GameAssetPreloader._finishAssetPreload_c__AnonStorey3 _finishAssetPreload_c__AnonStorey = new GameAssetPreloader._finishAssetPreload_c__AnonStorey3();
		_finishAssetPreload_c__AnonStorey.temp = temp;
		_finishAssetPreload_c__AnonStorey.context = context;
		this.timer.SetTimeout(500, new Action(_finishAssetPreload_c__AnonStorey.__m__0));
	}
}
