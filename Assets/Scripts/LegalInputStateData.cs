// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class LegalInputStateData : ICloneable, IMoveRequirementValidator
{
	public MetaState state;

	public bool running;

	public int runningMinFrames;

	public int runningMaxFrames;

	public bool dashing;

	public int dashingMinFrames;

	public int dashingMaxFrames;

	public bool takingOff;

	public bool idling = true;

	public bool braking;

	public int brakingMinFrames;

	public int brakingMaxFrames;

	public bool dashingBraking;

	public int dashingBrakingMinFrames;

	public int dashingBrakingMaxFrames;

	public bool tumbling = true;

	public int platformDropLeeway;

	public bool platformDropOnly;

	public bool pivoting;

	public int pivotingMinFrames;

	public int pivotingMaxFrames;

	public bool dashingPivoting;

	public int dashingPivotingMinFrames;

	public int dashingPivotingMaxFrames;

	public bool crouchBegin = true;

	public bool shieldEnd;

	public bool forbidAfterRunShield;

	public bool onlyAfterRunShield;

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public bool ValidateRequirements(MoveData move, IPlayerDelegate player, InputButtonsData input)
	{
		if (this.state != player.State.MetaState)
		{
			return false;
		}
		switch (player.State.MetaState)
		{
		case MetaState.Stand:
		{
			ActionState actionState = player.State.ActionState;
			switch (actionState)
			{
			case ActionState.Run:
				return this.checkMinMaxFrames(this.running, player.Model.actionStateFrame, this.runningMinFrames, this.runningMaxFrames);
			case ActionState.Grabbing:
			case ActionState.GrabbedBegin:
			case ActionState.DazedBegin:
			case ActionState.Pivot:
			case ActionState.Recoil:
				IL_84:
				switch (actionState)
				{
				case ActionState.TeeterLoop:
					goto IL_D0;
				case ActionState.AirJump:
				case ActionState.Death:
					IL_A5:
					switch (actionState)
					{
					case ActionState.Idle:
						goto IL_D0;
					case ActionState.WalkFast:
					case ActionState.UNUSED_1:
						IL_BB:
						if (actionState == ActionState.CrouchBegin)
						{
							return this.crouchBegin;
						}
						if (actionState != ActionState.RunPivotBrake)
						{
							goto IL_1C4;
						}
						goto IL_D7;
					case ActionState.TakeOff:
						return this.takingOff;
					}
					goto IL_BB;
				case ActionState.DashBrake:
					return this.checkMinMaxFrames(this.dashingBraking, player.Model.actionStateFrame, this.dashingBrakingMinFrames, this.dashingBrakingMaxFrames);
				case ActionState.DashPivot:
					return this.checkMinMaxFrames(this.dashingPivoting, player.Model.actionStateFrame, this.dashingPivotingMinFrames, this.dashingPivotingMaxFrames);
				case ActionState.UsingMove:
					goto IL_1C4;
				}
				goto IL_A5;
				IL_D0:
				return this.idling;
				IL_1C4:
				return this.idling;
			case ActionState.Dash:
				return this.checkMinMaxFrames(this.dashing, player.Model.actionStateFrame, this.dashingMinFrames, this.dashingMaxFrames);
			case ActionState.Brake:
				goto IL_D7;
			case ActionState.RunPivot:
				return this.checkMinMaxFrames(this.pivoting, player.Model.actionStateFrame, this.pivotingMinFrames, this.pivotingMaxFrames);
			case ActionState.ShieldEnd:
				return this.shieldEnd;
			}
			goto IL_84;
			IL_D7:
			return this.checkMinMaxFrames(this.braking, player.Model.actionStateFrame, this.brakingMinFrames, this.brakingMaxFrames);
		}
		case MetaState.Jump:
			if (this.platformDropOnly)
			{
				int num = player.Config.lagConfig.fallThroughPlatformDurationFrames - player.Physics.PlayerState.platformDropFrames;
				return player.State.IsPlatformDropping && num <= this.platformDropLeeway;
			}
			return !player.State.IsTumbling || this.tumbling;
		case MetaState.LedgeHang:
			return player.Model.ledgeLagFrames <= 0;
		case MetaState.Shielding:
			if (this.forbidAfterRunShield && player.Shield.WasRunning && player.FrameOwner.Frame - player.Shield.ShieldBeginFrame <= player.Config.lagConfig.runShieldDelayFrames)
			{
				return false;
			}
			if (this.onlyAfterRunShield)
			{
				if (!player.Shield.WasRunning)
				{
					return false;
				}
				if (player.FrameOwner.Frame - player.Shield.ShieldBeginFrame > player.Config.lagConfig.runShieldDelayFrames)
				{
					return false;
				}
			}
			return true;
		}
		return true;
	}

	private bool checkMinMaxFrames(bool enabled, int currentFrame, int minFrames, int maxFrames)
	{
		return enabled && (currentFrame >= minFrames || minFrames == 0 || minFrames == 1) && (currentFrame <= maxFrames || maxFrames == 0);
	}
}
