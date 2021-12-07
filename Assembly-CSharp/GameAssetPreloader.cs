using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using FixedPoint;
using MemberwiseEquality;
using UnityEngine;
using Xft;

// Token: 0x0200048A RID: 1162
public class GameAssetPreloader : IDestroyable
{
	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06001908 RID: 6408 RVA: 0x00083509 File Offset: 0x00081909
	// (set) Token: 0x06001909 RID: 6409 RVA: 0x00083511 File Offset: 0x00081911
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x0600190A RID: 6410 RVA: 0x0008351A File Offset: 0x0008191A
	// (set) Token: 0x0600190B RID: 6411 RVA: 0x00083522 File Offset: 0x00081922
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x0600190C RID: 6412 RVA: 0x0008352B File Offset: 0x0008192B
	// (set) Token: 0x0600190D RID: 6413 RVA: 0x00083533 File Offset: 0x00081933
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x0600190E RID: 6414 RVA: 0x0008353C File Offset: 0x0008193C
	// (set) Token: 0x0600190F RID: 6415 RVA: 0x00083544 File Offset: 0x00081944
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x06001910 RID: 6416 RVA: 0x0008354D File Offset: 0x0008194D
	// (set) Token: 0x06001911 RID: 6417 RVA: 0x00083555 File Offset: 0x00081955
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06001912 RID: 6418 RVA: 0x0008355E File Offset: 0x0008195E
	// (set) Token: 0x06001913 RID: 6419 RVA: 0x00083566 File Offset: 0x00081966
	[Inject]
	public IGameItemsPreloader itemsPreloader { get; set; }

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06001914 RID: 6420 RVA: 0x0008356F File Offset: 0x0008196F
	// (set) Token: 0x06001915 RID: 6421 RVA: 0x00083577 File Offset: 0x00081977
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel { get; set; }

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x06001916 RID: 6422 RVA: 0x00083580 File Offset: 0x00081980
	// (set) Token: 0x06001917 RID: 6423 RVA: 0x00083588 File Offset: 0x00081988
	[Inject]
	public IPlayerTauntsFinder tauntsFinder { get; set; }

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x06001918 RID: 6424 RVA: 0x00083591 File Offset: 0x00081991
	// (set) Token: 0x06001919 RID: 6425 RVA: 0x00083599 File Offset: 0x00081999
	[Inject]
	public ICharacterDataLoader characterDataLoader { get; set; }

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x0600191A RID: 6426 RVA: 0x000835A2 File Offset: 0x000819A2
	// (set) Token: 0x0600191B RID: 6427 RVA: 0x000835AA File Offset: 0x000819AA
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x0600191C RID: 6428 RVA: 0x000835B3 File Offset: 0x000819B3
	// (set) Token: 0x0600191D RID: 6429 RVA: 0x000835BB File Offset: 0x000819BB
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x0600191E RID: 6430 RVA: 0x000835C4 File Offset: 0x000819C4
	// (set) Token: 0x0600191F RID: 6431 RVA: 0x000835CC File Offset: 0x000819CC
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06001920 RID: 6432 RVA: 0x000835D5 File Offset: 0x000819D5
	// (set) Token: 0x06001921 RID: 6433 RVA: 0x000835DD File Offset: 0x000819DD
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06001922 RID: 6434 RVA: 0x000835E6 File Offset: 0x000819E6
	// (set) Token: 0x06001923 RID: 6435 RVA: 0x000835F0 File Offset: 0x000819F0
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

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06001924 RID: 6436 RVA: 0x0008367E File Offset: 0x00081A7E
	private DynamicObjectContainer dynamicObjects
	{
		get
		{
			return this.gameController.currentGame.DynamicObjects;
		}
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x00083690 File Offset: 0x00081A90
	public void Destroy()
	{
		if (this.currentContext != null)
		{
			this.currentContext.Cancel();
			this.currentContext = null;
		}
		GameAssetPreloader.InProgress = false;
	}

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06001926 RID: 6438 RVA: 0x000836B5 File Offset: 0x00081AB5
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

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06001927 RID: 6439 RVA: 0x000836D3 File Offset: 0x00081AD3
	public bool IsComplete
	{
		get
		{
			return this.currentContext != null && this.currentContext.IsComplete();
		}
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x000836F0 File Offset: 0x00081AF0
	public void Preload(GameLoadPayload payload)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		GameAssetPreloader.InProgress = true;
		PreloadContext context = new PreloadContext();
		context.particleQuality = this.userVideoSettingsModel.ParticleQuality;
		this.currentContext = context;
		this.payload = payload;
		context.multiplier = this.getCharacterMultiplier(payload);
		if (this.battleServerAPI.IsConnected)
		{
			context.multiplier *= 2;
		}
		this.preloadMemberwiseEquality();
		this.config.RegisterPreload(context);
		GameModeData dataByType = this.gameDataManager.GameModeData.GetDataByType(payload.battleConfig.mode);
		if (dataByType != null)
		{
			dataByType.RegisterPreload(context);
		}
		List<CharacterDefinition> characterDefs = new List<CharacterDefinition>();
		List<SkinDefinition> skinDefs = new List<SkinDefinition>();
		IEnumerator enumerator = ((IEnumerable)payload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
				CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(playerSelectionInfo.characterID);
				if (characterDefinition != null)
				{
					foreach (CharacterDefinition item in this.characterDataHelper.GetLinkedCharacters(characterDefinition))
					{
						if (!characterDefs.Contains(item))
						{
							characterDefs.Add(item);
						}
					}
					SkinDefinition skinDefinition = this.characterDataHelper.GetSkinDefinition(playerSelectionInfo.characterID, playerSelectionInfo.skinKey);
					if (skinDefinition != null && !skinDefs.Contains(skinDefinition))
					{
						skinDefs.Add(skinDefinition);
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
		this.recursiveLoadCharacters(characterDefs, 0, delegate
		{
			this.recursiveLoadSkins(skinDefs, 0, delegate
			{
				this.onCharactersLoaded(characterDefs, context);
			});
		});
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x00083904 File Offset: 0x00081D04
	private void recursiveLoadCharacters(List<CharacterDefinition> characterDefs, int i, Action callback)
	{
		if (i >= characterDefs.Count)
		{
			callback();
		}
		else
		{
			this.characterDataLoader.Preload(characterDefs[i], delegate
			{
				this.recursiveLoadCharacters(characterDefs, i + 1, callback);
			});
		}
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x00083984 File Offset: 0x00081D84
	private void recursiveLoadSkins(List<SkinDefinition> skinDefs, int i, Action callback)
	{
		if (i >= skinDefs.Count)
		{
			callback();
		}
		else
		{
			this.skinDataManager.Preload(skinDefs[i], delegate
			{
				this.recursiveLoadSkins(skinDefs, i + 1, callback);
			});
		}
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x00083A04 File Offset: 0x00081E04
	private void onCharactersLoaded(List<CharacterDefinition> characterDefs, PreloadContext context)
	{
		List<CharacterData> list = new List<CharacterData>();
		foreach (CharacterDefinition characterDef in characterDefs)
		{
			CharacterData data = this.characterDataLoader.GetData(characterDef);
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
				foreach (MoveData moveData in playerEmoteMoveData)
				{
					moveData.RegisterPreload(context);
				}
				List<HologramData> playerHologramData = this.tauntsFinder.GetPlayerHologramData(playerNumFromInt, player.characterID);
				foreach (HologramData hologramData in playerHologramData)
				{
					hologramData.RegisterPreload(context);
				}
			}
		}
		if (this.config.warmAllShaders)
		{
			foreach (CharacterData characterData in list)
			{
				if (characterData.shaderVariantFile.obj != null)
				{
					characterData.shaderVariantFile.obj.WarmUp();
				}
			}
		}
		Util.StaticInitialize<Fixed[]>(SineTableData.SINE_TABLE);
		this.preloadAll(context, new Action(this.onGamePreloadComplete));
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x00083C34 File Offset: 0x00082034
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
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
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

	// Token: 0x0600192D RID: 6445 RVA: 0x00083CD8 File Offset: 0x000820D8
	private void onGamePreloadComplete()
	{
		this.preloadItems();
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x00083CE0 File Offset: 0x000820E0
	private void preloadItems()
	{
		this.itemsPreloader.Preload(this.payload, new Action(this.onPreloadItemsComplete));
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x00083CFF File Offset: 0x000820FF
	private void onPreloadItemsComplete()
	{
		GameAssetPreloader.InProgress = false;
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x00083D08 File Offset: 0x00082108
	private void preloadMemberwiseEquality()
	{
		foreach (Type type in this.getTypesInNamespace(Assembly.GetExecutingAssembly()))
		{
			if (!type.IsAbstract && type.IsSubclassOf(typeof(MemberwiseEqualityObject)))
			{
				Activator.CreateInstance(type);
			}
		}
	}

	// Token: 0x06001931 RID: 6449 RVA: 0x00083D60 File Offset: 0x00082160
	private Type[] getTypesInNamespace(Assembly assembly)
	{
		return assembly.GetTypes();
	}

	// Token: 0x06001932 RID: 6450 RVA: 0x00083D68 File Offset: 0x00082168
	private void addCharacters(CharacterData character, PreloadContext context)
	{
		if (character != null)
		{
			foreach (CharacterDefinition characterDef in this.characterDataHelper.GetLinkedCharacters(character.characterDefinition))
			{
				this.characterDataLoader.GetData(characterDef).RegisterPreload(context);
			}
		}
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x00083DC0 File Offset: 0x000821C0
	private void preloadAll(PreloadContext context, Action callback)
	{
		context.StartLoad(callback);
		foreach (KeyValuePair<PreloadDef, int> keyValuePair in context.list)
		{
			this.preloadFromDef(keyValuePair.Key, keyValuePair.Value, context);
		}
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x00083E34 File Offset: 0x00082234
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

	// Token: 0x06001935 RID: 6453 RVA: 0x00083F1C File Offset: 0x0008231C
	private void finishAssetPreload(GameObject temp, PreloadContext context)
	{
		this.timer.SetTimeout(500, delegate
		{
			if (temp != null)
			{
				temp.DestroySafe();
			}
			context.OneLoadComplete();
		});
	}

	// Token: 0x040012F0 RID: 4848
	private static int CREW_BATTLE_PLAYERS = 4;

	// Token: 0x040012FE RID: 4862
	private PreloadContext currentContext;

	// Token: 0x040012FF RID: 4863
	private GameLoadPayload payload;

	// Token: 0x04001300 RID: 4864
	private static Stopwatch inProgressTimer = new Stopwatch();

	// Token: 0x04001301 RID: 4865
	private static bool inProgress;
}
