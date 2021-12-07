using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200098F RID: 2447
public class LoadBattleStageMode : ClientBehavior
{
	// Token: 0x17000FB8 RID: 4024
	// (get) Token: 0x0600428C RID: 17036 RVA: 0x0012823A File Offset: 0x0012663A
	// (set) Token: 0x0600428D RID: 17037 RVA: 0x00128242 File Offset: 0x00126642
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x0600428E RID: 17038 RVA: 0x0012824C File Offset: 0x0012664C
	public void Load(StageData stage)
	{
		this.StageName.text = this.localization.GetText("gameData.stageSelect." + stage.stageName);
		string text = "gameData.stageLoading." + stage.stageName;
		if (stage.loadingTips.Length > 0)
		{
			int num = 0;
			foreach (StageData.LoadingTipWeightData loadingTipWeightData in stage.loadingTips)
			{
				num += loadingTipWeightData.weight;
			}
			int num2 = UnityEngine.Random.Range(0, num);
			foreach (StageData.LoadingTipWeightData loadingTipWeightData2 in stage.loadingTips)
			{
				if (num2 < loadingTipWeightData2.weight)
				{
					text = loadingTipWeightData2.loadingTip;
					break;
				}
				num2 -= loadingTipWeightData2.weight;
			}
		}
		if (text == "gameData.stageLoading.Generic7" && DateTime.UtcNow < new DateTime(2019, 10, 31))
		{
			text = "gameData.stageLoading.Generic6";
		}
		string text2 = this.localization.GetText(text);
		if (text2 != null)
		{
			text2 = text2.ToUpper();
		}
		this.StageDesc.text = text2;
		this.StageIcon.sprite = stage.worldIcon;
		this.StageBackground.sprite = stage.loadingScreen;
	}

	// Token: 0x0600428F RID: 17039 RVA: 0x001283A5 File Offset: 0x001267A5
	public void Start()
	{
	}

	// Token: 0x06004290 RID: 17040 RVA: 0x001283A8 File Offset: 0x001267A8
	private void tweenTop()
	{
		Vector3 position = this.TweenInTop.transform.position;
		this.TweenInTop.transform.position = this.TweenTopAnchor.transform.position;
		DOTween.To(() => this.TweenInTop.transform.position, delegate(Vector3 valueIn)
		{
			this.TweenInTop.transform.position = valueIn;
		}, position, 0.25f).SetEase(Ease.OutSine);
	}

	// Token: 0x06004291 RID: 17041 RVA: 0x00128410 File Offset: 0x00126810
	private void tweenBottom()
	{
		Vector3 position = this.TweenInBottom.transform.position;
		this.TweenInBottom.transform.position = this.TweenBottomAnchor.transform.position;
		DOTween.To(() => this.TweenInBottom.transform.position, delegate(Vector3 valueIn)
		{
			this.TweenInBottom.transform.position = valueIn;
		}, position, 0.4f).SetEase(Ease.OutSine);
	}

	// Token: 0x04002C74 RID: 11380
	public TextMeshProUGUI StageName;

	// Token: 0x04002C75 RID: 11381
	public Image StageIcon;

	// Token: 0x04002C76 RID: 11382
	public TextMeshProUGUI StageDesc;

	// Token: 0x04002C77 RID: 11383
	public Image LoadBar;

	// Token: 0x04002C78 RID: 11384
	public Image StageBackground;

	// Token: 0x04002C79 RID: 11385
	public GameObject TweenInTop;

	// Token: 0x04002C7A RID: 11386
	public GameObject TweenInBottom;

	// Token: 0x04002C7B RID: 11387
	public GameObject TweenTopAnchor;

	// Token: 0x04002C7C RID: 11388
	public GameObject TweenBottomAnchor;
}
