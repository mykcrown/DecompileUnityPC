// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadBattleStageMode : ClientBehavior
{
	public TextMeshProUGUI StageName;

	public Image StageIcon;

	public TextMeshProUGUI StageDesc;

	public Image LoadBar;

	public Image StageBackground;

	public GameObject TweenInTop;

	public GameObject TweenInBottom;

	public GameObject TweenTopAnchor;

	public GameObject TweenBottomAnchor;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	public void Load(StageData stage)
	{
		this.StageName.text = this.localization.GetText("gameData.stageSelect." + stage.stageName);
		string text = "gameData.stageLoading." + stage.stageName;
		if (stage.loadingTips.Length > 0)
		{
			int num = 0;
			StageData.LoadingTipWeightData[] loadingTips = stage.loadingTips;
			for (int i = 0; i < loadingTips.Length; i++)
			{
				StageData.LoadingTipWeightData loadingTipWeightData = loadingTips[i];
				num += loadingTipWeightData.weight;
			}
			int num2 = UnityEngine.Random.Range(0, num);
			StageData.LoadingTipWeightData[] loadingTips2 = stage.loadingTips;
			for (int j = 0; j < loadingTips2.Length; j++)
			{
				StageData.LoadingTipWeightData loadingTipWeightData2 = loadingTips2[j];
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

	public void Start()
	{
	}

	private void tweenTop()
	{
		Vector3 position = this.TweenInTop.transform.position;
		this.TweenInTop.transform.position = this.TweenTopAnchor.transform.position;
		DOTween.To(new DOGetter<Vector3>(this._tweenTop_m__0), new DOSetter<Vector3>(this._tweenTop_m__1), position, 0.25f).SetEase(Ease.OutSine);
	}

	private void tweenBottom()
	{
		Vector3 position = this.TweenInBottom.transform.position;
		this.TweenInBottom.transform.position = this.TweenBottomAnchor.transform.position;
		DOTween.To(new DOGetter<Vector3>(this._tweenBottom_m__2), new DOSetter<Vector3>(this._tweenBottom_m__3), position, 0.4f).SetEase(Ease.OutSine);
	}

	private Vector3 _tweenTop_m__0()
	{
		return this.TweenInTop.transform.position;
	}

	private void _tweenTop_m__1(Vector3 valueIn)
	{
		this.TweenInTop.transform.position = valueIn;
	}

	private Vector3 _tweenBottom_m__2()
	{
		return this.TweenInBottom.transform.position;
	}

	private void _tweenBottom_m__3(Vector3 valueIn)
	{
		this.TweenInBottom.transform.position = valueIn;
	}
}
