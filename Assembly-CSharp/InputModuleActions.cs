using System;
using InControl;

// Token: 0x0200069E RID: 1694
public class InputModuleActions : PlayerActionSet
{
	// Token: 0x06002A08 RID: 10760 RVA: 0x000DE038 File Offset: 0x000DC438
	public InputModuleActions()
	{
		this.Submit = base.CreatePlayerAction("Submit");
		this.Cancel = base.CreatePlayerAction("Cancel");
		this.YButtonAction = base.CreatePlayerAction("Y Button");
		this.XButtonAction = base.CreatePlayerAction("X Button");
		this.Left = base.CreatePlayerAction("Input Left");
		this.Right = base.CreatePlayerAction("Input Right");
		this.Up = base.CreatePlayerAction("Input Up");
		this.Down = base.CreatePlayerAction("Input Down");
		this.LeftStickLeft = base.CreatePlayerAction("Left Stick Left");
		this.LeftStickRight = base.CreatePlayerAction("Left Stick Right");
		this.LeftStickUp = base.CreatePlayerAction("Left Stick Up");
		this.LeftStickDown = base.CreatePlayerAction("Left Stick Down");
		this.LeftStick = base.CreateTwoAxisPlayerAction(this.LeftStickLeft, this.LeftStickRight, this.LeftStickDown, this.LeftStickUp);
		this.RightTrigger = base.CreatePlayerAction("Right Trigger");
		this.LeftTrigger = base.CreatePlayerAction("Left Trigger");
		this.LeftBumper = base.CreatePlayerAction("Left Bumper");
		this.ZButtonAction = base.CreatePlayerAction("Z Button");
		this.RightStickLeft = base.CreatePlayerAction("Right Stick Left");
		this.RightStickRight = base.CreatePlayerAction("Right Stick Right");
		this.RightStickUp = base.CreatePlayerAction("Right Stick Up");
		this.RightStickDown = base.CreatePlayerAction("Right Stick Down");
		this.RightStick = base.CreateTwoAxisPlayerAction(this.RightStickLeft, this.RightStickRight, this.RightStickDown, this.RightStickUp);
		this.DPadLeft = base.CreatePlayerAction("DPad Left");
		this.DPadRight = base.CreatePlayerAction("DPad Right");
		this.DPadUp = base.CreatePlayerAction("DPad Up");
		this.DPadDown = base.CreatePlayerAction("DPad Down");
	}

	// Token: 0x04001E18 RID: 7704
	public PlayerAction Submit;

	// Token: 0x04001E19 RID: 7705
	public PlayerAction Cancel;

	// Token: 0x04001E1A RID: 7706
	public PlayerAction Left;

	// Token: 0x04001E1B RID: 7707
	public PlayerAction Right;

	// Token: 0x04001E1C RID: 7708
	public PlayerAction Up;

	// Token: 0x04001E1D RID: 7709
	public PlayerAction Down;

	// Token: 0x04001E1E RID: 7710
	public PlayerAction YButtonAction;

	// Token: 0x04001E1F RID: 7711
	public PlayerAction XButtonAction;

	// Token: 0x04001E20 RID: 7712
	public PlayerAction RightTrigger;

	// Token: 0x04001E21 RID: 7713
	public PlayerAction LeftTrigger;

	// Token: 0x04001E22 RID: 7714
	public PlayerAction LeftBumper;

	// Token: 0x04001E23 RID: 7715
	public PlayerAction ZButtonAction;

	// Token: 0x04001E24 RID: 7716
	public PlayerAction LeftStickLeft;

	// Token: 0x04001E25 RID: 7717
	public PlayerAction LeftStickRight;

	// Token: 0x04001E26 RID: 7718
	public PlayerAction LeftStickUp;

	// Token: 0x04001E27 RID: 7719
	public PlayerAction LeftStickDown;

	// Token: 0x04001E28 RID: 7720
	public PlayerTwoAxisAction LeftStick;

	// Token: 0x04001E29 RID: 7721
	public PlayerAction RightStickLeft;

	// Token: 0x04001E2A RID: 7722
	public PlayerAction RightStickRight;

	// Token: 0x04001E2B RID: 7723
	public PlayerAction RightStickUp;

	// Token: 0x04001E2C RID: 7724
	public PlayerAction RightStickDown;

	// Token: 0x04001E2D RID: 7725
	public PlayerTwoAxisAction RightStick;

	// Token: 0x04001E2E RID: 7726
	public PlayerAction DPadRight;

	// Token: 0x04001E2F RID: 7727
	public PlayerAction DPadLeft;

	// Token: 0x04001E30 RID: 7728
	public PlayerAction DPadUp;

	// Token: 0x04001E31 RID: 7729
	public PlayerAction DPadDown;
}
