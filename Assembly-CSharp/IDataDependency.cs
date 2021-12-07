using System;

// Token: 0x020006C2 RID: 1730
public interface IDataDependency
{
	// Token: 0x06002B77 RID: 11127
	void Load(Action<DataLoadResult> callback);
}
