using System;

namespace XInputDotNetPure
{
	// Token: 0x020001E2 RID: 482
	public struct GamePadDPad
	{
		// Token: 0x060008B3 RID: 2227 RVA: 0x0004CB82 File Offset: 0x0004AF82
		internal GamePadDPad(ButtonState up, ButtonState down, ButtonState left, ButtonState right)
		{
			this.up = up;
			this.down = down;
			this.left = left;
			this.right = right;
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x0004CBA1 File Offset: 0x0004AFA1
		public ButtonState Up
		{
			get
			{
				return this.up;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x0004CBA9 File Offset: 0x0004AFA9
		public ButtonState Down
		{
			get
			{
				return this.down;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x0004CBB1 File Offset: 0x0004AFB1
		public ButtonState Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x0004CBB9 File Offset: 0x0004AFB9
		public ButtonState Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x04000619 RID: 1561
		private ButtonState up;

		// Token: 0x0400061A RID: 1562
		private ButtonState down;

		// Token: 0x0400061B RID: 1563
		private ButtonState left;

		// Token: 0x0400061C RID: 1564
		private ButtonState right;
	}
}
