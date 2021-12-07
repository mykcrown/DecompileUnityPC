using System;

namespace XInputDotNetPure
{
	// Token: 0x020001E6 RID: 486
	public struct GamePadState
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x0004CC34 File Offset: 0x0004B034
		internal GamePadState(bool isConnected, GamePadState.RawState rawState)
		{
			this.isConnected = isConnected;
			if (!isConnected)
			{
				rawState.dwPacketNumber = 0U;
				rawState.Gamepad.dwButtons = 0;
				rawState.Gamepad.bLeftTrigger = 0;
				rawState.Gamepad.bRightTrigger = 0;
				rawState.Gamepad.sThumbLX = 0;
				rawState.Gamepad.sThumbLY = 0;
				rawState.Gamepad.sThumbRX = 0;
				rawState.Gamepad.sThumbRY = 0;
			}
			this.packetNumber = rawState.dwPacketNumber;
			this.buttons = new GamePadButtons(((rawState.Gamepad.dwButtons & 16) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 32) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 64) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 128) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 256) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 512) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 4096) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 8192) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 16384) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 32768) == 0) ? ButtonState.Released : ButtonState.Pressed);
			this.dPad = new GamePadDPad(((rawState.Gamepad.dwButtons & 1) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 2) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 4) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 8) == 0) ? ButtonState.Released : ButtonState.Pressed);
			this.thumbSticks = new GamePadThumbSticks(new GamePadThumbSticks.StickValue((float)rawState.Gamepad.sThumbLX / 32767f, (float)rawState.Gamepad.sThumbLY / 32767f), new GamePadThumbSticks.StickValue((float)rawState.Gamepad.sThumbRX / 32767f, (float)rawState.Gamepad.sThumbRY / 32767f));
			this.triggers = new GamePadTriggers((float)rawState.Gamepad.bLeftTrigger / 255f, (float)rawState.Gamepad.bRightTrigger / 255f);
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0004CEF1 File Offset: 0x0004B2F1
		public uint PacketNumber
		{
			get
			{
				return this.packetNumber;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x0004CEF9 File Offset: 0x0004B2F9
		public bool IsConnected
		{
			get
			{
				return this.isConnected;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0004CF01 File Offset: 0x0004B301
		public GamePadButtons Buttons
		{
			get
			{
				return this.buttons;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060008C6 RID: 2246 RVA: 0x0004CF09 File Offset: 0x0004B309
		public GamePadDPad DPad
		{
			get
			{
				return this.dPad;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0004CF11 File Offset: 0x0004B311
		public GamePadTriggers Triggers
		{
			get
			{
				return this.triggers;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060008C8 RID: 2248 RVA: 0x0004CF19 File Offset: 0x0004B319
		public GamePadThumbSticks ThumbSticks
		{
			get
			{
				return this.thumbSticks;
			}
		}

		// Token: 0x04000622 RID: 1570
		private bool isConnected;

		// Token: 0x04000623 RID: 1571
		private uint packetNumber;

		// Token: 0x04000624 RID: 1572
		private GamePadButtons buttons;

		// Token: 0x04000625 RID: 1573
		private GamePadDPad dPad;

		// Token: 0x04000626 RID: 1574
		private GamePadThumbSticks thumbSticks;

		// Token: 0x04000627 RID: 1575
		private GamePadTriggers triggers;

		// Token: 0x020001E7 RID: 487
		internal struct RawState
		{
			// Token: 0x04000628 RID: 1576
			public uint dwPacketNumber;

			// Token: 0x04000629 RID: 1577
			public GamePadState.RawState.GamePad Gamepad;

			// Token: 0x020001E8 RID: 488
			public struct GamePad
			{
				// Token: 0x0400062A RID: 1578
				public ushort dwButtons;

				// Token: 0x0400062B RID: 1579
				public byte bLeftTrigger;

				// Token: 0x0400062C RID: 1580
				public byte bRightTrigger;

				// Token: 0x0400062D RID: 1581
				public short sThumbLX;

				// Token: 0x0400062E RID: 1582
				public short sThumbLY;

				// Token: 0x0400062F RID: 1583
				public short sThumbRX;

				// Token: 0x04000630 RID: 1584
				public short sThumbRY;
			}
		}

		// Token: 0x020001E9 RID: 489
		private enum ButtonsConstants
		{
			// Token: 0x04000632 RID: 1586
			DPadUp = 1,
			// Token: 0x04000633 RID: 1587
			DPadDown,
			// Token: 0x04000634 RID: 1588
			DPadLeft = 4,
			// Token: 0x04000635 RID: 1589
			DPadRight = 8,
			// Token: 0x04000636 RID: 1590
			Start = 16,
			// Token: 0x04000637 RID: 1591
			Back = 32,
			// Token: 0x04000638 RID: 1592
			LeftThumb = 64,
			// Token: 0x04000639 RID: 1593
			RightThumb = 128,
			// Token: 0x0400063A RID: 1594
			LeftShoulder = 256,
			// Token: 0x0400063B RID: 1595
			RightShoulder = 512,
			// Token: 0x0400063C RID: 1596
			A = 4096,
			// Token: 0x0400063D RID: 1597
			B = 8192,
			// Token: 0x0400063E RID: 1598
			X = 16384,
			// Token: 0x0400063F RID: 1599
			Y = 32768
		}
	}
}
