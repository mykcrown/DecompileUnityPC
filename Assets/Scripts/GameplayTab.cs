// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameplayTab : SettingsTabElement
{
	public VerticalLayoutGroup Column1;

	public VerticalLayoutGroup Column2;

	public VideoOptionToggle TogglePrefab;

	private MenuItemList buttonList;

	private VideoOptionToggle muteEnemyHolosToggle;

	private VideoOptionToggle muteEnemyVoicelinesToggle;

	[Inject]
	public IGameplayTabAPI api
	{
		get;
		set;
	}

	[Inject]
	public IWindowDisplay windowDisplay
	{
		get;
		set;
	}

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

	private VideoOptionToggle addOption(Transform parent, string text, Action callback)
	{
		VideoOptionToggle component = UnityEngine.Object.Instantiate<VideoOptionToggle>(this.TogglePrefab).GetComponent<VideoOptionToggle>();
		component.transform.SetParent(parent);
		component.Title.text = text;
		this.buttonList.AddButton(component.Toggle.Button, callback);
		return component;
	}

	private void onUpdate()
	{
		if (this.muteEnemyHolosToggle != null)
		{
			this.muteEnemyHolosToggle.Toggle.SetToggle(this.api.MuteEnemyHolos);
			this.muteEnemyVoicelinesToggle.Toggle.SetToggle(this.api.MuteEnemyVoicelines);
		}
	}

	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.syncButtonNavigation();
	}

	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && base.allowInteraction() && this.windowDisplay.GetWindowCount() == 0 && this.buttonList.CurrentSelection == null)
		{
			this.buttonList.AutoSelect(this.buttonList.GetButtons()[0]);
		}
	}

	private void toggleMuteEnemyHolos()
	{
		this.api.MuteEnemyHolos = !this.api.MuteEnemyHolos;
	}

	private void toggleMuteEnemyVoicelines()
	{
		this.api.MuteEnemyVoicelines = !this.api.MuteEnemyVoicelines;
	}

	public override void ResetClicked(InputEventData eventData)
	{
		base.ResetClicked(eventData);
		this.attemptReset();
	}

	private void attemptReset()
	{
		GenericDialog genericDialog = base.dialogController.ShowTwoButtonDialog(base.localization.GetText("ui.settings.resetWarning.title"), base.localization.GetText("ui.settings.resetWarning.body"), base.localization.GetText("ui.settings.resetWarning.confirm"), base.localization.GetText("ui.settings.resetWarning.cancel"));
		GenericDialog expr_4D = genericDialog;
		expr_4D.ConfirmCallback = (Action)Delegate.Combine(expr_4D.ConfirmCallback, new Action(this._attemptReset_m__0));
	}

	public override void OnYButtonPressed()
	{
		base.OnYButtonPressed();
		this.attemptReset();
	}

	private void _attemptReset_m__0()
	{
		this.api.Reset();
		base.audioManager.PlayMenuSound(SoundKey.settings_settingsReset, 0f);
	}
}
