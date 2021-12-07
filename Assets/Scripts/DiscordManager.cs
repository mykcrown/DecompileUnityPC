// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class DiscordManager : IRichPresence
{
	private string discordAppID;

	private ProxyMono proxyObject;

	private DiscordRpc.EventHandlers handlers;

	private DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();

	[Inject]
	public IOfflineModeDetector offlineMode
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
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public bool Initialized
	{
		get;
		private set;
	}

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

	public void RequestRespondYes()
	{
		UnityEngine.Debug.Log("Discord: responding yes to Ask to Join request");
	}

	public void RequestRespondNo()
	{
		UnityEngine.Debug.Log("Discord: responding no to Ask to Join request");
	}

	public void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser)
	{
		UnityEngine.Debug.Log("Discord: ready");
	}

	public void DisconnectedCallback(int errorCode, string message)
	{
		UnityEngine.Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
	}

	public void ErrorCallback(int errorCode, string message)
	{
		UnityEngine.Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
	}

	public void JoinCallback(string secret)
	{
		ulong num = 0uL;
		this.decryptSteam(secret, out num);
		UnityEngine.Debug.Log("Attempting to join the game" + num);
		if (num == 0uL)
		{
			return;
		}
		this.events.Broadcast(new OnStartJoinSteamLobbyCommand(num));
	}

	public void SpectateCallback(string secret)
	{
		UnityEngine.Debug.Log(string.Format("Discord: spectate ({0})", secret));
	}

	public void RequestCallback(ref DiscordRpc.DiscordUser connectedUser)
	{
	}

	private void Update()
	{
		if (!this.Initialized)
		{
			return;
		}
		DiscordRpc.RunCallbacks();
	}

	public void ClearPresence()
	{
		DiscordRpc.ClearPresence();
	}

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

	private string encrypt(string lobbyName, string lobbyPass)
	{
		return lobbyName + "." + lobbyPass;
	}

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

	private string encryptSteam(ulong steamID)
	{
		return steamID.ToString();
	}

	private void decryptSteam(string inHash, out ulong steamID)
	{
		if (!ulong.TryParse(inHash, out steamID))
		{
			steamID = 0uL;
		}
	}

	public void SetLobbyParameters(PresenceLobbyParameters presenceParams)
	{
		if (presenceParams.lobbySteamID != 0uL)
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

	private void OnDestroy()
	{
		DiscordRpc.Shutdown();
	}
}
