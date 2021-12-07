using System;

namespace AI
{
	// Token: 0x0200031A RID: 794
	public class Shield : Leaf
	{
		// Token: 0x06001111 RID: 4369 RVA: 0x00063E50 File Offset: 0x00062250
		public Shield(int frameDuration) : base(frameDuration)
		{
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00063E59 File Offset: 0x00062259
		public override NodeResult TickFrame()
		{
			if (!this.canShield())
			{
				return base.resultFailure;
			}
			if (this.currentFrame >= this.frameDuration)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00063E8B File Offset: 0x0006228B
		private bool canShield()
		{
			return base.player.State.IsGrounded && base.isAbleToAct;
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00063EAC File Offset: 0x000622AC
		protected override void run()
		{
			if (base.player.State.IsShieldingState)
			{
				this.context.AddInput(InputType.Button, ButtonPress.Shield1);
			}
			else if (base.isInputBreak)
			{
				this.context.AddInput(InputType.Button, ButtonPress.Shield1);
			}
		}
	}
}
