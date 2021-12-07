// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Collections.Generic;

public class MenuActions : InputModuleActions
{
	private List<ActionBinding> keyboardBindings = new List<ActionBinding>();

	private List<ActionBinding> controllerBindings = new List<ActionBinding>();

	private bool isKeyboardEnabled;

	private bool isControllerEnabled;

	private bool isPauseMode;

	private bool shouldUseKeyboard;

	private bool suppressKeyboard;

	public MenuActions()
	{
		this.keyboardBindings.Add(new ActionBinding(this.Cancel, Key.Escape));
		this.keyboardBindings.Add(new ActionBinding(this.Up, Key.UpArrow));
		this.keyboardBindings.Add(new ActionBinding(this.Up, Key.W));
		this.keyboardBindings.Add(new ActionBinding(this.Down, Key.DownArrow));
		this.keyboardBindings.Add(new ActionBinding(this.Down, Key.S));
		this.keyboardBindings.Add(new ActionBinding(this.Left, Key.LeftArrow));
		this.keyboardBindings.Add(new ActionBinding(this.Left, Key.A));
		this.keyboardBindings.Add(new ActionBinding(this.Right, Key.RightArrow));
		this.keyboardBindings.Add(new ActionBinding(this.Right, Key.D));
		this.keyboardBindings.Add(new ActionBinding(this.Submit, Key.Space));
		this.keyboardBindings.Add(new ActionBinding(this.Submit, Key.Return));
		this.keyboardBindings.Add(new ActionBinding(this.RightTrigger, Key.RightBracket));
		this.keyboardBindings.Add(new ActionBinding(this.RightTrigger, Key.E));
		this.keyboardBindings.Add(new ActionBinding(this.LeftTrigger, Key.LeftBracket));
		this.keyboardBindings.Add(new ActionBinding(this.LeftTrigger, Key.Q));
		this.keyboardBindings.Add(new ActionBinding(this.YButtonAction, Key.Backspace));
		this.keyboardBindings.Add(new ActionBinding(this.YButtonAction, Key.Delete));
		this.keyboardBindings.Add(new ActionBinding(this.XButtonAction, Key.T));
		this.keyboardBindings.Add(new ActionBinding(this.RightStickUp, Key.Pad8));
		this.keyboardBindings.Add(new ActionBinding(this.RightStickDown, Key.Pad2));
		this.keyboardBindings.Add(new ActionBinding(this.RightStickLeft, Key.Pad4));
		this.keyboardBindings.Add(new ActionBinding(this.RightStickRight, Key.Pad6));
		this.keyboardBindings.Add(new ActionBinding(this.ZButtonAction, Key.Z));
		this.keyboardBindings.Add(new ActionBinding(this.ZButtonAction, Key.R));
		this.keyboardBindings.Add(new ActionBinding(this.ZButtonAction, Key.C));
		this.keyboardBindings.Add(new ActionBinding(this.LeftBumper, Key.X));
		this.keyboardBindings.Add(new ActionBinding(this.LeftBumper, Key.L));
		this.controllerBindings.Add(new ActionBinding(this.LeftStickLeft, InputControlType.LeftStickLeft));
		this.controllerBindings.Add(new ActionBinding(this.LeftStickDown, InputControlType.LeftStickDown));
		this.controllerBindings.Add(new ActionBinding(this.LeftStickUp, InputControlType.LeftStickUp));
		this.controllerBindings.Add(new ActionBinding(this.LeftStickRight, InputControlType.LeftStickRight));
		this.controllerBindings.Add(new ActionBinding(this.RightStickUp, InputControlType.RightStickUp));
		this.controllerBindings.Add(new ActionBinding(this.RightStickDown, InputControlType.RightStickDown));
		this.controllerBindings.Add(new ActionBinding(this.RightStickLeft, InputControlType.RightStickLeft));
		this.controllerBindings.Add(new ActionBinding(this.RightStickRight, InputControlType.RightStickRight));
		this.controllerBindings.Add(new ActionBinding(this.Submit, InputControlType.Action1));
		this.controllerBindings.Add(new ActionBinding(this.Submit, InputControlType.Start));
		this.controllerBindings.Add(new ActionBinding(this.Submit, InputControlType.Menu));
		this.controllerBindings.Add(new ActionBinding(this.Submit, InputControlType.Options));
		this.controllerBindings.Add(new ActionBinding(this.Cancel, InputControlType.Action2));
		this.controllerBindings.Add(new ActionBinding(this.Up, InputControlType.DPadUp));
		this.controllerBindings.Add(new ActionBinding(this.Down, InputControlType.DPadDown));
		this.controllerBindings.Add(new ActionBinding(this.Left, InputControlType.DPadLeft));
		this.controllerBindings.Add(new ActionBinding(this.Right, InputControlType.DPadRight));
		this.controllerBindings.Add(new ActionBinding(this.YButtonAction, InputControlType.Action4));
		this.controllerBindings.Add(new ActionBinding(this.XButtonAction, InputControlType.Action3));
		this.controllerBindings.Add(new ActionBinding(this.RightTrigger, InputControlType.RightTrigger));
		this.controllerBindings.Add(new ActionBinding(this.LeftTrigger, InputControlType.LeftTrigger));
		this.controllerBindings.Add(new ActionBinding(this.LeftBumper, InputControlType.LeftBumper));
		this.controllerBindings.Add(new ActionBinding(this.ZButtonAction, InputControlType.RightBumper));
		this.controllerBindings.Add(new ActionBinding(this.DPadLeft, InputControlType.DPadLeft));
		this.controllerBindings.Add(new ActionBinding(this.DPadRight, InputControlType.DPadRight));
		this.controllerBindings.Add(new ActionBinding(this.DPadDown, InputControlType.DPadDown));
		this.controllerBindings.Add(new ActionBinding(this.DPadUp, InputControlType.DPadUp));
		this.EnableKeyboard();
		this.EnableController();
	}

	public void SuppressKeyboard(bool value)
	{
		this.suppressKeyboard = value;
		this.updateKeyboardState();
	}

	public void EnableKeyboard()
	{
		this.shouldUseKeyboard = true;
		this.updateKeyboardState();
	}

	public void DisableKeyboard()
	{
		this.shouldUseKeyboard = false;
		this.updateKeyboardState();
	}

	private void updateKeyboardState()
	{
		if (this.suppressKeyboard || !this.shouldUseKeyboard)
		{
			this.disableKeyboard();
		}
		else
		{
			this.enableKeyboard();
		}
	}

	private void enableKeyboard()
	{
		if (!this.isKeyboardEnabled)
		{
			this.isKeyboardEnabled = true;
			foreach (ActionBinding current in this.keyboardBindings)
			{
				current.Action.AddBinding(current.Key);
			}
		}
	}

	private void disableKeyboard()
	{
		if (this.isKeyboardEnabled)
		{
			this.isKeyboardEnabled = false;
			foreach (ActionBinding current in this.keyboardBindings)
			{
				current.Action.RemoveBinding(current.Key);
			}
		}
	}

	public void EnableController()
	{
		if (!this.isControllerEnabled)
		{
			this.isControllerEnabled = true;
			this.updateControllerState();
		}
	}

	public void DisableController()
	{
		if (this.isControllerEnabled)
		{
			this.isControllerEnabled = false;
			this.updateControllerState();
		}
	}

	public void SetPauseMode(bool isPause)
	{
		if (this.isPauseMode != isPause)
		{
			this.isPauseMode = isPause;
			this.updateControllerState();
		}
	}

	private void updateControllerState()
	{
		foreach (ActionBinding current in this.controllerBindings)
		{
			current.Action.HardRemoveBinding(current.Button);
			if (this.isControllerEnabled && (!this.isPauseMode || current.Action != this.Submit || current.Button.Control != InputControlType.Start))
			{
				current.Action.AddBinding(current.Button);
			}
		}
	}

	public bool KeyboardPressed()
	{
		foreach (ActionBinding current in this.keyboardBindings)
		{
			if (current.Action.WasPressed && current.Action.ActiveDevice.DeviceClass == InputDeviceClass.Unknown)
			{
				return true;
			}
		}
		return false;
	}
}
