using System;

// Token: 0x02000697 RID: 1687
public interface IInputDevice
{
	// Token: 0x17000A38 RID: 2616
	// (get) Token: 0x0600299F RID: 10655
	string Name { get; }

	// Token: 0x060029A0 RID: 10656
	void MangleDefaultSettings(InputSettingsData settings);
}
