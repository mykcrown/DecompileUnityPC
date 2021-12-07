// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class ApplicationFramerateManager : IApplicationFramerateManager
{
	private FrameSyncMode _frameSyncMode;

	private int _overrideTargetFramerate;

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel
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

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

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

	public bool UseRenderWait
	{
		get
		{
			return this.frameSyncMode == FrameSyncMode.RENDER_WAIT && !this.userVideoSettingsModel.Vsync;
		}
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserVideoSettingsModel.UPDATED, new Action(this.sync));
		this.signalBus.AddListener(StageLoader.GAME_BEGINNING, new Action(this.sync));
		this.signalBus.AddListener(ExitGame.GAME_ENDED, new Action(this.sync));
		this._frameSyncMode = this.config.frameSyncMode;
		this.sync();
	}

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
}
