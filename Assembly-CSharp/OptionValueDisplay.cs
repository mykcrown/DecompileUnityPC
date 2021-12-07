using System;
using System.Text;

// Token: 0x020008E7 RID: 2279
public class OptionValueDisplay
{
	// Token: 0x17000E0A RID: 3594
	// (get) Token: 0x06003A5F RID: 14943 RVA: 0x001117AD File Offset: 0x0010FBAD
	// (set) Token: 0x06003A60 RID: 14944 RVA: 0x001117B5 File Offset: 0x0010FBB5
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x06003A61 RID: 14945 RVA: 0x001117C0 File Offset: 0x0010FBC0
	public string ModeValueDisplay(int value)
	{
		return this.localization.GetText("gameRules." + (GameMode)value);
	}

	// Token: 0x06003A62 RID: 14946 RVA: 0x001117EC File Offset: 0x0010FBEC
	public string RulesValueDisplay(int value)
	{
		return this.localization.GetText("gameMode." + (GameRules)value);
	}

	// Token: 0x06003A63 RID: 14947 RVA: 0x00111818 File Offset: 0x0010FC18
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

	// Token: 0x06003A64 RID: 14948 RVA: 0x00111852 File Offset: 0x0010FC52
	public string TeamAttackValueDisplay(int value)
	{
		return (value != 0) ? this.localization.GetText("ui.option.on") : this.localization.GetText("ui.option.off");
	}

	// Token: 0x06003A65 RID: 14949 RVA: 0x0011187F File Offset: 0x0010FC7F
	public string AssistStealingValueDisplay(int value)
	{
		return (value != 0) ? this.localization.GetText("ui.option.on") : this.localization.GetText("ui.option.off");
	}

	// Token: 0x06003A66 RID: 14950 RVA: 0x001118AC File Offset: 0x0010FCAC
	public string PauseModeValueDisplay(int value)
	{
		return this.localization.GetText("pauseMode." + (PauseMode)value);
	}
}
