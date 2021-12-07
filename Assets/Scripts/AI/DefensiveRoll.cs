// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

namespace AI
{
	public class DefensiveRoll : Leaf
	{
		public DefensiveRoll() : base(30)
		{
		}

		public override NodeResult TickFrame()
		{
			if (!this.canRoll())
			{
				return base.resultFailure;
			}
			if (this.currentFrame >= this.frameDuration)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}

		private bool canRoll()
		{
			return base.player.State.IsGrounded && base.isAbleToAct;
		}

		protected override void run()
		{
			if (base.isInputBreak)
			{
				this.context.AddInput(InputType.Button, ButtonPress.Shield1);
				float value = (float)((!(this.context.GenerateRandomNumber() < (Fixed)0.5)) ? 1 : (-1));
				this.context.AddInput(InputType.HorizontalAxis, value);
			}
		}
	}
}
