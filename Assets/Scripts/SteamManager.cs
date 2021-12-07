// Decompile from assembly: Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class SteamManager : IRichPresence
{
	public class LobbyListDataEntry
	{
		public ulong SteamId;

		public string LobbyName;

		public int PlayerCount;

		public int PlayerLimit;
	}

	public struct SteamLobbyData
	{
		public CSteamID steamID;

		public bool isHost;

		public bool isSelf;
	}

	private sealed class _CreateLobby_c__AnonStorey0
	{
		internal string name;

		internal string pass;

		internal string stage;

		internal Action<ulong> onCreated;

		internal SteamManager _this;

		internal void __m__0(LobbyCreated_t lobbyResult)
		{
			this._this.OnCreateLobby(this.name, this.pass, this.stage, lobbyResult);
			this.onCreated(lobbyResult.m_ulSteamIDLobby);
		}
	}

	private static readonly bool disableSteam;

	public static readonly string LobbyNameKey = "name";

	public static readonly string LobbyCreator = "creator";

	public static readonly string LobbyStageKey = "stage";

	public static readonly string LobbyVersionKey = "version";

	public static readonly string LobbyGameModeKey = "mode";

	public static readonly string STEAM_LOBBY_LIST_UPDATED = "LOBBY_LIST_UPDATED";

	public static readonly string STEAM_INITIALIZED = "INITIALIZED";

	public static readonly string STEAM_LOBBY_FAILED_CREATE = "FAILED_CREATE";

	private ProxyMono proxyObject;

	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	public const string STEAM_LOBBY_ID_UPDATED = "STEAM_LOBBY_ID_UPDATED";

	private Action<EChatRoomEnterResponse> currentLobbyJoinCallback;

	private List<SteamManager.LobbyListDataEntry> lobbyList = new List<SteamManager.LobbyListDataEntry>();

	protected Callback<GameLobbyJoinRequested_t> lobbyJoinRequestEvent;

	protected Callback<LobbyCreated_t> lobbyCreated;

	protected Callback<LobbyEnter_t> lobbyEnterEvent;

	protected Callback<LobbyKicked_t> lobbyKickedEvent;

	protected Callback<LobbyChatUpdate_t> lobbyChatUpdateEvent;

	protected Callback<LobbyDataUpdate_t> lobbyDataUpdate;

	protected Callback<LobbyMatchList_t> lobbyMatchListEvent;

	private Callback<P2PSessionRequest_t> p2PSessionRequestCallback;

	private Callback<P2PSessionConnectFail_t> p2pSessionConnectFailCallback;

	private bool searchActive;

	public string LobbyStatus = "NULL";

	private ulong currentSteamLobbyId;

	public bool Initialized
	{
		get;
		private set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public IOfflineModeDetector offlineMode
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IStartupArgs startupArgs
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public P2PServerMgr p2pServerMgr
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServer
	{
		private get;
		set;
	}

	public List<SteamManager.LobbyListDataEntry> LobbyList
	{
		get
		{
			return this.lobbyList;
		}
	}

	public string UserName
	{
		get
		{
			string personaName = SteamFriends.GetPersonaName();
			return Regex.Replace(personaName, "[^a-zA-Z0-9 ]+", string.Empty, RegexOptions.Compiled);
		}
	}

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

	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		UnityEngine.Debug.LogWarning(pchDebugText);
	}

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
			UnityEngine.Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
		}
		if (!DllCheck.Test())
		{
			UnityEngine.Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
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
			UnityEngine.Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg);
			Application.Quit();
			return;
		}
		this.Initialized = SteamAPI.Init();
		if (!this.Initialized)
		{
			this.dialogController.ShowOneButtonDialog("STEAM NOT FOUND", "Please make sure Steam is running, you have Icons installed on Steam, and the steam_app.txt id file is present.", "OK", WindowTransition.STANDARD_FADE, false, default(AudioData));
			UnityEngine.Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.");
			return;
		}
		this.InitializeSteamLobbyJoinCallbacks();
		this.p2PSessionRequestCallback = Callback<P2PSessionRequest_t>.Create(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.OnP2PSessionRequest));
		this.p2pSessionConnectFailCallback = Callback<P2PSessionConnectFail_t>.Create(new Callback<P2PSessionConnectFail_t>.DispatchDelegate(this.OnP2PSessionConnectFail));
		SteamFriends.SetRichPresence("status", null);
		this.signalBus.Dispatch(SteamManager.STEAM_INITIALIZED);
	}

	private void OnP2PSessionRequest(P2PSessionRequest_t request)
	{
		UnityEngine.Debug.LogError("WE GOT IT  " + request.m_steamIDRemote.m_SteamID);
		CSteamID steamIDRemote = request.m_steamIDRemote;
		SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote);
	}

	private void OnP2PSessionConnectFail(P2PSessionConnectFail_t failEvent)
	{
		UnityEngine.Debug.LogError(string.Concat(new object[]
		{
			"SESSION CONNECT FAIL ",
			failEvent.m_steamIDRemote.m_SteamID,
			" ",
			failEvent.m_eP2PSessionError
		}));
	}

	private void InitializeSteamLobbyJoinCallbacks()
	{
		this.lobbyJoinRequestEvent = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(this._InitializeSteamLobbyJoinCallbacks_m__0));
		this.lobbyEnterEvent = Callback<LobbyEnter_t>.Create(new Callback<LobbyEnter_t>.DispatchDelegate(this._InitializeSteamLobbyJoinCallbacks_m__1));
		this.lobbyChatUpdateEvent = Callback<LobbyChatUpdate_t>.Create(new Callback<LobbyChatUpdate_t>.DispatchDelegate(this._InitializeSteamLobbyJoinCallbacks_m__2));
		this.lobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(new Callback<LobbyDataUpdate_t>.DispatchDelegate(this._InitializeSteamLobbyJoinCallbacks_m__3));
		this.lobbyKickedEvent = Callback<LobbyKicked_t>.Create(new Callback<LobbyKicked_t>.DispatchDelegate(this._InitializeSteamLobbyJoinCallbacks_m__4));
		this.lobbyMatchListEvent = Callback<LobbyMatchList_t>.Create(new Callback<LobbyMatchList_t>.DispatchDelegate(this._InitializeSteamLobbyJoinCallbacks_m__5));
		if (this.startupArgs.HasArg(StartupArgs.StartupArgType.SteamConnectLobby))
		{
			ulong argULongValue = this.startupArgs.GetArgULongValue(StartupArgs.StartupArgType.SteamConnectLobby);
			UnityEngine.Debug.LogFormat("Attempting to Join Steam Lobby From Startup Args: {0}", new object[]
			{
				argULongValue
			});
			if (argULongValue != 0uL)
			{
				SteamMatchmaking.JoinLobby(new CSteamID(argULongValue));
			}
		}
	}

	public void QuietlyAttemptJoinSteamLobby(ulong lobbyId)
	{
		if (this.CurrentSteamLobbyId != lobbyId)
		{
			this.LeaveLobby();
			this.CurrentSteamLobbyId = lobbyId;
			UnityEngine.Debug.LogFormat("Attempting to Join Steam Lobby From Icons Lobby Params: {0}", new object[]
			{
				lobbyId
			});
			if (lobbyId != 0uL)
			{
				SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
			}
		}
	}

	public bool IsOverlayEnabled()
	{
		return this.Initialized && SteamUtils.IsOverlayEnabled();
	}

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

	private void OnDestroy()
	{
		UnityEngine.Debug.Log("Steam manager on destroy");
		if (!this.Initialized)
		{
			return;
		}
		this.p2PSessionRequestCallback.Dispose();
		this.p2pSessionConnectFailCallback.Dispose();
		SteamAPI.Shutdown();
	}

	private void Update()
	{
		if (!this.Initialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	public void InviteToLobby(string lobbyName, string lobbyPass)
	{
		if (this.CurrentSteamLobbyId != 0uL)
		{
			SteamFriends.ActivateGameOverlayInviteDialog(new CSteamID(this.CurrentSteamLobbyId));
		}
	}

	public void CreateLobby(string name, string pass, ELobbyType lobbyType, string stage, Action<ulong> onCreated)
	{
		SteamManager._CreateLobby_c__AnonStorey0 _CreateLobby_c__AnonStorey = new SteamManager._CreateLobby_c__AnonStorey0();
		_CreateLobby_c__AnonStorey.name = name;
		_CreateLobby_c__AnonStorey.pass = pass;
		_CreateLobby_c__AnonStorey.stage = stage;
		_CreateLobby_c__AnonStorey.onCreated = onCreated;
		_CreateLobby_c__AnonStorey._this = this;
		SteamMatchmaking.CreateLobby(lobbyType, this.config.lobbySettings.maxPlayerNum);
		if (this.lobbyCreated != null)
		{
			this.lobbyCreated.Dispose();
		}
		this.lobbyCreated = Callback<LobbyCreated_t>.Create(new Callback<LobbyCreated_t>.DispatchDelegate(_CreateLobby_c__AnonStorey.__m__0));
	}

	public void LeaveLobby()
	{
		if (this.CurrentSteamLobbyId != 0uL)
		{
			this.LobbyStatus = "DISCONNECTED";
			SteamMatchmaking.LeaveLobby(new CSteamID(this.CurrentSteamLobbyId));
			this.CurrentSteamLobbyId = 0uL;
			this.p2pServerMgr.CleanupLobby();
		}
	}

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

	public void ClearPresence()
	{
		SteamFriends.ClearRichPresence();
		UnityEngine.Debug.Log("Steam cleared presence");
	}

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

	public bool GetMessage(ref byte[] message, ref uint messageLength, ref CSteamID remoteID)
	{
		uint cubDest;
		return !SteamManager.disableSteam && (SteamNetworking.IsP2PPacketAvailable(out cubDest, 0) && SteamNetworking.ReadP2PPacket(message, cubDest, out messageLength, out remoteID, 0));
	}

	public void SetLobbyValue(string key, string value)
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		if (this.CurrentSteamLobbyId != 0uL)
		{
			SteamMatchmaking.SetLobbyData(new CSteamID(this.CurrentSteamLobbyId), key, value);
		}
	}

	public string GetLobbyValue(string key)
	{
		if (SteamManager.disableSteam)
		{
			return null;
		}
		if (this.CurrentSteamLobbyId != 0uL)
		{
			return SteamMatchmaking.GetLobbyData(new CSteamID(this.CurrentSteamLobbyId), key);
		}
		return null;
	}

	public void SetLobbyParameters(PresenceLobbyParameters presenceParams)
	{
	}

	public void Send(CSteamID steamIDRemote, byte[] bytes, uint length, bool useTCP)
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		if (!SteamNetworking.SendP2PPacket(steamIDRemote, bytes, length, (!useTCP) ? EP2PSend.k_EP2PSendUnreliableNoDelay : EP2PSend.k_EP2PSendReliable, 0))
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
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

	public List<SteamManager.SteamLobbyData> GetReceivers()
	{
		if (SteamManager.disableSteam)
		{
			return new List<SteamManager.SteamLobbyData>();
		}
		List<SteamManager.SteamLobbyData> list = new List<SteamManager.SteamLobbyData>();
		if (this.currentSteamLobbyId != 0uL)
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

	public CSteamID MySteamID()
	{
		if (SteamManager.disableSteam)
		{
			return CSteamID.Nil;
		}
		return SteamUser.GetSteamID();
	}

	public string GetUserName(CSteamID steamID)
	{
		if (SteamManager.disableSteam)
		{
			return null;
		}
		return SteamFriends.GetFriendPersonaName(steamID);
	}

	public bool IsFriend(ulong steamId)
	{
		return !SteamManager.disableSteam && SteamFriends.HasFriend(new CSteamID(steamId), EFriendFlags.k_EFriendFlagImmediate);
	}

	public void JoinFriendLobby()
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		SteamFriends.ActivateGameOverlay("Friends");
	}

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

	public void JoinLobby(ulong lobbyId, Action<EChatRoomEnterResponse> onJoinAttempted)
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		this.currentLobbyJoinCallback = onJoinAttempted;
		SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
	}

	public void SetJoinable(bool isJoinable)
	{
		if (SteamManager.disableSteam)
		{
			return;
		}
		SteamMatchmaking.SetLobbyJoinable(new CSteamID(this.currentSteamLobbyId), isJoinable);
	}

	private void _InitializeSteamLobbyJoinCallbacks_m__0(GameLobbyJoinRequested_t enterResult)
	{
		if (this.CurrentSteamLobbyId != enterResult.m_steamIDLobby.m_SteamID)
		{
			this.LeaveLobby();
			UnityEngine.Debug.LogFormat("Attempting to Join Steam Lobby From Direct Invite: {0}", new object[]
			{
				enterResult.m_steamIDLobby.m_SteamID
			});
			if (enterResult.m_steamIDLobby.m_SteamID != 0uL)
			{
				SteamMatchmaking.JoinLobby(enterResult.m_steamIDLobby);
			}
		}
	}

	private void _InitializeSteamLobbyJoinCallbacks_m__1(LobbyEnter_t enterResult)
	{
		this.battleServer.ResetRoom();
		UnityEngine.Debug.Log("Steam Lobby Enter Response: " + enterResult.m_EChatRoomEnterResponse);
		if (enterResult.m_EChatRoomEnterResponse == 1u)
		{
			this.CurrentSteamLobbyId = enterResult.m_ulSteamIDLobby;
			this.p2pServerMgr.OnUpdateCustomLobby();
		}
		if (this.currentLobbyJoinCallback != null)
		{
			this.currentLobbyJoinCallback((EChatRoomEnterResponse)enterResult.m_EChatRoomEnterResponse);
			this.currentLobbyJoinCallback = null;
		}
	}

	private void _InitializeSteamLobbyJoinCallbacks_m__2(LobbyChatUpdate_t chatUpdate)
	{
		this.p2pServerMgr.OnUpdateCustomLobby();
	}

	private void _InitializeSteamLobbyJoinCallbacks_m__3(LobbyDataUpdate_t chatUpdate)
	{
		this.p2pServerMgr.OnUpdateCustomLobbyData();
	}

	private void _InitializeSteamLobbyJoinCallbacks_m__4(LobbyKicked_t kickResult)
	{
		if (kickResult.m_ulSteamIDLobby == this.CurrentSteamLobbyId)
		{
			this.CurrentSteamLobbyId = 0uL;
			this.p2pServerMgr.OnLeftLobby();
		}
	}

	private void _InitializeSteamLobbyJoinCallbacks_m__5(LobbyMatchList_t lobbyMatchList)
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
	}
}
