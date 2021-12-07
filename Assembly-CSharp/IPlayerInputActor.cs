using System;

// Token: 0x0200068C RID: 1676
public interface IPlayerInputActor : IPlayerDataOwner, PlayerStateActor.IPlayerActorDelegate, IFacing
{
	// Token: 0x17000A26 RID: 2598
	// (get) Token: 0x06002975 RID: 10613
	// (set) Token: 0x06002976 RID: 10614
	int ButtonsPressedThisFrame { get; set; }

	// Token: 0x17000A27 RID: 2599
	// (get) Token: 0x06002977 RID: 10615
	// (set) Token: 0x06002978 RID: 10616
	int LastBackTapFrame { get; set; }

	// Token: 0x17000A28 RID: 2600
	// (get) Token: 0x06002979 RID: 10617
	// (set) Token: 0x0600297A RID: 10618
	int LastTechFrame { get; set; }

	// Token: 0x17000A29 RID: 2601
	// (get) Token: 0x0600297B RID: 10619
	// (set) Token: 0x0600297C RID: 10620
	int FallThroughPlatformHeldFrames { get; set; }

	// Token: 0x17000A2A RID: 2602
	// (get) Token: 0x0600297D RID: 10621
	bool TriggerHeldInputAsTaps { get; }

	// Token: 0x17000A2B RID: 2603
	// (get) Token: 0x0600297E RID: 10622
	bool ReadAnyBufferedInput { get; }

	// Token: 0x17000A2C RID: 2604
	// (get) Token: 0x0600297F RID: 10623
	bool AllowFastFall { get; }

	// Token: 0x06002980 RID: 10624
	void OnDropInput();
}
