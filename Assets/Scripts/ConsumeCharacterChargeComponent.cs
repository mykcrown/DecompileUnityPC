// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ConsumeCharacterChargeComponent : MoveComponent, IMoveStartComponent
{
	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		player.ExecuteCharacterComponents<IChargeLevelComponent>(new PlayerController.ComponentExecution<IChargeLevelComponent>(this.onStartFn));
	}

	private bool onStartFn(IChargeLevelComponent charge)
	{
		charge.OnChargeMoveUsed();
		return false;
	}
}
