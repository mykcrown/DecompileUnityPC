using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020009D0 RID: 2512
public class StageSelectItem : MonoBehaviour
{
	// Token: 0x170010DB RID: 4315
	// (get) Token: 0x06004645 RID: 17989 RVA: 0x00132AE4 File Offset: 0x00130EE4
	// (set) Token: 0x06004646 RID: 17990 RVA: 0x00132AEC File Offset: 0x00130EEC
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170010DC RID: 4316
	// (get) Token: 0x06004647 RID: 17991 RVA: 0x00132AF5 File Offset: 0x00130EF5
	// (set) Token: 0x06004648 RID: 17992 RVA: 0x00132AFD File Offset: 0x00130EFD
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x170010DD RID: 4317
	// (get) Token: 0x06004649 RID: 17993 RVA: 0x00132B06 File Offset: 0x00130F06
	// (set) Token: 0x0600464A RID: 17994 RVA: 0x00132B0E File Offset: 0x00130F0E
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x170010DE RID: 4318
	// (get) Token: 0x0600464B RID: 17995 RVA: 0x00132B17 File Offset: 0x00130F17
	// (set) Token: 0x0600464C RID: 17996 RVA: 0x00132B1F File Offset: 0x00130F1F
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x0600464D RID: 17997 RVA: 0x00132B28 File Offset: 0x00130F28
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

	// Token: 0x0600464E RID: 17998 RVA: 0x00132C84 File Offset: 0x00131084
	public void SetInteractability(bool canBeInteracted)
	{
		this._canBeInteractedWith = canBeInteracted;
	}

	// Token: 0x0600464F RID: 17999 RVA: 0x00132C90 File Offset: 0x00131090
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

	// Token: 0x06004650 RID: 18000 RVA: 0x00132CE8 File Offset: 0x001310E8
	private void setStrike(bool struck)
	{
		this.events.Broadcast(new SetStageStateRequest(this.stageData.stageID, (!struck) ? StageState.None : StageState.Struck));
		this.audioManager.PlayMenuSound((!struck) ? SoundKey.stageSelect_stageUnstrike : SoundKey.stageSelect_stageStrike, 0f);
	}

	// Token: 0x06004651 RID: 18001 RVA: 0x00132D3C File Offset: 0x0013113C
	public void SetStageState(StageState newState, bool firstTime = false)
	{
		if (this.state == newState && !firstTime)
		{
			return;
		}
		this.state = newState;
		this.updateOverlayIcon();
	}

	// Token: 0x06004652 RID: 18002 RVA: 0x00132D60 File Offset: 0x00131160
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

	// Token: 0x06004653 RID: 18003 RVA: 0x00132DED File Offset: 0x001311ED
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

	// Token: 0x06004654 RID: 18004 RVA: 0x00132E18 File Offset: 0x00131218
	private void onSelect(CursorTargetButton target, PointerEventData eventData)
	{
		if (!this._canBeInteractedWith)
		{
			return;
		}
		this.events.Broadcast(new SelectStageRequest(this.stageData.stageID, false));
		this.HighlightImage.SetActive(true);
	}

	// Token: 0x06004655 RID: 18005 RVA: 0x00132E4E File Offset: 0x0013124E
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

	// Token: 0x06004656 RID: 18006 RVA: 0x00132E73 File Offset: 0x00131273
	public void SetHighlightImageActive(bool isActive)
	{
		this.forcedActive = true;
		this.HighlightImage.SetActive(isActive);
	}

	// Token: 0x06004657 RID: 18007 RVA: 0x00132E88 File Offset: 0x00131288
	public void SetAlpha(float alpha)
	{
		this.StagePortrait.CrossFadeAlpha(alpha, 0f, false);
		this.WithWorldIconTitle.CrossFadeAlpha(alpha, 0f, false);
		this.WithoutWorldIconTitle.CrossFadeAlpha(alpha, 0f, false);
	}

	// Token: 0x04002E8C RID: 11916
	public Image StagePortrait;

	// Token: 0x04002E8D RID: 11917
	public GameObject StruckOverlay;

	// Token: 0x04002E8E RID: 11918
	public GameObject VisualContainer;

	// Token: 0x04002E8F RID: 11919
	public GameObject HighlightImage;

	// Token: 0x04002E90 RID: 11920
	public TextMeshProUGUI WithWorldIconTitle;

	// Token: 0x04002E91 RID: 11921
	public TextMeshProUGUI WithoutWorldIconTitle;

	// Token: 0x04002E92 RID: 11922
	public Image Icon;

	// Token: 0x04002E93 RID: 11923
	public CursorTargetButton buttonInteract;

	// Token: 0x04002E94 RID: 11924
	public GameObject WithWorldIcon;

	// Token: 0x04002E95 RID: 11925
	public GameObject WithoutWorldIcon;

	// Token: 0x04002E96 RID: 11926
	private StageState state;

	// Token: 0x04002E97 RID: 11927
	private StageData stageData;

	// Token: 0x04002E98 RID: 11928
	private bool _canBeInteractedWith = true;

	// Token: 0x04002E99 RID: 11929
	private bool forcedActive;
}
