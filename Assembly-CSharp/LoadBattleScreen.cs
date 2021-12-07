using System;
using UnityEngine;

// Token: 0x0200098E RID: 2446
public class LoadBattleScreen : GameScreen
{
	// Token: 0x17000FB6 RID: 4022
	// (get) Token: 0x06004286 RID: 17030 RVA: 0x00128198 File Offset: 0x00126598
	// (set) Token: 0x06004287 RID: 17031 RVA: 0x001281A0 File Offset: 0x001265A0
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x17000FB7 RID: 4023
	// (get) Token: 0x06004288 RID: 17032 RVA: 0x001281A9 File Offset: 0x001265A9
	public override bool AlwaysHideMouse
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004289 RID: 17033 RVA: 0x001281AC File Offset: 0x001265AC
	public override void LoadPayload(Payload payload)
	{
		base.LoadPayload(payload);
		this.loader = new GameObject("StageLoader").AddComponent<StageLoader>();
		base.injector.Inject(this.loader);
		this.loader.Initialize(base.events, this.enterNewGame.GamePayload);
		this.useStageMode(this.loader.stage);
	}

	// Token: 0x0600428A RID: 17034 RVA: 0x00128213 File Offset: 0x00126613
	private void useStageMode(StageData stage)
	{
		this.StageMode.gameObject.SetActive(true);
		this.StageMode.Load(stage);
	}

	// Token: 0x04002C71 RID: 11377
	public LoadBattleStageMode StageMode;

	// Token: 0x04002C72 RID: 11378
	private StageLoader loader;
}
