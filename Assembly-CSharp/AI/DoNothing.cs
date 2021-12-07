using System;

namespace AI
{
	// Token: 0x02000305 RID: 773
	public class DoNothing : Leaf
	{
		// Token: 0x060010DF RID: 4319 RVA: 0x00062DDE File Offset: 0x000611DE
		public DoNothing(int frameDuration) : base(frameDuration)
		{
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00062DE7 File Offset: 0x000611E7
		public override NodeResult TickFrame()
		{
			if (this.currentFrame == this.frameDuration)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}
	}
}
