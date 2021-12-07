using System;

// Token: 0x0200068A RID: 1674
public interface IInputProcessor
{
	// Token: 0x0600296B RID: 10603
	void ProcessInput(int currentFrame, InputController inputController, PlayerReference player, bool retainBuffer);
}
