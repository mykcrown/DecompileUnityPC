// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public class Shield : Leaf
	{
		public Shield(int frameDuration) : base(frameDuration)
		{
		}

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

		private bool canShield()
		{
			return base.player.State.IsGrounded && base.isAbleToAct;
		}

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
