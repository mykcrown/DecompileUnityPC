using System;
using InControl;

namespace MultiplayerWithBindingsExample
{
	// Token: 0x02000049 RID: 73
	public class PlayerActions : PlayerActionSet
	{
		// Token: 0x0600026A RID: 618 RVA: 0x00010F1C File Offset: 0x0000F31C
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

		// Token: 0x0600026B RID: 619 RVA: 0x00010FDC File Offset: 0x0000F3DC
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

		// Token: 0x0600026C RID: 620 RVA: 0x000110A0 File Offset: 0x0000F4A0
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

		// Token: 0x040001C8 RID: 456
		public PlayerAction Green;

		// Token: 0x040001C9 RID: 457
		public PlayerAction Red;

		// Token: 0x040001CA RID: 458
		public PlayerAction Blue;

		// Token: 0x040001CB RID: 459
		public PlayerAction Yellow;

		// Token: 0x040001CC RID: 460
		public PlayerAction Left;

		// Token: 0x040001CD RID: 461
		public PlayerAction Right;

		// Token: 0x040001CE RID: 462
		public PlayerAction Up;

		// Token: 0x040001CF RID: 463
		public PlayerAction Down;

		// Token: 0x040001D0 RID: 464
		public PlayerTwoAxisAction Rotate;
	}
}
