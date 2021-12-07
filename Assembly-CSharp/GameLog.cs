using System;
using UnityEngine;

// Token: 0x02000B12 RID: 2834
public class GameLog
{
	// Token: 0x0600514F RID: 20815 RVA: 0x0015200E File Offset: 0x0015040E
	public GameLog(IEvents events)
	{
		this.events = events;
	}

	// Token: 0x06005150 RID: 20816 RVA: 0x0015201D File Offset: 0x0015041D
	public void Debug(string message)
	{
		UnityEngine.Debug.Log(message);
	}

	// Token: 0x06005151 RID: 20817 RVA: 0x00152025 File Offset: 0x00150425
	public void Error(string message)
	{
		UnityEngine.Debug.LogError(message);
	}

	// Token: 0x06005152 RID: 20818 RVA: 0x0015202D File Offset: 0x0015042D
	public void FatalError(string message)
	{
		UnityEngine.Debug.LogError(message);
	}

	// Token: 0x04003464 RID: 13412
	private IEvents events;

	// Token: 0x04003465 RID: 13413
	private bool haltOnFatalError;
}
