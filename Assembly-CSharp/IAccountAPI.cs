using System;

// Token: 0x02000776 RID: 1910
public interface IAccountAPI
{
	// Token: 0x06002F56 RID: 12118
	void Initialize();

	// Token: 0x17000B71 RID: 2929
	// (get) Token: 0x06002F57 RID: 12119
	string UserName { get; }

	// Token: 0x17000B72 RID: 2930
	// (get) Token: 0x06002F58 RID: 12120
	ulong ID { get; }
}
