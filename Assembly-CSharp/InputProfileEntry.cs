using System;

// Token: 0x02000480 RID: 1152
[Serializable]
public class InputProfileEntry
{
	// Token: 0x060018E3 RID: 6371 RVA: 0x00082F4C File Offset: 0x0008134C
	public InputProfileEntry Clone()
	{
		return new InputProfileEntry
		{
			onReleaseExecution = this.onReleaseExecution,
			onPressExecution = this.onPressExecution,
			buttonsPressed = (this.buttonsPressed.Clone() as ButtonPress[]),
			buttonsHeld = (this.buttonsHeld.Clone() as ButtonPress[]),
			buttonsNotHeld = (this.buttonsNotHeld.Clone() as ButtonPress[]),
			stateRequirements = this.stateRequirements.Clone()
		};
	}

	// Token: 0x040012C2 RID: 4802
	public bool onReleaseExecution;

	// Token: 0x040012C3 RID: 4803
	public bool onPressExecution = true;

	// Token: 0x040012C4 RID: 4804
	public ButtonPress[] buttonsPressed = new ButtonPress[0];

	// Token: 0x040012C5 RID: 4805
	public ButtonPress[] buttonsHeld = new ButtonPress[0];

	// Token: 0x040012C6 RID: 4806
	public ButtonPress[] buttonsNotHeld = new ButtonPress[0];

	// Token: 0x040012C7 RID: 4807
	public InputRequirementsData stateRequirements;
}
