// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public class Grab : Leaf
	{
		public Grab() : base(0)
		{
		}

		public override NodeResult TickFrame()
		{
			if (this.inProgress)
			{
				if (this.moveCompleteCount >= 1)
				{
					return base.resultSuccess;
				}
				return base.resultRunning;
			}
			else
			{
				if (!base.isAbleToAct)
				{
					return base.resultFailure;
				}
				return base.resultRunning;
			}
		}

		protected override void run()
		{
			if (base.isInputBreak && !base.isDoingMove && this.moveStartedCount == 0)
			{
				this.inProgress = true;
				this.context.AddInput(InputType.Button, ButtonPress.Grab);
			}
		}
	}
}
