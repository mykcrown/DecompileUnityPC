using System;

// Token: 0x020005A8 RID: 1448
public interface IDropListener
{
	// Token: 0x0600209F RID: 8351
	void OnDrop();

	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x060020A0 RID: 8352
	bool AllowFastFall { get; }
}
