// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

namespace AI
{
	public class Recover : Leaf
	{
		[Serializable]
		public class Data
		{
			public Fixed rngWeight_Low = 10;

			public Fixed rngWeight_High = 10;

			public Fixed rngWeight_Mid = 10;
		}

		private Fixed recoverHigh = (Fixed)2.0;

		private Fixed recoverMid = -(Fixed)0.5;

		private Fixed recoveryLow = -(Fixed)2.5;

		private Fixed recoveryYPoint = -(Fixed)0.5;

		private bool decidedRecovery;

		public Recover.Data data = new Recover.Data();

		public Recover() : base(0)
		{
		}

		public override NodeResult TickFrame()
		{
			if (!this.shouldRecover())
			{
				return base.resultFailure;
			}
			return base.resultRunning;
		}

		private bool shouldRecover()
		{
			return !base.player.State.IsRespawning && !base.player.State.IsGrounded && !base.player.State.IsLedgeHangingState && !base.player.State.IsLedgeGrabbing && !base.player.State.IsLedgeHanging && !base.player.State.IsLedgeRecovering && base.calculator.TestOffstage(base.player);
		}

		protected override void beginRun()
		{
			this.decidedRecovery = false;
		}

		protected override void run()
		{
			if (!this.decidedRecovery)
			{
				this.decidedRecovery = true;
				Fixed one = this.context.GenerateRandomNumber();
				Fixed other = this.data.rngWeight_Low + this.data.rngWeight_High + this.data.rngWeight_Mid;
				Fixed @fixed = this.data.rngWeight_Low / other;
				Fixed other2 = @fixed + this.data.rngWeight_High / other;
				if (one < @fixed)
				{
					this.recoveryYPoint = this.recoveryLow;
				}
				else if (one < other2)
				{
					this.recoveryYPoint = this.recoverHigh;
				}
				else
				{
					this.recoveryYPoint = this.recoverMid;
				}
			}
			if (base.reactionSpeedCheck)
			{
				int num = (!(base.player.Physics.Center.x < 0)) ? (-1) : 1;
				this.context.AddInput(InputType.HorizontalAxis, (float)num * 1f);
				this.context.AddInput(InputType.VerticalAxis, 1f);
				if (base.player.Physics.Center.y < this.recoveryYPoint)
				{
					if (!base.player.Physics.UsedAirJump)
					{
						if (base.isInputBreak)
						{
							this.context.AddInput(Macro.Jump);
						}
					}
					else if (!base.player.State.IsHelpless && base.player.Physics.Velocity.y < 0 && !base.player.ActiveMove.IsActive && base.isInputBreak)
					{
						this.context.AddInput(Macro.UpSpecial);
					}
				}
			}
		}
	}
}
