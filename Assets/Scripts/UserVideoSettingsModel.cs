// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UserVideoSettingsModel : IUserVideoSettingsModel
{
	public static string UPDATED = "UserVideoSettingsModel.UPDATED";

	private static string FILENAME = "settings/video4.settings";

	private float ASPECT_RATIO = 1.777778f;

	private UserVideoSettings data = new UserVideoSettings();

	private List<WD_Resolution> resolutionBuffer = new List<WD_Resolution>();

	[Inject]
	public IAutoQualityDetection autoQualityDetection
	{
		get;
		set;
	}

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

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

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

	public void Init()
	{
		this.loadFromFile();
	}

	public void Reset()
	{
		this.data = this.getDefaultSettings();
		this.saveAndUpdate();
	}

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

	public void GetPossibleResolutions(List<WD_Resolution> resolutionBuffer)
	{
		resolutionBuffer.Clear();
		Resolution[] resolutions = Screen.resolutions;
		for (int i = 0; i < resolutions.Length; i++)
		{
			Resolution resolution = resolutions[i];
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

	private void saveAndUpdate()
	{
		this.saveFile();
		this.syncToData();
	}

	private void saveFile()
	{
		this.saveFileData.SaveToXmlFile<UserVideoSettings>(UserVideoSettingsModel.FILENAME, this.data);
	}

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

	private UserVideoSettings applyQualityIndex(UserVideoSettings settings, int qualityIndex, int graphicsQualityIndex)
	{
		List<DefaultQualityLevelSettings> allLevelSettings = this.gameDataManager.ConfigData.defaultQualityConfig.allLevelSettings;
		if (allLevelSettings.Count == 0)
		{
			UnityEngine.Debug.LogError("No default quality level setting set. Set at least one default in the Config.");
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

	public void ApplySecondarySettingsFromQualityIndex()
	{
		this.data = this.applyQualityIndex(this.data, this.data.qualityIndex, this.data.qualityIndex);
		this.saveAndUpdate();
	}

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
			UnityEngine.Debug.Log(string.Concat(new object[]
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

	private void updateScreenSize()
	{
		WD_Resolution resolution = this.Resolution;
		bool flag = Screen.width == resolution.width && Screen.height == resolution.height;
		if (this.data.fullScreen != Screen.fullScreen || !flag)
		{
			Screen.SetResolution(resolution.width, resolution.height, this.data.fullScreen);
		}
	}

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

	private UserVideoSettings getDefaultSettings()
	{
		return this.SetAutoQuality(new UserVideoSettings());
	}

	private void dispatchUpdate()
	{
		this.signalBus.Dispatch(UserVideoSettingsModel.UPDATED);
	}
}
