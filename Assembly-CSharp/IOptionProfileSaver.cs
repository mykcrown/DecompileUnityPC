using System;

// Token: 0x02000537 RID: 1335
public interface IOptionProfileSaver
{
	// Token: 0x06001D11 RID: 7441
	void Load(Action<LoadOptionsProfileListResult> callback);

	// Token: 0x06001D12 RID: 7442
	void Save(OptionsProfileSet data, Action<SaveOptionsProfileResult> callback);
}
