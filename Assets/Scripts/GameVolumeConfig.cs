// Decompile from assembly: Assembly-CSharp.dll

using System;

public class GameVolumeConfig : IAudioVolumeConfig
{
	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public float MusicVolume
	{
		get
		{
			bool flag = false;
			if (this.gameController.currentGame != null)
			{
				flag = this.gameController.currentGame.IsPaused;
			}
			return (!flag) ? 1f : (this.gameDataManager.ConfigData.pausedMusicVolume / this.gameDataManager.ConfigData.musicVolume);
		}
	}

	public float MusicFadeTime
	{
		get
		{
			return this.gameDataManager.ConfigData.musicFadeTime;
		}
	}
}
