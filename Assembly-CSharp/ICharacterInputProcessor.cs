using System;

// Token: 0x02000689 RID: 1673
public interface ICharacterInputProcessor : IInputProcessor
{
	// Token: 0x06002967 RID: 10599
	ProcessInputStateResult ProcessInputState(InputButtonsData inputButtonsData, int frame, bool allowTapJump, bool allowRecoveryJumping, bool autoRun);

	// Token: 0x06002968 RID: 10600
	void Cache();

	// Token: 0x06002969 RID: 10601
	void ChangeStateIfNecessary(InputController inputController, MoveLabel fromMove = MoveLabel.None);

	// Token: 0x17000A1F RID: 2591
	// (get) Token: 0x0600296A RID: 10602
	InputButtonsData CurrentInputData { get; }
}
