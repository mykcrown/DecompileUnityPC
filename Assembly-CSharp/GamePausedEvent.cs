using System;

// Token: 0x02000AF8 RID: 2808
public class GamePausedEvent : GameEvent
{
	// Token: 0x060050E1 RID: 20705 RVA: 0x0015097D File Offset: 0x0014ED7D
	public GamePausedEvent(bool paused, PlayerNum player)
	{
		this.paused = paused;
		this.player = player;
	}

	// Token: 0x0400343D RID: 13373
	public bool paused;

	// Token: 0x0400343E RID: 13374
	public PlayerNum player;
}
