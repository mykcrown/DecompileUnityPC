// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IChargeLevelComponent
{
	Fixed ChargeLevel
	{
		get;
	}

	int ChargeFrames
	{
		get;
	}

	void OnChargeMoveUsed();
}
