using System;

// Token: 0x02000617 RID: 1559
public interface IInputSettingsSaveManager
{
	// Token: 0x06002678 RID: 9848
	InputSettingsData LoadInputSettings(IInputDevice device, bool isP2Debug, bool shouldPersist);

	// Token: 0x06002679 RID: 9849
	InputSettingsData GetDefaultInputSettings(IInputDevice device, bool isP2Debug);

	// Token: 0x0600267A RID: 9850
	void SaveInputSettings(InputSettingsData settingsData, IInputDevice device, bool shouldPersist);
}
