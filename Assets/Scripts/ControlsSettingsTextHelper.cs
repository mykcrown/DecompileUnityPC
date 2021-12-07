// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Text.RegularExpressions;

public static class ControlsSettingsTextHelper
{
	private const string EMPTY_BINDING_TEXT = "---";

	public static string GetLocalizedBindingTitle(ILocalization localization, ButtonPress button)
	{
		string key = "ui.settings.controls." + button.ToString();
		return localization.GetText(key);
	}

	public static string GetDirectionaBindingTitle(ILocalization localization, ButtonPress button)
	{
		switch (button)
		{
		case ButtonPress.TauntLeft:
		case ButtonPress.TauntRight:
		case ButtonPress.TauntUp:
		case ButtonPress.TauntDown:
			return localization.GetText("ui.dialog.advancedController.emotes");
		case ButtonPress.Shield2:
		case ButtonPress.Strike:
			IL_51:
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
		goto IL_51;
	}

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
			XInputDevice xInputDevice = inputDevice as XInputDevice;
			if (xInputDevice != null)
			{
				num = xInputDevice.DeviceIndex + 1;
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

	public static string GetDirectionalBindingText(ILocalization localization, DirectionalBindingHelper.DirectionalDevice directionalDevice)
	{
		if (directionalDevice == DirectionalBindingHelper.DirectionalDevice.None)
		{
			return "---";
		}
		return localization.GetText(string.Format("ui.dialog.advancedController.{0}", directionalDevice));
	}

	public static string GetDirectionalButtonGroupText(ILocalization localization, DirectionalBindingHelper.DirectionalButtonGroup buttonGroup)
	{
		return localization.GetText(string.Format("ui.dialog.advancedKeyboard.{0}", buttonGroup));
	}
}
