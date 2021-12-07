using System;
using InControl;

// Token: 0x020006AA RID: 1706
public class PlayerCursorActions : PlayerActionSet
{
	// Token: 0x06002A5F RID: 10847 RVA: 0x000DF980 File Offset: 0x000DDD80
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

	// Token: 0x04001E63 RID: 7779
	public PlayerAction Submit;

	// Token: 0x04001E64 RID: 7780
	public PlayerAction Start;

	// Token: 0x04001E65 RID: 7781
	public PlayerAction Cancel;

	// Token: 0x04001E66 RID: 7782
	public PlayerAction AltModifier;

	// Token: 0x04001E67 RID: 7783
	public PlayerAction AltSubmit;

	// Token: 0x04001E68 RID: 7784
	public PlayerAction AltCancel;

	// Token: 0x04001E69 RID: 7785
	public PlayerAction FaceButton3;

	// Token: 0x04001E6A RID: 7786
	public PlayerAction Left;

	// Token: 0x04001E6B RID: 7787
	public PlayerAction Right;

	// Token: 0x04001E6C RID: 7788
	public PlayerAction Up;

	// Token: 0x04001E6D RID: 7789
	public PlayerAction Down;

	// Token: 0x04001E6E RID: 7790
	public PlayerTwoAxisAction Move;

	// Token: 0x04001E6F RID: 7791
	public PlayerTwoAxisAction RightStick;

	// Token: 0x04001E70 RID: 7792
	public PlayerAction RightStickUp;

	// Token: 0x04001E71 RID: 7793
	public PlayerAction RightStickDown;

	// Token: 0x04001E72 RID: 7794
	public PlayerAction RightStickLeft;

	// Token: 0x04001E73 RID: 7795
	public PlayerAction RightStickRight;

	// Token: 0x04001E74 RID: 7796
	public PlayerAction Advance1;

	// Token: 0x04001E75 RID: 7797
	public PlayerAction Previous1;

	// Token: 0x04001E76 RID: 7798
	public PlayerAction Advance2;

	// Token: 0x04001E77 RID: 7799
	public PlayerAction Previous2;
}
