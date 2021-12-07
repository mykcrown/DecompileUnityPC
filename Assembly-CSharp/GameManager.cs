using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FixedPoint;
using IconsServer;
using RollbackDebug;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000473 RID: 1139
public class GameManager : MonoBehaviour, IRollbackStateOwner, IRollbackGameClient, IFrameOwner, IPlayerLookup, IGame, IModeOwner, IStageTriggerDependency, IRollbackClient, IEndableClient
{
	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06001827 RID: 6183 RVA: 0x0008056B File Offset: 0x0007E96B
	// (set) Token: 0x06001828 RID: 6184 RVA: 0x00080573 File Offset: 0x0007E973
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06001829 RID: 6185 RVA: 0x0008057C File Offset: 0x0007E97C
	// (set) Token: 0x0600182A RID: 6186 RVA: 0x00080584 File Offset: 0x0007E984
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x0600182B RID: 6187 RVA: 0x0008058D File Offset: 0x0007E98D
	// (set) Token: 0x0600182C RID: 6188 RVA: 0x00080595 File Offset: 0x0007E995
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x0600182D RID: 6189 RVA: 0x0008059E File Offset: 0x0007E99E
	// (set) Token: 0x0600182E RID: 6190 RVA: 0x000805A6 File Offset: 0x0007E9A6
	[Inject]
	public IGameTauntsSetup gameTauntsSetup { get; set; }

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x0600182F RID: 6191 RVA: 0x000805AF File Offset: 0x0007E9AF
	// (set) Token: 0x06001830 RID: 6192 RVA: 0x000805B7 File Offset: 0x0007E9B7
	[Inject]
	public NetGraphVisualizer netGraph { get; set; }

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06001831 RID: 6193 RVA: 0x000805C0 File Offset: 0x0007E9C0
	// (set) Token: 0x06001832 RID: 6194 RVA: 0x000805C8 File Offset: 0x0007E9C8
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x06001833 RID: 6195 RVA: 0x000805D1 File Offset: 0x0007E9D1
	// (set) Token: 0x06001834 RID: 6196 RVA: 0x000805D9 File Offset: 0x0007E9D9
	[Inject]
	public IDevConsole devConsole { get; set; }

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x06001835 RID: 6197 RVA: 0x000805E2 File Offset: 0x0007E9E2
	// (set) Token: 0x06001836 RID: 6198 RVA: 0x000805EA File Offset: 0x0007E9EA
	[Inject]
	public IRollbackDebugLayer rollbackDebug { get; set; }

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x06001837 RID: 6199 RVA: 0x000805F3 File Offset: 0x0007E9F3
	// (set) Token: 0x06001838 RID: 6200 RVA: 0x000805FB File Offset: 0x0007E9FB
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x06001839 RID: 6201 RVA: 0x00080604 File Offset: 0x0007EA04
	// (set) Token: 0x0600183A RID: 6202 RVA: 0x0008060C File Offset: 0x0007EA0C
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x0600183B RID: 6203 RVA: 0x00080615 File Offset: 0x0007EA15
	// (set) Token: 0x0600183C RID: 6204 RVA: 0x0008061D File Offset: 0x0007EA1D
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x0600183D RID: 6205 RVA: 0x00080626 File Offset: 0x0007EA26
	// (set) Token: 0x0600183E RID: 6206 RVA: 0x0008062E File Offset: 0x0007EA2E
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x0600183F RID: 6207 RVA: 0x00080637 File Offset: 0x0007EA37
	// (set) Token: 0x06001840 RID: 6208 RVA: 0x0008063F File Offset: 0x0007EA3F
	[Inject]
	public IPerformanceTracker performanceTracker { get; set; }

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06001841 RID: 6209 RVA: 0x00080648 File Offset: 0x0007EA48
	// (set) Token: 0x06001842 RID: 6210 RVA: 0x00080650 File Offset: 0x0007EA50
	[Inject]
	public HitCollisionManager Hits { get; set; }

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06001843 RID: 6211 RVA: 0x00080659 File Offset: 0x0007EA59
	// (set) Token: 0x06001844 RID: 6212 RVA: 0x00080661 File Offset: 0x0007EA61
	[Inject]
	public AnnouncementHelper announcements { get; set; }

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06001845 RID: 6213 RVA: 0x0008066A File Offset: 0x0007EA6A
	// (set) Token: 0x06001846 RID: 6214 RVA: 0x00080672 File Offset: 0x0007EA72
	[Inject]
	public IReplaySystem replaySystem { get; set; }

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06001847 RID: 6215 RVA: 0x0008067B File Offset: 0x0007EA7B
	// (set) Token: 0x06001848 RID: 6216 RVA: 0x00080683 File Offset: 0x0007EA83
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06001849 RID: 6217 RVA: 0x0008068C File Offset: 0x0007EA8C
	// (set) Token: 0x0600184A RID: 6218 RVA: 0x00080694 File Offset: 0x0007EA94
	[Inject]
	public IServerConnectionManager serverConnectionManager { get; set; }

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x0600184B RID: 6219 RVA: 0x0008069D File Offset: 0x0007EA9D
	// (set) Token: 0x0600184C RID: 6220 RVA: 0x000806A5 File Offset: 0x0007EAA5
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x0600184D RID: 6221 RVA: 0x000806AE File Offset: 0x0007EAAE
	// (set) Token: 0x0600184E RID: 6222 RVA: 0x000806B6 File Offset: 0x0007EAB6
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x0600184F RID: 6223 RVA: 0x000806BF File Offset: 0x0007EABF
	// (set) Token: 0x06001850 RID: 6224 RVA: 0x000806C7 File Offset: 0x0007EAC7
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x06001851 RID: 6225 RVA: 0x000806D0 File Offset: 0x0007EAD0
	// (set) Token: 0x06001852 RID: 6226 RVA: 0x000806D8 File Offset: 0x0007EAD8
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel { get; set; }

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x06001853 RID: 6227 RVA: 0x000806E1 File Offset: 0x0007EAE1
	// (set) Token: 0x06001854 RID: 6228 RVA: 0x000806E9 File Offset: 0x0007EAE9
	[Inject]
	public IMatchDeepLogging deepLogging { get; set; }

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x06001855 RID: 6229 RVA: 0x000806F2 File Offset: 0x0007EAF2
	// (set) Token: 0x06001856 RID: 6230 RVA: 0x000806FA File Offset: 0x0007EAFA
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x06001857 RID: 6231 RVA: 0x00080703 File Offset: 0x0007EB03
	// (set) Token: 0x06001858 RID: 6232 RVA: 0x0008070B File Offset: 0x0007EB0B
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x06001859 RID: 6233 RVA: 0x00080714 File Offset: 0x0007EB14
	// (set) Token: 0x0600185A RID: 6234 RVA: 0x0008071C File Offset: 0x0007EB1C
	[Inject]
	public DebugKeys debugKeys { get; set; }

	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x0600185B RID: 6235 RVA: 0x00080725 File Offset: 0x0007EB25
	public int Frame
	{
		get
		{
			return (!(this.FrameController == null)) ? this.FrameController.Frame : 0;
		}
	}

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x0600185C RID: 6236 RVA: 0x00080749 File Offset: 0x0007EB49
	// (set) Token: 0x0600185D RID: 6237 RVA: 0x00080751 File Offset: 0x0007EB51
	public int GameStartInputFrame
	{
		get
		{
			return this.gameStartInputFrame;
		}
		set
		{
			this.gameStartInputFrame = value;
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x0600185E RID: 6238 RVA: 0x0008075A File Offset: 0x0007EB5A
	public int AllowGameplayInputsFrame
	{
		get
		{
			return this.allowGameplayInputsFrame;
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x0600185F RID: 6239 RVA: 0x00080762 File Offset: 0x0007EB62
	// (set) Token: 0x06001860 RID: 6240 RVA: 0x0008076A File Offset: 0x0007EB6A
	public string MatchID { get; private set; }

	// Token: 0x06001861 RID: 6241 RVA: 0x00080774 File Offset: 0x0007EB74
	public bool LoadState(RollbackStateContainer container)
	{
		if (container == null || container.Count == 0)
		{
			UnityEngine.Debug.LogError("Attempted to load state with no content!");
			return false;
		}
		int frame = this.Frame;
		container.ReadState<GameManagerState>(ref this.gameManagerState);
		this.FrameController.LoadState(container);
		this.Stage.LoadState(container);
		this.playerSpawner.LoadState(container);
		this.Hits.LoadState(container);
		this.statsTracker.LoadState(container);
		this.ComboManager.LoadState(container);
		this.announcements.LoadState(container);
		int num = frame - this.Frame;
		if (!this.rollbackCounts.ContainsKey(num))
		{
			this.rollbackCounts.Add(num, 1);
		}
		else
		{
			Dictionary<int, int> dictionary;
			int key;
			(dictionary = this.rollbackCounts)[key = num] = dictionary[key] + 1;
		}
		if (this.UI.DebugTextEnabled)
		{
			this.UI.AddDebugTextEvent(string.Concat(new object[]
			{
				"Rolled back ",
				frame - this.Frame,
				" frames (",
				frame,
				"->",
				this.Frame,
				")"
			}));
		}
		for (int i = 0; i < this.CharacterControllers.Count; i++)
		{
			PlayerController playerController = this.CharacterControllers[i];
			playerController.LoadState(container);
		}
		for (int j = 0; j < this.playerReferences.Count; j++)
		{
			PlayerReference playerReference = this.playerReferences[j];
			playerReference.LoadState(container);
		}
		this.CurrentGameMode.LoadState(container);
		this.DynamicObjects.LoadState(container);
		this.Camera.LoadState(container);
		return true;
	}

	// Token: 0x06001862 RID: 6242 RVA: 0x00080958 File Offset: 0x0007ED58
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.Clear();
		container.WriteState(this.rollbackStatePooling.Clone<GameManagerState>(this.gameManagerState));
		this.FrameController.ExportState(ref container);
		this.Stage.ExportState(ref container);
		this.playerSpawner.ExportState(ref container);
		this.Hits.ExportState(ref container);
		this.statsTracker.ExportState(ref container);
		this.ComboManager.ExportState(ref container);
		this.announcements.ExportState(ref container);
		for (int i = 0; i < this.CharacterControllers.Count; i++)
		{
			PlayerController playerController = this.CharacterControllers[i];
			playerController.ExportState(ref container);
		}
		for (int j = 0; j < this.playerReferences.Count; j++)
		{
			PlayerReference playerReference = this.playerReferences[j];
			playerReference.ExportState(ref container);
		}
		this.CurrentGameMode.ExportState(ref container);
		this.DynamicObjects.ExportState(ref container);
		this.Camera.ExportState(ref container);
		return true;
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x06001863 RID: 6243 RVA: 0x00080A6A File Offset: 0x0007EE6A
	// (set) Token: 0x06001864 RID: 6244 RVA: 0x00080A72 File Offset: 0x0007EE72
	public StageSceneData Stage { get; private set; }

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x06001865 RID: 6245 RVA: 0x00080A7B File Offset: 0x0007EE7B
	// (set) Token: 0x06001866 RID: 6246 RVA: 0x00080A83 File Offset: 0x0007EE83
	public PhysicsSimulator Physics { get; private set; }

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x06001867 RID: 6247 RVA: 0x00080A8C File Offset: 0x0007EE8C
	// (set) Token: 0x06001868 RID: 6248 RVA: 0x00080A94 File Offset: 0x0007EE94
	public PhysicsWorld PhysicsWorld { get; private set; }

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x06001869 RID: 6249 RVA: 0x00080A9D File Offset: 0x0007EE9D
	// (set) Token: 0x0600186A RID: 6250 RVA: 0x00080AA5 File Offset: 0x0007EEA5
	public FrameController FrameController { get; private set; }

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x0600186B RID: 6251 RVA: 0x00080AAE File Offset: 0x0007EEAE
	// (set) Token: 0x0600186C RID: 6252 RVA: 0x00080AB6 File Offset: 0x0007EEB6
	public GameLog Log { get; private set; }

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x0600186D RID: 6253 RVA: 0x00080ABF File Offset: 0x0007EEBF
	// (set) Token: 0x0600186E RID: 6254 RVA: 0x00080AC7 File Offset: 0x0007EEC7
	public GameObject GameContainer { get; private set; }

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x0600186F RID: 6255 RVA: 0x00080AD0 File Offset: 0x0007EED0
	// (set) Token: 0x06001870 RID: 6256 RVA: 0x00080AD8 File Offset: 0x0007EED8
	public DynamicObjectContainer DynamicObjects { get; private set; }

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06001871 RID: 6257 RVA: 0x00080AE1 File Offset: 0x0007EEE1
	// (set) Token: 0x06001872 RID: 6258 RVA: 0x00080AE9 File Offset: 0x0007EEE9
	public PointAudio PointAudio { get; private set; }

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06001873 RID: 6259 RVA: 0x00080AF2 File Offset: 0x0007EEF2
	// (set) Token: 0x06001874 RID: 6260 RVA: 0x00080AFA File Offset: 0x0007EEFA
	public bool IsRunning { get; private set; }

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06001875 RID: 6261 RVA: 0x00080B03 File Offset: 0x0007EF03
	public bool IsPaused
	{
		get
		{
			return this.PausedPort != null;
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06001876 RID: 6262 RVA: 0x00080B11 File Offset: 0x0007EF11
	public PlayerNum PausedPlayer
	{
		get
		{
			return this.userInputManager.GetPlayerNum(this.PausedPort);
		}
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06001877 RID: 6263 RVA: 0x00080B24 File Offset: 0x0007EF24
	// (set) Token: 0x06001878 RID: 6264 RVA: 0x00080B2C File Offset: 0x0007EF2C
	public PlayerInputPort PausedPort { get; private set; }

	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x06001879 RID: 6265 RVA: 0x00080B35 File Offset: 0x0007EF35
	// (set) Token: 0x0600187A RID: 6266 RVA: 0x00080B3D File Offset: 0x0007EF3D
	public int PlayerCount { get; private set; }

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x0600187B RID: 6267 RVA: 0x00080B46 File Offset: 0x0007EF46
	// (set) Token: 0x0600187C RID: 6268 RVA: 0x00080B4E File Offset: 0x0007EF4E
	public BattleSettings BattleSettings { get; private set; }

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x0600187D RID: 6269 RVA: 0x00080B57 File Offset: 0x0007EF57
	// (set) Token: 0x0600187E RID: 6270 RVA: 0x00080B5F File Offset: 0x0007EF5F
	public bool IsRollingBack { get; set; }

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x0600187F RID: 6271 RVA: 0x00080B68 File Offset: 0x0007EF68
	// (set) Token: 0x06001880 RID: 6272 RVA: 0x00080B70 File Offset: 0x0007EF70
	public CameraController Camera { get; private set; }

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06001881 RID: 6273 RVA: 0x00080B79 File Offset: 0x0007EF79
	public UIManager UI
	{
		get
		{
			return this.Client.UI;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06001882 RID: 6274 RVA: 0x00080B86 File Offset: 0x0007EF86
	public AudioManager Audio
	{
		get
		{
			return this.Client.Audio;
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06001883 RID: 6275 RVA: 0x00080B93 File Offset: 0x0007EF93
	// (set) Token: 0x06001884 RID: 6276 RVA: 0x00080B9B File Offset: 0x0007EF9B
	public GameClient Client { get; private set; }

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06001885 RID: 6277 RVA: 0x00080BA4 File Offset: 0x0007EFA4
	// (set) Token: 0x06001886 RID: 6278 RVA: 0x00080BAC File Offset: 0x0007EFAC
	public CapsulePool CapsulePool { get; private set; }

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06001887 RID: 6279 RVA: 0x00080BB5 File Offset: 0x0007EFB5
	public GameLoadPayload GameConfig
	{
		get
		{
			return this.gameConfig;
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x06001888 RID: 6280 RVA: 0x00080BBD File Offset: 0x0007EFBD
	public GameDataManager GameData
	{
		get
		{
			return this.Client.GameData;
		}
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06001889 RID: 6281 RVA: 0x00080BCA File Offset: 0x0007EFCA
	public GameMode Mode
	{
		get
		{
			return this.BattleSettings.mode;
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x0600188A RID: 6282 RVA: 0x00080BD7 File Offset: 0x0007EFD7
	// (set) Token: 0x0600188B RID: 6283 RVA: 0x00080BDF File Offset: 0x0007EFDF
	public GameModeData ModeData { get; private set; }

	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x0600188C RID: 6284 RVA: 0x00080BE8 File Offset: 0x0007EFE8
	// (set) Token: 0x0600188D RID: 6285 RVA: 0x00080BF0 File Offset: 0x0007EFF0
	public IGameMode CurrentGameMode { get; private set; }

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x0600188E RID: 6286 RVA: 0x00080BF9 File Offset: 0x0007EFF9
	public GameModeSettings GameModeSettings
	{
		get
		{
			return this.ModeData.settings;
		}
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x0600188F RID: 6287 RVA: 0x00080C06 File Offset: 0x0007F006
	public bool StartedGame
	{
		get
		{
			return this.gameManagerState.gameStarted;
		}
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06001890 RID: 6288 RVA: 0x00080C13 File Offset: 0x0007F013
	// (set) Token: 0x06001891 RID: 6289 RVA: 0x00080C1B File Offset: 0x0007F01B
	public bool EndedGame { get; private set; }

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x06001892 RID: 6290 RVA: 0x00080C24 File Offset: 0x0007F024
	public bool IsTrainingMode
	{
		get
		{
			return this.Mode == GameMode.Training;
		}
	}

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x06001893 RID: 6291 RVA: 0x00080C2F File Offset: 0x0007F02F
	public bool IsNetworkGame
	{
		get
		{
			return this.battleServerAPI.IsConnected;
		}
	}

	// Token: 0x1700050B RID: 1291
	// (get) Token: 0x06001894 RID: 6292 RVA: 0x00080C3C File Offset: 0x0007F03C
	public PlayerSpawner PlayerSpawner
	{
		get
		{
			return this.playerSpawner;
		}
	}

	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x06001895 RID: 6293 RVA: 0x00080C44 File Offset: 0x0007F044
	// (set) Token: 0x06001896 RID: 6294 RVA: 0x00080C4C File Offset: 0x0007F04C
	public ComboManager ComboManager { get; private set; }

	// Token: 0x1700050D RID: 1293
	// (get) Token: 0x06001897 RID: 6295 RVA: 0x00080C55 File Offset: 0x0007F055
	// (set) Token: 0x06001898 RID: 6296 RVA: 0x00080C5D File Offset: 0x0007F05D
	public ObjectPoolManager ObjectPools { get; private set; }

	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x06001899 RID: 6297 RVA: 0x00080C66 File Offset: 0x0007F066
	public bool IsPauseEnabled
	{
		get
		{
			return this.BattleSettings.IsPauseEnabled && (this.ModeData == null || !this.ModeData.settings.disablePausing);
		}
	}

	// Token: 0x0600189A RID: 6298 RVA: 0x00080CA2 File Offset: 0x0007F0A2
	private void Awake()
	{
		base.tag = Tags.GameManager;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.sceneLoaded += this.onSceneLoaded;
	}

	// Token: 0x0600189B RID: 6299 RVA: 0x00080CCC File Offset: 0x0007F0CC
	public void Initialize(GameClient client)
	{
		this.Client = client;
		this.gameManagerState.gameVersion = BuildConfigUtil.GetCompareVersion(this.config);
		this.GameContainer = new GameObject("Game");
		this.GameContainer.transform.SetParent(base.transform);
		this.DynamicObjects = new GameObject("DynamicObjects").AddComponent<DynamicObjectContainer>();
		this.injector.Inject(this.DynamicObjects);
		this.DynamicObjects.transform.SetParent(this.GameContainer.transform, false);
		this.inactiveObjects = new GameObject("InactiveObjects");
		UnityEngine.Object.DontDestroyOnLoad(this.inactiveObjects);
		this.inactiveObjects.SetActive(false);
		this.PointAudio = new GameObject("PointAudio").AddComponent<PointAudio>();
		this.injector.Inject(this.PointAudio);
		this.PointAudio.transform.SetParent(this.GameContainer.transform, false);
		this.PointAudio.Init();
		this.ObjectPools = this.injector.GetInstance<ObjectPoolManager>();
		this.ObjectPools.Init(this.DynamicObjects.transform, this.inactiveObjects.transform);
		this.Camera = this.GameContainer.AddComponent<CameraController>();
		this.injector.Inject(this.Camera);
		this.Physics = this.injector.GetInstance<PhysicsSimulator>();
		this.PhysicsWorld = new PhysicsWorld(this.devConsole);
		this.events.Subscribe(typeof(GameEndEvent), new Events.EventHandler(this.onGameEnd));
		this.events.Subscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
		this.events.Subscribe(typeof(DebugDesyncEvent), new Events.EventHandler(this.onDebugDesync));
		this.grabManager = new GrabManager(this);
		this.PausedPort = null;
		Fixed one = (!this.devConfig.disableCountdown) ? this.config.uiuxSettings.countdownIntervalSeconds : Fixed.Zero;
		this.gameStartFrame = (int)((this.config.uiuxSettings.countdownAmt + 1) * one * WTime.fps);
		this.allowGameplayInputsFrame = this.gameStartFrame - this.config.inputConfig.inputBufferFrames;
		this.Log = new GameLog(this.events);
		if (this.config.uiuxSettings.emotiveStartup)
		{
			this.GameStartInputFrame = this.config.uiuxSettings.inputFramesBuffer - this.config.inputConfig.inputBufferFrames;
		}
		else
		{
			this.GameStartInputFrame = this.allowGameplayInputsFrame;
		}
		this.localInputsBuffer.Clear();
	}

	// Token: 0x0600189C RID: 6300 RVA: 0x00080F9C File Offset: 0x0007F39C
	private void onSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.Stage = UnityEngine.Object.FindObjectOfType<StageSceneData>();
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x00080FAC File Offset: 0x0007F3AC
	public void StartGame(GameLoadPayload payload)
	{
		this.updatePlayerTeams(payload);
		this.updatePlayerPorts(payload);
		this.stageData = this.gameDataManager.StageData.GetDataByID(payload.stage);
		if (this.stageData != null && this.stageData.stageType == StageType.Random)
		{
			this.stageData = this.gameDataManager.StageData.GetDataByID(payload.stagePayloadData.lastRandomStage);
		}
		if (this.IsNetworkGame)
		{
			this.MatchID = this.battleServerAPI.MatchID.ToString();
		}
		else
		{
			this.MatchID = string.Format("{0}:{1}", (!this.serverConnectionManager.IsConnectedToNexus) ? Environment.UserName : this.iconsServerAPI.Username, Guid.NewGuid());
		}
		string empty = string.Empty;
		if (!this.validateGameLoadPayload(payload, ref empty))
		{
			UnityEngine.Debug.LogError("Invalid game load payload:\n" + empty);
			return;
		}
		this.gameConfig = payload;
		this.gameConfig.isOnlineGame = this.battleServerAPI.IsConnected;
		this.BattleSettings = payload.battleConfig;
		this.ModeData = this.GameData.GameModeData.GetDataByType(this.BattleSettings.mode);
		this.FrameController = this.injector.CreateComponentWithGameObject<FrameController>(null);
		this.statsTracker = new StatsTracker();
		this.injector.Inject(this.statsTracker);
		this.statsTracker.Init(this.gameConfig.players);
		if (this.Stage == null)
		{
			UnityEngine.Debug.LogWarning("GameManager failed to find stage during the scene load. Attempting again.");
			this.Stage = UnityEngine.Object.FindObjectOfType<StageSceneData>();
		}
		this.injector.Inject(this.Stage);
		this.Stage.Startup();
		IEnumerator enumerator = ((IEnumerable)payload.players).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
				if (playerSelectionInfo.type == PlayerType.Human)
				{
					this.events.Broadcast(new SetPlayerTypeRequest(playerSelectionInfo.playerNum, PlayerType.Human, true));
				}
				else if (playerSelectionInfo.type == PlayerType.CPU)
				{
					this.events.Broadcast(new SetPlayerTypeRequest(playerSelectionInfo.playerNum, PlayerType.CPU, true));
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
		for (int i = 0; i < payload.players.Length; i++)
		{
			PlayerSelectionInfo playerSelectionInfo2 = payload.players[i];
			if (playerSelectionInfo2.type != PlayerType.None)
			{
				this.createPlayerReference(playerSelectionInfo2);
			}
		}
		this.CurrentGameMode = this.injector.GetInstance<IGameMode>(this.BattleSettings.mode);
		if (this.CurrentGameMode != null)
		{
			this.CurrentGameMode.Init(this.ModeData, this.config, this.BattleSettings, this.events, this.playerReferences, this);
			this.playerSpawner = this.CurrentGameMode.CreateSpawner(this, this.playerReferenceMap);
			if (this.playerSpawner == null)
			{
				this.playerSpawner = this.injector.GetInstance<PlayerSpawner>();
				this.playerSpawner.Init(this.events, this.Stage, this.ModeData, this.config.respawnConfig, this.playerReferenceMap, this.playerReferences);
			}
			this.playerSpawner.PerformInitialSpawn(payload, this, new Action<PlayerReference, SpawnPointBase>(this.spawnPlayerReference));
			for (int j = 0; j < this.CharacterControllers.Count; j++)
			{
				this.CharacterControllers[j].LoadSharedAnimations(this.playerReferences);
			}
			this.inputsThisFrameBuffer = new RollbackInput[this.CharacterControllers.Count];
			this.rollbackInputPool = new GenericResetObjectPool<RollbackInput>(0, () => new RollbackInput(), null);
			this.applyQualitySettings();
			this.Camera.Init();
			this.PlayerCount = this.playerReferences.Count;
			this.IsRunning = true;
			this.ComboManager = this.injector.GetInstance<ComboManager>();
			this.ComboManager.Setup(this.playerReferences);
			List<RollbackPlayerData> list = new List<RollbackPlayerData>();
			foreach (PlayerReference playerReference in this.playerReferences)
			{
				int intFromPlayerNum = PlayerUtil.GetIntFromPlayerNum(playerReference.PlayerNum, false);
				bool flag = this.battleServerAPI.IsLocalPlayer(playerReference.PlayerNum);
				if (flag)
				{
					this.localPlayers[playerReference.PlayerNum] = true;
				}
				RollbackPlayerData rollbackPlayerData = new RollbackPlayerData();
				rollbackPlayerData.playerID = intFromPlayerNum;
				rollbackPlayerData.userID = playerReference.PlayerInfo.userID;
				rollbackPlayerData.isSpectator = playerReference.PlayerInfo.isSpectator;
				rollbackPlayerData.isLocal = flag;
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Add player ",
					playerReference.PlayerInfo.curProfile.profileName,
					" ",
					rollbackPlayerData.playerID,
					" ",
					rollbackPlayerData.userID
				}));
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Local: ",
					flag,
					", Spectator: ",
					rollbackPlayerData.isSpectator
				}));
				list.Add(rollbackPlayerData);
			}
			RollbackSettings rollbackSettings = new RollbackSettings(this.PlayerCount, list, this.replaySystem, this.rollbackInputPool, this.config.networkSettings);
			rollbackSettings.UpdatePerMatchNetworkSettings(this.battleServerAPI);
			IRollbackLayerDebugger instance = this.injector.GetInstance<IRollbackLayerDebugger>();
			this.FrameController.Init(rollbackSettings, this.config.rollbackDebugSettings, instance, this.replaySystem.IsReplaying);
			if (this.config.DebugConfig.beginDebugPaused)
			{
				this.FrameController.SetControlMode(FrameControlMode.Manual);
			}
			this.gameTauntsSetup.Execute(this);
			this.events.Broadcast(new GameInitEvent(this));
			return;
		}
		throw new Exception("GameMode not found for type " + this.BattleSettings.mode + ". Please make sure it is bound in ContextConfigGameplay.cs");
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x00081654 File Offset: 0x0007FA54
	public PlayerReference getAllyReferenceWithValidController(PlayerReference player)
	{
		if (this.playerReferenceMap[player.PlayerNum].InputController == null)
		{
			foreach (KeyValuePair<PlayerNum, PlayerReference> keyValuePair in this.playerReferenceMap)
			{
				if (keyValuePair.Key != player.PlayerNum && keyValuePair.Value.Team == player.Team && keyValuePair.Value.InputController != null)
				{
					return keyValuePair.Value;
				}
			}
		}
		return null;
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x0008171C File Offset: 0x0007FB1C
	public PlayerReference getAllyReferenceWithInvalidController(PlayerReference player)
	{
		if (this.playerReferenceMap[player.PlayerNum].InputController != null)
		{
			foreach (KeyValuePair<PlayerNum, PlayerReference> keyValuePair in this.playerReferenceMap)
			{
				if (keyValuePair.Key != player.PlayerNum && keyValuePair.Value.Team == player.Team && keyValuePair.Value.InputController == null)
				{
					return keyValuePair.Value;
				}
			}
		}
		return null;
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x000817E4 File Offset: 0x0007FBE4
	private void applyQualitySettings()
	{
		if (this.userVideoSettingsModel.MaterialQuality == ThreeTierQualityLevel.Low)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			List<Renderer> list = new List<Renderer>();
			foreach (PlayerController playerController in this.CharacterControllers)
			{
				if (playerController)
				{
					list.AddRange(playerController.GetComponentsInChildren<Renderer>(true));
				}
			}
			SceneUtil.AssignCheapMaterialToRenderers(list, new Material(Shader.Find("WaveDash/SimpleCharacter")), null);
			SceneUtil.AssignCheapMaterialToTransforms(new List<Transform>
			{
				this.ObjectPools.ParentTransform,
				this.ObjectPools.InactiveTransform
			});
			stopwatch.Stop();
			UnityEngine.Debug.LogFormat("Assign Cheap Materials: {0}s", new object[]
			{
				stopwatch.Elapsed.TotalSeconds
			});
		}
		foreach (PlayerController playerController2 in this.CharacterControllers)
		{
			if (playerController2)
			{
				playerController2.InitMaterials();
			}
		}
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x00081944 File Offset: 0x0007FD44
	private void updatePlayerPorts(GameLoadPayload gamePayload)
	{
		if (!this.battleServerAPI.IsOnlineMatchReady || !this.battleServerAPI.IsSinglePlayerNetworkGame)
		{
			for (int i = 0; i < gamePayload.players.Length; i++)
			{
				PlayerSelectionInfo playerSelectionInfo = gamePayload.players[i];
				if (playerSelectionInfo.type == PlayerType.Human && this.battleServerAPI.IsLocalPlayer(playerSelectionInfo.playerNum))
				{
					PlayerInputPort playerInputPort = this.userInputManager.GetPortWithPlayerNum(playerSelectionInfo.playerNum);
					if (playerInputPort == null)
					{
						playerInputPort = this.userInputManager.GetFirstPortWithNoPlayer();
						if (this.userInputManager.AssignPlayerNum(playerInputPort.Id, playerSelectionInfo.playerNum))
						{
							this.signalBus.GetSignal<PlayerAssignedSignal>().Dispatch(playerSelectionInfo.playerNum);
							this.Client.Input.AssignFirstAvailableDevice(playerInputPort, DevicePreference.Any);
						}
					}
				}
			}
		}
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x00081A30 File Offset: 0x0007FE30
	private void updatePlayerTeams(GameLoadPayload gamePayload)
	{
		GameMode mode = gamePayload.battleConfig.mode;
		GameModeData dataByType = this.GameData.GameModeData.GetDataByType(mode);
		if (!dataByType.settings.usesTeams)
		{
			for (int i = 0; i < gamePayload.players.Length; i++)
			{
				PlayerSelectionInfo playerSelectionInfo = gamePayload.players[i];
				playerSelectionInfo.SetTeam(mode, PlayerUtil.GetTeamNumFromInt(i, true));
			}
		}
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x00081AA4 File Offset: 0x0007FEA4
	private bool validateGameLoadPayload(GameLoadPayload payload, ref string error)
	{
		bool result = true;
		GameMode mode = payload.battleConfig.mode;
		GameModeData dataByType = this.GameData.GameModeData.GetDataByType(mode);
		if (!dataByType.settings.usesTeams)
		{
			HashSet<TeamNum> hashSet = new HashSet<TeamNum>(default(TeamNumComparer));
			for (int i = 0; i < payload.players.Length; i++)
			{
				PlayerSelectionInfo playerSelectionInfo = payload.players[i];
				if (playerSelectionInfo.type != PlayerType.None && !playerSelectionInfo.isSpectator)
				{
					if (hashSet.Contains(playerSelectionInfo.GetTeam(mode)))
					{
						result = false;
						string text = error;
						error = string.Concat(new object[]
						{
							text,
							"Duplicate team in non-team mode: ",
							playerSelectionInfo.GetTeam(mode),
							"\n"
						});
						break;
					}
					hashSet.Add(playerSelectionInfo.GetTeam(mode));
				}
			}
		}
		return result;
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x00081BA4 File Offset: 0x0007FFA4
	protected PlayerReference createPlayerReference(PlayerSelectionInfo info)
	{
		PlayerReference instance = this.injector.GetInstance<PlayerReference>();
		instance.Init(info);
		this.playerReferences.Add(instance);
		this.playerReferenceMap[instance.PlayerNum] = instance;
		return instance;
	}

	// Token: 0x060018A5 RID: 6309 RVA: 0x00081BE4 File Offset: 0x0007FFE4
	protected void spawnPlayerReference(PlayerReference reference, SpawnPointBase point)
	{
		GameObject gameObject = this.injector.CreateGameObjectWithComponent<PlayerController>("Player" + this.playerReferences.Count);
		PlayerController component = gameObject.GetComponent<PlayerController>();
		gameObject.transform.SetParent(this.GameContainer.transform, false);
		this.CharacterControllers.Add(component);
		this.CameraInfluencers.Add(component);
		reference.AddController(component);
		component.Init(reference.PlayerInfo, reference, this.ModeData, point);
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x00081C68 File Offset: 0x00080068
	public PlayerReference GetPlayerReference(PlayerNum player)
	{
		if (!this.playerReferenceMap.ContainsKey(player))
		{
			UnityEngine.Debug.LogError("Failed to find reference for " + player);
			return null;
		}
		return this.playerReferenceMap[player];
	}

	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x060018A7 RID: 6311 RVA: 0x00081C9E File Offset: 0x0008009E
	public List<PlayerReference> PlayerReferences
	{
		get
		{
			return this.playerReferences;
		}
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x00081CA8 File Offset: 0x000800A8
	public void OnDestroy()
	{
		if (this.config != null)
		{
			Time.timeScale = this.config.gameSpeed;
		}
		this.IsRunning = false;
		if (this.GameContainer != null)
		{
			UnityEngine.Object.Destroy(this.GameContainer);
			this.GameContainer = null;
		}
		if (this.DynamicObjects != null)
		{
			UnityEngine.Object.Destroy(this.DynamicObjects.gameObject);
			this.DynamicObjects = null;
		}
		if (this.PointAudio != null)
		{
			UnityEngine.Object.Destroy(this.PointAudio.gameObject);
			this.PointAudio = null;
		}
		if (this.inactiveObjects != null)
		{
			UnityEngine.Object.DestroyImmediate(this.inactiveObjects);
			this.inactiveObjects = null;
		}
		if (this.CurrentGameMode != null)
		{
			this.CurrentGameMode.Destroy();
			this.CurrentGameMode = null;
		}
		if (this.statsTracker != null)
		{
			this.statsTracker.Destroy();
			this.statsTracker = null;
		}
		if (this.playerSpawner != null)
		{
			this.playerSpawner.Destroy();
		}
		if (this.ComboManager != null)
		{
			this.ComboManager.Destroy();
		}
		this.events.Unsubscribe(typeof(GameEndEvent), new Events.EventHandler(this.onGameEnd));
		this.events.Unsubscribe(typeof(GameStartEvent), new Events.EventHandler(this.onGameStart));
		this.events.Unsubscribe(typeof(DebugDesyncEvent), new Events.EventHandler(this.onDebugDesync));
		for (int i = 0; i < this.playerReferences.Count; i++)
		{
			this.playerReferences[i].Destroy();
		}
		this.Hits.OnDestroy();
		SceneManager.sceneLoaded -= this.onSceneLoaded;
		if (this.FrameController != null)
		{
			this.FrameController.OnGameManagerDestroyed();
		}
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x00081EA4 File Offset: 0x000802A4
	public void TogglePaused(PlayerInputPort port)
	{
		if (!this.replaySystem.IsReplaying && (!this.StartedGame || !this.IsPauseEnabled))
		{
			return;
		}
		if (this.IsPaused)
		{
			if (port != this.PausedPort)
			{
				return;
			}
			this.PausedPort = null;
			this.Client.UI.LockingPort = null;
		}
		else
		{
			this.PausedPort = port;
			this.Client.UI.LockingPort = port;
		}
		this.Client.UI.SetPauseMode(this.IsPaused);
		this.Audio.PlayMenuSound((!this.IsPaused) ? SoundKey.inGame_unpause : SoundKey.inGame_pause, 0f);
		this.events.Broadcast(new GamePausedEvent(this.IsPaused, this.PausedPlayer));
		this.Audio.UpdateVolume();
		this.events.Broadcast(new PauseSoundCommand(SoundType.SFX, this.IsPaused));
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x00081FA4 File Offset: 0x000803A4
	private void onGameEnd(GameEvent message)
	{
		GameEndEvent gameEndEvent = message as GameEndEvent;
		if (!this.EndedGame)
		{
			this.EndedGame = true;
			this.Audio.PlayMenuSound(SoundKey.inGame_endGame, 0f);
			bool notifiedBattleServer = false;
			if (this.battleServerAPI.IsConnected)
			{
				this.Client.PreCompleteMatch();
				int num = (this.BattleSettings.mode != GameMode.CrewBattle) ? this.BattleSettings.durationSeconds : this.BattleSettings.crewsBattle_durationSeconds;
				num *= (int)WTime.fps;
				if (this.Frame - this.gameStartFrame >= num - 5 * this.config.uiuxSettings.endGameDelayFrames)
				{
					notifiedBattleServer = true;
					this.battleServerAPI.SendWinner(gameEndEvent.winningTeams);
				}
			}
			base.StartCoroutine(this.endGame((float)this.config.uiuxSettings.endGameDelayFrames / WTime.fps, gameEndEvent.winners, gameEndEvent.winningTeams, notifiedBattleServer));
		}
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x00082099 File Offset: 0x00080499
	private void onGameStart(GameEvent message)
	{
		this.gameManagerState.gameStarted = true;
	}

	// Token: 0x060018AC RID: 6316 RVA: 0x000820A7 File Offset: 0x000804A7
	private void onDebugDesync(GameEvent message)
	{
		this.gameManagerState.gameDebugDesync = true;
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x000820B8 File Offset: 0x000804B8
	private IEnumerator endGame(float seconds, List<PlayerNum> winners, List<TeamNum> winningTeams, bool notifiedBattleServer)
	{
		yield return new WaitForSeconds(seconds);
		if (this.battleServerAPI.IsConnected && !notifiedBattleServer)
		{
			this.battleServerAPI.SendWinner(winningTeams);
		}
		VictoryScreenPayload victoryPayload = new VictoryScreenPayload();
		victoryPayload.stats = this.statsTracker.PlayerStats;
		this.GetEndGameCharacterIndicies(victoryPayload.endGameCharacterIndicies);
		victoryPayload.victors = winners;
		victoryPayload.winningTeams = winningTeams;
		victoryPayload.gamePayload = this.gameConfig;
		victoryPayload.wasForfeited = this.gameManagerState.gameWasForfeitted;
		victoryPayload.nextScreen = ScreenType.VictoryGUI;
		this.EndGame(victoryPayload);
		yield break;
	}

	// Token: 0x060018AE RID: 6318 RVA: 0x000820F0 File Offset: 0x000804F0
	public void EndGame()
	{
		this.setupEndGame((!this.IsTrainingMode && !this.replaySystem.IsReplaying) ? ScreenType.VictoryGUI : ScreenType.MainMenu, true);
	}

	// Token: 0x060018AF RID: 6319 RVA: 0x0008211C File Offset: 0x0008051C
	private void setupEndGame(ScreenType nextScreen, bool forceOnlineMatchEnd)
	{
		if (!this.EndedGame)
		{
			this.EndedGame = true;
			this.Client.UI.LockingPort = null;
			VictoryScreenPayload victoryScreenPayload = new VictoryScreenPayload();
			victoryScreenPayload.stats = this.statsTracker.PlayerStats;
			this.GetEndGameCharacterIndicies(victoryScreenPayload.endGameCharacterIndicies);
			victoryScreenPayload.wasForfeited = forceOnlineMatchEnd;
			victoryScreenPayload.wasExited = !forceOnlineMatchEnd;
			victoryScreenPayload.gamePayload = this.gameConfig;
			victoryScreenPayload.nextScreen = nextScreen;
			victoryScreenPayload.winningTeams.Add(TeamNum.None);
			if (this.battleServerAPI.IsConnected && forceOnlineMatchEnd)
			{
				this.Client.PreCompleteMatch();
				this.battleServerAPI.LeaveRoom(true);
			}
			this.EndGame(victoryScreenPayload);
		}
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x000821D4 File Offset: 0x000805D4
	public void EndPreviewGame()
	{
		if (this.battleServerAPI.IsConnected)
		{
			throw new Exception("Unhandled winner");
		}
		List<PlayerNum> list = new List<PlayerNum>();
		List<TeamNum> list2 = new List<TeamNum>();
		TeamNum teamNum = TeamNum.None;
		for (int i = 0; i < this.playerReferences.Count; i++)
		{
			PlayerReference playerReference = this.playerReferences[i];
			if (teamNum == TeamNum.None)
			{
				teamNum = playerReference.PlayerInfo.team;
				list2.Add(teamNum);
			}
			if (teamNum == playerReference.PlayerInfo.team)
			{
				list.Add(playerReference.PlayerNum);
			}
		}
		VictoryScreenPayload victoryScreenPayload = new VictoryScreenPayload();
		victoryScreenPayload.stats = this.statsTracker.PlayerStats;
		victoryScreenPayload.victors = list;
		this.GetEndGameCharacterIndicies(victoryScreenPayload.endGameCharacterIndicies);
		victoryScreenPayload.winningTeams = list2;
		victoryScreenPayload.gamePayload = this.gameConfig;
		victoryScreenPayload.wasForfeited = false;
		victoryScreenPayload.nextScreen = ScreenType.VictoryGUI;
		this.EndGame(victoryScreenPayload);
	}

	// Token: 0x060018B1 RID: 6321 RVA: 0x000822CC File Offset: 0x000806CC
	private void GetEndGameCharacterIndicies(List<int> output)
	{
		output.Clear();
		for (int i = 0; i < this.statsTracker.PlayerStats.Count; i++)
		{
			int item = 0;
			PlayerStats playerStats = this.statsTracker.PlayerStats[i];
			if (playerStats.playerInfo.type != PlayerType.None && PlayerUtil.IsValidPlayer(playerStats.playerInfo.playerNum) && !playerStats.playerInfo.isSpectator)
			{
				CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(playerStats.playerInfo.characterID);
				PlayerReference playerReference = this.GetPlayerReference(playerStats.playerInfo.playerNum);
				item = this.characterDataHelper.GetIndexOfLinkedCharacterData(characterDefinition, playerReference.Controller.CharacterData.characterDefinition);
			}
			output.Add(item);
		}
	}

	// Token: 0x060018B2 RID: 6322 RVA: 0x00082399 File Offset: 0x00080799
	public void ForfeitGame(ScreenType nextScreen)
	{
		this.setupEndGame(nextScreen, true);
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x000823A3 File Offset: 0x000807A3
	public void ExitGame(ScreenType nextScreen, int activePlayerCount)
	{
		this.setupEndGame(nextScreen, activePlayerCount == 0);
	}

	// Token: 0x060018B4 RID: 6324 RVA: 0x000823B0 File Offset: 0x000807B0
	public void EndGame(VictoryScreenPayload victoryPayload)
	{
		this.Audio.StopMusic(null, -1f);
		this.Audio.PlayMenuSound(SoundKey.inGame_exitGame, 0f);
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HitBoxes, false);
		DebugDraw.Instance.SetChannelActive(DebugDrawChannel.HurtBoxes, false);
		this.debugKeys.SetFrameAdvanceOn(false);
		this.FrameController.OnEndGame();
		this.deepLogging.EndMatch();
		this.signalBus.GetSignal<EndGameSignal>().Dispatch(victoryPayload);
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x0008242C File Offset: 0x0008082C
	private void syncInputs()
	{
		this.FrameController.SyncInputForFrame(ref this.inputsThisFrameBuffer);
		if (this.inputsThisFrameBuffer[0].values == null)
		{
			UnityEngine.Debug.LogError("Something seems wrong... we didn't get any input for p1 this frame");
		}
		for (int i = 0; i < this.inputsThisFrameBuffer.Length; i++)
		{
			PlayerReference playerReference = this.playerReferences[i];
			PlayerController controller = playerReference.Controller;
			if (!(controller == null) && !(controller.InputController == null))
			{
				controller.InputController.LoadInputValues(this.inputsThisFrameBuffer[i].values);
			}
		}
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x000824D0 File Offset: 0x000808D0
	public void TickInput(int frame, bool isSkippedFrame)
	{
		if (this.config == null)
		{
			return;
		}
		if (this.IsPaused && this.FrameController.IsLocal)
		{
			return;
		}
		for (int i = 0; i < this.playerReferences.Count; i++)
		{
			PlayerController controller = this.playerReferences[i].Controller;
			if (!(controller == null))
			{
				if (this.localPlayers.ContainsKey(controller.PlayerNum))
				{
					RollbackInput rollbackInput = this.rollbackInputPool.New();
					rollbackInput.playerID = PlayerUtil.GetIntFromPlayerNum(controller.PlayerNum, false);
					if (frame + this.FrameController.rollbackStatus.InputDelayFrames >= this.GameStartInputFrame)
					{
						if (isSkippedFrame)
						{
							this.FrameController.FillSkippedLocalInputFrame(frame, rollbackInput);
						}
						else
						{
							bool tauntsOnly = false;
							if (this.config.uiuxSettings.emotiveStartup && frame + this.FrameController.rollbackStatus.InputDelayFrames < this.AllowGameplayInputsFrame)
							{
								tauntsOnly = true;
							}
							InputValuesSnapshot values = rollbackInput.values;
							if (controller.InputController != null)
							{
								controller.InputController.ReadPlayerInputValues(ref values, tauntsOnly);
							}
						}
					}
					this.localInputsBuffer.Add(rollbackInput);
				}
			}
		}
		this.FrameController.SaveLocalInputs(frame, this.localInputsBuffer, !isSkippedFrame);
		foreach (RollbackInput obj in this.localInputsBuffer)
		{
			this.rollbackInputPool.Store(obj);
		}
		this.localInputsBuffer.Clear();
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x00082694 File Offset: 0x00080A94
	public void TickUpdate()
	{
		if (this.CurrentGameMode != null)
		{
			this.CurrentGameMode.TickUpdate();
		}
	}

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x060018B8 RID: 6328 RVA: 0x000826AC File Offset: 0x00080AAC
	public bool IsGameComplete
	{
		get
		{
			return this.CurrentGameMode != null && this.CurrentGameMode.IsGameComplete;
		}
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x000826C8 File Offset: 0x00080AC8
	public void TickFrame()
	{
		this.Audio.TickFrame();
		this.PhysicsWorld.ClearFrameDebuggingInfo();
		if (this.config == null)
		{
			return;
		}
		if (!this.StartedGame && this.Frame > this.gameStartFrame)
		{
			this.gameManagerState.gameStarted = true;
			this.events.Broadcast(new GameStartEvent());
		}
		this.Client.GameTickFPSCounter.TickFrame();
		if (this.IsPaused && this.FrameController.IsLocal)
		{
			return;
		}
		this.syncInputs();
		this.playerSpawner.TickFrame();
		for (int i = 0; i < this.playerReferences.Count; i++)
		{
			this.playerReferences[i].TickFrame();
		}
		for (int j = 0; j < this.CharacterControllers.Count; j++)
		{
			PlayerController playerController = this.CharacterControllers[j];
			playerController.TickFrame();
		}
		this.Hits.TickFrame();
		this.grabManager.TickFrame();
		this.Camera.TickFrame();
		this.ComboManager.TickFrame();
		this.Stage.TickFrame();
		foreach (PlayerController playerController2 in this.CharacterControllers)
		{
			playerController2.PlayDelayedParticles();
		}
		if (this.DynamicObjects != null)
		{
			this.DynamicObjects.TickFrame();
		}
		if (this.PointAudio != null)
		{
			this.PointAudio.TickTimeDelta(WTime.frameTime);
		}
		this.ObjectPools.TickFrame();
		if (this.CurrentGameMode != null)
		{
			this.CurrentGameMode.TickFrame();
		}
		this.announcements.TickFrame();
		for (int k = 0; k < this.playerReferences.Count; k++)
		{
			if (this.playerReferences[k] != null && !this.playerReferences[k].IsSpectating)
			{
				PlayerController controller = this.playerReferences[k].Controller;
				controller.UpdateDebugText();
			}
		}
		this.FrameController.OnFrameAdvanced();
		this.replaySystem.Tick(this.Frame, this);
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x00082940 File Offset: 0x00080D40
	public void Update()
	{
		if (this.config == null)
		{
			return;
		}
		this.generateGlobalDebugText();
	}

	// Token: 0x060018BB RID: 6331 RVA: 0x0008295C File Offset: 0x00080D5C
	public PlayerController GetPlayerController(PlayerNum playerNum)
	{
		for (int i = 0; i < this.CharacterControllers.Count; i++)
		{
			PlayerController playerController = this.CharacterControllers[i];
			if (playerController.PlayerNum == playerNum && playerController.IsActive)
			{
				return playerController;
			}
		}
		return null;
	}

	// Token: 0x060018BC RID: 6332 RVA: 0x000829AC File Offset: 0x00080DAC
	public List<PlayerController> GetPlayers()
	{
		return this.CharacterControllers;
	}

	// Token: 0x060018BD RID: 6333 RVA: 0x000829B4 File Offset: 0x00080DB4
	void IRollbackClient.ReportWaiting(double waitingDurationMs)
	{
		this.performanceTracker.RecordWaiting(waitingDurationMs);
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x000829C4 File Offset: 0x00080DC4
	void IRollbackClient.ReportHealth(NetworkHealthReport health)
	{
		this.UI.ReportRollbackHealth(health);
		this.netGraph.ReportHealth(health);
		this.Client.PerformanceDisplay.ReportNetHealth(health);
		this.performanceTracker.RecordPing((float)health.calculatedLatencyMs, this.battleServerAPI.ServerPing);
		this.performanceTracker.RecordSkippedFrames(health.skippedFrames);
	}

	// Token: 0x060018BF RID: 6335 RVA: 0x00082A2C File Offset: 0x00080E2C
	void IRollbackClient.ReportErrors(List<string> errors)
	{
		for (int i = 0; i < errors.Count; i++)
		{
			this.UI.AddDebugTextEvent(errors[i]);
		}
	}

	// Token: 0x060018C0 RID: 6336 RVA: 0x00082A62 File Offset: 0x00080E62
	void IRollbackClient.Halt()
	{
		UnityEngine.Debug.LogError("Halting simulation");
		this.FrameController.SetControlMode(FrameControlMode.Manual);
	}

	// Token: 0x060018C1 RID: 6337 RVA: 0x00082A7A File Offset: 0x00080E7A
	public void DestroyCharacter(PlayerController character)
	{
		this.Hits.Unregister(character);
		this.CharacterControllers.Remove(character);
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x00082A98 File Offset: 0x00080E98
	private void generateGlobalDebugText()
	{
		string text = string.Empty;
		if (this.UI.DebugTextEnabled)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"Display FPS: ",
				this.Client.DisplayFPSCounter.FPS,
				" Game Tick FPS: ",
				this.Client.GameTickFPSCounter.FPS,
				" \n"
			});
			if (this.FrameController != null)
			{
				text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"Frame: ",
					this.FrameController.Frame,
					(!this.IsRollingBack) ? string.Empty : "(rb)",
					"\n"
				});
			}
			string debugString = DebugDraw.Instance.getDebugString();
			if (debugString.Length > 0)
			{
				text = text + "Debug Draw: " + debugString + "\n";
			}
			text = text + WTime.currentTimeMs.ToString() + "\n";
		}
		this.UI.GlobalDebugText.text = text;
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x00082BD1 File Offset: 0x00080FD1
	private void OnDrawGizmos()
	{
		this.PhysicsWorld.DrawDebugInfo();
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.Grid))
		{
			DebugDraw.Instance.DrawGridGizmos();
		}
	}

	// Token: 0x0400128E RID: 4750
	private int gameStartFrame;

	// Token: 0x0400128F RID: 4751
	public StageData stageData;

	// Token: 0x04001290 RID: 4752
	private int gameStartInputFrame;

	// Token: 0x04001291 RID: 4753
	private int allowGameplayInputsFrame;

	// Token: 0x04001293 RID: 4755
	private Dictionary<int, int> rollbackCounts = new Dictionary<int, int>();

	// Token: 0x04001299 RID: 4761
	private IGrabManager grabManager;

	// Token: 0x0400129D RID: 4765
	private GameObject inactiveObjects;

	// Token: 0x040012A6 RID: 4774
	private GameLoadPayload gameConfig;

	// Token: 0x040012AA RID: 4778
	private GameManagerState gameManagerState = new GameManagerState();

	// Token: 0x040012AB RID: 4779
	private StatsTracker statsTracker;

	// Token: 0x040012AC RID: 4780
	public List<PlayerController> CharacterControllers = new List<PlayerController>();

	// Token: 0x040012AD RID: 4781
	public List<ICameraInfluencer> CameraInfluencers = new List<ICameraInfluencer>();

	// Token: 0x040012AE RID: 4782
	private List<PlayerReference> playerReferences = new List<PlayerReference>();

	// Token: 0x040012AF RID: 4783
	private Dictionary<PlayerNum, PlayerReference> playerReferenceMap = new Dictionary<PlayerNum, PlayerReference>(default(PlayerNumComparer));

	// Token: 0x040012B0 RID: 4784
	private Dictionary<PlayerNum, bool> localPlayers = new Dictionary<PlayerNum, bool>(default(PlayerNumComparer));

	// Token: 0x040012B1 RID: 4785
	private PlayerSpawner playerSpawner;

	// Token: 0x040012B4 RID: 4788
	private List<RollbackInput> localInputsBuffer = new List<RollbackInput>();

	// Token: 0x040012B5 RID: 4789
	private GenericResetObjectPool<RollbackInput> rollbackInputPool;

	// Token: 0x040012B6 RID: 4790
	private RollbackInput[] inputsThisFrameBuffer;
}
