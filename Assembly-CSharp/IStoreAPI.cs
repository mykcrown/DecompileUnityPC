using System;

// Token: 0x02000A28 RID: 2600
public interface IStoreAPI : IDataDependency
{
	// Token: 0x06004BCA RID: 19402
	void OnScreenOpened();

	// Token: 0x17001208 RID: 4616
	// (get) Token: 0x06004BCB RID: 19403
	// (set) Token: 0x06004BCC RID: 19404
	StoreMode Mode { get; set; }

	// Token: 0x17001209 RID: 4617
	// (get) Token: 0x06004BCD RID: 19405
	// (set) Token: 0x06004BCE RID: 19406
	int Port { get; set; }

	// Token: 0x1700120A RID: 4618
	// (get) Token: 0x06004BCF RID: 19407
	string PortDisplay { get; }
}
