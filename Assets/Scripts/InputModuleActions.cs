// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;

public class InputModuleActions : PlayerActionSet
{
	public PlayerAction Submit;

	public PlayerAction Cancel;

	public PlayerAction Left;

	public PlayerAction Right;

	public PlayerAction Up;

	public PlayerAction Down;

	public PlayerAction YButtonAction;

	public PlayerAction XButtonAction;

	public PlayerAction RightTrigger;

	public PlayerAction LeftTrigger;

	public PlayerAction LeftBumper;

	public PlayerAction ZButtonAction;

	public PlayerAction LeftStickLeft;

	public PlayerAction LeftStickRight;

	public PlayerAction LeftStickUp;

	public PlayerAction LeftStickDown;

	public PlayerTwoAxisAction LeftStick;

	public PlayerAction RightStickLeft;

	public PlayerAction RightStickRight;

	public PlayerAction RightStickUp;

	public PlayerAction RightStickDown;

	public PlayerTwoAxisAction RightStick;

	public PlayerAction DPadRight;

	public PlayerAction DPadLeft;

	public PlayerAction DPadUp;

	public PlayerAction DPadDown;

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
}
