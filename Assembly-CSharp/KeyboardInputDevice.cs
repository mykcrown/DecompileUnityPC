using System;
using InControl;

// Token: 0x02000694 RID: 1684
public class KeyboardInputDevice : IInputDevice
{
	// Token: 0x17000A2D RID: 2605
	// (get) Token: 0x0600298C RID: 10636 RVA: 0x000DDC82 File Offset: 0x000DC082
	public string Name
	{
		get
		{
			return "Keyboard";
		}
	}

	// Token: 0x0600298D RID: 10637 RVA: 0x000DDC8C File Offset: 0x000DC08C
	public void DoUpdate()
	{
		this.lastActivationKeyPressed = this.thisActivationKeyPressed;
		KeyCombo keyCombo = KeyCombo.Detect(true);
		keyCombo.AddExclude(Key.Escape);
		this.thisActivationKeyPressed = keyCombo.IsPressed;
	}

	// Token: 0x0600298E RID: 10638 RVA: 0x000DDCC2 File Offset: 0x000DC0C2
	public void MangleDefaultSettings(InputSettingsData settings)
	{
		settings.tapToJumpEnabled = true;
		settings.tapToStrikeEnabled = true;
		settings.recoveryJumpingEnabled = false;
		settings.doubleTapToRun = false;
	}

	// Token: 0x17000A2E RID: 2606
	// (get) Token: 0x0600298F RID: 10639 RVA: 0x000DDCE0 File Offset: 0x000DC0E0
	public bool WasAnyKeyPressed
	{
		get
		{
			return this.thisActivationKeyPressed && !this.lastActivationKeyPressed;
		}
	}

	// Token: 0x04001DED RID: 7661
	private bool thisActivationKeyPressed;

	// Token: 0x04001DEE RID: 7662
	private bool lastActivationKeyPressed;
}
