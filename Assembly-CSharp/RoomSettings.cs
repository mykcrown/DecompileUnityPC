using System;

// Token: 0x02000809 RID: 2057
public class RoomSettings : IRoomSettings
{
	// Token: 0x060032C0 RID: 12992 RVA: 0x000F3333 File Offset: 0x000F1733
	public RoomSettings(bool isVisible, byte maxPlayers)
	{
		this.IsVisible = isVisible;
		this.MaxPlayers = maxPlayers;
	}

	// Token: 0x17000C5C RID: 3164
	// (get) Token: 0x060032C1 RID: 12993 RVA: 0x000F3349 File Offset: 0x000F1749
	// (set) Token: 0x060032C2 RID: 12994 RVA: 0x000F3351 File Offset: 0x000F1751
	public bool IsVisible { get; set; }

	// Token: 0x17000C5D RID: 3165
	// (get) Token: 0x060032C3 RID: 12995 RVA: 0x000F335A File Offset: 0x000F175A
	// (set) Token: 0x060032C4 RID: 12996 RVA: 0x000F3362 File Offset: 0x000F1762
	public byte MaxPlayers { get; set; }
}
