using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	// Token: 0x02000328 RID: 808
	public class Leaf : INode
	{
		// Token: 0x0600114C RID: 4428 RVA: 0x0006250C File Offset: 0x0006090C
		public Leaf(int frameDuration)
		{
			this.frameDuration = frameDuration;
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x00062529 File Offset: 0x00060929
		protected PlayerController player
		{
			get
			{
				return this.playerRef.Controller;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x00062536 File Offset: 0x00060936
		protected IAICalculator calculator
		{
			get
			{
				return this.context.calculator;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x00062543 File Offset: 0x00060943
		// (set) Token: 0x06001150 RID: 4432 RVA: 0x0006254B File Offset: 0x0006094B
		public int shuffleWeight { get; set; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x00062554 File Offset: 0x00060954
		// (set) Token: 0x06001152 RID: 4434 RVA: 0x00062557 File Offset: 0x00060957
		public List<INode> children
		{
			get
			{
				return null;
			}
			set
			{
				throw new UnityException("Leaf nodes not have children!");
			}
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00062563 File Offset: 0x00060963
		public virtual void Init(BehaviorTree context)
		{
			this.context = context;
			this.playerRef = context.playerRef;
			this.baseReactionSpeed = this.calculator.reactionFrames;
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00062589 File Offset: 0x00060989
		public virtual NodeResult TickFrame()
		{
			return NodeResult.Failure;
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x0006258C File Offset: 0x0006098C
		protected bool isDoingMove
		{
			get
			{
				return this.player.State.IsMoveActive || this.startedMove;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x000625AC File Offset: 0x000609AC
		protected bool isInputBreak
		{
			get
			{
				bool flag = this.allowedInputAt == -1 || this.currentFrame >= this.allowedInputAt;
				if (flag)
				{
					this.allowedInputAt = this.currentFrame + this.calculator.RandomizeButtonSpam(this.calculator.buttonSpamFrames);
				}
				return flag;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x00062604 File Offset: 0x00060A04
		protected bool isNeutral
		{
			get
			{
				return this.isAbleToAct && !this.player.State.IsMoveActive;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x00062628 File Offset: 0x00060A28
		protected bool isAbleToAct
		{
			get
			{
				return !this.player.State.IsRespawning && !this.player.State.IsDownState && !this.player.State.IsDownedLooping && !this.player.State.IsLedgeGrabbing && !this.player.State.IsLedgeHangingState && !this.player.State.IsLedgeRecovering && !this.player.State.IsLedgeHanging && !this.player.State.IsLanding && !this.player.State.IsHitStunned && !this.player.State.IsHitLagPaused && !this.player.State.IsGrabbedState && !this.player.State.IsBusyWithMove && !this.player.State.IsDead && this.player.State.IsInControl;
			}
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00062759 File Offset: 0x00060B59
		private void baseBeginRun()
		{
			this.baseReset();
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00062761 File Offset: 0x00060B61
		protected virtual void beginRun()
		{
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00062763 File Offset: 0x00060B63
		private void baseReset()
		{
			this.currentFrame = 0;
			this.allowedInputAt = -1;
			this.delayedReactionActivate = -1;
			this.moveCompleteCount = 0;
			this.moveStartedCount = 0;
			this.startedMove = false;
			this.inProgress = false;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00062796 File Offset: 0x00060B96
		protected virtual void reset()
		{
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00062798 File Offset: 0x00060B98
		protected virtual void checkForEnd()
		{
			int frame = this.context.gameController.currentGame.Frame;
			if (frame - this.lastRunFrame > 1)
			{
				this.baseReset();
				this.reset();
			}
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x000627D8 File Offset: 0x00060BD8
		private void baseRun()
		{
			int frame = this.context.gameController.currentGame.Frame;
			if (frame - this.lastRunFrame > 1)
			{
				this.baseBeginRun();
				this.beginRun();
			}
			this.currentFrame++;
			this.lastRunFrame = frame;
			if (!this.startedMove && this.player.State.IsMoveActive && this.player.ActiveMove.Model.gameFrame > this.lastMoveFrame)
			{
				this.startedMove = true;
				this.moveStartedCount++;
			}
			if (this.startedMove && (!this.player.State.IsMoveActive || this.player.ActiveMove.Model.gameFrame < this.lastMoveFrame))
			{
				this.startedMove = false;
				this.moveCompleteCount++;
			}
			if (this.player.State.IsMoveActive)
			{
				this.lastMoveFrame = this.player.ActiveMove.Model.gameFrame;
			}
			else
			{
				this.lastMoveFrame = 0;
			}
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00062910 File Offset: 0x00060D10
		protected virtual void run()
		{
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x00062912 File Offset: 0x00060D12
		protected bool reactionSpeedCheck
		{
			get
			{
				if (this.delayedReactionActivate == -1)
				{
					this.delayedReactionActivate = this.currentFrame + this.calculator.RandomizeReactionSpeed(this.baseReactionSpeed);
				}
				return this.currentFrame >= this.delayedReactionActivate;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001161 RID: 4449 RVA: 0x0006294F File Offset: 0x00060D4F
		protected NodeResult resultFailure
		{
			get
			{
				this.baseReset();
				this.reset();
				return NodeResult.Failure;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x0006295E File Offset: 0x00060D5E
		protected NodeResult resultSuccess
		{
			get
			{
				this.baseReset();
				this.reset();
				return NodeResult.Success;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06001163 RID: 4451 RVA: 0x0006296D File Offset: 0x00060D6D
		protected NodeResult resultRunning
		{
			get
			{
				this.baseRun();
				this.run();
				return NodeResult.Running;
			}
		}

		// Token: 0x04000AFC RID: 2812
		protected int currentFrame;

		// Token: 0x04000AFD RID: 2813
		protected int frameDuration;

		// Token: 0x04000AFE RID: 2814
		protected int allowedInputAt = -1;

		// Token: 0x04000AFF RID: 2815
		protected int baseReactionSpeed;

		// Token: 0x04000B00 RID: 2816
		protected int moveCompleteCount;

		// Token: 0x04000B01 RID: 2817
		protected int moveStartedCount;

		// Token: 0x04000B02 RID: 2818
		protected bool inProgress;

		// Token: 0x04000B03 RID: 2819
		private bool startedMove;

		// Token: 0x04000B04 RID: 2820
		private int lastMoveFrame;

		// Token: 0x04000B05 RID: 2821
		private int lastRunFrame;

		// Token: 0x04000B06 RID: 2822
		private int delayedReactionActivate = -1;

		// Token: 0x04000B07 RID: 2823
		private int randomizedReactionSpeed;

		// Token: 0x04000B08 RID: 2824
		protected BehaviorTree context;

		// Token: 0x04000B09 RID: 2825
		protected PlayerReference playerRef;
	}
}
