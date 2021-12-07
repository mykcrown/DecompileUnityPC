using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Steamworks;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class SteamManager : IRichPresence
{
	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x0600097A RID: 2426 RVA: 0x0004FB7A File Offset: 0x0004DF7A
	// (set) Token: 0x0600097B RID: 2427 RVA: 0x0004FB82 File Offset: 0x0004DF82
	public bool Initialized { get; private set; }

	// Token: 0x0600097C RID: 2428 RVA: 0x0004FB8B File Offset: 0x0004DF8B
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x0600097D RID: 2429 RVA: 0x0004FB93 File Offset: 0x0004DF93
	// (set) Token: 0x0600097E RID: 2430 RVA: 0x0004FB9B File Offset: 0x0004DF9B
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x0600097F RID: 2431 RVA: 0x0004FBA4 File Offset: 0x0004DFA4
	// (set) Token: 0x06000980 RID: 2432 RVA: 0x0004FBAC File Offset: 0x0004DFAC
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000981 RID: 2433 RVA: 0x0004FBB5 File Offset: 0x0004DFB5
	// (set) Token: 0x06000982 RID: 2434 RVA: 0x0004FBBD File Offset: 0x0004DFBD
	[Inject]
	public IOfflineModeDetector offlineMode { get; set; }

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000983 RID: 2435 RVA: 0x0004FBC6 File Offset: 0x0004DFC6
	// (set) Token: 0x06000984 RID: 2436 RVA: 0x0004FBCE File Offset: 0x0004DFCE
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000985 RID: 2437 RVA: 0x0004FBD7 File Offset: 0x0004DFD7
	// (set) Token: 0x06000986 RID: 2438 RVA: 0x0004FBDF File Offset: 0x0004DFDF
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000987 RID: 2439 RVA: 0x0004FBE8 File Offset: 0x0004DFE8
	// (set) Token: 0x06000988 RID: 2440 RVA: 0x0004FBF0 File Offset: 0x0004DFF0
	[Inject]
	public IStartupArgs startupArgs { get; set; }

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000989 RID: 2441 RVA: 0x0004FBF9 File Offset: 0x0004DFF9
	// (set) Token: 0x0600098A RID: 2442 RVA: 0x0004FC01 File Offset: 0x0004E001
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x0600098B RID: 2443 RVA: 0x0004FC0A File Offset: 0x0004E00A
	// (set) Token: 0x0600098C RID: 2444 RVA: 0x0004FC12 File Offset: 0x0004E012
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x0600098D RID: 2445 RVA: 0x0004FC1B File Offset: 0x0004E01B
	// (set) Token: 0x0600098E RID: 2446 RVA: 0x0004FC23 File Offset: 0x0004E023
	[Inject]
	public P2PServerMgr p2pServerMgr { get; set; }

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x0600098F RID: 2447 RVA: 0x0004FC2C File Offset: 0x0004E02C
	// (set) Token: 0x06000990 RID: 2448 RVA: 0x0004FC34 File Offset: 0x0004E034
	[Inject]
	public IBattleServerAPI battleServer { private get; set; }

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000991 RID: 2449 RVA: 0x0004FC3D File Offset: 0x0004E03D
	public List<SteamManager.LobbyListDataEntry> LobbyList
	{
		get
		{
			return this.lobbyList;
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000992 RID: 2450 RVA: 0x0004FC48 File Offset: 0x0004E048
	public string UserName
	{
		get
		{
			string personaName = SteamFriends.GetPersonaName();
			return Regex.Replace(personaName, "[^a-zA-Z0-9 ]+", string.Empty, RegexOptions.Compiled);
		}
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0004FC70 File Offset: 0x0004E070
	public void Startup()
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		this.proxyObject = ProxyMono.CreateNew("SteamManager");
		this.proxyObject.OnUpdate = new Action(this.Update);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
		}
		try
		{
			AppId_t unOwnAppID = (AppId_t)((uint)this.config.networkSettings.AppID);
			if (SteamAPI.RestartAppIfNecessary(unOwnAppID))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException arg)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg);
			Application.Quit();
			return;
		}
		this.Initialized = SteamAPI.Init();
		if (!this.Initialized)
		{
			this.dialogController.ShowOneButtonDialog("STEAM NOT FOUND", "Please make sure Steam is running, you have Icons installed on Steam, and the steam_app.txt id file is present.", "OK", WindowTransition.STANDARD_FADE, false, default(AudioData));
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.");
			return;
		}
		this.InitializeSteamLobbyJoinCallbacks();
		this.p2PSessionRequestCallback = Callback<P2PSessionRequest_t>.Create(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.OnP2PSessionRequest));
		this.p2pSessionConnectFailCallback = Callback<P2PSessionConnectFail_t>.Create(new Callback<P2PSessionConnectFail_t>.DispatchDelegate(this.OnP2PSessionConnectFail));
		SteamFriends.SetRichPresence("status", null);
		this.signalBus.Dispatch(SteamManager.STEAM_INITIALIZED);
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0004FDCC File Offset: 0x0004E1CC
	private void OnP2PSessionRequest(P2PSessionRequest_t request)
	{
		Debug.LogError("WE GOT IT  " + request.m_steamIDRemote.m_SteamID);
		CSteamID steamIDRemote = request.m_steamIDRemote;
		SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote);
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0004FE08 File Offset: 0x0004E208
	private void OnP2PSessionConnectFail(P2PSessionConnectFail_t failEvent)
	{
		Debug.LogError(string.Concat(new object[]
		{
			"SESSION CONNECT FAIL ",
			failEvent.m_steamIDRemote.m_SteamID,
			" ",
			failEvent.m_eP2PSessionError
		}));
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0004FE58 File Offset: 0x0004E258
	private void InitializeSteamLobbyJoinCallbacks()
	{
		this.lobbyJoinRequestEvent = Callback<GameLobbyJoinRequested_t>.Create(delegate(GameLobbyJoinRequested_t enterResult)
		{
			if (this.CurrentSteamLobbyId != enterResult.m_steamIDLobby.m_SteamID)
			{
				this.LeaveLobby();
				Debug.LogFormat("Attempting to Join Steam Lobby From Direct Invite: {0}", new object[]
				{
					enterResult.m_steamIDLobby.m_SteamID
				});
				if (enterResult.m_steamIDLobby.m_SteamID != 0UL)
				{
					SteamMatchmaking.JoinLobby(enterResult.m_steamIDLobby);
				}
			}
		});
		this.lobbyEnterEvent = Callback<LobbyEnter_t>.Create(delegate(LobbyEnter_t enterResult)
		{
			this.battleServer.ResetRoom();
			Debug.Log("Steam Lobby Enter Response: " + enterResult.m_EChatRoomEnterResponse);
			if (enterResult.m_EChatRoomEnterResponse == 1U)
			{
				this.CurrentSteamLobbyId = enterResult.m_ulSteamIDLobby;
				this.p2pServerMgr.OnUpdateCustomLobby();
			}
			if (this.currentLobbyJoinCallback != null)
			{
				this.currentLobbyJoinCallback((EChatRoomEnterResponse)enterResult.m_EChatRoomEnterResponse);
				this.currentLobbyJoinCallback = null;
			}
		});
		this.lobbyChatUpdateEvent = Callback<LobbyChatUpdate_t>.Create(delegate(LobbyChatUpdate_t chatUpdate)
		{
			this.p2pServerMgr.OnUpdateCustomLobby();
		});
		this.lobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(delegate(LobbyDataUpdate_t chatUpdate)
		{
			this.p2pServerMgr.OnUpdateCustomLobbyData();
		});
		this.lobbyKickedEvent = Callback<LobbyKicked_t>.Create(delegate(LobbyKicked_t kickResult)
		{
			if (kickResult.m_ulSteamIDLobby == this.CurrentSteamLobbyId)
			{
				this.CurrentSteamLobbyId = 0UL;
				this.p2pServerMgr.OnLeftLobby();
			}
		});
		this.lobbyMatchListEvent = Callback<LobbyMatchList_t>.Create(delegate(LobbyMatchList_t lobbyMatchList)
		{
			this.searchActive = false;
			this.lobbyList.Clear();
			int num = 0;
			while ((long)num < (long)((ulong)lobbyMatchList.m_nLobbiesMatching))
			{
				CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(num);
				if (lobbyByIndex.IsValid())
				{
					string text = SteamMatchmaking.GetLobbyData(lobbyByIndex, SteamManager.LobbyNameKey);
					string lobbyData = SteamMatchmaking.GetLobbyData(lobbyByIndex, SteamManager.LobbyCreator);
					int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(lobbyByIndex);
					int lobbyMemberLimit = SteamMatchmaking.GetLobbyMemberLimit(lobbyByIndex);
					text = text + " - " + lobbyData;
					if (!string.IsNullOrEmpty(text))
					{
						this.lobbyList.Add(new SteamManager.LobbyListDataEntry
						{
							SteamId = lobbyByIndex.m_SteamID,
							LobbyName = text,
							PlayerCount = numLobbyMembers,
							PlayerLimit = lobbyMemberLimit
						});
					}
				}
				num++;
			}
			this.signalBus.Dispatch(SteamManager.STEAM_LOBBY_LIST_UPDATED);
		});
		if (this.startupArgs.HasArg(StartupArgs.StartupArgType.SteamConnectLobby))
		{
			ulong argULongValue = this.startupArgs.GetArgULongValue(StartupArgs.StartupArgType.SteamConnectLobby);
			Debug.LogFormat("Attempting to Join Steam Lobby From Startup Args: {0}", new object[]
			{
				argULongValue
			});
			if (argULongValue != 0UL)
			{
				SteamMatchmaking.JoinLobby(new CSteamID(argULongValue));
			}
		}
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x0004FF3C File Offset: 0x0004E33C
	public void QuietlyAttemptJoinSteamLobby(ulong lobbyId)
	{
		if (this.CurrentSteamLobbyId != lobbyId)
		{
			this.LeaveLobby();
			this.CurrentSteamLobbyId = lobbyId;
			Debug.LogFormat("Attempting to Join Steam Lobby From Icons Lobby Params: {0}", new object[]
			{
				lobbyId
			});
			if (lobbyId != 0UL)
			{
				SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
			}
		}
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x0004FF8F File Offset: 0x0004E38F
	public bool IsOverlayEnabled()
	{
		return this.Initialized && SteamUtils.IsOverlayEnabled();
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x0004FFA4 File Offset: 0x0004E3A4
	private void OnEnable()
	{
		if (!this.Initialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0004FFDA File Offset: 0x0004E3DA
	private void OnDestroy()
	{
		Debug.Log("Steam manager on destroy");
		if (!this.Initialized)
		{
			return;
		}
		this.p2PSessionRequestCallback.Dispose();
		this.p2pSessionConnectFailCallback.Dispose();
		SteamAPI.Shutdown();
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0005000D File Offset: 0x0004E40D
	private void Update()
	{
		if (!this.Initialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x00050020 File Offset: 0x0004E420
	public void InviteToLobby(string lobbyName, string lobbyPass)
	{
		if (this.CurrentSteamLobbyId != 0UL)
		{
			SteamFriends.ActivateGameOverlayInviteDialog(new CSteamID(this.CurrentSteamLobbyId));
		}
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00050040 File Offset: 0x0004E440
	public void CreateLobby(string name, string pass, ELobbyType lobbyType, string stage, Action<ulong> onCreated)
	{
		SteamMatchmaking.CreateLobby(lobbyType, this.config.lobbySettings.maxPlayerNum);
		if (this.lobbyCreated != null)
		{
			this.lobbyCreated.Dispose();
		}
		this.lobbyCreated = Callback<LobbyCreated_t>.Create(delegate(LobbyCreated_t lobbyResult)
		{
			this.OnCreateLobby(name, pass, stage, lobbyResult);
			onCreated(lobbyResult.m_ulSteamIDLobby);
		});
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x000500BC File Offset: 0x0004E4BC
	public void LeaveLobby()
	{
		if (this.CurrentSteamLobbyId != 0UL)
		{
			this.LobbyStatus = "DISCONNECTED";
			SteamMatchmaking.LeaveLobby(new CSteamID(this.CurrentSteamLobbyId));
			this.CurrentSteamLobbyId = 0UL;
			this.p2pServerMgr.CleanupLobby();
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x0600099F RID: 2463 RVA: 0x000500F9 File Offset: 0x0004E4F9
	// (set) Token: 0x060009A0 RID: 2464 RVA: 0x00050101 File Offset: 0x0004E501
	public ulong CurrentSteamLobbyId
	{
		get
		{
			return this.currentSteamLobbyId;
		}
		private set
		{
			if (this.currentSteamLobbyId != value)
			{
				this.currentSteamLobbyId = value;
				this.signalBus.Dispatch("STEAM_LOBBY_ID_UPDATED");
			}
		}
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00050128 File Offset: 0x0004E528
	public void OnCreateLobby(string name, string pass, string stage, LobbyCreated_t lobbyResult)
	{
		this.LobbyStatus = lobbyResult.m_eResult.ToString();
		if (lobbyResult.m_eResult == EResult.k_EResultOK)
		{
			this.CurrentSteamLobbyId = lobbyResult.m_ulSteamIDLobby;
			CSteamID steamIDLobby = new CSteamID(lobbyResult.m_ulSteamIDLobby);
			SteamMatchmaking.SetLobbyData(steamIDLobby, SteamManager.LobbyNameKey, name);
			SteamMatchmaking.SetLobbyData(steamIDLobby, SteamManager.LobbyCreator, this.UserName);
			SteamMatchmaking.SetLobbyData(steamIDLobby, SteamManager.LobbyStageKey, stage);
			SteamMatchmaking.SetLobbyData(steamIDLobby, SteamManager.LobbyVersionKey, BuildConfigUtil.GetCompareVersion(this.config));
		}
		else
		{
			this.signalBus.Dispatch(SteamManager.STEAM_LOBBY_FAILED_CREATE);
		}
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x000501CD File Offset: 0x0004E5CD
	public void ClearPresence()
	{
		SteamFriends.ClearRichPresence();
		Debug.Log("Steam cleared presence");
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x000501E0 File Offset: 0x0004E5E0
	public void SetPresence(string statusKey, string locKey1, string portraitKey, string portraitCaption)
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		string pchValue = string.Empty;
		if (statusKey == null)
		{
			pchValue = null;
		}
		else
		{
			string replace = null;
			if (locKey1 != null)
			{
				replace = this.localization.GetText("steam." + locKey1);
			}
			pchValue = this.localization.GetText("steam.state." + statusKey, replace);
		}
		SteamFriends.SetRichPresence("status", pchValue);
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x00050250 File Offset: 0x0004E650
	public bool GetMessage(ref byte[] message, ref uint messageLength, ref CSteamID remoteID)
	{
		uint cubDest;
		return !SteamManager.disableSteam && (SteamNetworking.IsP2PPacketAvailable(out cubDest, 0) && SteamNetworking.ReadP2PPacket(message, cubDest, out messageLength, out remoteID, 0));
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x00050289 File Offset: 0x0004E689
	public void SetLobbyValue(string key, string value)
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		if (this.CurrentSteamLobbyId != 0UL)
		{
			SteamMatchmaking.SetLobbyData(new CSteamID(this.CurrentSteamLobbyId), key, value);
		}
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x000502B6 File Offset: 0x0004E6B6
	public string GetLobbyValue(string key)
	{
		if (SteamManager.disableSteam)
		{
			return null;
		}
		if (this.CurrentSteamLobbyId != 0UL)
		{
			return SteamMatchmaking.GetLobbyData(new CSteamID(this.CurrentSteamLobbyId), key);
		}
		return null;
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x000502E4 File Offset: 0x0004E6E4
	public void SetLobbyParameters(PresenceLobbyParameters presenceParams)
	{
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x000502E8 File Offset: 0x0004E6E8
	public void Send(CSteamID steamIDRemote, byte[] bytes, uint length, bool useTCP)
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		if (!SteamNetworking.SendP2PPacket(steamIDRemote, bytes, length, (!useTCP) ? EP2PSend.k_EP2PSendUnreliableNoDelay : EP2PSend.k_EP2PSendReliable, 0))
		{
			Debug.LogError(string.Concat(new object[]
			{
				"STEAM P2P SEND FAILURE ",
				steamIDRemote.m_SteamID,
				" ",
				length,
				" ",
				useTCP
			}));
		}
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x00050368 File Offset: 0x0004E768
	public List<SteamManager.SteamLobbyData> GetReceivers()
	{
		if (SteamManager.disableSteam)
		{
			return new List<SteamManager.SteamLobbyData>();
		}
		List<SteamManager.SteamLobbyData> list = new List<SteamManager.SteamLobbyData>();
		if (this.currentSteamLobbyId != 0UL)
		{
			CSteamID steamIDLobby = new CSteamID(this.currentSteamLobbyId);
			CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(steamIDLobby);
			int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(steamIDLobby);
			for (int i = 0; i < numLobbyMembers; i++)
			{
				CSteamID lobbyMemberByIndex = SteamMatchmaking.GetLobbyMemberByIndex(steamIDLobby, i);
				list.Add(new SteamManager.SteamLobbyData
				{
					isHost = (lobbyOwner == lobbyMemberByIndex),
					steamID = lobbyMemberByIndex,
					isSelf = (lobbyMemberByIndex == this.MySteamID())
				});
			}
		}
		return list;
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x060009AA RID: 2474 RVA: 0x00050410 File Offset: 0x0004E810
	public bool IsHost
	{
		get
		{
			if (SteamManager.disableSteam)
			{
				return false;
			}
			CSteamID steamIDLobby = new CSteamID(this.currentSteamLobbyId);
			CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(steamIDLobby);
			return lobbyOwner == SteamUser.GetSteamID();
		}
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x00050448 File Offset: 0x0004E848
	public CSteamID MySteamID()
	{
		if (SteamManager.disableSteam)
		{
			return CSteamID.Nil;
		}
		return SteamUser.GetSteamID();
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0005045F File Offset: 0x0004E85F
	public string GetUserName(CSteamID steamID)
	{
		if (SteamManager.disableSteam)
		{
			return null;
		}
		return SteamFriends.GetFriendPersonaName(steamID);
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00050473 File Offset: 0x0004E873
	public bool IsFriend(ulong steamId)
	{
		return !SteamManager.disableSteam && SteamFriends.HasFriend(new CSteamID(steamId), EFriendFlags.k_EFriendFlagImmediate);
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0005048D File Offset: 0x0004E88D
	public void JoinFriendLobby()
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		SteamFriends.ActivateGameOverlay("Friends");
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x000504A4 File Offset: 0x0004E8A4
	public void RequestLobbyList()
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		if (!this.searchActive)
		{
			this.searchActive = true;
			SteamMatchmaking.RequestLobbyList();
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x000504C9 File Offset: 0x0004E8C9
	public void JoinLobby(ulong lobbyId, Action<EChatRoomEnterResponse> onJoinAttempted)
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		this.currentLobbyJoinCallback = onJoinAttempted;
		SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x000504E9 File Offset: 0x0004E8E9
	public void SetJoinable(bool isJoinable)
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		SteamMatchmaking.SetLobbyJoinable(new CSteamID(this.currentSteamLobbyId), isJoinable);
	}

	// Token: 0x040006B3 RID: 1715
	private static readonly bool disableSteam;

	// Token: 0x040006B4 RID: 1716
	public static readonly string LobbyNameKey = "name";

	// Token: 0x040006B5 RID: 1717
	public static readonly string LobbyCreator = "creator";

	// Token: 0x040006B6 RID: 1718
	public static readonly string LobbyStageKey = "stage";

	// Token: 0x040006B7 RID: 1719
	public static readonly string LobbyVersionKey = "version";

	// Token: 0x040006B8 RID: 1720
	public static readonly string LobbyGameModeKey = "mode";

	// Token: 0x040006B9 RID: 1721
	public static readonly string STEAM_LOBBY_LIST_UPDATED = "LOBBY_LIST_UPDATED";

	// Token: 0x040006BA RID: 1722
	public static readonly string STEAM_INITIALIZED = "INITIALIZED";

	// Token: 0x040006BB RID: 1723
	public static readonly string STEAM_LOBBY_FAILED_CREATE = "FAILED_CREATE";

	// Token: 0x040006BC RID: 1724
	private ProxyMono proxyObject;

	// Token: 0x040006BE RID: 1726
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	// Token: 0x040006BF RID: 1727
	public const string STEAM_LOBBY_ID_UPDATED = "STEAM_LOBBY_ID_UPDATED";

	// Token: 0x040006CA RID: 1738
	private Action<EChatRoomEnterResponse> currentLobbyJoinCallback;

	// Token: 0x040006CB RID: 1739
	private List<SteamManager.LobbyListDataEntry> lobbyList = new List<SteamManager.LobbyListDataEntry>();

	// Token: 0x040006CC RID: 1740
	protected Callback<GameLobbyJoinRequested_t> lobbyJoinRequestEvent;

	// Token: 0x040006CD RID: 1741
	protected Callback<LobbyCreated_t> lobbyCreated;

	// Token: 0x040006CE RID: 1742
	protected Callback<LobbyEnter_t> lobbyEnterEvent;

	// Token: 0x040006CF RID: 1743
	protected Callback<LobbyKicked_t> lobbyKickedEvent;

	// Token: 0x040006D0 RID: 1744
	protected Callback<LobbyChatUpdate_t> lobbyChatUpdateEvent;

	// Token: 0x040006D1 RID: 1745
	protected Callback<LobbyDataUpdate_t> lobbyDataUpdate;

	// Token: 0x040006D2 RID: 1746
	protected Callback<LobbyMatchList_t> lobbyMatchListEvent;

	// Token: 0x040006D3 RID: 1747
	private Callback<P2PSessionRequest_t> p2PSessionRequestCallback;

	// Token: 0x040006D4 RID: 1748
	private Callback<P2PSessionConnectFail_t> p2pSessionConnectFailCallback;

	// Token: 0x040006D5 RID: 1749
	private bool searchActive;

	// Token: 0x040006D6 RID: 1750
	public string LobbyStatus = "NULL";

	// Token: 0x040006D7 RID: 1751
	private ulong currentSteamLobbyId;

	// Token: 0x02000203 RID: 515
	public class LobbyListDataEntry
	{
		// Token: 0x040006D8 RID: 1752
		public ulong SteamId;

		// Token: 0x040006D9 RID: 1753
		public string LobbyName;

		// Token: 0x040006DA RID: 1754
		public int PlayerCount;

		// Token: 0x040006DB RID: 1755
		public int PlayerLimit;
	}

	// Token: 0x02000204 RID: 516
	public struct SteamLobbyData
	{
		// Token: 0x040006DC RID: 1756
		public CSteamID steamID;

		// Token: 0x040006DD RID: 1757
		public bool isHost;

		// Token: 0x040006DE RID: 1758
		public bool isSelf;
	}
}
