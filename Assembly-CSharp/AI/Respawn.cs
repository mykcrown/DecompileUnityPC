using System;
using FixedPoint;

namespace AI
{
	// Token: 0x02000319 RID: 793
	public class Respawn : Leaf
	{
		// Token: 0x0600110D RID: 4365 RVA: 0x00063D9B File Offset: 0x0006219B
		public Respawn() : base(0)
		{
			this.baseReactionSpeed = 36;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00063DAC File Offset: 0x000621AC
		public override NodeResult TickFrame()
		{
			if (!this.shouldRespawn())
			{
				return base.resultFailure;
			}
			return base.resultRunning;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00063DC6 File Offset: 0x000621C6
		private bool shouldRespawn()
		{
			return base.player.State.IsRespawning && !base.player.State.IsBusyRespawning;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00063DF4 File Offset: 0x000621F4
		protected override void run()
		{
			if (base.reactionSpeedCheck && base.isInputBreak)
			{
				int num = (!(this.context.GenerateRandomNumber() < (Fixed)0.5)) ? 1 : -1;
				this.context.AddInput(InputType.HorizontalAxis, (float)num);
			}
		}
	}
}
