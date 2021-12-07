// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICharacterInputProcessor : IInputProcessor
{
	InputButtonsData CurrentInputData
	{
		get;
	}

	ProcessInputStateResult ProcessInputState(InputButtonsData inputButtonsData, int frame, bool allowTapJump, bool allowRecoveryJumping, bool autoRun);

	void Cache();

	void ChangeStateIfNecessary(InputController inputController, MoveLabel fromMove = MoveLabel.None);
}
