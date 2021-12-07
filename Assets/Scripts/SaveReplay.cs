// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;

public class SaveReplay
{
	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	public void Execute()
	{
		ConfigData configData = this.gameDataManager.ConfigData;
		string text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
		text = text.Replace("/", "-");
		string text2 = "replay " + text + ".log";
		string replayName = configData.replaySettings.replayName;
		if (File.Exists(replayName))
		{
			UnityEngine.Debug.Log("Copied to " + text2);
			File.Copy(replayName, text2);
		}
	}
}
