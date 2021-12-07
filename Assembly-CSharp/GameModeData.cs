using System;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
public class GameModeData : ScriptableObject, IGameDataElement
{
	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06001A55 RID: 6741 RVA: 0x000890DC File Offset: 0x000874DC
	public string Key
	{
		get
		{
			return this.ModeTitle;
		}
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06001A56 RID: 6742 RVA: 0x000890E4 File Offset: 0x000874E4
	public int ID
	{
		get
		{
			return (int)this.Type;
		}
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06001A57 RID: 6743 RVA: 0x000890EC File Offset: 0x000874EC
	LocalizationData IGameDataElement.Localization
	{
		get
		{
			return this.localization;
		}
	}

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06001A58 RID: 6744 RVA: 0x000890F4 File Offset: 0x000874F4
	bool IGameDataElement.Enabled
	{
		get
		{
			return this.enabled;
		}
	}

	// Token: 0x06001A59 RID: 6745 RVA: 0x000890FC File Offset: 0x000874FC
	public virtual void RegisterPreload(PreloadContext context)
	{
		if (this.customData != null)
		{
			this.customData.RegisterPreload(context);
		}
	}

	// Token: 0x0400138E RID: 5006
	public string ModeTitle;

	// Token: 0x0400138F RID: 5007
	public GameMode Type;

	// Token: 0x04001390 RID: 5008
	public LocalizationData localization;

	// Token: 0x04001391 RID: 5009
	public bool enabled = true;

	// Token: 0x04001392 RID: 5010
	public bool demoEnabled = true;

	// Token: 0x04001393 RID: 5011
	public bool selectableInFreePlay = true;

	// Token: 0x04001394 RID: 5012
	public bool debugOnly;

	// Token: 0x04001395 RID: 5013
	public GameModeSettings settings;

	// Token: 0x04001396 RID: 5014
	public CustomGameModeData customData;
}
