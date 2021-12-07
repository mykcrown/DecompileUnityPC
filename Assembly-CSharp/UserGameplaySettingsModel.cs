using System;

// Token: 0x02000619 RID: 1561
public class UserGameplaySettingsModel : IUserGameplaySettingsModel
{
	// Token: 0x1700097A RID: 2426
	// (get) Token: 0x0600267D RID: 9853 RVA: 0x000BD27F File Offset: 0x000BB67F
	// (set) Token: 0x0600267E RID: 9854 RVA: 0x000BD287 File Offset: 0x000BB687
	[Inject]
	public ISaveFileData saveFileData { get; set; }

	// Token: 0x1700097B RID: 2427
	// (get) Token: 0x0600267F RID: 9855 RVA: 0x000BD290 File Offset: 0x000BB690
	// (set) Token: 0x06002680 RID: 9856 RVA: 0x000BD298 File Offset: 0x000BB698
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x06002681 RID: 9857 RVA: 0x000BD2A1 File Offset: 0x000BB6A1
	public void Init()
	{
		this.loadFromFile();
	}

	// Token: 0x06002682 RID: 9858 RVA: 0x000BD2A9 File Offset: 0x000BB6A9
	public void Reset()
	{
		this.data = this.getDefaultSettings();
		this.saveAndUpdate();
	}

	// Token: 0x06002683 RID: 9859 RVA: 0x000BD2BD File Offset: 0x000BB6BD
	private void saveAndUpdate()
	{
		this.saveFile();
		this.syncToData();
	}

	// Token: 0x06002684 RID: 9860 RVA: 0x000BD2CB File Offset: 0x000BB6CB
	private void saveFile()
	{
		this.saveFileData.SaveToXmlFile<UserGameplaySettings>(UserGameplaySettingsModel.FILENAME, this.data);
	}

	// Token: 0x06002685 RID: 9861 RVA: 0x000BD2E4 File Offset: 0x000BB6E4
	private void loadFromFile()
	{
		UserGameplaySettings fromXmlFile = this.saveFileData.GetFromXmlFile<UserGameplaySettings>(UserGameplaySettingsModel.FILENAME);
		if (fromXmlFile != null)
		{
			this.data = fromXmlFile;
		}
		else
		{
			this.data = this.getDefaultSettings();
		}
		this.syncToData();
	}

	// Token: 0x06002686 RID: 9862 RVA: 0x000BD326 File Offset: 0x000BB726
	private UserGameplaySettings getDefaultSettings()
	{
		return new UserGameplaySettings();
	}

	// Token: 0x06002687 RID: 9863 RVA: 0x000BD32D File Offset: 0x000BB72D
	private void syncToData()
	{
		this.dispatchUpdate();
	}

	// Token: 0x06002688 RID: 9864 RVA: 0x000BD335 File Offset: 0x000BB735
	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(UserGameplaySettingsModel.UPDATED);
	}

	// Token: 0x1700097C RID: 2428
	// (get) Token: 0x06002689 RID: 9865 RVA: 0x000BD347 File Offset: 0x000BB747
	// (set) Token: 0x0600268A RID: 9866 RVA: 0x000BD354 File Offset: 0x000BB754
	public bool MuteEnemyHolos
	{
		get
		{
			return this.data.muteEnemyHolos;
		}
		set
		{
			this.data.muteEnemyHolos = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x0600268B RID: 9867 RVA: 0x000BD368 File Offset: 0x000BB768
	// (set) Token: 0x0600268C RID: 9868 RVA: 0x000BD375 File Offset: 0x000BB775
	public bool MuteEnemyVoicelines
	{
		get
		{
			return this.data.muteEnemyVoicelines;
		}
		set
		{
			this.data.muteEnemyVoicelines = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x04001C28 RID: 7208
	public static string UPDATED = "UserGameplaySettingsModel.UPDATED";

	// Token: 0x04001C29 RID: 7209
	private static string FILENAME = "settings/gameplay.settings";

	// Token: 0x04001C2C RID: 7212
	private UserGameplaySettings data = new UserGameplaySettings();
}
