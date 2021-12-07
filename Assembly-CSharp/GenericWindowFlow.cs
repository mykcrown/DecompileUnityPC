using System;
using System.Collections.Generic;

// Token: 0x0200095F RID: 2399
public class GenericWindowFlow
{
	// Token: 0x06004019 RID: 16409 RVA: 0x00120E89 File Offset: 0x0011F289
	public void StartFlow(List<Func<BaseWindow>> createWindows)
	{
		this.createWindows = createWindows;
		this.openNextWindowOnClose(0);
	}

	// Token: 0x0600401A RID: 16410 RVA: 0x00120E9C File Offset: 0x0011F29C
	private void openNextWindowOnClose(int currentIndex)
	{
		if (currentIndex >= this.createWindows.Count)
		{
			if (this.AllClosedCallback != null)
			{
				this.AllClosedCallback();
			}
		}
		else
		{
			BaseWindow baseWindow = this.createWindows[currentIndex]();
			BaseWindow baseWindow2 = baseWindow;
			baseWindow2.CloseCallback = (Action)Delegate.Combine(baseWindow2.CloseCallback, new Action(delegate()
			{
				this.openNextWindowOnClose(currentIndex + 1);
			}));
		}
	}

	// Token: 0x04002B48 RID: 11080
	public Action AllClosedCallback;

	// Token: 0x04002B49 RID: 11081
	private List<Func<BaseWindow>> createWindows;
}
