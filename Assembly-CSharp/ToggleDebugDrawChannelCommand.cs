using System;

// Token: 0x02000AD4 RID: 2772
public class ToggleDebugDrawChannelCommand : GameEvent
{
	// Token: 0x060050B5 RID: 20661 RVA: 0x00150651 File Offset: 0x0014EA51
	public ToggleDebugDrawChannelCommand(DebugDrawChannel channel, bool enabled)
	{
		this.channel = channel;
		this.enabled = enabled;
	}

	// Token: 0x040033FD RID: 13309
	public DebugDrawChannel channel;

	// Token: 0x040033FE RID: 13310
	public bool enabled;
}
