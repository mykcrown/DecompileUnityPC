using System;

// Token: 0x02000AF3 RID: 2803
public class ChangePlaybackSpeedCommand : GameEvent
{
	// Token: 0x060050DC RID: 20700 RVA: 0x00150932 File Offset: 0x0014ED32
	public ChangePlaybackSpeedCommand(ChangePlaybackSpeedType type, float newSpeed = 1f)
	{
		this.type = type;
		this.newSpeed = newSpeed;
	}

	// Token: 0x04003438 RID: 13368
	public ChangePlaybackSpeedType type;

	// Token: 0x04003439 RID: 13369
	public float newSpeed;
}
