// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

namespace AI
{
	public class Respawn : Leaf
	{
		public Respawn() : base(0)
		{
			this.baseReactionSpeed = 36;
		}

		public override NodeResult TickFrame()
		{
			if (!this.shouldRespawn())
			{
				return base.resultFailure;
			}
			return base.resultRunning;
		}

		private bool shouldRespawn()
		{
			return base.player.State.IsRespawning && !base.player.State.IsBusyRespawning;
		}

		protected override void run()
		{
			if (base.reactionSpeedCheck && base.isInputBreak)
			{
				int num = (!(this.context.GenerateRandomNumber() < (Fixed)0.5)) ? 1 : (-1);
				this.context.AddInput(InputType.HorizontalAxis, (float)num);
			}
		}
	}
}
