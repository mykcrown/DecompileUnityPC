// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IInputDevice
{
	string Name
	{
		get;
	}

	void MangleDefaultSettings(InputSettingsData settings);
}
