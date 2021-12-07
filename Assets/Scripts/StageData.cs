// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class StageData : ScriptableObject, IGameDataElement
{
	[Serializable]
	public class LoadingTipWeightData
	{
		public string loadingTip;

		public int weight = 100;
	}

	public string stageName;

	public StageID stageID;

	public LocalizationData localization;

	public Sprite smallPortrait;

	public Sprite largePortrait;

	public Sprite loadingScreen;

	public Sprite worldIcon;

	public GameObject prefab;

	public string sceneName;

	public string lowDetailSceneName;

	public bool enabled = true;

	public bool isTemporary;

	public bool isDev;

	public StageType stageType = StageType.Normal;

	public bool useCustomCameraData;

	public CameraConfig.StageData cameraData = new CameraConfig.StageData();

	public StageData.LoadingTipWeightData[] loadingTips = new StageData.LoadingTipWeightData[0];

	bool IGameDataElement.Enabled
	{
		get
		{
			return this.enabled;
		}
	}

	LocalizationData IGameDataElement.Localization
	{
		get
		{
			return this.localization;
		}
	}

	public int ID
	{
		get
		{
			return (int)this.stageID;
		}
	}

	public string Key
	{
		get
		{
			return this.stageName;
		}
	}
}
