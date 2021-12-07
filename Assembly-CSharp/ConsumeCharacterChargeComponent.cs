using System;

// Token: 0x020004D0 RID: 1232
public class ConsumeCharacterChargeComponent : MoveComponent, IMoveStartComponent
{
	// Token: 0x06001B39 RID: 6969 RVA: 0x0008ADBD File Offset: 0x000891BD
	public void OnStart(IPlayerDelegate player, InputButtonsData input)
	{
		player.ExecuteCharacterComponents<IChargeLevelComponent>(new PlayerController.ComponentExecution<IChargeLevelComponent>(this.onStartFn));
	}

	// Token: 0x06001B3A RID: 6970 RVA: 0x0008ADD2 File Offset: 0x000891D2
	private bool onStartFn(IChargeLevelComponent charge)
	{
		charge.OnChargeMoveUsed();
		return false;
	}
}
