// Decompile from assembly: Assembly-CSharp.dll

using System;

public class UserGameplaySettingsModel : IUserGameplaySettingsModel
{
	public static string UPDATED = "UserGameplaySettingsModel.UPDATED";

	private static string FILENAME = "settings/gameplay.settings";

	private UserGameplaySettings data = new UserGameplaySettings();

	[Inject]
	public ISaveFileData saveFileData
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

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

	public void Init()
	{
		this.loadFromFile();
	}

	public void Reset()
	{
		this.data = this.getDefaultSettings();
		this.saveAndUpdate();
	}

	private void saveAndUpdate()
	{
		this.saveFile();
		this.syncToData();
	}

	private void saveFile()
	{
		this.saveFileData.SaveToXmlFile<UserGameplaySettings>(UserGameplaySettingsModel.FILENAME, this.data);
	}

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

	private UserGameplaySettings getDefaultSettings()
	{
		return new UserGameplaySettings();
	}

	private void syncToData()
	{
		this.dispatchUpdate();
	}

	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(UserGameplaySettingsModel.UPDATED);
	}
}
