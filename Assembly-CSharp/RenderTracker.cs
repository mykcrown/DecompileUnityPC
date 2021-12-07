using System;

// Token: 0x02000B5A RID: 2906
public class RenderTracker : ClientBehavior
{
	// Token: 0x17001376 RID: 4982
	// (get) Token: 0x06005429 RID: 21545 RVA: 0x001B1257 File Offset: 0x001AF657
	// (set) Token: 0x0600542A RID: 21546 RVA: 0x001B125F File Offset: 0x001AF65F
	[Inject]
	public IMatchDeepLogging deepLogging { get; set; }

	// Token: 0x0600542B RID: 21547 RVA: 0x001B1268 File Offset: 0x001AF668
	private void OnPreRender()
	{
		this.deepLogging.RecordPreRender();
		if (this.PreRenderCallback != null)
		{
			this.PreRenderCallback();
		}
	}

	// Token: 0x0600542C RID: 21548 RVA: 0x001B128B File Offset: 0x001AF68B
	private void OnPostRender()
	{
		this.deepLogging.RecordPostRender();
	}

	// Token: 0x04003559 RID: 13657
	public Action PreRenderCallback;
}
