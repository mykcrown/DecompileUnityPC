// Decompile from assembly: Assembly-CSharp.dll

using System;

public class DefaultCharacterComponents : IMoveModelInitializer
{
	public MoveModel InitializeMoveModel(MoveData move)
	{
		MoveModel moveModel = new MoveModel();
		moveModel.data = move;
		moveModel.internalFrame = 0;
		moveModel.gameFrame = 0;
		if (move.chargeOptions.hasOverrideChargeConfig)
		{
			moveModel.ChargeData = move.chargeOptions.overrideChargeConfig;
		}
		return moveModel;
	}
}
