using System;

// Token: 0x020005FC RID: 1532
public interface IPlayerDataOwner : IFacing
{
	// Token: 0x17000915 RID: 2325
	// (get) Token: 0x0600254C RID: 9548
	IPlayerState State { get; }

	// Token: 0x17000916 RID: 2326
	// (get) Token: 0x0600254D RID: 9549
	IAnimationPlayer AnimationPlayer { get; }

	// Token: 0x17000917 RID: 2327
	// (get) Token: 0x0600254E RID: 9550
	AudioManager Audio { get; }

	// Token: 0x17000918 RID: 2328
	// (get) Token: 0x0600254F RID: 9551
	IPlayerOrientation Orientation { get; }

	// Token: 0x17000919 RID: 2329
	// (get) Token: 0x06002550 RID: 9552
	IRespawnController RespawnController { get; }

	// Token: 0x1700091A RID: 2330
	// (get) Token: 0x06002551 RID: 9553
	IBoneController Bones { get; }

	// Token: 0x1700091B RID: 2331
	// (get) Token: 0x06002552 RID: 9554
	ICharacterRenderer Renderer { get; }

	// Token: 0x1700091C RID: 2332
	// (get) Token: 0x06002553 RID: 9555
	MoveController ActiveMove { get; }

	// Token: 0x1700091D RID: 2333
	// (get) Token: 0x06002554 RID: 9556
	CharacterActionData ActionData { get; }

	// Token: 0x1700091E RID: 2334
	// (get) Token: 0x06002555 RID: 9557
	PlayerPhysicsController Physics { get; }

	// Token: 0x1700091F RID: 2335
	// (get) Token: 0x06002556 RID: 9558
	IShield Shield { get; }

	// Token: 0x17000920 RID: 2336
	// (get) Token: 0x06002557 RID: 9559
	IGrabController GrabController { get; }

	// Token: 0x17000921 RID: 2337
	// (get) Token: 0x06002558 RID: 9560
	CharacterData CharacterData { get; }

	// Token: 0x17000922 RID: 2338
	// (get) Token: 0x06002559 RID: 9561
	CharacterMenusData CharacterMenusData { get; }

	// Token: 0x17000923 RID: 2339
	// (get) Token: 0x0600255A RID: 9562
	PlayerModel Model { get; }

	// Token: 0x17000924 RID: 2340
	// (get) Token: 0x0600255B RID: 9563
	IGameVFX GameVFX { get; }

	// Token: 0x17000925 RID: 2341
	// (get) Token: 0x0600255C RID: 9564
	IGameInput GameInput { get; }

	// Token: 0x17000926 RID: 2342
	// (get) Token: 0x0600255D RID: 9565
	ILedgeGrabController LedgeGrabController { get; }

	// Token: 0x17000927 RID: 2343
	// (get) Token: 0x0600255E RID: 9566
	bool AreInputsLocked { get; }
}
