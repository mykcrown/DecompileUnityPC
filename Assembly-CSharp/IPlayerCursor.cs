using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020009BD RID: 2493
public interface IPlayerCursor
{
	// Token: 0x17001075 RID: 4213
	// (get) Token: 0x0600457D RID: 17789
	PlayerCursorActions Actions { get; }

	// Token: 0x17001076 RID: 4214
	// (get) Token: 0x0600457E RID: 17790
	// (set) Token: 0x0600457F RID: 17791
	RaycastResult[] RaycastCache { get; set; }

	// Token: 0x17001077 RID: 4215
	// (get) Token: 0x06004580 RID: 17792
	// (set) Token: 0x06004581 RID: 17793
	GameObject LastSelectedObject { get; set; }

	// Token: 0x17001078 RID: 4216
	// (get) Token: 0x06004582 RID: 17794
	Vector2 Position { get; }

	// Token: 0x17001079 RID: 4217
	// (get) Token: 0x06004583 RID: 17795
	Vector3 PositionDelta { get; }

	// Token: 0x1700107A RID: 4218
	// (get) Token: 0x06004584 RID: 17796
	int PointerId { get; }

	// Token: 0x1700107B RID: 4219
	// (get) Token: 0x06004585 RID: 17797
	bool SubmitPressed { get; }

	// Token: 0x1700107C RID: 4220
	// (get) Token: 0x06004586 RID: 17798
	bool SubmitHeld { get; }

	// Token: 0x1700107D RID: 4221
	// (get) Token: 0x06004587 RID: 17799
	bool SubmitReleased { get; }

	// Token: 0x1700107E RID: 4222
	// (get) Token: 0x06004588 RID: 17800
	bool CancelPressed { get; }

	// Token: 0x1700107F RID: 4223
	// (get) Token: 0x06004589 RID: 17801
	bool AltSubmitPressed { get; }

	// Token: 0x17001080 RID: 4224
	// (get) Token: 0x0600458A RID: 17802
	bool FaceButton3Pressed { get; }

	// Token: 0x17001081 RID: 4225
	// (get) Token: 0x0600458B RID: 17803
	bool AltCancelPressed { get; }

	// Token: 0x17001082 RID: 4226
	// (get) Token: 0x0600458C RID: 17804
	bool StartPressed { get; }

	// Token: 0x17001083 RID: 4227
	// (get) Token: 0x0600458D RID: 17805
	bool Advance1Pressed { get; }

	// Token: 0x17001084 RID: 4228
	// (get) Token: 0x0600458E RID: 17806
	bool Previous1Pressed { get; }

	// Token: 0x17001085 RID: 4229
	// (get) Token: 0x0600458F RID: 17807
	bool Advance2Pressed { get; }

	// Token: 0x17001086 RID: 4230
	// (get) Token: 0x06004590 RID: 17808
	bool Previous2Pressed { get; }

	// Token: 0x17001087 RID: 4231
	// (get) Token: 0x06004591 RID: 17809
	bool RightStickUpPressed { get; }

	// Token: 0x17001088 RID: 4232
	// (get) Token: 0x06004592 RID: 17810
	bool RightStickDownPressed { get; }

	// Token: 0x17001089 RID: 4233
	// (get) Token: 0x06004593 RID: 17811
	bool AdvanceSelectedPressed { get; }

	// Token: 0x1700108A RID: 4234
	// (get) Token: 0x06004594 RID: 17812
	bool PreviousSelectedPressed { get; }

	// Token: 0x1700108B RID: 4235
	// (get) Token: 0x06004595 RID: 17813
	bool AnythingPressed { get; }

	// Token: 0x1700108C RID: 4236
	// (get) Token: 0x06004596 RID: 17814
	// (set) Token: 0x06004597 RID: 17815
	bool IsHidden { get; set; }

	// Token: 0x1700108D RID: 4237
	// (get) Token: 0x06004598 RID: 17816
	// (set) Token: 0x06004599 RID: 17817
	bool IsPaused { get; set; }

	// Token: 0x0600459A RID: 17818
	void ResetPosition(Vector2 vect);

	// Token: 0x0600459B RID: 17819
	void SuppressKeyboard(bool suppress);

	// Token: 0x1700108E RID: 4238
	// (get) Token: 0x0600459C RID: 17820
	global::CursorMode CurrentMode { get; }
}
