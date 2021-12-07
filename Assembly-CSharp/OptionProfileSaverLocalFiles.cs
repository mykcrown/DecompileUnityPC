using System;

// Token: 0x02000536 RID: 1334
public class OptionProfileSaverLocalFiles : IOptionProfileSaver
{
	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x06001D0D RID: 7437 RVA: 0x000956EE File Offset: 0x00093AEE
	// (set) Token: 0x06001D0E RID: 7438 RVA: 0x000956F6 File Offset: 0x00093AF6
	[Inject]
	public ISaveFileData saveFileData { get; set; }

	// Token: 0x06001D0F RID: 7439 RVA: 0x00095700 File Offset: 0x00093B00
	public void Load(Action<LoadOptionsProfileListResult> callback)
	{
		LoadOptionsProfileListResult loadOptionsProfileListResult = new LoadOptionsProfileListResult();
		loadOptionsProfileListResult.success = true;
		OptionsProfileSet fromFile = this.saveFileData.GetFromFile<OptionsProfileSet>("gameOptions/completeSet_3.options");
		loadOptionsProfileListResult.state = fromFile;
		callback(loadOptionsProfileListResult);
	}

	// Token: 0x06001D10 RID: 7440 RVA: 0x00095739 File Offset: 0x00093B39
	public void Save(OptionsProfileSet data, Action<SaveOptionsProfileResult> callback)
	{
		this.saveFileData.SaveToFile<OptionsProfileSet>("gameOptions/completeSet_3.options", data);
		if (callback != null)
		{
			callback(SaveOptionsProfileResult.SUCCESS);
		}
	}

	// Token: 0x040017D0 RID: 6096
	private const string FILE_NAME = "gameOptions/completeSet_3.options";
}
