// Decompile from assembly: Assembly-CSharp.dll

using System;

public class OptionProfileSaverLocalFiles : IOptionProfileSaver
{
	private const string FILE_NAME = "gameOptions/completeSet_3.options";

	[Inject]
	public ISaveFileData saveFileData
	{
		get;
		set;
	}

	public void Load(Action<LoadOptionsProfileListResult> callback)
	{
		LoadOptionsProfileListResult loadOptionsProfileListResult = new LoadOptionsProfileListResult();
		loadOptionsProfileListResult.success = true;
		OptionsProfileSet fromFile = this.saveFileData.GetFromFile<OptionsProfileSet>("gameOptions/completeSet_3.options");
		loadOptionsProfileListResult.state = fromFile;
		callback(loadOptionsProfileListResult);
	}

	public void Save(OptionsProfileSet data, Action<SaveOptionsProfileResult> callback)
	{
		this.saveFileData.SaveToFile<OptionsProfileSet>("gameOptions/completeSet_3.options", data);
		if (callback != null)
		{
			callback(SaveOptionsProfileResult.SUCCESS);
		}
	}
}
