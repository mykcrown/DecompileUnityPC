using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200097A RID: 2426
public class GameplayTab : SettingsTabElement
{
	// Token: 0x17000F54 RID: 3924
	// (get) Token: 0x06004115 RID: 16661 RVA: 0x001251D7 File Offset: 0x001235D7
	// (set) Token: 0x06004116 RID: 16662 RVA: 0x001251DF File Offset: 0x001235DF
	[Inject]
	public IGameplayTabAPI api { get; set; }

	// Token: 0x17000F55 RID: 3925
	// (get) Token: 0x06004117 RID: 16663 RVA: 0x001251E8 File Offset: 0x001235E8
	// (set) Token: 0x06004118 RID: 16664 RVA: 0x001251F0 File Offset: 0x001235F0
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x06004119 RID: 16665 RVA: 0x001251FC File Offset: 0x001235FC
	public override void Awake()
	{
		base.Awake();
		if (this.api == null)
		{
			return;
		}
		this.buttonList = base.injector.GetInstance<MenuItemList>();
		this.muteEnemyHolosToggle = this.addOption(this.Column1.transform, base.localization.GetText("ui.gameplay.muteEnemyHolos"), new Action(this.toggleMuteEnemyHolos));
		this.muteEnemyVoicelinesToggle = this.addOption(this.Column2.transform, base.localization.GetText("ui.gameplay.muteEnemyVoicelines"), new Action(this.toggleMuteEnemyVoicelines));
		int childCount = this.Column1.transform.childCount;
		this.buttonList.SetNavigationType(MenuItemList.NavigationType.GridVerticalFill, childCount);
		this.buttonList.DisableGridWrap();
		this.buttonList.Initialize();
		base.listen(UserGameplaySettingsModel.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	// Token: 0x0600411A RID: 16666 RVA: 0x001252E4 File Offset: 0x001236E4
	private VideoOptionToggle addOption(Transform parent, string text, Action callback)
	{
		VideoOptionToggle component = UnityEngine.Object.Instantiate<VideoOptionToggle>(this.TogglePrefab).GetComponent<VideoOptionToggle>();
		component.transform.SetParent(parent);
		component.Title.text = text;
		this.buttonList.AddButton(component.Toggle.Button, callback);
		return component;
	}

	// Token: 0x0600411B RID: 16667 RVA: 0x00125334 File Offset: 0x00123734
	private void onUpdate()
	{
		if (this.muteEnemyHolosToggle != null)
		{
			this.muteEnemyHolosToggle.Toggle.SetToggle(this.api.MuteEnemyHolos);
			this.muteEnemyVoicelinesToggle.Toggle.SetToggle(this.api.MuteEnemyVoicelines);
		}
	}

	// Token: 0x0600411C RID: 16668 RVA: 0x00125388 File Offset: 0x00123788
	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.syncButtonNavigation();
	}

	// Token: 0x0600411D RID: 16669 RVA: 0x00125398 File Offset: 0x00123798
	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && base.allowInteraction() && this.windowDisplay.GetWindowCount() == 0 && this.buttonList.CurrentSelection == null)
		{
			this.buttonList.AutoSelect(this.buttonList.GetButtons()[0]);
		}
	}

	// Token: 0x0600411E RID: 16670 RVA: 0x00125408 File Offset: 0x00123808
	private void toggleMuteEnemyHolos()
	{
		this.api.MuteEnemyHolos = !this.api.MuteEnemyHolos;
	}

	// Token: 0x0600411F RID: 16671 RVA: 0x00125423 File Offset: 0x00123823
	private void toggleMuteEnemyVoicelines()
	{
		this.api.MuteEnemyVoicelines = !this.api.MuteEnemyVoicelines;
	}

	// Token: 0x06004120 RID: 16672 RVA: 0x0012543E File Offset: 0x0012383E
	public override void ResetClicked(InputEventData eventData)
	{
		base.ResetClicked(eventData);
		this.attemptReset();
	}

	// Token: 0x06004121 RID: 16673 RVA: 0x00125450 File Offset: 0x00123850
	private void attemptReset()
	{
		GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.resetWarning.title"), base.localization.GetText("ui.settings.resetWarning.body"), base.localization.GetText("ui.settings.resetWarning.confirm"), base.localization.GetText("ui.settings.resetWarning.cancel"));
		GenericDialog genericDialog2 = genericDialog;
		genericDialog2.ConfirmCallback = (Action)Delegate.Combine(genericDialog2.ConfirmCallback, new Action(delegate()
		{
			this.api.Reset();
			base.audioManager.PlayMenuSound(SoundKey.settings_settingsReset, 0f);
		}));
	}

	// Token: 0x06004122 RID: 16674 RVA: 0x001254CB File Offset: 0x001238CB
	public override void OnYButtonPressed()
	{
		base.OnYButtonPressed();
		this.attemptReset();
	}

	// Token: 0x04002BF5 RID: 11253
	public VerticalLayoutGroup Column1;

	// Token: 0x04002BF6 RID: 11254
	public VerticalLayoutGroup Column2;

	// Token: 0x04002BF7 RID: 11255
	public VideoOptionToggle TogglePrefab;

	// Token: 0x04002BF8 RID: 11256
	private MenuItemList buttonList;

	// Token: 0x04002BF9 RID: 11257
	private VideoOptionToggle muteEnemyHolosToggle;

	// Token: 0x04002BFA RID: 11258
	private VideoOptionToggle muteEnemyVoicelinesToggle;
}
