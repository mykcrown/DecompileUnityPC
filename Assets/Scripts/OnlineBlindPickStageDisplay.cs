// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnlineBlindPickStageDisplay : MonoBehaviour
{
	public Image StagePortrait;

	public TextMeshProUGUI WithWorldIconTitle;

	public TextMeshProUGUI WithoutWorldIconTitle;

	public Image Icon;

	public GameObject WithWorldIcon;

	public GameObject WithoutWorldIcon;

	public GameObject DarkOverlay;

	public GameObject StrikeIcon;

	private OnlineBlindPickStageDisplayMode mode;

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialogController
	{
		get;
		set;
	}

	public void Init(StageData stageData)
	{
		this.StagePortrait.overrideSprite = stageData.smallPortrait;
		string text = this.localization.GetText("gameData.stageSelect." + stageData.name);
		if (stageData.isTemporary)
		{
			text = text + " <#e01c1c>" + this.localization.GetText("gameData.stageSelect.temp") + "</color>";
		}
		if (stageData.worldIcon == null)
		{
			this.WithWorldIcon.SetActive(false);
			this.WithoutWorldIcon.SetActive(true);
			this.WithoutWorldIconTitle.text = text;
		}
		else
		{
			this.WithWorldIcon.SetActive(true);
			this.WithoutWorldIcon.SetActive(false);
			this.WithWorldIconTitle.text = text;
			this.Icon.sprite = stageData.worldIcon;
		}
	}

	public void SetMode(OnlineBlindPickStageDisplayMode mode)
	{
		if (this.mode != mode)
		{
			this.mode = mode;
			this.DarkOverlay.gameObject.SetActive(false);
			this.StrikeIcon.gameObject.SetActive(false);
			if (mode != OnlineBlindPickStageDisplayMode.Current)
			{
				if (mode != OnlineBlindPickStageDisplayMode.Played)
				{
					if (mode == OnlineBlindPickStageDisplayMode.Upcoming)
					{
						this.DarkOverlay.gameObject.SetActive(true);
					}
				}
				else
				{
					this.DarkOverlay.gameObject.SetActive(true);
					this.StrikeIcon.gameObject.SetActive(true);
				}
			}
		}
	}
}
