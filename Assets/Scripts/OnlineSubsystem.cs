// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class OnlineSubsystem : IOnlineSubsystem, IRichPresence
{
	private List<IRichPresence> richPresenseComponents = new List<IRichPresence>();

	[Inject]
	public IAutoJoin autoJoin
	{
		get;
		set;
	}

	[Inject]
	public DiscordManager discord
	{
		get;
		set;
	}

	[Inject]
	public SteamManager steam
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

	public void Startup()
	{
		if (this.discord != null && this.gameDataManager.IsFeatureEnabled(FeatureID.DiscordRichPresence) && !this.gameDataManager.IsFeatureEnabled(FeatureID.DisableRichPresenceStatus))
		{
			this.discord.Startup();
			this.richPresenseComponents.Add(this.discord);
		}
		if (this.steam != null)
		{
			this.steam.Startup();
			if (this.gameDataManager.IsFeatureEnabled(FeatureID.SteamRichPresence))
			{
				this.richPresenseComponents.Add(this.steam);
			}
		}
	}

	public void ClearPresence()
	{
		foreach (IRichPresence current in this.richPresenseComponents)
		{
			current.ClearPresence();
		}
	}

	public void SetPresence(string statusString, string loc1, string portraitKey, string portraitCaption)
	{
		if (!this.gameDataManager.IsFeatureEnabled(FeatureID.DisableRichPresenceStatus))
		{
			foreach (IRichPresence current in this.richPresenseComponents)
			{
				current.SetPresence(statusString, loc1, portraitKey, portraitCaption);
			}
		}
	}

	public void SetLobbyParameters(PresenceLobbyParameters presenceParams)
	{
		foreach (IRichPresence current in this.richPresenseComponents)
		{
			current.SetLobbyParameters(presenceParams);
		}
	}
}
