using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000B93 RID: 2963
public class UserVideoSettingsModel : IUserVideoSettingsModel
{
	// Token: 0x170013A7 RID: 5031
	// (get) Token: 0x0600556B RID: 21867 RVA: 0x001B59EF File Offset: 0x001B3DEF
	// (set) Token: 0x0600556C RID: 21868 RVA: 0x001B59F7 File Offset: 0x001B3DF7
	[Inject]
	public IAutoQualityDetection autoQualityDetection { get; set; }

	// Token: 0x170013A8 RID: 5032
	// (get) Token: 0x0600556D RID: 21869 RVA: 0x001B5A00 File Offset: 0x001B3E00
	// (set) Token: 0x0600556E RID: 21870 RVA: 0x001B5A08 File Offset: 0x001B3E08
	[Inject]
	public ISaveFileData saveFileData { get; set; }

	// Token: 0x170013A9 RID: 5033
	// (get) Token: 0x0600556F RID: 21871 RVA: 0x001B5A11 File Offset: 0x001B3E11
	// (set) Token: 0x06005570 RID: 21872 RVA: 0x001B5A19 File Offset: 0x001B3E19
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170013AA RID: 5034
	// (get) Token: 0x06005571 RID: 21873 RVA: 0x001B5A22 File Offset: 0x001B3E22
	// (set) Token: 0x06005572 RID: 21874 RVA: 0x001B5A2A File Offset: 0x001B3E2A
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x170013AB RID: 5035
	// (get) Token: 0x06005573 RID: 21875 RVA: 0x001B5A33 File Offset: 0x001B3E33
	// (set) Token: 0x06005574 RID: 21876 RVA: 0x001B5A3B File Offset: 0x001B3E3B
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x06005575 RID: 21877 RVA: 0x001B5A44 File Offset: 0x001B3E44
	public void Init()
	{
		this.loadFromFile();
	}

	// Token: 0x06005576 RID: 21878 RVA: 0x001B5A4C File Offset: 0x001B3E4C
	public void Reset()
	{
		this.data = this.getDefaultSettings();
		this.saveAndUpdate();
	}

	// Token: 0x06005577 RID: 21879 RVA: 0x001B5A60 File Offset: 0x001B3E60
	public void Update()
	{
		if (this.data.fullScreen)
		{
			return;
		}
		WD_Resolution resolution = this.Resolution;
		if (resolution.width != Screen.width || resolution.height != Screen.height)
		{
			float f = (float)Screen.width - (float)resolution.width;
			float f2 = (float)Screen.height - (float)resolution.height;
			float num;
			if (Mathf.Abs(f) > Mathf.Abs(f2))
			{
				num = (float)Screen.width / (float)resolution.width;
			}
			else
			{
				num = (float)Screen.height / (float)resolution.height;
			}
			this.data.windowedResolution.width = (int)((float)resolution.width * num);
			this.data.windowedResolution.height = (int)((float)resolution.width * num / this.ASPECT_RATIO);
			this.updateScreenSize();
			this.dispatchUpdate();
		}
	}

	// Token: 0x06005578 RID: 21880 RVA: 0x001B5B4C File Offset: 0x001B3F4C
	public void GetPossibleResolutions(List<WD_Resolution> resolutionBuffer)
	{
		resolutionBuffer.Clear();
		foreach (Resolution resolution in Screen.resolutions)
		{
			WD_Resolution item = new WD_Resolution
			{
				width = resolution.width,
				height = resolution.height
			};
			if (!resolutionBuffer.Contains(item))
			{
				resolutionBuffer.Add(item);
			}
		}
	}

	// Token: 0x170013AC RID: 5036
	// (get) Token: 0x06005579 RID: 21881 RVA: 0x001B5BC0 File Offset: 0x001B3FC0
	// (set) Token: 0x0600557A RID: 21882 RVA: 0x001B5BE8 File Offset: 0x001B3FE8
	public int QualityIndex
	{
		get
		{
			return Math.Min(QualitySettings.names.Length - 1, this.data.qualityIndex);
		}
		set
		{
			this.data.qualityIndex = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013AD RID: 5037
	// (get) Token: 0x0600557B RID: 21883 RVA: 0x001B5BFC File Offset: 0x001B3FFC
	// (set) Token: 0x0600557C RID: 21884 RVA: 0x001B5BFF File Offset: 0x001B3FFF
	public ThreeTierQualityLevel ParticleQuality
	{
		get
		{
			return ThreeTierQualityLevel.Medium;
		}
		set
		{
			this.data.particleQualityLevel = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013AE RID: 5038
	// (get) Token: 0x0600557D RID: 21885 RVA: 0x001B5C13 File Offset: 0x001B4013
	// (set) Token: 0x0600557E RID: 21886 RVA: 0x001B5C16 File Offset: 0x001B4016
	public ThreeTierQualityLevel StageQuality
	{
		get
		{
			return ThreeTierQualityLevel.High;
		}
		set
		{
			this.data.stageQualityLevel = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013AF RID: 5039
	// (get) Token: 0x0600557F RID: 21887 RVA: 0x001B5C2A File Offset: 0x001B402A
	// (set) Token: 0x06005580 RID: 21888 RVA: 0x001B5C37 File Offset: 0x001B4037
	public ThreeTierQualityLevel GraphicsQuality
	{
		get
		{
			return this.data.graphicsQualityLevel;
		}
		set
		{
			this.data.graphicsQualityLevel = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013B0 RID: 5040
	// (get) Token: 0x06005581 RID: 21889 RVA: 0x001B5C4B File Offset: 0x001B404B
	// (set) Token: 0x06005582 RID: 21890 RVA: 0x001B5C58 File Offset: 0x001B4058
	public ThreeTierQualityLevel MaterialQuality
	{
		get
		{
			return this.data.materialQualityLevel;
		}
		set
		{
			this.data.materialQualityLevel = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013B1 RID: 5041
	// (get) Token: 0x06005583 RID: 21891 RVA: 0x001B5C6C File Offset: 0x001B406C
	// (set) Token: 0x06005584 RID: 21892 RVA: 0x001B5C95 File Offset: 0x001B4095
	public WD_Resolution Resolution
	{
		get
		{
			if (this.data.fullScreen)
			{
				return this.data.fullScreenResolution;
			}
			return this.data.windowedResolution;
		}
		set
		{
			if (this.data.fullScreen)
			{
				this.data.fullScreenResolution = value;
			}
			else
			{
				this.data.windowedResolution = value;
			}
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013B2 RID: 5042
	// (get) Token: 0x06005585 RID: 21893 RVA: 0x001B5CCC File Offset: 0x001B40CC
	// (set) Token: 0x06005586 RID: 21894 RVA: 0x001B5CF4 File Offset: 0x001B40F4
	public int DisplayIndex
	{
		get
		{
			return Math.Min(Display.displays.Length - 1, this.data.displayIndex);
		}
		set
		{
			this.data.displayIndex = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013B3 RID: 5043
	// (get) Token: 0x06005587 RID: 21895 RVA: 0x001B5D08 File Offset: 0x001B4108
	// (set) Token: 0x06005588 RID: 21896 RVA: 0x001B5D15 File Offset: 0x001B4115
	public bool MultiSampleAntiAliasing
	{
		get
		{
			return this.data.multiSampleAntiAliasing;
		}
		set
		{
			this.data.multiSampleAntiAliasing = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013B4 RID: 5044
	// (get) Token: 0x06005589 RID: 21897 RVA: 0x001B5D29 File Offset: 0x001B4129
	// (set) Token: 0x0600558A RID: 21898 RVA: 0x001B5D36 File Offset: 0x001B4136
	public bool VisibleInBackground
	{
		get
		{
			return this.data.visibleInBackground;
		}
		set
		{
			this.data.visibleInBackground = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013B5 RID: 5045
	// (get) Token: 0x0600558B RID: 21899 RVA: 0x001B5D4A File Offset: 0x001B414A
	// (set) Token: 0x0600558C RID: 21900 RVA: 0x001B5D57 File Offset: 0x001B4157
	public bool FullScreen
	{
		get
		{
			return this.data.fullScreen;
		}
		set
		{
			this.data.fullScreen = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013B6 RID: 5046
	// (get) Token: 0x0600558D RID: 21901 RVA: 0x001B5D6B File Offset: 0x001B416B
	// (set) Token: 0x0600558E RID: 21902 RVA: 0x001B5D78 File Offset: 0x001B4178
	public bool ShowPerformance
	{
		get
		{
			return this.data.showPerformance;
		}
		set
		{
			this.data.showPerformance = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013B7 RID: 5047
	// (get) Token: 0x0600558F RID: 21903 RVA: 0x001B5D8C File Offset: 0x001B418C
	// (set) Token: 0x06005590 RID: 21904 RVA: 0x001B5D99 File Offset: 0x001B4199
	public bool ShowSystemClock
	{
		get
		{
			return this.data.showSystemClock;
		}
		set
		{
			this.data.showSystemClock = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013B8 RID: 5048
	// (get) Token: 0x06005591 RID: 21905 RVA: 0x001B5DAD File Offset: 0x001B41AD
	// (set) Token: 0x06005592 RID: 21906 RVA: 0x001B5DBA File Offset: 0x001B41BA
	public bool PostProcessing
	{
		get
		{
			return this.data.postProcessing;
		}
		set
		{
			this.data.postProcessing = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013B9 RID: 5049
	// (get) Token: 0x06005593 RID: 21907 RVA: 0x001B5DCE File Offset: 0x001B41CE
	// (set) Token: 0x06005594 RID: 21908 RVA: 0x001B5DDB File Offset: 0x001B41DB
	public bool MotionBlur
	{
		get
		{
			return this.data.motionBlur;
		}
		set
		{
			this.data.motionBlur = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x170013BA RID: 5050
	// (get) Token: 0x06005595 RID: 21909 RVA: 0x001B5DEF File Offset: 0x001B41EF
	// (set) Token: 0x06005596 RID: 21910 RVA: 0x001B5DFC File Offset: 0x001B41FC
	public bool Vsync
	{
		get
		{
			return this.data.vsync;
		}
		set
		{
			this.data.vsync = value;
			this.saveAndUpdate();
		}
	}

	// Token: 0x06005597 RID: 21911 RVA: 0x001B5E10 File Offset: 0x001B4210
	private void saveAndUpdate()
	{
		this.saveFile();
		this.syncToData();
	}

	// Token: 0x06005598 RID: 21912 RVA: 0x001B5E1E File Offset: 0x001B421E
	private void saveFile()
	{
		this.saveFileData.SaveToXmlFile<UserVideoSettings>(UserVideoSettingsModel.FILENAME, this.data);
	}

	// Token: 0x06005599 RID: 21913 RVA: 0x001B5E38 File Offset: 0x001B4238
	private void loadFromFile()
	{
		UserVideoSettings fromXmlFile = this.saveFileData.GetFromXmlFile<UserVideoSettings>(UserVideoSettingsModel.FILENAME);
		if (fromXmlFile != null)
		{
			this.data = fromXmlFile;
		}
		else
		{
			this.data = this.getDefaultSettings();
		}
		this.checkUpgradeDataVersion(this.data);
		this.syncToData();
	}

	// Token: 0x0600559A RID: 21914 RVA: 0x001B5E88 File Offset: 0x001B4288
	public UserVideoSettings SetAutoQuality(UserVideoSettings settings)
	{
		int graphicsQualityIndex;
		int num;
		if (this.autoQualityDetection.PassmarkGpuScore != 0)
		{
			graphicsQualityIndex = this.autoQualityDetection.PassmarkGpuQualityLevel;
			if (this.autoQualityDetection.PassmarkCpuScore != 0)
			{
				num = Mathf.Min(this.autoQualityDetection.PassmarkCpuQualityLevel, this.autoQualityDetection.PassmarkGpuQualityLevel);
			}
			else
			{
				num = Mathf.Min(this.autoQualityDetection.PassmarkGpuQualityLevel, this.autoQualityDetection.FallbackQualityLevel);
			}
		}
		else if (this.autoQualityDetection.PassmarkCpuScore != 0)
		{
			num = Mathf.Min(this.autoQualityDetection.PassmarkCpuQualityLevel, this.autoQualityDetection.FallbackQualityLevel);
			graphicsQualityIndex = num;
		}
		else
		{
			num = this.autoQualityDetection.FallbackQualityLevel;
			graphicsQualityIndex = num;
		}
		settings = this.applyQualityIndex(settings, num, graphicsQualityIndex);
		return settings;
	}

	// Token: 0x0600559B RID: 21915 RVA: 0x001B5F54 File Offset: 0x001B4354
	private UserVideoSettings applyQualityIndex(UserVideoSettings settings, int qualityIndex, int graphicsQualityIndex)
	{
		List<DefaultQualityLevelSettings> allLevelSettings = this.gameDataManager.ConfigData.defaultQualityConfig.allLevelSettings;
		if (allLevelSettings.Count == 0)
		{
			Debug.LogError("No default quality level setting set. Set at least one default in the Config.");
			return settings;
		}
		DefaultQualityLevelSettings defaultQualityLevelSettings;
		if (qualityIndex >= allLevelSettings.Count)
		{
			defaultQualityLevelSettings = allLevelSettings[allLevelSettings.Count - 1];
		}
		else
		{
			defaultQualityLevelSettings = allLevelSettings[qualityIndex];
		}
		DefaultQualityLevelSettings defaultQualityLevelSettings2;
		if (graphicsQualityIndex >= allLevelSettings.Count)
		{
			defaultQualityLevelSettings2 = allLevelSettings[allLevelSettings.Count - 1];
		}
		else
		{
			defaultQualityLevelSettings2 = allLevelSettings[graphicsQualityIndex];
		}
		WD_Resolution closestPossibleResolution = this.getClosestPossibleResolution(defaultQualityLevelSettings.minResolution);
		settings.fullScreenResolution = closestPossibleResolution;
		settings.windowedResolution = closestPossibleResolution;
		settings.postProcessing = defaultQualityLevelSettings.postProcessing;
		settings.motionBlur = false;
		settings.multiSampleAntiAliasing = defaultQualityLevelSettings.multiSampleAntiAliasing;
		settings.stageQualityLevel = defaultQualityLevelSettings2.stageQualityLevel;
		settings.graphicsQualityLevel = defaultQualityLevelSettings2.graphicsQualityLevel;
		settings.materialQualityLevel = ((graphicsQualityIndex != 0 && SystemInfo.graphicsShaderLevel >= 30) ? ThreeTierQualityLevel.High : ThreeTierQualityLevel.Low);
		return settings;
	}

	// Token: 0x0600559C RID: 21916 RVA: 0x001B6050 File Offset: 0x001B4450
	public void ApplySecondarySettingsFromQualityIndex()
	{
		this.data = this.applyQualityIndex(this.data, this.data.qualityIndex, this.data.qualityIndex);
		this.saveAndUpdate();
	}

	// Token: 0x0600559D RID: 21917 RVA: 0x001B6080 File Offset: 0x001B4480
	private void checkUpgradeDataVersion(UserVideoSettings data)
	{
		if (data.version < UserVideoSettings.CURRENT_VERSION)
		{
			if (data.version < 3)
			{
				data.particleQualityLevel = ThreeTierQualityLevel.Medium;
			}
			if (data.version < 5)
			{
				this.SetAutoQuality(data);
			}
			Debug.Log(string.Concat(new object[]
			{
				"Upgrade video settings from version ",
				data.version,
				" to ",
				UserVideoSettings.CURRENT_VERSION
			}));
			data.version = UserVideoSettings.CURRENT_VERSION;
			this.saveFile();
		}
	}

	// Token: 0x0600559E RID: 21918 RVA: 0x001B6110 File Offset: 0x001B4510
	private void syncToData()
	{
		if (this.QualityIndex != QualitySettings.GetQualityLevel())
		{
			QualitySettings.SetQualityLevel(this.QualityIndex, true);
		}
		QualitySettings.vSyncCount = ((!this.Vsync) ? 0 : 1);
		if (this.GraphicsQuality == ThreeTierQualityLevel.Low)
		{
			Graphics.activeTier = GraphicsTier.Tier1;
		}
		else if (this.GraphicsQuality == ThreeTierQualityLevel.Medium)
		{
			Graphics.activeTier = GraphicsTier.Tier2;
		}
		else
		{
			Graphics.activeTier = GraphicsTier.Tier3;
		}
		this.updateScreenSize();
		this.dispatchUpdate();
	}

	// Token: 0x0600559F RID: 21919 RVA: 0x001B6190 File Offset: 0x001B4590
	private void updateScreenSize()
	{
		WD_Resolution resolution = this.Resolution;
		bool flag = Screen.width == resolution.width && Screen.height == resolution.height;
		if (this.data.fullScreen != Screen.fullScreen || !flag)
		{
			Screen.SetResolution(resolution.width, resolution.height, this.data.fullScreen);
		}
	}

	// Token: 0x060055A0 RID: 21920 RVA: 0x001B6200 File Offset: 0x001B4600
	private WD_Resolution getClosestPossibleResolution(WD_Resolution res)
	{
		this.GetPossibleResolutions(this.resolutionBuffer);
		for (int i = 0; i < this.resolutionBuffer.Count; i++)
		{
			if (this.resolutionBuffer[i].width >= res.width)
			{
				return this.resolutionBuffer[i];
			}
		}
		return this.resolutionBuffer[this.resolutionBuffer.Count - 1];
	}

	// Token: 0x060055A1 RID: 21921 RVA: 0x001B627A File Offset: 0x001B467A
	private UserVideoSettings getDefaultSettings()
	{
		return this.SetAutoQuality(new UserVideoSettings());
	}

	// Token: 0x060055A2 RID: 21922 RVA: 0x001B6287 File Offset: 0x001B4687
	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(UserVideoSettingsModel.UPDATED);
	}

	// Token: 0x0400365A RID: 13914
	public static string UPDATED = "UserVideoSettingsModel.UPDATED";

	// Token: 0x0400365B RID: 13915
	private static string FILENAME = "settings/video4.settings";

	// Token: 0x0400365C RID: 13916
	private float ASPECT_RATIO = 1.777778f;

	// Token: 0x04003662 RID: 13922
	private UserVideoSettings data = new UserVideoSettings();

	// Token: 0x04003663 RID: 13923
	private List<WD_Resolution> resolutionBuffer = new List<WD_Resolution>();
}
