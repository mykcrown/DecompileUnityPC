// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelectItem : MonoBehaviour
{
	public Image StagePortrait;

	public GameObject StruckOverlay;

	public GameObject VisualContainer;

	public GameObject HighlightImage;

	public TextMeshProUGUI WithWorldIconTitle;

	public TextMeshProUGUI WithoutWorldIconTitle;

	public Image Icon;

	public CursorTargetButton buttonInteract;

	public GameObject WithWorldIcon;

	public GameObject WithoutWorldIcon;

	private StageState state;

	private StageData stageData;

	private bool _canBeInteractedWith = true;

	private bool forcedActive;

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
		this.stageData = stageData;
		this.buttonInteract.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClicked);
		this.buttonInteract.AltSubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onStrikeButton);
		this.buttonInteract.SelectCallback = new Action<CursorTargetButton, PointerEventData>(this.onSelect);
		this.buttonInteract.DeselectCallback = new Action<CursorTargetButton, PointerEventData>(this.onDeselect);
		this.buttonInteract.FaceButton3Callback = new Action<CursorTargetButton, PointerEventData>(this.onStrikeButton);
		this.StagePortrait.overrideSprite = stageData.smallPortrait;
		string text = this.localization.GetText("gameData.stageSelect." + stageData.stageName);
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
		this.SetStageState(this.state, true);
	}

	public void SetInteractability(bool canBeInteracted)
	{
		this._canBeInteractedWith = canBeInteracted;
	}

	private void onStrikeButton(CursorTargetButton target, PointerEventData eventData)
	{
		if (!this._canBeInteractedWith)
		{
			return;
		}
		if (this.stageData.stageType == StageType.Random)
		{
			return;
		}
		if (this.state != StageState.Struck)
		{
			this.setStrike(true);
		}
		else if (this.state == StageState.Struck)
		{
			this.setStrike(false);
		}
	}

	private void setStrike(bool struck)
	{
		this.events.Broadcast(new SetStageStateRequest(this.stageData.stageID, (!struck) ? StageState.None : StageState.Struck));
		this.audioManager.PlayMenuSound((!struck) ? SoundKey.stageSelect_stageUnstrike : SoundKey.stageSelect_stageStrike, 0f);
	}

	public void SetStageState(StageState newState, bool firstTime = false)
	{
		if (this.state == newState && !firstTime)
		{
			return;
		}
		this.state = newState;
		this.updateOverlayIcon();
	}

	private void onClicked(CursorTargetButton target, PointerEventData eventData)
	{
		if (!this._canBeInteractedWith)
		{
			return;
		}
		if (this.state == StageState.Struck)
		{
			return;
		}
		if (this.stageData.stageID == StageID.None)
		{
			this.dialogController.ShowOneButtonDialog("MISSING ID", "No valid StageID found for stage " + this.stageData.stageID, "OK", WindowTransition.STANDARD_FADE, false, default(AudioData));
		}
		this.events.Broadcast(new SelectStageRequest(this.stageData.stageID, true));
	}

	private void updateOverlayIcon()
	{
		if (this.state == StageState.Struck)
		{
			this.StruckOverlay.SetActive(true);
		}
		else
		{
			this.StruckOverlay.SetActive(false);
		}
	}

	private void onSelect(CursorTargetButton target, PointerEventData eventData)
	{
		if (!this._canBeInteractedWith)
		{
			return;
		}
		this.events.Broadcast(new SelectStageRequest(this.stageData.stageID, false));
		this.HighlightImage.SetActive(true);
	}

	private void onDeselect(CursorTargetButton target, PointerEventData eventData)
	{
		if (!this._canBeInteractedWith)
		{
			return;
		}
		if (!this.forcedActive)
		{
			this.HighlightImage.SetActive(false);
		}
	}

	public void SetHighlightImageActive(bool isActive)
	{
		this.forcedActive = true;
		this.HighlightImage.SetActive(isActive);
	}

	public void SetAlpha(float alpha)
	{
		this.StagePortrait.CrossFadeAlpha(alpha, 0f, false);
		this.WithWorldIconTitle.CrossFadeAlpha(alpha, 0f, false);
		this.WithoutWorldIconTitle.CrossFadeAlpha(alpha, 0f, false);
	}
}
