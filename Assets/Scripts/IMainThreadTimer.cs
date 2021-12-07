// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IMainThreadTimer
{
	void SetOrReplaceTimeout(int time, Action callback);

	void SetTimeout(int time, Action callback);

	void CancelTimeout(Action callback);

	void UnblockThread(Action callback);

	void NextFrame(Action callback);

	void EndOfFrame(Action callback);
}
