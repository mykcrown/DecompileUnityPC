using System;

// Token: 0x02000210 RID: 528
public interface ISignalBus
{
	// Token: 0x06000A02 RID: 2562
	void AddListener(string signalName, Action theFunction);

	// Token: 0x06000A03 RID: 2563
	void AddOnce(string signalName, Action theFunction);

	// Token: 0x06000A04 RID: 2564
	void RemoveListener(string signalName, Action theFunction);

	// Token: 0x06000A05 RID: 2565
	T GetSignal<T>(string name) where T : new();

	// Token: 0x06000A06 RID: 2566
	T GetSignal<T>() where T : new();

	// Token: 0x06000A07 RID: 2567
	void Dispatch(string signalName);
}
