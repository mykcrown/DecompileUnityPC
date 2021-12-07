// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Text;

public class OptionValueDisplay
{
	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	public string ModeValueDisplay(int value)
	{
		return this.localization.GetText("gameRules." + (GameMode)value);
	}

	public string RulesValueDisplay(int value)
	{
		return this.localization.GetText("gameMode." + (GameRules)value);
	}

	public string TimeValueDisplay(int value)
	{
		if (value == 0)
		{
			return this.localization.GetText("ui.option.unlimitedTime");
		}
		StringBuilder stringBuilder = new StringBuilder();
		TimeUtil.FormatTime((float)value, stringBuilder, 0, false);
		return stringBuilder.ToString();
	}

	public string TeamAttackValueDisplay(int value)
	{
		return (value != 0) ? this.localization.GetText("ui.option.on") : this.localization.GetText("ui.option.off");
	}

	public string AssistStealingValueDisplay(int value)
	{
		return (value != 0) ? this.localization.GetText("ui.option.on") : this.localization.GetText("ui.option.off");
	}

	public string PauseModeValueDisplay(int value)
	{
		return this.localization.GetText("pauseMode." + (PauseMode)value);
	}
}
