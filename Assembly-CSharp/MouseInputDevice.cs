using System;

// Token: 0x02000695 RID: 1685
public class MouseInputDevice : IInputDevice
{
	// Token: 0x17000A2F RID: 2607
	// (get) Token: 0x06002991 RID: 10641 RVA: 0x000DDD01 File Offset: 0x000DC101
	public string Name
	{
		get
		{
			return "Mouse";
		}
	}

	// Token: 0x06002992 RID: 10642 RVA: 0x000DDD08 File Offset: 0x000DC108
	public void MangleDefaultSettings(InputSettingsData settings)
	{
		settings.tapToJumpEnabled = true;
		settings.tapToStrikeEnabled = true;
		settings.recoveryJumpingEnabled = false;
		settings.doubleTapToRun = false;
	}
}
