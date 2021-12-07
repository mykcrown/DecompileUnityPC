// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ExitGame : IExitGame
{
	private sealed class _ExitGameMode_c__AnonStorey0
	{
		internal Action callback;

		internal ExitGame _this;

		internal void __m__0()
		{
			this._this.unloadAssets();
			this._this.signalBus.Dispatch(ExitGame.GAME_ENDED);
			this.callback();
		}
	}

	public static string GAME_ENDED = "ExitGame.GAME_ENDED";

	private bool inProgress;

	[Inject]
	public GameController gameController
	{
		private get;
		set;
	}

	[Inject]
	public ISceneController sceneController
	{
		private get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		private get;
		set;
	}

	[Inject]
	public IBakedAnimationDataManager bakedAnimationDataManager
	{
		get;
		set;
	}

	[Inject]
	public UIPreload3DAssets preload3dAssets
	{
		get;
		set;
	}

	public void InstantTerminate()
	{
		if (this.inProgress)
		{
			return;
		}
		this.inProgress = true;
		this.cancelStageLoad(new Action(this._InstantTerminate_m__0));
	}

	public void DestroyGameManager()
	{
		if (this.gameController.currentGame != null)
		{
			UnityEngine.Object.Destroy(this.gameController.currentGame.gameObject);
			this.gameController.SetCurrentGame(null);
			this.gameController.EndPreload();
		}
	}

	public void ExitGameMode(Action callback)
	{
		ExitGame._ExitGameMode_c__AnonStorey0 _ExitGameMode_c__AnonStorey = new ExitGame._ExitGameMode_c__AnonStorey0();
		_ExitGameMode_c__AnonStorey.callback = callback;
		_ExitGameMode_c__AnonStorey._this = this;
		this.sceneController.ExitBattle(new Action(_ExitGameMode_c__AnonStorey.__m__0));
	}

	private void unloadAssets()
	{
	}

	private void cancelStageLoad(Action callback)
	{
		StageLoader stageLoader = UnityEngine.Object.FindObjectOfType<StageLoader>();
		if (stageLoader == null)
		{
			callback();
		}
		else
		{
			stageLoader.TerminateGame(callback);
		}
	}

	private void _InstantTerminate_m__0()
	{
		this.DestroyGameManager();
		this.ExitGameMode(new Action(this._InstantTerminate_m__1));
	}

	private void _InstantTerminate_m__1()
	{
		this.inProgress = false;
	}
}
