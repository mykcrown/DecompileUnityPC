// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IMoveSet
{
	CharacterActionSet Actions
	{
		get;
	}

	CharacterMoveSetData MoveSetData
	{
		get;
	}

	MoveData GetMove(string moveName);

	MoveData GetMove(MoveLabel label);

	bool CanInputBufferMove(MoveData move, List<ButtonPress> moveButtonsPressed);

	MoveData[] GetMoves(MoveLabel label);

	MoveData GetMove(InputButtonsData inputButtonsData, IPlayerDelegate player, out InterruptData interruptData, ref ButtonPress buttonUsed);

	void LoadMoveInfo(MoveData move);

	void LoadMoveSet(CharacterMoveSetData setData);
}
