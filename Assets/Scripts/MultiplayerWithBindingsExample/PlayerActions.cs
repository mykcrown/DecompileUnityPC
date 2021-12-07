// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;

namespace MultiplayerWithBindingsExample
{
	public class PlayerActions : PlayerActionSet
	{
		public PlayerAction Green;

		public PlayerAction Red;

		public PlayerAction Blue;

		public PlayerAction Yellow;

		public PlayerAction Left;

		public PlayerAction Right;

		public PlayerAction Up;

		public PlayerAction Down;

		public PlayerTwoAxisAction Rotate;

		public PlayerActions()
		{
			this.Green = base.CreatePlayerAction("Green");
			this.Red = base.CreatePlayerAction("Red");
			this.Blue = base.CreatePlayerAction("Blue");
			this.Yellow = base.CreatePlayerAction("Yellow");
			this.Left = base.CreatePlayerAction("Left");
			this.Right = base.CreatePlayerAction("Right");
			this.Up = base.CreatePlayerAction("Up");
			this.Down = base.CreatePlayerAction("Down");
			this.Rotate = base.CreateTwoAxisPlayerAction(this.Left, this.Right, this.Down, this.Up);
		}

		public static PlayerActions CreateWithKeyboardBindings()
		{
			PlayerActions playerActions = new PlayerActions();
			playerActions.Green.AddDefaultBinding(new Key[]
			{
				Key.A
			});
			playerActions.Red.AddDefaultBinding(new Key[]
			{
				Key.S
			});
			playerActions.Blue.AddDefaultBinding(new Key[]
			{
				Key.D
			});
			playerActions.Yellow.AddDefaultBinding(new Key[]
			{
				Key.F
			});
			playerActions.Up.AddDefaultBinding(new Key[]
			{
				Key.UpArrow
			});
			playerActions.Down.AddDefaultBinding(new Key[]
			{
				Key.DownArrow
			});
			playerActions.Left.AddDefaultBinding(new Key[]
			{
				Key.LeftArrow
			});
			playerActions.Right.AddDefaultBinding(new Key[]
			{
				Key.RightArrow
			});
			return playerActions;
		}

		public static PlayerActions CreateWithJoystickBindings()
		{
			PlayerActions playerActions = new PlayerActions();
			playerActions.Green.AddDefaultBinding(InputControlType.Action1);
			playerActions.Red.AddDefaultBinding(InputControlType.Action2);
			playerActions.Blue.AddDefaultBinding(InputControlType.Action3);
			playerActions.Yellow.AddDefaultBinding(InputControlType.Action4);
			playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
			playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
			playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
			playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
			playerActions.Up.AddDefaultBinding(InputControlType.DPadUp);
			playerActions.Down.AddDefaultBinding(InputControlType.DPadDown);
			playerActions.Left.AddDefaultBinding(InputControlType.DPadLeft);
			playerActions.Right.AddDefaultBinding(InputControlType.DPadRight);
			return playerActions;
		}
	}
}
