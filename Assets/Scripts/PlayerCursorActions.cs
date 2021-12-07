// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;

public class PlayerCursorActions : PlayerActionSet
{
	public PlayerAction Submit;

	public PlayerAction Start;

	public PlayerAction Cancel;

	public PlayerAction AltModifier;

	public PlayerAction AltSubmit;

	public PlayerAction AltCancel;

	public PlayerAction FaceButton3;

	public PlayerAction Left;

	public PlayerAction Right;

	public PlayerAction Up;

	public PlayerAction Down;

	public PlayerTwoAxisAction Move;

	public PlayerTwoAxisAction RightStick;

	public PlayerAction RightStickUp;

	public PlayerAction RightStickDown;

	public PlayerAction RightStickLeft;

	public PlayerAction RightStickRight;

	public PlayerAction Advance1;

	public PlayerAction Previous1;

	public PlayerAction Advance2;

	public PlayerAction Previous2;

	public PlayerCursorActions()
	{
		this.Submit = base.CreatePlayerAction("Cursor Submit");
		this.Cancel = base.CreatePlayerAction("Cursor Cancel");
		this.AltModifier = base.CreatePlayerAction("Cursor AltModifier");
		this.AltSubmit = base.CreatePlayerAction("Cursor AltSubmit");
		this.FaceButton3 = base.CreatePlayerAction("Face Button 3");
		this.AltCancel = base.CreatePlayerAction("Cursor AltCancel");
		this.Start = base.CreatePlayerAction("Cursor Start");
		this.Left = base.CreatePlayerAction("Cursor Left");
		this.Right = base.CreatePlayerAction("Cursor Right");
		this.Up = base.CreatePlayerAction("Cursor Up");
		this.Down = base.CreatePlayerAction("Cursor Down");
		this.Move = base.CreateTwoAxisPlayerAction(this.Left, this.Right, this.Down, this.Up);
		this.Advance1 = base.CreatePlayerAction("Cursor Advance 1");
		this.Previous1 = base.CreatePlayerAction("Cursor Previous 1");
		this.Advance2 = base.CreatePlayerAction("Cursor Advance 2");
		this.Previous2 = base.CreatePlayerAction("Cursor Previous 2");
		this.RightStickLeft = base.CreatePlayerAction("Right Stick Left");
		this.RightStickRight = base.CreatePlayerAction("Right Stick Right");
		this.RightStickUp = base.CreatePlayerAction("Right Stick Up");
		this.RightStickDown = base.CreatePlayerAction("Right Stick Down");
		this.RightStick = base.CreateTwoAxisPlayerAction(this.RightStickLeft, this.RightStickRight, this.RightStickDown, this.RightStickUp);
	}
}
