using System;

// Token: 0x020006C4 RID: 1732
public interface ILoadDataSequence
{
	// Token: 0x06002B7D RID: 11133
	void Load(DataRequirement[] list, Action<LoadSequenceResults> callback);
}
