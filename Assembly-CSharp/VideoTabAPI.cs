using System;
using System.Collections.Generic;
using IconsServer;

// Token: 0x0200098B RID: 2443
public class VideoTabAPI : IVideoTabAPI
{
	// Token: 0x17000F96 RID: 3990
	// (get) Token: 0x06004240 RID: 16960 RVA: 0x00127FB3 File Offset: 0x001263B3
	// (set) Token: 0x06004241 RID: 16961 RVA: 0x00127FBB File Offset: 0x001263BB
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel { get; set; }

	// Token: 0x17000F97 RID: 3991
	// (get) Token: 0x06004242 RID: 16962 RVA: 0x00127FC4 File Offset: 0x001263C4
	// (set) Token: 0x06004243 RID: 16963 RVA: 0x00127FCC File Offset: 0x001263CC
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x06004244 RID: 16964 RVA: 0x00127FD5 File Offset: 0x001263D5
	public void Reset()
	{
		this.userVideoSettingsModel.Reset();
	}

	// Token: 0x06004245 RID: 16965 RVA: 0x00127FE2 File Offset: 0x001263E2
	public void GetPossibleResolutions(List<WD_Resolution> resolutionBuffer)
	{
		this.userVideoSettingsModel.GetPossibleResolutions(resolutionBuffer);
	}

	// Token: 0x17000F98 RID: 3992
	// (get) Token: 0x06004246 RID: 16966 RVA: 0x00127FF0 File Offset: 0x001263F0
	// (set) Token: 0x06004247 RID: 16967 RVA: 0x00127FFD File Offset: 0x001263FD
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

	// Token: 0x17000F99 RID: 3993
	// (get) Token: 0x06004248 RID: 16968 RVA: 0x00128016 File Offset: 0x00126416
	// (set) Token: 0x06004249 RID: 16969 RVA: 0x00128023 File Offset: 0x00126423
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

	// Token: 0x17000F9A RID: 3994
	// (get) Token: 0x0600424A RID: 16970 RVA: 0x00128031 File Offset: 0x00126431
	// (set) Token: 0x0600424B RID: 16971 RVA: 0x0012803E File Offset: 0x0012643E
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

	// Token: 0x17000F9B RID: 3995
	// (get) Token: 0x0600424C RID: 16972 RVA: 0x0012804C File Offset: 0x0012644C
	// (set) Token: 0x0600424D RID: 16973 RVA: 0x00128059 File Offset: 0x00126459
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

	// Token: 0x17000F9C RID: 3996
	// (get) Token: 0x0600424E RID: 16974 RVA: 0x00128067 File Offset: 0x00126467
	// (set) Token: 0x0600424F RID: 16975 RVA: 0x00128074 File Offset: 0x00126474
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

	// Token: 0x17000F9D RID: 3997
	// (get) Token: 0x06004250 RID: 16976 RVA: 0x00128082 File Offset: 0x00126482
	// (set) Token: 0x06004251 RID: 16977 RVA: 0x0012808F File Offset: 0x0012648F
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

	// Token: 0x17000F9E RID: 3998
	// (get) Token: 0x06004252 RID: 16978 RVA: 0x0012809D File Offset: 0x0012649D
	// (set) Token: 0x06004253 RID: 16979 RVA: 0x001280AA File Offset: 0x001264AA
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

	// Token: 0x17000F9F RID: 3999
	// (get) Token: 0x06004254 RID: 16980 RVA: 0x001280B8 File Offset: 0x001264B8
	// (set) Token: 0x06004255 RID: 16981 RVA: 0x001280C5 File Offset: 0x001264C5
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

	// Token: 0x17000FA0 RID: 4000
	// (get) Token: 0x06004256 RID: 16982 RVA: 0x001280D3 File Offset: 0x001264D3
	// (set) Token: 0x06004257 RID: 16983 RVA: 0x001280E0 File Offset: 0x001264E0
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

	// Token: 0x17000FA1 RID: 4001
	// (get) Token: 0x06004258 RID: 16984 RVA: 0x001280EE File Offset: 0x001264EE
	// (set) Token: 0x06004259 RID: 16985 RVA: 0x001280FB File Offset: 0x001264FB
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

	// Token: 0x17000FA2 RID: 4002
	// (get) Token: 0x0600425A RID: 16986 RVA: 0x00128109 File Offset: 0x00126509
	// (set) Token: 0x0600425B RID: 16987 RVA: 0x00128116 File Offset: 0x00126516
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

	// Token: 0x17000FA3 RID: 4003
	// (get) Token: 0x0600425C RID: 16988 RVA: 0x00128124 File Offset: 0x00126524
	// (set) Token: 0x0600425D RID: 16989 RVA: 0x00128131 File Offset: 0x00126531
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

	// Token: 0x17000FA4 RID: 4004
	// (get) Token: 0x0600425E RID: 16990 RVA: 0x0012813F File Offset: 0x0012653F
	// (set) Token: 0x0600425F RID: 16991 RVA: 0x0012814C File Offset: 0x0012654C
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

	// Token: 0x17000FA5 RID: 4005
	// (get) Token: 0x06004260 RID: 16992 RVA: 0x0012815A File Offset: 0x0012655A
	// (set) Token: 0x06004261 RID: 16993 RVA: 0x00128167 File Offset: 0x00126567
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

	// Token: 0x17000FA6 RID: 4006
	// (get) Token: 0x06004262 RID: 16994 RVA: 0x00128175 File Offset: 0x00126575
	// (set) Token: 0x06004263 RID: 16995 RVA: 0x00128182 File Offset: 0x00126582
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
}
