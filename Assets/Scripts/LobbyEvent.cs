// Decompile from assembly: Assembly-CSharp.dll

using System;

public enum LobbyEvent
{
	None,
	CreateFailedNameTaken,
	CreateFailedInLobby,
	CreateFailedInQueued,
	CreateFailedInMatch,
	CreateFailedBadParams,
	CreateSystemError,
	UnrequestedLobbyCreated,
	DestroyFailedNotInMatch,
	DestroyFailedMissingMatch,
	DestroyFailedNotOwner,
	DestroySystemError,
	SetParamsFailedNotInMatch,
	SetParamsFailedNotOwner,
	SetParamsFailedNoStage,
	SetParamsFailedInvalidStages,
	SetParamsSystemError,
	JoinFailedInvalidCreds,
	JoinFailedInQueue,
	JoinFailedInMatch,
	JoinFailedTooLate,
	JoinFailedLobbyFull,
	JoinFailedMatchRunning,
	JoinSystemError,
	UnrequestedJoinedLobby,
	LeaveFailedInQueue,
	LeaveFailedNotInLobby,
	LeaveFailedTooLate,
	LeaveSystemError,
	StartSystemError,
	DestroyedOwnerLeft,
	StartingLobbyMatch
}
