using System;
using UnityEngine;

// Token: 0x0200029B RID: 667
public class ApplicationFramerateManager : IApplicationFramerateManager
{
	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06000E0C RID: 3596 RVA: 0x00058964 File Offset: 0x00056D64
	// (set) Token: 0x06000E0D RID: 3597 RVA: 0x0005896C File Offset: 0x00056D6C
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06000E0E RID: 3598 RVA: 0x00058975 File Offset: 0x00056D75
	// (set) Token: 0x06000E0F RID: 3599 RVA: 0x0005897D File Offset: 0x00056D7D
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel { get; set; }

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06000E10 RID: 3600 RVA: 0x00058986 File Offset: 0x00056D86
	// (set) Token: 0x06000E11 RID: 3601 RVA: 0x0005898E File Offset: 0x00056D8E
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06000E12 RID: 3602 RVA: 0x00058997 File Offset: 0x00056D97
	// (set) Token: 0x06000E13 RID: 3603 RVA: 0x0005899F File Offset: 0x00056D9F
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x06000E14 RID: 3604 RVA: 0x000589A8 File Offset: 0x00056DA8
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserVideoSettingsModel.UPDATED, new Action(this.sync));
		this.signalBus.AddListener(StageLoader.GAME_BEGINNING, new Action(this.sync));
		this.signalBus.AddListener(ExitGame.GAME_ENDED, new Action(this.sync));
		this._frameSyncMode = this.config.frameSyncMode;
		this.sync();
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06000E15 RID: 3605 RVA: 0x00058A20 File Offset: 0x00056E20
	// (set) Token: 0x06000E16 RID: 3606 RVA: 0x00058A28 File Offset: 0x00056E28
	public FrameSyncMode frameSyncMode
	{
		get
		{
			return this._frameSyncMode;
		}
		set
		{
			this._frameSyncMode = value;
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06000E17 RID: 3607 RVA: 0x00058A31 File Offset: 0x00056E31
	// (set) Token: 0x06000E18 RID: 3608 RVA: 0x00058A39 File Offset: 0x00056E39
	public int overrideTargetFramerate
	{
		get
		{
			return this._overrideTargetFramerate;
		}
		set
		{
			this._overrideTargetFramerate = value;
			this.sync();
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06000E19 RID: 3609 RVA: 0x00058A48 File Offset: 0x00056E48
	public bool UseRenderWait
	{
		get
		{
			return this.frameSyncMode == FrameSyncMode.RENDER_WAIT && !this.userVideoSettingsModel.Vsync;
		}
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x00058A68 File Offset: 0x00056E68
	private void sync()
	{
		QualitySettings.vSyncCount = ((!this.userVideoSettingsModel.Vsync) ? 0 : 1);
		QualitySettings.maxQueuedFrames = 1;
		if (this._overrideTargetFramerate != 0)
		{
			Application.targetFrameRate = this._overrideTargetFramerate;
		}
		else if (this.frameSyncMode == FrameSyncMode.RENDER_WAIT || this.frameSyncMode == FrameSyncMode.UNITY_FREE)
		{
			Application.targetFrameRate = 60;
		}
		else if (this.gameController.currentGame != null)
		{
			Application.targetFrameRate = 75;
		}
		else
		{
			Application.targetFrameRate = 144;
		}
	}

	// Token: 0x04000846 RID: 2118
	private FrameSyncMode _frameSyncMode;

	// Token: 0x04000847 RID: 2119
	private int _overrideTargetFramerate;
}
