// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IInputBlocker
{
	InputBlock Request();

	void Release(InputBlock block);

	bool IsLocked();
}
