using System;

// Token: 0x02000A4A RID: 2634
public class GameModeChangedEvent : UIEvent
{
	// Token: 0x06004CF5 RID: 19701 RVA: 0x00145790 File Offset: 0x00143B90
	public GameModeChangedEvent(GameMode rules, GameModeData data)
	{
		this.rules = rules;
		this.data = data;
	}

	// Token: 0x04003275 RID: 12917
	public GameMode rules;

	// Token: 0x04003276 RID: 12918
	public GameModeData data;
}
