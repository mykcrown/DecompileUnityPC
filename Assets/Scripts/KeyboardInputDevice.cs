// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;

public class KeyboardInputDevice : IInputDevice
{
	private bool thisActivationKeyPressed;

	private bool lastActivationKeyPressed;

	public string Name
	{
		get
		{
			return "Keyboard";
		}
	}

	public bool WasAnyKeyPressed
	{
		get
		{
			return this.thisActivationKeyPressed && !this.lastActivationKeyPressed;
		}
	}

	public void DoUpdate()
	{
		this.lastActivationKeyPressed = this.thisActivationKeyPressed;
		KeyCombo keyCombo = KeyCombo.Detect(true);
		keyCombo.AddExclude(Key.Escape);
		this.thisActivationKeyPressed = keyCombo.IsPressed;
	}

	public void MangleDefaultSettings(InputSettingsData settings)
	{
		settings.tapToJumpEnabled = true;
		settings.tapToStrikeEnabled = true;
		settings.recoveryJumpingEnabled = false;
		settings.doubleTapToRun = false;
	}
}
