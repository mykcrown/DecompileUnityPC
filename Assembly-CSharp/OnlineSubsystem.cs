using System;
using System.Collections.Generic;

// Token: 0x020001FE RID: 510
public class OnlineSubsystem : IOnlineSubsystem, IRichPresence
{
	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000969 RID: 2409 RVA: 0x0004F957 File Offset: 0x0004DD57
	// (set) Token: 0x0600096A RID: 2410 RVA: 0x0004F95F File Offset: 0x0004DD5F
	[Inject]
	public IAutoJoin autoJoin { get; set; }

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x0600096B RID: 2411 RVA: 0x0004F968 File Offset: 0x0004DD68
	// (set) Token: 0x0600096C RID: 2412 RVA: 0x0004F970 File Offset: 0x0004DD70
	[Inject]
	public DiscordManager discord { get; set; }

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x0600096D RID: 2413 RVA: 0x0004F979 File Offset: 0x0004DD79
	// (set) Token: 0x0600096E RID: 2414 RVA: 0x0004F981 File Offset: 0x0004DD81
	[Inject]
	public SteamManager steam { get; set; }

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x0600096F RID: 2415 RVA: 0x0004F98A File Offset: 0x0004DD8A
	// (set) Token: 0x06000970 RID: 2416 RVA: 0x0004F992 File Offset: 0x0004DD92
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x06000971 RID: 2417 RVA: 0x0004F99C File Offset: 0x0004DD9C
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

	// Token: 0x06000972 RID: 2418 RVA: 0x0004FA30 File Offset: 0x0004DE30
	public void ClearPresence()
	{
		foreach (IRichPresence richPresence in this.richPresenseComponents)
		{
			richPresence.ClearPresence();
		}
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0004FA8C File Offset: 0x0004DE8C
	public void SetPresence(string statusString, string loc1, string portraitKey, string portraitCaption)
	{
		if (!this.gameDataManager.IsFeatureEnabled(FeatureID.DisableRichPresenceStatus))
		{
			foreach (IRichPresence richPresence in this.richPresenseComponents)
			{
				richPresence.SetPresence(statusString, loc1, portraitKey, portraitCaption);
			}
		}
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0004FB00 File Offset: 0x0004DF00
	public void SetLobbyParameters(PresenceLobbyParameters presenceParams)
	{
		foreach (IRichPresence richPresence in this.richPresenseComponents)
		{
			richPresence.SetLobbyParameters(presenceParams);
		}
	}

	// Token: 0x040006AC RID: 1708
	private List<IRichPresence> richPresenseComponents = new List<IRichPresence>();
}
