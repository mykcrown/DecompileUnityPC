// Decompile from assembly: Assembly-CSharp.dll

using System;

public class MouseInputDevice : IInputDevice
{
	public string Name
	{
		get
		{
			return "Mouse";
		}
	}

	public void MangleDefaultSettings(InputSettingsData settings)
	{
		settings.tapToJumpEnabled = true;
		settings.tapToStrikeEnabled = true;
		settings.recoveryJumpingEnabled = false;
		settings.doubleTapToRun = false;
	}
}
