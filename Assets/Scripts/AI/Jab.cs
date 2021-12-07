// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public class Jab : Leaf
	{
		[Serializable]
		public class Data
		{
			public int jabCount = 3;
		}

		public Jab.Data data = new Jab.Data();

		public Jab() : base(0)
		{
		}

		public override NodeResult TickFrame()
		{
			if (this.inProgress)
			{
				if (this.moveCompleteCount >= this.data.jabCount)
				{
					return base.resultSuccess;
				}
				return base.resultRunning;
			}
			else
			{
				if (!base.isAbleToAct || !base.player.State.IsGrounded)
				{
					return base.resultFailure;
				}
				return base.resultRunning;
			}
		}

		protected override void run()
		{
			if (base.isInputBreak && this.moveStartedCount < this.data.jabCount)
			{
				this.inProgress = true;
				this.context.AddInput(InputType.Button, ButtonPress.Attack);
			}
		}
	}
}
