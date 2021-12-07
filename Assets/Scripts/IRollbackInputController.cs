// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IRollbackInputController
{
	void ReadPlayerInputValues(ref InputValuesSnapshot state, bool tauntsOnly);

	void LoadInputValues(InputValuesSnapshot state);
}
