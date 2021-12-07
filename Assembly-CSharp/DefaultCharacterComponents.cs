using System;

// Token: 0x020005C6 RID: 1478
public class DefaultCharacterComponents : IMoveModelInitializer
{
	// Token: 0x060020E7 RID: 8423 RVA: 0x000A4E4C File Offset: 0x000A324C
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
