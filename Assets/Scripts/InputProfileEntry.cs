// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class InputProfileEntry
{
	public bool onReleaseExecution;

	public bool onPressExecution = true;

	public ButtonPress[] buttonsPressed = new ButtonPress[0];

	public ButtonPress[] buttonsHeld = new ButtonPress[0];

	public ButtonPress[] buttonsNotHeld = new ButtonPress[0];

	public InputRequirementsData stateRequirements;

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
}
