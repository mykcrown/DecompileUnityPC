using System;

// Token: 0x02000A18 RID: 2584
public interface IPurchaseResponseDialog
{
	// Token: 0x06004B22 RID: 19234
	void Close();

	// Token: 0x06004B23 RID: 19235
	void ShowError(string title, string body);

	// Token: 0x170011E6 RID: 4582
	// (set) Token: 0x06004B24 RID: 19236
	Action CloseCallback { set; }
}
