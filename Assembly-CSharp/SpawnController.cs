using System;

// Token: 0x02000621 RID: 1569
public class SpawnController : ISpawnController
{
	// Token: 0x060026C7 RID: 9927 RVA: 0x000BDB40 File Offset: 0x000BBF40
	public void AddPlayerToScene(PlayerController playerController, int startingDamage)
	{
		playerController.Model.isDead = false;
		playerController.Model.isRespawning = false;
		playerController.ClearState(startingDamage);
		playerController.State.MetaState = MetaState.Jump;
		playerController.State.SubState = SubStates.Resting;
		playerController.Physics.StopMovement(true, true, VelocityType.Total);
		playerController.OnSpawned();
	}
}
