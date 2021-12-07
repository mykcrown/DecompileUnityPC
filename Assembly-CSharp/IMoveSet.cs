using System;
using System.Collections.Generic;

// Token: 0x0200052E RID: 1326
public interface IMoveSet
{
	// Token: 0x06001CB9 RID: 7353
	MoveData GetMove(string moveName);

	// Token: 0x06001CBA RID: 7354
	MoveData GetMove(MoveLabel label);

	// Token: 0x06001CBB RID: 7355
	bool CanInputBufferMove(MoveData move, List<ButtonPress> moveButtonsPressed);

	// Token: 0x06001CBC RID: 7356
	MoveData[] GetMoves(MoveLabel label);

	// Token: 0x06001CBD RID: 7357
	MoveData GetMove(InputButtonsData inputButtonsData, IPlayerDelegate player, out InterruptData interruptData, ref ButtonPress buttonUsed);

	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x06001CBE RID: 7358
	CharacterActionSet Actions { get; }

	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x06001CBF RID: 7359
	CharacterMoveSetData MoveSetData { get; }

	// Token: 0x06001CC0 RID: 7360
	void LoadMoveInfo(MoveData move);

	// Token: 0x06001CC1 RID: 7361
	void LoadMoveSet(CharacterMoveSetData setData);
}
