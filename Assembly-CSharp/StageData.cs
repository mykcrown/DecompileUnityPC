using System;
using UnityEngine;

// Token: 0x02000641 RID: 1601
[Serializable]
public class StageData : ScriptableObject, IGameDataElement
{
	// Token: 0x170009A7 RID: 2471
	// (get) Token: 0x06002740 RID: 10048 RVA: 0x000BF8F5 File Offset: 0x000BDCF5
	public int ID
	{
		get
		{
			return (int)this.stageID;
		}
	}

	// Token: 0x170009A8 RID: 2472
	// (get) Token: 0x06002741 RID: 10049 RVA: 0x000BF8FD File Offset: 0x000BDCFD
	public string Key
	{
		get
		{
			return this.stageName;
		}
	}

	// Token: 0x170009A5 RID: 2469
	// (get) Token: 0x06002742 RID: 10050 RVA: 0x000BF905 File Offset: 0x000BDD05
	bool IGameDataElement.Enabled
	{
		get
		{
			return this.enabled;
		}
	}

	// Token: 0x170009A6 RID: 2470
	// (get) Token: 0x06002743 RID: 10051 RVA: 0x000BF90D File Offset: 0x000BDD0D
	LocalizationData IGameDataElement.Localization
	{
		get
		{
			return this.localization;
		}
	}

	// Token: 0x04001CBA RID: 7354
	public string stageName;

	// Token: 0x04001CBB RID: 7355
	public StageID stageID;

	// Token: 0x04001CBC RID: 7356
	public LocalizationData localization;

	// Token: 0x04001CBD RID: 7357
	public Sprite smallPortrait;

	// Token: 0x04001CBE RID: 7358
	public Sprite largePortrait;

	// Token: 0x04001CBF RID: 7359
	public Sprite loadingScreen;

	// Token: 0x04001CC0 RID: 7360
	public Sprite worldIcon;

	// Token: 0x04001CC1 RID: 7361
	public GameObject prefab;

	// Token: 0x04001CC2 RID: 7362
	public string sceneName;

	// Token: 0x04001CC3 RID: 7363
	public string lowDetailSceneName;

	// Token: 0x04001CC4 RID: 7364
	public bool enabled = true;

	// Token: 0x04001CC5 RID: 7365
	public bool isTemporary;

	// Token: 0x04001CC6 RID: 7366
	public bool isDev;

	// Token: 0x04001CC7 RID: 7367
	public StageType stageType = StageType.Normal;

	// Token: 0x04001CC8 RID: 7368
	public bool useCustomCameraData;

	// Token: 0x04001CC9 RID: 7369
	public CameraConfig.StageData cameraData = new CameraConfig.StageData();

	// Token: 0x04001CCA RID: 7370
	public StageData.LoadingTipWeightData[] loadingTips = new StageData.LoadingTipWeightData[0];

	// Token: 0x02000642 RID: 1602
	[Serializable]
	public class LoadingTipWeightData
	{
		// Token: 0x04001CCB RID: 7371
		public string loadingTip;

		// Token: 0x04001CCC RID: 7372
		public int weight = 100;
	}
}
