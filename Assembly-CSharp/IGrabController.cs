using System;

// Token: 0x020005D0 RID: 1488
public interface IGrabController
{
	// Token: 0x0600215E RID: 8542
	void ReleaseGrabbedOpponent(bool escaped);

	// Token: 0x0600215F RID: 8543
	void ReleaseFromGrab(bool escaped);

	// Token: 0x06002160 RID: 8544
	void OnGrabOpponent(PlayerController opponent, MoveData moveData, HitData hitData);

	// Token: 0x06002161 RID: 8545
	void OnGrabbedBy(PlayerController opponent, GrabType grabType);

	// Token: 0x06002162 RID: 8546
	void TickStandardGrabbed();

	// Token: 0x06002163 RID: 8547
	bool IsThrowMove(MoveData throwMove);

	// Token: 0x06002164 RID: 8548
	void OnBeginThrow(MoveData throwMove);

	// Token: 0x17000764 RID: 1892
	// (get) Token: 0x06002165 RID: 8549
	bool IsReadyToRelease { get; }

	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x06002166 RID: 8550
	bool IsGrabbing { get; }

	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x06002167 RID: 8551
	PlayerNum GrabbedOpponent { get; }

	// Token: 0x17000767 RID: 1895
	// (get) Token: 0x06002168 RID: 8552
	GrabData GrabData { get; }

	// Token: 0x17000768 RID: 1896
	// (get) Token: 0x06002169 RID: 8553
	bool IsGrabbed { get; }

	// Token: 0x17000769 RID: 1897
	// (get) Token: 0x0600216A RID: 8554
	PlayerNum GrabbingOpponent { get; }
}
