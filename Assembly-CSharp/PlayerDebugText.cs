using System;
using System.Text;
using UnityEngine;

// Token: 0x020005E8 RID: 1512
public class PlayerDebugText : IDebugStringComponent
{
	// Token: 0x060023B3 RID: 9139 RVA: 0x000B3E2D File Offset: 0x000B222D
	public PlayerDebugText(IPlayerDelegate player, PlayerPhysicsController physics, ConfigData config)
	{
		this.player = player;
		this.config = config;
		this.physics = physics;
	}

	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x060023B4 RID: 9140 RVA: 0x000B3E58 File Offset: 0x000B2258
	string IDebugStringComponent.DebugString
	{
		get
		{
			this.stringBuilder.Length = 0;
			float num = (!this.config.DebugConfig.displayUnitsSeconds) ? (1f / (float)this.config.fps) : 1f;
			string text = (!this.config.DebugConfig.displayUnitsSeconds) ? "m/f" : "m/s";
			int currentAnimationGameFramelength = this.player.AnimationPlayer.CurrentAnimationGameFramelength;
			this.stringBuilder.AppendFormat("-----Player {0} -----\n", PlayerUtil.GetIntFromPlayerNum(this.player.PlayerNum, false));
			this.stringBuilder.AppendFormat("Position: {0}\n", StringUtil.FormatVector(this.player.Transform.position, 6, 1f, "m"));
			this.stringBuilder.AppendFormat("Facing: {0}\n", this.player.Facing);
			this.stringBuilder.AppendFormat("State: {0}\n", this.player.State.MetaState);
			this.stringBuilder.Append(this.player.PlayerInput.GenerateDebugString());
			int num2 = (!this.player.State.IsMoveActive) ? (this.player.Model.actionStateFrame % currentAnimationGameFramelength + 1) : this.player.ActiveMove.Model.gameFrame;
			this.stringBuilder.AppendFormat("Animation State (base 1): {0} ({1}/{2}))\n", this.player.State.ActionState, num2, currentAnimationGameFramelength);
			AnimationData currentAnimationData = this.player.AnimationPlayer.CurrentAnimationData;
			this.stringBuilder.AppendFormat("Animation: {0} ({1}/{2})\n", currentAnimationData.clipName, currentAnimationData.timeElapsed, currentAnimationData.length);
			if (this.player.Model.stunFrames > 0)
			{
				this.stringBuilder.AppendFormat("Stun Frames {0}: {1}\n", (!this.player.State.IsHitLagPaused) ? string.Empty : "(HitLag Paused)", this.player.Model.stunFrames);
			}
			if (this.player.Model.jumpStunFrames > 0)
			{
				this.stringBuilder.AppendFormat("Jump Stun Frames{0}: {1}\n", (!this.player.State.IsHitLagPaused) ? string.Empty : "(HitLag Paused)", this.player.Model.jumpStunFrames);
			}
			if (this.player.Model.grabData.grabbedOpponent != PlayerNum.None)
			{
				this.stringBuilder.AppendFormat("Grab Duration Frames: {0}\n", this.player.Model.grabData.grabDurationFrames);
			}
			if (this.player.Model.chainGrabPreventionFrames > 0)
			{
				this.stringBuilder.AppendFormat("Chain Grab Prevention Frames: {0}\n", this.player.Model.chainGrabPreventionFrames);
			}
			Vector3 vector = (Vector3)this.physics.MovementVelocity;
			Vector3 vector2 = (Vector3)this.physics.KnockbackVelocity;
			this.stringBuilder.AppendFormat("Velocity (total): {0}\n", StringUtil.FormatVector((Vector3)this.physics.Velocity, 3, num, text));
			if (vector.sqrMagnitude > 0.001f)
			{
				this.stringBuilder.AppendFormat("Velocity (char): {0}\n", StringUtil.FormatVector(vector, 3, num, text));
			}
			if (vector2.sqrMagnitude > 0.001f)
			{
				this.stringBuilder.AppendFormat("Velocity (KB): {0}\n", StringUtil.FormatVector(vector2, 3, num, text));
			}
			this.stringBuilder.AppendFormat("Speed: {0} {1}\n", ((float)this.physics.Velocity.magnitude * num).ToString("N3"), text);
			if (this.player.ActiveMove.IsActive)
			{
				this.stringBuilder.AppendFormat("Move: {0} ({1}/{2}) ({3}/{4})\n", new object[]
				{
					this.player.ActiveMove.Data.name,
					this.player.ActiveMove.Model.internalFrame,
					this.player.ActiveMove.Data.totalInternalFrames,
					this.player.ActiveMove.Model.gameFrame,
					this.player.ActiveMove.Model.totalGameFrames
				});
				if (this.player.ActiveMove.Data.articles.Length > 0)
				{
					this.stringBuilder.AppendFormat("Article Firing Angle: {0}\n", this.player.ActiveMove.Model.articleFireAngle);
				}
			}
			if (this.physics.IsFastFalling)
			{
				this.stringBuilder.Append("FastFalling\n");
			}
			if (this.player.PlayerInput is AIInput)
			{
				this.stringBuilder.Append("AI\n");
			}
			if (this.player.Invincibility.IsInvincible)
			{
				this.stringBuilder.AppendFormat("Invincibility Frames: {0}\n", this.player.Invincibility.InvincibilityFramesRemaining);
			}
			string arg = "null";
			StageSurface stageSurface = this.player.Physics.State.groundedMovingStageObject as StageSurface;
			if (stageSurface != null)
			{
				arg = stageSurface.gameObject.name;
			}
			this.stringBuilder.AppendFormat("Standing Surface: {0}\nSurface Delta: {1}\n", arg, this.player.Physics.State.movingPlatformDeltaPosition);
			this.player.ExecuteCharacterComponents<IDebugStringComponent>(delegate(IDebugStringComponent debugComponent)
			{
				this.stringBuilder.AppendFormat("{0}\n", debugComponent.DebugString);
				return false;
			});
			this.stringBuilder.Append(this.player.StaleMoves.GenerateDebugString());
			return this.stringBuilder.ToString();
		}
	}

	// Token: 0x04001AE2 RID: 6882
	private ConfigData config;

	// Token: 0x04001AE3 RID: 6883
	private IPlayerDelegate player;

	// Token: 0x04001AE4 RID: 6884
	private PlayerPhysicsController physics;

	// Token: 0x04001AE5 RID: 6885
	private StringBuilder stringBuilder = new StringBuilder();
}
