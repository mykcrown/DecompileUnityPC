using System;

// Token: 0x0200097B RID: 2427
public class GameplayTabAPI : IGameplayTabAPI
{
	// Token: 0x17000F56 RID: 3926
	// (get) Token: 0x06004125 RID: 16677 RVA: 0x00125500 File Offset: 0x00123900
	// (set) Token: 0x06004126 RID: 16678 RVA: 0x00125508 File Offset: 0x00123908
	[Inject]
	public IUserGameplaySettingsModel userGameplaySettingsModel { get; set; }

	// Token: 0x06004127 RID: 16679 RVA: 0x00125511 File Offset: 0x00123911
	public void Reset()
	{
		this.userGameplaySettingsModel.Reset();
	}

	// Token: 0x17000F57 RID: 3927
	// (get) Token: 0x06004128 RID: 16680 RVA: 0x0012551E File Offset: 0x0012391E
	// (set) Token: 0x06004129 RID: 16681 RVA: 0x0012552B File Offset: 0x0012392B
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

	// Token: 0x17000F58 RID: 3928
	// (get) Token: 0x0600412A RID: 16682 RVA: 0x00125539 File Offset: 0x00123939
	// (set) Token: 0x0600412B RID: 16683 RVA: 0x00125546 File Offset: 0x00123946
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
}
