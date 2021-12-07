// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IInputSettingsSaveManager
{
	InputSettingsData LoadInputSettings(IInputDevice device, bool isP2Debug, bool shouldPersist);

	InputSettingsData GetDefaultInputSettings(IInputDevice device, bool isP2Debug);

	void SaveInputSettings(InputSettingsData settingsData, IInputDevice device, bool shouldPersist);
}
