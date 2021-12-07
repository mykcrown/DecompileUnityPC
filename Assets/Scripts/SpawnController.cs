// Decompile from assembly: Assembly-CSharp.dll

using System;

public class SpawnController : ISpawnController
{
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
