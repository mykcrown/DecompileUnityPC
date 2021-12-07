using System;

// Token: 0x020002B1 RID: 689
public class GameVolumeConfig : IAudioVolumeConfig
{
	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06000F09 RID: 3849 RVA: 0x0005B950 File Offset: 0x00059D50
	// (set) Token: 0x06000F0A RID: 3850 RVA: 0x0005B958 File Offset: 0x00059D58
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06000F0B RID: 3851 RVA: 0x0005B961 File Offset: 0x00059D61
	// (set) Token: 0x06000F0C RID: 3852 RVA: 0x0005B969 File Offset: 0x00059D69
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0005B974 File Offset: 0x00059D74
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

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0005B9DB File Offset: 0x00059DDB
	public float MusicFadeTime
	{
		get
		{
			return this.gameDataManager.ConfigData.musicFadeTime;
		}
	}
}
