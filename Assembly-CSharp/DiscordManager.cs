using System;
using UnityEngine;

// Token: 0x020001F1 RID: 497
public class DiscordManager : IRichPresence
{
	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06000924 RID: 2340 RVA: 0x0004F186 File Offset: 0x0004D586
	// (set) Token: 0x06000925 RID: 2341 RVA: 0x0004F18E File Offset: 0x0004D58E
	[Inject]
	public IOfflineModeDetector offlineMode { get; set; }

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06000926 RID: 2342 RVA: 0x0004F197 File Offset: 0x0004D597
	// (set) Token: 0x06000927 RID: 2343 RVA: 0x0004F19F File Offset: 0x0004D59F
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06000928 RID: 2344 RVA: 0x0004F1A8 File Offset: 0x0004D5A8
	// (set) Token: 0x06000929 RID: 2345 RVA: 0x0004F1B0 File Offset: 0x0004D5B0
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x0600092A RID: 2346 RVA: 0x0004F1B9 File Offset: 0x0004D5B9
	// (set) Token: 0x0600092B RID: 2347 RVA: 0x0004F1C1 File Offset: 0x0004D5C1
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x0600092C RID: 2348 RVA: 0x0004F1CA File Offset: 0x0004D5CA
	// (set) Token: 0x0600092D RID: 2349 RVA: 0x0004F1D2 File Offset: 0x0004D5D2
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x0600092E RID: 2350 RVA: 0x0004F1DB File Offset: 0x0004D5DB
	// (set) Token: 0x0600092F RID: 2351 RVA: 0x0004F1E3 File Offset: 0x0004D5E3
	public bool Initialized { get; private set; }

	// Token: 0x06000930 RID: 2352 RVA: 0x0004F1EC File Offset: 0x0004D5EC
	public void Startup()
	{
		if (this.offlineMode.IsOfflineMode())
		{
			return;
		}
		this.proxyObject = ProxyMono.CreateNew("DiscordManager");
		this.proxyObject.OnUpdate = new Action(this.Update);
		this.proxyObject.OnDestroyFn = new Action(this.OnDestroy);
		this.handlers = default(DiscordRpc.EventHandlers);
		this.handlers.readyCallback = new DiscordRpc.ReadyCallback(this.ReadyCallback);
		this.handlers.disconnectedCallback = (DiscordRpc.DisconnectedCallback)Delegate.Combine(this.handlers.disconnectedCallback, new DiscordRpc.DisconnectedCallback(this.DisconnectedCallback));
		this.handlers.errorCallback = (DiscordRpc.ErrorCallback)Delegate.Combine(this.handlers.errorCallback, new DiscordRpc.ErrorCallback(this.ErrorCallback));
		this.handlers.joinCallback = (DiscordRpc.JoinCallback)Delegate.Combine(this.handlers.joinCallback, new DiscordRpc.JoinCallback(this.JoinCallback));
		this.handlers.spectateCallback = (DiscordRpc.SpectateCallback)Delegate.Combine(this.handlers.spectateCallback, new DiscordRpc.SpectateCallback(this.SpectateCallback));
		this.handlers.requestCallback = (DiscordRpc.RequestCallback)Delegate.Combine(this.handlers.requestCallback, new DiscordRpc.RequestCallback(this.RequestCallback));
		DiscordRpc.Initialize(this.config.networkSettings.DiscordAppID.ToString(), ref this.handlers, true, this.config.networkSettings.AppID.ToString());
		this.presence.details = this.localization.GetText("discord.details.inGame");
		DiscordRpc.UpdatePresence(this.presence);
		this.Initialized = true;
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0004F3A0 File Offset: 0x0004D7A0
	public void RequestRespondYes()
	{
		Debug.Log("Discord: responding yes to Ask to Join request");
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0004F3AC File Offset: 0x0004D7AC
	public void RequestRespondNo()
	{
		Debug.Log("Discord: responding no to Ask to Join request");
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0004F3B8 File Offset: 0x0004D7B8
	public void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser)
	{
		Debug.Log("Discord: ready");
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0004F3C4 File Offset: 0x0004D7C4
	public void DisconnectedCallback(int errorCode, string message)
	{
		Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0004F3DC File Offset: 0x0004D7DC
	public void ErrorCallback(int errorCode, string message)
	{
		Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0004F3F4 File Offset: 0x0004D7F4
	public void JoinCallback(string secret)
	{
		ulong num = 0UL;
		this.decryptSteam(secret, out num);
		Debug.Log("Attempting to join the game" + num);
		if (num == 0UL)
		{
			return;
		}
		this.events.Broadcast(new OnStartJoinSteamLobbyCommand(num));
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0004F43C File Offset: 0x0004D83C
	public void SpectateCallback(string secret)
	{
		Debug.Log(string.Format("Discord: spectate ({0})", secret));
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x0004F44E File Offset: 0x0004D84E
	public void RequestCallback(ref DiscordRpc.DiscordUser connectedUser)
	{
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0004F450 File Offset: 0x0004D850
	private void Update()
	{
		if (!this.Initialized)
		{
			return;
		}
		DiscordRpc.RunCallbacks();
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0004F463 File Offset: 0x0004D863
	public void ClearPresence()
	{
		DiscordRpc.ClearPresence();
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0004F46C File Offset: 0x0004D86C
	public void SetPresence(string statusString, string locKey1, string portraitKey, string portraitCaption)
	{
		this.presence.largeImageKey = null;
		this.presence.largeImageText = null;
		if (statusString == null)
		{
			this.presence.state = null;
		}
		else
		{
			string replace = null;
			if (locKey1 != null)
			{
				replace = this.localization.GetText("discord." + locKey1);
			}
			this.presence.state = this.localization.GetText("discord.state." + statusString, replace);
			if (portraitKey != null)
			{
				this.presence.largeImageKey = this.localization.GetText("discord.presence." + statusString + ".portrait", portraitKey.ToLower());
				this.presence.largeImageText = this.localization.GetText("discord.presence." + statusString + ".portraitCaption", this.localization.GetText("gameData.characters.name." + portraitKey), portraitCaption);
			}
		}
		DiscordRpc.UpdatePresence(this.presence);
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x0004F564 File Offset: 0x0004D964
	private string encrypt(string lobbyName, string lobbyPass)
	{
		return lobbyName + "." + lobbyPass;
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0004F574 File Offset: 0x0004D974
	private void decrypt(string inHash, out string lobbyName, out string lobbyPass)
	{
		string[] array = inHash.Split(new char[]
		{
			'.'
		});
		if (array.Length != 2)
		{
			lobbyName = null;
			lobbyPass = null;
			return;
		}
		lobbyName = array[0];
		lobbyPass = array[1];
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0004F5AD File Offset: 0x0004D9AD
	private string encryptSteam(ulong steamID)
	{
		return steamID.ToString();
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0004F5BC File Offset: 0x0004D9BC
	private void decryptSteam(string inHash, out ulong steamID)
	{
		if (!ulong.TryParse(inHash, out steamID))
		{
			steamID = 0UL;
		}
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0004F5D0 File Offset: 0x0004D9D0
	public void SetLobbyParameters(PresenceLobbyParameters presenceParams)
	{
		if (presenceParams.lobbySteamID != 0UL)
		{
			this.presence.partyId = presenceParams.lobbyGuid.ToString();
			this.presence.partySize = presenceParams.numMembers;
			this.presence.partyMax = presenceParams.maxMembers;
			this.presence.joinSecret = this.encryptSteam(presenceParams.lobbySteamID);
		}
		else
		{
			this.presence.partyId = null;
			this.presence.partySize = 0;
			this.presence.partyMax = 0;
			this.presence.joinSecret = null;
		}
		DiscordRpc.UpdatePresence(this.presence);
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0004F684 File Offset: 0x0004DA84
	private void OnDestroy()
	{
		DiscordRpc.Shutdown();
	}

	// Token: 0x04000675 RID: 1653
	private string discordAppID;

	// Token: 0x04000676 RID: 1654
	private ProxyMono proxyObject;

	// Token: 0x04000678 RID: 1656
	private DiscordRpc.EventHandlers handlers;

	// Token: 0x04000679 RID: 1657
	private DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();
}
