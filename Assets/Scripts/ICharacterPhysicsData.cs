// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface ICharacterPhysicsData
{
	Fixed SlowWalkMaxSpeed
	{
		get;
	}

	Fixed MediumWalkMaxSpeed
	{
		get;
	}

	Fixed FastWalkMaxSpeed
	{
		get;
	}

	Fixed RunMaxSpeed
	{
		get;
	}

	Fixed GroundToAirMaxSpeed
	{
		get;
	}

	Fixed WalkAcceleration
	{
		get;
	}

	Fixed RunPivotAcceleration
	{
		get;
	}

	Fixed Friction
	{
		get;
	}

	Fixed HighSpeedFriction
	{
		get;
	}

	Fixed DashStartSpeed
	{
		get;
	}

	Fixed DashMaxSpeed
	{
		get;
	}

	Fixed DashAcceleration
	{
		get;
	}

	Fixed AirMaxSpeed
	{
		get;
	}

	Fixed AirAcceleration
	{
		get;
	}

	Fixed AirFriction
	{
		get;
	}

	Fixed Gravity
	{
		get;
	}

	Fixed MaxFallSpeed
	{
		get;
	}

	Fixed HelplessAirSpeedMultiplier
	{
		get;
	}

	Fixed HelplessAirAccelerationMultiplier
	{
		get;
	}

	Fixed JumpSpeed
	{
		get;
	}

	Fixed SecondaryJumpSpeed
	{
		get;
	}

	Fixed ShortJumpSpeed
	{
		get;
	}

	int JumpCount
	{
		get;
	}

	Fixed Weight
	{
		get;
	}

	Fixed FastFallSpeed
	{
		get;
	}

	Fixed ShieldBreakSpeed
	{
		get;
	}

	bool IgnorePlatforms
	{
		get;
	}
}
