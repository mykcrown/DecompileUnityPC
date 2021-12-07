using System;

namespace XInputDotNetPure
{
	// Token: 0x020001E5 RID: 485
	public struct GamePadTriggers
	{
		// Token: 0x060008BF RID: 2239 RVA: 0x0004CC12 File Offset: 0x0004B012
		internal GamePadTriggers(float left, float right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0004CC22 File Offset: 0x0004B022
		public float Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0004CC2A File Offset: 0x0004B02A
		public float Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x04000620 RID: 1568
		private float left;

		// Token: 0x04000621 RID: 1569
		private float right;
	}
}
