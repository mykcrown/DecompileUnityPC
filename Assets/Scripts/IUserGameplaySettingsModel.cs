// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUserGameplaySettingsModel
{
	bool MuteEnemyHolos
	{
		get;
		set;
	}

	bool MuteEnemyVoicelines
	{
		get;
		set;
	}

	void Init();

	void Reset();
}
