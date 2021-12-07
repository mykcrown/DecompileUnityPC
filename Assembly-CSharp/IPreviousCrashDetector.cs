using System;

// Token: 0x0200089F RID: 2207
public interface IPreviousCrashDetector
{
	// Token: 0x06003754 RID: 14164
	void ReportPreviousCrash();

	// Token: 0x06003755 RID: 14165
	void UpdateStatus(PreviousCrashDetector.GameStatus gameStatus);
}
