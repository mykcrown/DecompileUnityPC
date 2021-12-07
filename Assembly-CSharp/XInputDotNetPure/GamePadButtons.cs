using System;

namespace XInputDotNetPure
{
	// Token: 0x020001E1 RID: 481
	public struct GamePadButtons
	{
		// Token: 0x060008A8 RID: 2216 RVA: 0x0004CAD8 File Offset: 0x0004AED8
		internal GamePadButtons(ButtonState start, ButtonState back, ButtonState leftStick, ButtonState rightStick, ButtonState leftShoulder, ButtonState rightShoulder, ButtonState a, ButtonState b, ButtonState x, ButtonState y)
		{
			this.start = start;
			this.back = back;
			this.leftStick = leftStick;
			this.rightStick = rightStick;
			this.leftShoulder = leftShoulder;
			this.rightShoulder = rightShoulder;
			this.a = a;
			this.b = b;
			this.x = x;
			this.y = y;
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060008A9 RID: 2217 RVA: 0x0004CB32 File Offset: 0x0004AF32
		public ButtonState Start
		{
			get
			{
				return this.start;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x0004CB3A File Offset: 0x0004AF3A
		public ButtonState Back
		{
			get
			{
				return this.back;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0004CB42 File Offset: 0x0004AF42
		public ButtonState LeftStick
		{
			get
			{
				return this.leftStick;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x0004CB4A File Offset: 0x0004AF4A
		public ButtonState RightStick
		{
			get
			{
				return this.rightStick;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x0004CB52 File Offset: 0x0004AF52
		public ButtonState LeftShoulder
		{
			get
			{
				return this.leftShoulder;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x0004CB5A File Offset: 0x0004AF5A
		public ButtonState RightShoulder
		{
			get
			{
				return this.rightShoulder;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x0004CB62 File Offset: 0x0004AF62
		public ButtonState A
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0004CB6A File Offset: 0x0004AF6A
		public ButtonState B
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0004CB72 File Offset: 0x0004AF72
		public ButtonState X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x0004CB7A File Offset: 0x0004AF7A
		public ButtonState Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x0400060F RID: 1551
		private ButtonState start;

		// Token: 0x04000610 RID: 1552
		private ButtonState back;

		// Token: 0x04000611 RID: 1553
		private ButtonState leftStick;

		// Token: 0x04000612 RID: 1554
		private ButtonState rightStick;

		// Token: 0x04000613 RID: 1555
		private ButtonState leftShoulder;

		// Token: 0x04000614 RID: 1556
		private ButtonState rightShoulder;

		// Token: 0x04000615 RID: 1557
		private ButtonState a;

		// Token: 0x04000616 RID: 1558
		private ButtonState b;

		// Token: 0x04000617 RID: 1559
		private ButtonState x;

		// Token: 0x04000618 RID: 1560
		private ButtonState y;
	}
}
