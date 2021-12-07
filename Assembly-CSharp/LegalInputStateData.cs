using System;

// Token: 0x02000482 RID: 1154
[Serializable]
public class LegalInputStateData : ICloneable, IMoveRequirementValidator
{
	// Token: 0x060018E7 RID: 6375 RVA: 0x00083027 File Offset: 0x00081427
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x060018E8 RID: 6376 RVA: 0x00083030 File Offset: 0x00081430
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
			default:
				switch (actionState)
				{
				case ActionState.TeeterLoop:
					break;
				default:
					switch (actionState)
					{
					case ActionState.Idle:
						break;
					default:
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
					break;
				case ActionState.DashBrake:
					return this.checkMinMaxFrames(this.dashingBraking, player.Model.actionStateFrame, this.dashingBrakingMinFrames, this.dashingBrakingMaxFrames);
				case ActionState.DashPivot:
					return this.checkMinMaxFrames(this.dashingPivoting, player.Model.actionStateFrame, this.dashingPivotingMinFrames, this.dashingPivotingMaxFrames);
				case ActionState.UsingMove:
					goto IL_1C4;
				}
				return this.idling;
				IL_1C4:
				return this.idling;
			case ActionState.Dash:
				return this.checkMinMaxFrames(this.dashing, player.Model.actionStateFrame, this.dashingMinFrames, this.dashingMaxFrames);
			case ActionState.Brake:
				break;
			case ActionState.RunPivot:
				return this.checkMinMaxFrames(this.pivoting, player.Model.actionStateFrame, this.pivotingMinFrames, this.pivotingMaxFrames);
			case ActionState.ShieldEnd:
				return this.shieldEnd;
			}
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

	// Token: 0x060018E9 RID: 6377 RVA: 0x00083317 File Offset: 0x00081717
	private bool checkMinMaxFrames(bool enabled, int currentFrame, int minFrames, int maxFrames)
	{
		return enabled && (currentFrame >= minFrames || minFrames == 0 || minFrames == 1) && (currentFrame <= maxFrames || maxFrames == 0);
	}

	// Token: 0x040012C9 RID: 4809
	public MetaState state;

	// Token: 0x040012CA RID: 4810
	public bool running;

	// Token: 0x040012CB RID: 4811
	public int runningMinFrames;

	// Token: 0x040012CC RID: 4812
	public int runningMaxFrames;

	// Token: 0x040012CD RID: 4813
	public bool dashing;

	// Token: 0x040012CE RID: 4814
	public int dashingMinFrames;

	// Token: 0x040012CF RID: 4815
	public int dashingMaxFrames;

	// Token: 0x040012D0 RID: 4816
	public bool takingOff;

	// Token: 0x040012D1 RID: 4817
	public bool idling = true;

	// Token: 0x040012D2 RID: 4818
	public bool braking;

	// Token: 0x040012D3 RID: 4819
	public int brakingMinFrames;

	// Token: 0x040012D4 RID: 4820
	public int brakingMaxFrames;

	// Token: 0x040012D5 RID: 4821
	public bool dashingBraking;

	// Token: 0x040012D6 RID: 4822
	public int dashingBrakingMinFrames;

	// Token: 0x040012D7 RID: 4823
	public int dashingBrakingMaxFrames;

	// Token: 0x040012D8 RID: 4824
	public bool tumbling = true;

	// Token: 0x040012D9 RID: 4825
	public int platformDropLeeway;

	// Token: 0x040012DA RID: 4826
	public bool platformDropOnly;

	// Token: 0x040012DB RID: 4827
	public bool pivoting;

	// Token: 0x040012DC RID: 4828
	public int pivotingMinFrames;

	// Token: 0x040012DD RID: 4829
	public int pivotingMaxFrames;

	// Token: 0x040012DE RID: 4830
	public bool dashingPivoting;

	// Token: 0x040012DF RID: 4831
	public int dashingPivotingMinFrames;

	// Token: 0x040012E0 RID: 4832
	public int dashingPivotingMaxFrames;

	// Token: 0x040012E1 RID: 4833
	public bool crouchBegin = true;

	// Token: 0x040012E2 RID: 4834
	public bool shieldEnd;

	// Token: 0x040012E3 RID: 4835
	public bool forbidAfterRunShield;

	// Token: 0x040012E4 RID: 4836
	public bool onlyAfterRunShield;
}
