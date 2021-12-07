using System;
using System.Collections.Generic;
using InControl;

// Token: 0x020006A9 RID: 1705
public class MenuActions : InputModuleActions
{
	// Token: 0x06002A53 RID: 10835 RVA: 0x000DF170 File Offset: 0x000DD570
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

	// Token: 0x06002A54 RID: 10836 RVA: 0x000DF6AE File Offset: 0x000DDAAE
	public void SuppressKeyboard(bool value)
	{
		this.suppressKeyboard = value;
		this.updateKeyboardState();
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x000DF6BD File Offset: 0x000DDABD
	public void EnableKeyboard()
	{
		this.shouldUseKeyboard = true;
		this.updateKeyboardState();
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x000DF6CC File Offset: 0x000DDACC
	public void DisableKeyboard()
	{
		this.shouldUseKeyboard = false;
		this.updateKeyboardState();
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x000DF6DB File Offset: 0x000DDADB
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

	// Token: 0x06002A58 RID: 10840 RVA: 0x000DF704 File Offset: 0x000DDB04
	private void enableKeyboard()
	{
		if (!this.isKeyboardEnabled)
		{
			this.isKeyboardEnabled = true;
			foreach (ActionBinding actionBinding in this.keyboardBindings)
			{
				actionBinding.Action.AddBinding(actionBinding.Key);
			}
		}
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x000DF780 File Offset: 0x000DDB80
	private void disableKeyboard()
	{
		if (this.isKeyboardEnabled)
		{
			this.isKeyboardEnabled = false;
			foreach (ActionBinding actionBinding in this.keyboardBindings)
			{
				actionBinding.Action.RemoveBinding(actionBinding.Key);
			}
		}
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x000DF7F8 File Offset: 0x000DDBF8
	public void EnableController()
	{
		if (!this.isControllerEnabled)
		{
			this.isControllerEnabled = true;
			this.updateControllerState();
		}
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x000DF812 File Offset: 0x000DDC12
	public void DisableController()
	{
		if (this.isControllerEnabled)
		{
			this.isControllerEnabled = false;
			this.updateControllerState();
		}
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x000DF82C File Offset: 0x000DDC2C
	public void SetPauseMode(bool isPause)
	{
		if (this.isPauseMode != isPause)
		{
			this.isPauseMode = isPause;
			this.updateControllerState();
		}
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x000DF848 File Offset: 0x000DDC48
	private void updateControllerState()
	{
		foreach (ActionBinding actionBinding in this.controllerBindings)
		{
			actionBinding.Action.HardRemoveBinding(actionBinding.Button);
			if (this.isControllerEnabled && (!this.isPauseMode || actionBinding.Action != this.Submit || actionBinding.Button.Control != InputControlType.Start))
			{
				actionBinding.Action.AddBinding(actionBinding.Button);
			}
		}
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x000DF8FC File Offset: 0x000DDCFC
	public bool KeyboardPressed()
	{
		foreach (ActionBinding actionBinding in this.keyboardBindings)
		{
			if (actionBinding.Action.WasPressed && actionBinding.Action.ActiveDevice.DeviceClass == InputDeviceClass.Unknown)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04001E5C RID: 7772
	private List<ActionBinding> keyboardBindings = new List<ActionBinding>();

	// Token: 0x04001E5D RID: 7773
	private List<ActionBinding> controllerBindings = new List<ActionBinding>();

	// Token: 0x04001E5E RID: 7774
	private bool isKeyboardEnabled;

	// Token: 0x04001E5F RID: 7775
	private bool isControllerEnabled;

	// Token: 0x04001E60 RID: 7776
	private bool isPauseMode;

	// Token: 0x04001E61 RID: 7777
	private bool shouldUseKeyboard;

	// Token: 0x04001E62 RID: 7778
	private bool suppressKeyboard;
}
