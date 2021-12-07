// Decompile from assembly: Assembly-CSharp.dll

using System;

public class GameplayTabAPI : IGameplayTabAPI
{
	[Inject]
	public IUserGameplaySettingsModel userGameplaySettingsModel
	{
		get;
		set;
	}

	public bool MuteEnemyHolos
	{
		get
		{
			return this.userGameplaySettingsModel.MuteEnemyHolos;
		}
		set
		{
			this.userGameplaySettingsModel.MuteEnemyHolos = value;
		}
	}

	public bool MuteEnemyVoicelines
	{
		get
		{
			return this.userGameplaySettingsModel.MuteEnemyVoicelines;
		}
		set
		{
			this.userGameplaySettingsModel.MuteEnemyVoicelines = value;
		}
	}

	public void Reset()
	{
		this.userGameplaySettingsModel.Reset();
	}
}
