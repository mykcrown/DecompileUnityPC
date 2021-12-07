using System;
using System.IO;
using UnityEngine;

// Token: 0x0200084B RID: 2123
public class SaveReplay
{
	// Token: 0x17000CE5 RID: 3301
	// (get) Token: 0x0600351B RID: 13595 RVA: 0x000F9552 File Offset: 0x000F7952
	// (set) Token: 0x0600351C RID: 13596 RVA: 0x000F955A File Offset: 0x000F795A
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x0600351D RID: 13597 RVA: 0x000F9564 File Offset: 0x000F7964
	public void Execute()
	{
		ConfigData configData = this.gameDataManager.ConfigData;
		string text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
		text = text.Replace("/", "-");
		string text2 = "replay " + text + ".log";
		string replayName = configData.replaySettings.replayName;
		if (File.Exists(replayName))
		{
			Debug.Log("Copied to " + text2);
			File.Copy(replayName, text2);
		}
	}
}
