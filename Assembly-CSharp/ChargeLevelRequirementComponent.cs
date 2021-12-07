using System;
using FixedPoint;

// Token: 0x020004CD RID: 1229
public class ChargeLevelRequirementComponent : MoveComponent, IMoveRequirementValidator, IMoveCompareComponent, IMoveStartComponent
{
	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x06001B1F RID: 6943 RVA: 0x0008A595 File Offset: 0x00088995
	public int MinimumCharge
	{
		get
		{
			return (!this.useChargeFrames) ? this.minimumChargeLevel : this.minimumChargeFrames;
		}
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x0008A5B3 File Offset: 0x000889B3
	public override bool ValidateRequirements(MoveData move, IPlayerDelegate player, InputButtonsData input)
	{
		if (this.useChargeFrames)
		{
			return player.ExecuteCharacterComponents<IChargeLevelComponent>(new PlayerController.ComponentExecution<IChargeLevelComponent>(this.checkChargeFramesRequirementFn));
		}
		return player.ExecuteCharacterComponents<IChargeLevelComponent>(new PlayerController.ComponentExecution<IChargeLevelComponent>(this.checkChargeLevelRequirementFn));
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x0008A5E5 File Offset: 0x000889E5
	private bool checkChargeLevelRequirementFn(IChargeLevelComponent charge)
	{
		return charge.ChargeLevel >= this.minimumChargeLevel;
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x0008A600 File Offset: 0x00088A00
	private bool checkChargeFramesRequirementFn(IChargeLevelComponent charge)
	{
		return charge.ChargeFrames >= this.minimumChargeFrames;
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x0008A616 File Offset: 0x00088A16
	private bool onStartFn(IChargeLevelComponent charge)
	{
		charge.OnChargeMoveUsed();
		return false;
	}

	// Token: 0x06001B24 RID: 6948 RVA: 0x0008A620 File Offset: 0x00088A20
	public int CompareMove(MoveData otherMove)
	{
		int result = 0;
		bool flag = false;
		for (int i = 0; i < otherMove.components.Length; i++)
		{
			MoveComponent moveComponent = otherMove.components[i];
			ChargeLevelRequirementComponent chargeLevelRequirementComponent = moveComponent as ChargeLevelRequirementComponent;
			if (chargeLevelRequirementComponent != null)
			{
				flag = true;
				if (this.useChargeFrames)
				{
					if (chargeLevelRequirementComponent.minimumChargeFrames != this.minimumChargeFrames)
					{
						result = ((this.minimumChargeFrames <= chargeLevelRequirementComponent.minimumChargeFrames) ? 1 : -1);
					}
				}
				else if (chargeLevelRequirementComponent.minimumChargeLevel != this.minimumChargeLevel)
				{
					result = ((this.minimumChargeLevel <= chargeLevelRequirementComponent.minimumChargeLevel) ? 1 : -1);
				}
				break;
			}
		}
		if (!flag)
		{
			result = -1;
		}
		return result;
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x0008A6E0 File Offset: 0x00088AE0
	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		player.ExecuteCharacterComponents<IChargeLevelComponent>(new PlayerController.ComponentExecution<IChargeLevelComponent>(this.onStartFn));
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x0008A6F8 File Offset: 0x00088AF8
	public int CompareChargeInterrupt(IPlayerDelegate player, ChargeLevelRequirementComponent other)
	{
		IChargeLevelComponent characterComponent = player.GetCharacterComponent<IChargeLevelComponent>();
		if (this.useChargeFrames)
		{
			int num = (characterComponent == null) ? 0 : characterComponent.ChargeFrames;
			if (this.minimumChargeFrames <= num == other.minimumChargeFrames <= num)
			{
				return other.minimumChargeFrames.CompareTo(this.minimumChargeFrames);
			}
			return (this.minimumChargeFrames > num) ? 1 : -1;
		}
		else
		{
			Fixed one = (characterComponent == null) ? 0 : characterComponent.ChargeLevel;
			if (this.minimumChargeLevel <= one == other.minimumChargeLevel <= one)
			{
				return other.minimumChargeLevel.CompareTo(this.minimumChargeLevel);
			}
			return (!(this.minimumChargeLevel <= one)) ? 1 : -1;
		}
	}

	// Token: 0x04001470 RID: 5232
	public bool useChargeFrames;

	// Token: 0x04001471 RID: 5233
	public int minimumChargeLevel;

	// Token: 0x04001472 RID: 5234
	public int minimumChargeFrames;
}
