using System;
using FixedPoint;

namespace AI
{
	// Token: 0x02000317 RID: 791
	public class Recover : Leaf
	{
		// Token: 0x06001107 RID: 4359 RVA: 0x00063A48 File Offset: 0x00061E48
		public Recover() : base(0)
		{
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00063AC6 File Offset: 0x00061EC6
		public override NodeResult TickFrame()
		{
			if (!this.shouldRecover())
			{
				return base.resultFailure;
			}
			return base.resultRunning;
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00063AE0 File Offset: 0x00061EE0
		private bool shouldRecover()
		{
			return !base.player.State.IsRespawning && !base.player.State.IsGrounded && !base.player.State.IsLedgeHangingState && !base.player.State.IsLedgeGrabbing && !base.player.State.IsLedgeHanging && !base.player.State.IsLedgeRecovering && base.calculator.TestOffstage(base.player);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00063B7F File Offset: 0x00061F7F
		protected override void beginRun()
		{
			this.decidedRecovery = false;
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00063B88 File Offset: 0x00061F88
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
				int num = (!(base.player.Physics.Center.x < 0)) ? -1 : 1;
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

		// Token: 0x04000ACD RID: 2765
		private Fixed recoverHigh = (Fixed)2.0;

		// Token: 0x04000ACE RID: 2766
		private Fixed recoverMid = -(Fixed)0.5;

		// Token: 0x04000ACF RID: 2767
		private Fixed recoveryLow = -(Fixed)2.5;

		// Token: 0x04000AD0 RID: 2768
		private Fixed recoveryYPoint = -(Fixed)0.5;

		// Token: 0x04000AD1 RID: 2769
		private bool decidedRecovery;

		// Token: 0x04000AD2 RID: 2770
		public Recover.Data data = new Recover.Data();

		// Token: 0x02000318 RID: 792
		[Serializable]
		public class Data
		{
			// Token: 0x04000AD3 RID: 2771
			public Fixed rngWeight_Low = 10;

			// Token: 0x04000AD4 RID: 2772
			public Fixed rngWeight_High = 10;

			// Token: 0x04000AD5 RID: 2773
			public Fixed rngWeight_Mid = 10;
		}
	}
}
