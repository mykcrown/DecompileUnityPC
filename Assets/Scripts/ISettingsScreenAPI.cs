// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ISettingsScreenAPI
{
	PlayerInputPort InputPort
	{
		get;
	}

	void SetPlayer(int portId);
}
