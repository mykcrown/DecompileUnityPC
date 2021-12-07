// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class Leaf : INode
	{
		protected int currentFrame;

		protected int frameDuration;

		protected int allowedInputAt = -1;

		protected int baseReactionSpeed;

		protected int moveCompleteCount;

		protected int moveStartedCount;

		protected bool inProgress;

		private bool startedMove;

		private int lastMoveFrame;

		private int lastRunFrame;

		private int delayedReactionActivate = -1;

		private int randomizedReactionSpeed;

		protected BehaviorTree context;

		protected PlayerReference playerRef;

		protected PlayerController player
		{
			get
			{
				return this.playerRef.Controller;
			}
		}

		protected IAICalculator calculator
		{
			get
			{
				return this.context.calculator;
			}
		}

		public int shuffleWeight
		{
			get;
			set;
		}

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

		protected bool isDoingMove
		{
			get
			{
				return this.player.State.IsMoveActive || this.startedMove;
			}
		}

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

		protected bool isNeutral
		{
			get
			{
				return this.isAbleToAct && !this.player.State.IsMoveActive;
			}
		}

		protected bool isAbleToAct
		{
			get
			{
				return !this.player.State.IsRespawning && !this.player.State.IsDownState && !this.player.State.IsDownedLooping && !this.player.State.IsLedgeGrabbing && !this.player.State.IsLedgeHangingState && !this.player.State.IsLedgeRecovering && !this.player.State.IsLedgeHanging && !this.player.State.IsLanding && !this.player.State.IsHitStunned && !this.player.State.IsHitLagPaused && !this.player.State.IsGrabbedState && !this.player.State.IsBusyWithMove && !this.player.State.IsDead && this.player.State.IsInControl;
			}
		}

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

		protected NodeResult resultFailure
		{
			get
			{
				this.baseReset();
				this.reset();
				return NodeResult.Failure;
			}
		}

		protected NodeResult resultSuccess
		{
			get
			{
				this.baseReset();
				this.reset();
				return NodeResult.Success;
			}
		}

		protected NodeResult resultRunning
		{
			get
			{
				this.baseRun();
				this.run();
				return NodeResult.Running;
			}
		}

		public Leaf(int frameDuration)
		{
			this.frameDuration = frameDuration;
		}

		public virtual void Init(BehaviorTree context)
		{
			this.context = context;
			this.playerRef = context.playerRef;
			this.baseReactionSpeed = this.calculator.reactionFrames;
		}

		public virtual NodeResult TickFrame()
		{
			return NodeResult.Failure;
		}

		private void baseBeginRun()
		{
			this.baseReset();
		}

		protected virtual void beginRun()
		{
		}

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

		protected virtual void reset()
		{
		}

		protected virtual void checkForEnd()
		{
			int frame = this.context.gameController.currentGame.Frame;
			if (frame - this.lastRunFrame > 1)
			{
				this.baseReset();
				this.reset();
			}
		}

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

		protected virtual void run()
		{
		}
	}
}
