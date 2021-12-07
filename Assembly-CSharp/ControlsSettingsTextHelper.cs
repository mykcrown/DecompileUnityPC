using System;
using System.Text.RegularExpressions;
using InControl;

// Token: 0x0200096E RID: 2414
public static class ControlsSettingsTextHelper
{
	// Token: 0x060040B1 RID: 16561 RVA: 0x001237A8 File Offset: 0x00121BA8
	public static string GetLocalizedBindingTitle(ILocalization localization, ButtonPress button)
	{
		string key = "ui.settings.controls." + button.ToString();
		return localization.GetText(key);
	}

	// Token: 0x060040B2 RID: 16562 RVA: 0x001237D4 File Offset: 0x00121BD4
	public static string GetDirectionaBindingTitle(ILocalization localization, ButtonPress button)
	{
		switch (button)
		{
		case ButtonPress.TauntLeft:
		case ButtonPress.TauntRight:
		case ButtonPress.TauntUp:
		case ButtonPress.TauntDown:
			return localization.GetText("ui.dialog.advancedController.emotes");
		default:
			switch (button)
			{
			case ButtonPress.Forward:
			case ButtonPress.Backward:
			case ButtonPress.Up:
			case ButtonPress.Down:
				return localization.GetText("ui.dialog.advancedController.movement");
			default:
				return string.Empty;
			}
			break;
		case ButtonPress.UpStrike:
		case ButtonPress.DownStrike:
		case ButtonPress.BackwardStrike:
		case ButtonPress.ForwardStrike:
			return localization.GetText("ui.dialog.advancedController.strikes");
		case ButtonPress.UpTilt:
		case ButtonPress.DownTilt:
		case ButtonPress.BackwardTilt:
		case ButtonPress.ForwardTilt:
			return localization.GetText("ui.dialog.advancedController.tilts");
		case ButtonPress.UpSpecial:
		case ButtonPress.DownSpecial:
		case ButtonPress.BackwardSpecial:
		case ButtonPress.ForwardSpecial:
			return localization.GetText("ui.dialog.advancedController.specials");
		}
	}

	// Token: 0x060040B3 RID: 16563 RVA: 0x00123890 File Offset: 0x00121C90
	public static string GetDeviceTitle(ILocalization localization, IInputDevice device)
	{
		InputDevice inputDevice = device as InputDevice;
		KeyboardInputDevice keyboardInputDevice = device as KeyboardInputDevice;
		if (inputDevice != null)
		{
			string text = localization.GetText("ui.settings.controls." + inputDevice.Name);
			int num = -1;
			UnityInputDevice unityInputDevice = inputDevice as UnityInputDevice;
			if (unityInputDevice != null)
			{
				num = unityInputDevice.JoystickId;
			}
			XInputDevice xinputDevice = inputDevice as XInputDevice;
			if (xinputDevice != null)
			{
				num = xinputDevice.DeviceIndex + 1;
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text + " " + num;
			}
			return localization.GetText("ui.settings.controls.unknown") + " " + num;
		}
		else
		{
			if (keyboardInputDevice != null)
			{
				return localization.GetText("ui.settings.controls.keyboard");
			}
			return localization.GetText("ui.settings.controls.unknown");
		}
	}

	// Token: 0x060040B4 RID: 16564 RVA: 0x00123950 File Offset: 0x00121D50
	public static string GetUnboundBindingText(ILocalization localization, ButtonPress press, BindingSource bindingSource)
	{
		string bindingText = ControlsSettingsTextHelper.GetBindingText(localization, bindingSource, false);
		string localizedBindingTitle = ControlsSettingsTextHelper.GetLocalizedBindingTitle(localization, press);
		if (string.IsNullOrEmpty(localizedBindingTitle))
		{
			return null;
		}
		return localization.GetText("ui.settings.controls.unbound", bindingText, localizedBindingTitle);
	}

	// Token: 0x060040B5 RID: 16565 RVA: 0x00123988 File Offset: 0x00121D88
	public static string GetDirectionalUnboundBindingText(ILocalization localization, ButtonPress press, DirectionalBindingHelper.DirectionalDevice directionalDevice)
	{
		string directionalBindingText = ControlsSettingsTextHelper.GetDirectionalBindingText(localization, directionalDevice);
		string directionaBindingTitle = ControlsSettingsTextHelper.GetDirectionaBindingTitle(localization, press);
		if (string.IsNullOrEmpty(directionaBindingTitle))
		{
			return null;
		}
		return localization.GetText("ui.settings.controls.unbound", directionalBindingText, directionaBindingTitle);
	}

	// Token: 0x060040B6 RID: 16566 RVA: 0x001239C0 File Offset: 0x00121DC0
	public static string GetBindingText(ILocalization localization, BindingSource binding, bool isListening)
	{
		string text = "---";
		if (isListening)
		{
			text = "---";
		}
		else if (binding != null && binding.DeviceClass != InputDeviceClass.Unknown)
		{
			if (binding is DeviceBindingSource)
			{
				DeviceBindingSource deviceBindingSource = (DeviceBindingSource)binding;
				text = deviceBindingSource.Name;
			}
			else if (binding is KeyBindingSource)
			{
				KeyBindingSource keyBindingSource = (KeyBindingSource)binding;
				KeyCombo control = keyBindingSource.Control;
				if (control.IncludeCount == 1)
				{
					text = control.GetInclude(0).ToString();
				}
			}
			else if (binding is MouseBindingSource)
			{
				MouseBindingSource mouseBindingSource = (MouseBindingSource)binding;
				text = mouseBindingSource.Name;
			}
			text = Regex.Replace(text, "([a-z])([A-Z])", "$1 $2");
			text = text.ToUpper();
		}
		return text;
	}

	// Token: 0x060040B7 RID: 16567 RVA: 0x00123A90 File Offset: 0x00121E90
	public static string GetDirectionalBindingText(ILocalization localization, DirectionalBindingHelper.DirectionalDevice directionalDevice)
	{
		if (directionalDevice == DirectionalBindingHelper.DirectionalDevice.None)
		{
			return "---";
		}
		return localization.GetText(string.Format("ui.dialog.advancedController.{0}", directionalDevice));
	}

	// Token: 0x060040B8 RID: 16568 RVA: 0x00123AB4 File Offset: 0x00121EB4
	public static string GetDirectionalButtonGroupText(ILocalization localization, DirectionalBindingHelper.DirectionalButtonGroup buttonGroup)
	{
		return localization.GetText(string.Format("ui.dialog.advancedKeyboard.{0}", buttonGroup));
	}

	// Token: 0x04002BA4 RID: 11172
	private const string EMPTY_BINDING_TEXT = "---";
}
