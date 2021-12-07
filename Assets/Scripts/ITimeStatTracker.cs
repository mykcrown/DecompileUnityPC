// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ITimeStatTracker : ITickable
{
	void RecordValue(float value);

	void Flush();
}
