// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;

public class VideoTabAPI : IVideoTabAPI
{
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel
	{
		get;
		set;
	}

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	public int QualityIndex
	{
		get
		{
			return this.userVideoSettingsModel.QualityIndex;
		}
		set
		{
			this.userVideoSettingsModel.QualityIndex = value;
			this.userVideoSettingsModel.ApplySecondarySettingsFromQualityIndex();
		}
	}

	public WD_Resolution Resolution
	{
		get
		{
			return this.userVideoSettingsModel.Resolution;
		}
		set
		{
			this.userVideoSettingsModel.Resolution = value;
		}
	}

	public int DisplayIndex
	{
		get
		{
			return this.userVideoSettingsModel.DisplayIndex;
		}
		set
		{
			this.userVideoSettingsModel.DisplayIndex = value;
		}
	}

	public bool MultiSampleAntiAliasing
	{
		get
		{
			return this.userVideoSettingsModel.MultiSampleAntiAliasing;
		}
		set
		{
			this.userVideoSettingsModel.MultiSampleAntiAliasing = value;
		}
	}

	public bool VisibleInBackground
	{
		get
		{
			return this.userVideoSettingsModel.VisibleInBackground;
		}
		set
		{
			this.userVideoSettingsModel.VisibleInBackground = value;
		}
	}

	public bool ShowPerformance
	{
		get
		{
			return this.userVideoSettingsModel.ShowPerformance;
		}
		set
		{
			this.userVideoSettingsModel.ShowPerformance = value;
		}
	}

	public bool ShowSystemClock
	{
		get
		{
			return this.userVideoSettingsModel.ShowSystemClock;
		}
		set
		{
			this.userVideoSettingsModel.ShowSystemClock = value;
		}
	}

	public bool FullScreen
	{
		get
		{
			return this.userVideoSettingsModel.FullScreen;
		}
		set
		{
			this.userVideoSettingsModel.FullScreen = value;
		}
	}

	public bool PostProcessing
	{
		get
		{
			return this.userVideoSettingsModel.PostProcessing;
		}
		set
		{
			this.userVideoSettingsModel.PostProcessing = value;
		}
	}

	public bool MotionBlur
	{
		get
		{
			return this.userVideoSettingsModel.MotionBlur;
		}
		set
		{
			this.userVideoSettingsModel.MotionBlur = value;
		}
	}

	public bool Vsync
	{
		get
		{
			return this.userVideoSettingsModel.Vsync;
		}
		set
		{
			this.userVideoSettingsModel.Vsync = value;
		}
	}

	public ThreeTierQualityLevel ParticleQuality
	{
		get
		{
			return this.userVideoSettingsModel.ParticleQuality;
		}
		set
		{
			this.userVideoSettingsModel.ParticleQuality = value;
		}
	}

	public ThreeTierQualityLevel StageQuality
	{
		get
		{
			return this.userVideoSettingsModel.StageQuality;
		}
		set
		{
			this.userVideoSettingsModel.StageQuality = value;
		}
	}

	public ThreeTierQualityLevel MaterialQuality
	{
		get
		{
			return this.userVideoSettingsModel.MaterialQuality;
		}
		set
		{
			this.userVideoSettingsModel.MaterialQuality = value;
		}
	}

	public ThreeTierQualityLevel GraphicsQuality
	{
		get
		{
			return this.userVideoSettingsModel.GraphicsQuality;
		}
		set
		{
			this.userVideoSettingsModel.GraphicsQuality = value;
		}
	}

	public void Reset()
	{
		this.userVideoSettingsModel.Reset();
	}

	public void GetPossibleResolutions(List<WD_Resolution> resolutionBuffer)
	{
		this.userVideoSettingsModel.GetPossibleResolutions(resolutionBuffer);
	}
}
