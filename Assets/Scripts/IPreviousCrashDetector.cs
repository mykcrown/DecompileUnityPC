// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IPreviousCrashDetector
{
	void ReportPreviousCrash();

	void UpdateStatus(PreviousCrashDetector.GameStatus gameStatus);
}
