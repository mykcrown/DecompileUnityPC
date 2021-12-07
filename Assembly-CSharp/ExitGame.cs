using System;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class ExitGame : IExitGame
{
	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x060018F1 RID: 6385 RVA: 0x00083364 File Offset: 0x00081764
	// (set) Token: 0x060018F2 RID: 6386 RVA: 0x0008336C File Offset: 0x0008176C
	[Inject]
	public GameController gameController { private get; set; }

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x060018F3 RID: 6387 RVA: 0x00083375 File Offset: 0x00081775
	// (set) Token: 0x060018F4 RID: 6388 RVA: 0x0008337D File Offset: 0x0008177D
	[Inject]
	public ISceneController sceneController { private get; set; }

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x060018F5 RID: 6389 RVA: 0x00083386 File Offset: 0x00081786
	// (set) Token: 0x060018F6 RID: 6390 RVA: 0x0008338E File Offset: 0x0008178E
	[Inject]
	public ISignalBus signalBus { private get; set; }

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x060018F7 RID: 6391 RVA: 0x00083397 File Offset: 0x00081797
	// (set) Token: 0x060018F8 RID: 6392 RVA: 0x0008339F File Offset: 0x0008179F
	[Inject]
	public IBakedAnimationDataManager bakedAnimationDataManager { get; set; }

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x060018F9 RID: 6393 RVA: 0x000833A8 File Offset: 0x000817A8
	// (set) Token: 0x060018FA RID: 6394 RVA: 0x000833B0 File Offset: 0x000817B0
	[Inject]
	public UIPreload3DAssets preload3dAssets { get; set; }

	// Token: 0x060018FB RID: 6395 RVA: 0x000833B9 File Offset: 0x000817B9
	public void InstantTerminate()
	{
		if (this.inProgress)
		{
			return;
		}
		this.inProgress = true;
		this.cancelStageLoad(delegate
		{
			this.DestroyGameManager();
			this.ExitGameMode(delegate
			{
				this.inProgress = false;
			});
		});
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x000833E0 File Offset: 0x000817E0
	public void DestroyGameManager()
	{
		if (this.gameController.currentGame != null)
		{
			UnityEngine.Object.Destroy(this.gameController.currentGame.gameObject);
			this.gameController.SetCurrentGame(null);
			this.gameController.EndPreload();
		}
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x00083430 File Offset: 0x00081830
	public void ExitGameMode(Action callback)
	{
		this.sceneController.ExitBattle(delegate
		{
			this.unloadAssets();
			this.signalBus.Dispatch(ExitGame.GAME_ENDED);
			callback();
		});
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x00083468 File Offset: 0x00081868
	private void unloadAssets()
	{
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x0008346C File Offset: 0x0008186C
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

	// Token: 0x040012E9 RID: 4841
	public static string GAME_ENDED = "ExitGame.GAME_ENDED";

	// Token: 0x040012EF RID: 4847
	private bool inProgress;
}
