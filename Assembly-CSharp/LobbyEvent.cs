using System;

// Token: 0x020007D5 RID: 2005
public enum LobbyEvent
{
	// Token: 0x040022D1 RID: 8913
	None,
	// Token: 0x040022D2 RID: 8914
	CreateFailedNameTaken,
	// Token: 0x040022D3 RID: 8915
	CreateFailedInLobby,
	// Token: 0x040022D4 RID: 8916
	CreateFailedInQueued,
	// Token: 0x040022D5 RID: 8917
	CreateFailedInMatch,
	// Token: 0x040022D6 RID: 8918
	CreateFailedBadParams,
	// Token: 0x040022D7 RID: 8919
	CreateSystemError,
	// Token: 0x040022D8 RID: 8920
	UnrequestedLobbyCreated,
	// Token: 0x040022D9 RID: 8921
	DestroyFailedNotInMatch,
	// Token: 0x040022DA RID: 8922
	DestroyFailedMissingMatch,
	// Token: 0x040022DB RID: 8923
	DestroyFailedNotOwner,
	// Token: 0x040022DC RID: 8924
	DestroySystemError,
	// Token: 0x040022DD RID: 8925
	SetParamsFailedNotInMatch,
	// Token: 0x040022DE RID: 8926
	SetParamsFailedNotOwner,
	// Token: 0x040022DF RID: 8927
	SetParamsFailedNoStage,
	// Token: 0x040022E0 RID: 8928
	SetParamsFailedInvalidStages,
	// Token: 0x040022E1 RID: 8929
	SetParamsSystemError,
	// Token: 0x040022E2 RID: 8930
	JoinFailedInvalidCreds,
	// Token: 0x040022E3 RID: 8931
	JoinFailedInQueue,
	// Token: 0x040022E4 RID: 8932
	JoinFailedInMatch,
	// Token: 0x040022E5 RID: 8933
	JoinFailedTooLate,
	// Token: 0x040022E6 RID: 8934
	JoinFailedLobbyFull,
	// Token: 0x040022E7 RID: 8935
	JoinFailedMatchRunning,
	// Token: 0x040022E8 RID: 8936
	JoinSystemError,
	// Token: 0x040022E9 RID: 8937
	UnrequestedJoinedLobby,
	// Token: 0x040022EA RID: 8938
	LeaveFailedInQueue,
	// Token: 0x040022EB RID: 8939
	LeaveFailedNotInLobby,
	// Token: 0x040022EC RID: 8940
	LeaveFailedTooLate,
	// Token: 0x040022ED RID: 8941
	LeaveSystemError,
	// Token: 0x040022EE RID: 8942
	StartSystemError,
	// Token: 0x040022EF RID: 8943
	DestroyedOwnerLeft,
	// Token: 0x040022F0 RID: 8944
	StartingLobbyMatch
}
