// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class ChargeLevelRequirementComponent : MoveComponent, IMoveRequirementValidator, IMoveCompareComponent, IMoveStartComponent
{
	public bool useChargeFrames;

	public int minimumChargeLevel;

	public int minimumChargeFrames;

	public int MinimumCharge
	{
		get
		{
			return (!this.useChargeFrames) ? this.minimumChargeLevel : this.minimumChargeFrames;
		}
	}

	public override bool ValidateRequirements(MoveData move, IPlayerDelegate player, InputButtonsData input)
	{
		if (this.useChargeFrames)
		{
			return player.ExecuteCharacterComponents<IChargeLevelComponent>(new PlayerController.ComponentExecution<IChargeLevelComponent>(this.checkChargeFramesRequirementFn));
		}
		return player.ExecuteCharacterComponents<IChargeLevelComponent>(new PlayerController.ComponentExecution<IChargeLevelComponent>(this.checkChargeLevelRequirementFn));
	}

	private bool checkChargeLevelRequirementFn(IChargeLevelComponent charge)
	{
		return charge.ChargeLevel >= this.minimumChargeLevel;
	}

	private bool checkChargeFramesRequirementFn(IChargeLevelComponent charge)
	{
		return charge.ChargeFrames >= this.minimumChargeFrames;
	}

	private bool onStartFn(IChargeLevelComponent charge)
	{
		charge.OnChargeMoveUsed();
		return false;
	}

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
						result = ((this.minimumChargeFrames <= chargeLevelRequirementComponent.minimumChargeFrames) ? 1 : (-1));
					}
				}
				else if (chargeLevelRequirementComponent.minimumChargeLevel != this.minimumChargeLevel)
				{
					result = ((this.minimumChargeLevel <= chargeLevelRequirementComponent.minimumChargeLevel) ? 1 : (-1));
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

	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		player.ExecuteCharacterComponents<IChargeLevelComponent>(new PlayerController.ComponentExecution<IChargeLevelComponent>(this.onStartFn));
	}

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
			return (this.minimumChargeFrames > num) ? 1 : (-1);
		}
		else
		{
			Fixed one = (characterComponent == null) ? 0 : characterComponent.ChargeLevel;
			if (this.minimumChargeLevel <= one == other.minimumChargeLevel <= one)
			{
				return other.minimumChargeLevel.CompareTo(this.minimumChargeLevel);
			}
			return (!(this.minimumChargeLevel <= one)) ? 1 : (-1);
		}
	}
}
