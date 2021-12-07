// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class LoadBattleScreen : GameScreen
{
	public LoadBattleStageMode StageMode;

	private StageLoader loader;

	[Inject]
	public IEnterNewGame enterNewGame
	{
		get;
		set;
	}

	public override bool AlwaysHideMouse
	{
		get
		{
			return true;
		}
	}

	public override void LoadPayload(Payload payload)
	{
		base.LoadPayload(payload);
		this.loader = new GameObject("StageLoader").AddComponent<StageLoader>();
		base.injector.Inject(this.loader);
		this.loader.Initialize(base.events, this.enterNewGame.GamePayload);
		this.useStageMode(this.loader.stage);
	}

	private void useStageMode(StageData stage)
	{
		this.StageMode.gameObject.SetActive(true);
		this.StageMode.Load(stage);
	}
}
