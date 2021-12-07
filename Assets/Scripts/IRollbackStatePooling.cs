// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IRollbackStatePooling
{
	void Init();

	T Clone<T>(T source) where T : RollbackStateTyped<T>;
}
