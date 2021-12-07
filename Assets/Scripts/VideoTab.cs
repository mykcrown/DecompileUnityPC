// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class VideoTab : SettingsTabElement
{
	private sealed class _addDropdown_c__AnonStorey0
	{
		internal VideoDropdownElement dropdown;

		internal VideoTab _this;

		internal void __m__0()
		{
			this._this.onDropdownClosed(this.dropdown);
		}
	}

	public VerticalLayoutGroup Column1;

	public VerticalLayoutGroup Column2;

	public VideoDropdownElement DropdownPrefab;

	public VideoOptionToggle TogglePrefab;

	public GameObject SpacerPrefab;

	public Transform DropdownContainer;

	private MenuItemList buttonList;

	private VideoDropdownElement qualityDropdown;

	private VideoDropdownElement resolutionDropdown;

	private VideoDropdownElement displayDropdown;

	private VideoDropdownElement stageQualityDropdown;

	private VideoDropdownElement materialQualityDropdown;

	private VideoDropdownElement graphicsQualityDropdown;

	private VideoOptionToggle visibleInBackgroundToggle;

	private VideoOptionToggle fullScreenToggle;

	private VideoOptionToggle showPerformance;

	private VideoOptionToggle showSystemClock;

	private VideoOptionToggle vsyncToggle;

	private VideoOptionToggle postProcessToggle;

	private List<VideoDropdownElement> videoDropdowns;

	private List<WD_Resolution> possibleResolutions = new List<WD_Resolution>();

	[Inject]
	public IVideoTabAPI api
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
		this.api.GetPossibleResolutions(this.possibleResolutions);
		this.buttonList = base.injector.GetInstance<MenuItemList>();
		this.videoDropdowns = new List<VideoDropdownElement>();
		this.qualityDropdown = this.addDropdown(this.Column1.transform, base.localization.GetText("ui.video.quality"), new Action(this.selectQuality), new Action<int>(this.setQuality));
		this.qualityDropdown.Dropdown.Initialize(this.getQualityOptions(), this.DropdownContainer, null);
		this.graphicsQualityDropdown = this.addDropdown(this.Column1.transform, base.localization.GetText("ui.video.graphicsQuality"), new Action(this.selectGraphicsQuality), new Action<int>(this.setGraphicsQuality));
		this.graphicsQualityDropdown.Dropdown.Initialize(this.getGraphicsQualityOptions(), this.DropdownContainer, null);
		this.resolutionDropdown = this.addDropdown(this.Column1.transform, base.localization.GetText("ui.video.resolution"), new Action(this.selectResolution), new Action<int>(this.setResolution));
		string text = base.localization.GetText("ui.video.resolution.custom");
		this.resolutionDropdown.Dropdown.Initialize(this.getResolutionOptions(), this.DropdownContainer, text);
		this.fullScreenToggle = this.addOption(this.Column1.transform, base.localization.GetText("ui.video.fullScreen"), new Action(this.toggleFullScreen));
		this.showSystemClock = this.addOption(this.Column1.transform, base.localization.GetText("ui.video.showSystemClock"), new Action(this.toggleShowSystemClock));
		this.showPerformance = this.addOption(this.Column1.transform, base.localization.GetText("ui.video.showPerformance"), new Action(this.toggleShowPerformance));
		this.materialQualityDropdown = this.addDropdown(this.Column2.transform, base.localization.GetText("ui.video.materialQuality"), new Action(this.selectMaterialQuality), new Action<int>(this.setMaterialQuality));
		this.materialQualityDropdown.Dropdown.Initialize(this.getLowHighQualityOptions(), this.DropdownContainer, base.localization.GetText("ui.video.materialQuality"));
		this.postProcessToggle = this.addOption(this.Column2.transform, base.localization.GetText("ui.video.postProcessing"), new Action(this.togglePostProcess));
		this.vsyncToggle = this.addOption(this.Column2.transform, base.localization.GetText("ui.video.vsync"), new Action(this.toggleVsync));
		int childCount = this.Column1.transform.childCount;
		this.buttonList.SetNavigationType(MenuItemList.NavigationType.GridVerticalFill, childCount);
		this.buttonList.DisableGridWrap();
		this.buttonList.Initialize();
		base.listen(UserVideoSettingsModel.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	private void addSpacer(Transform parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.SpacerPrefab);
		gameObject.transform.SetParent(parent);
	}

	private VideoDropdownElement addDropdown(Transform parent, string text, Action selectCallback, Action<int> valueSelectedCallback)
	{
		VideoTab._addDropdown_c__AnonStorey0 _addDropdown_c__AnonStorey = new VideoTab._addDropdown_c__AnonStorey0();
		_addDropdown_c__AnonStorey._this = this;
		_addDropdown_c__AnonStorey.dropdown = UnityEngine.Object.Instantiate<VideoDropdownElement>(this.DropdownPrefab, parent);
		base.injector.Inject(_addDropdown_c__AnonStorey.dropdown.Dropdown);
		_addDropdown_c__AnonStorey.dropdown.Title.text = text;
		this.buttonList.AddButton(_addDropdown_c__AnonStorey.dropdown.Dropdown.Button, selectCallback);
		DropdownElement expr_6D = _addDropdown_c__AnonStorey.dropdown.Dropdown;
		expr_6D.ValueSelected = (Action<int>)Delegate.Combine(expr_6D.ValueSelected, valueSelectedCallback);
		DropdownElement expr_8F = _addDropdown_c__AnonStorey.dropdown.Dropdown;
		expr_8F.OnDropdownClosed = (Action)Delegate.Combine(expr_8F.OnDropdownClosed, new Action(_addDropdown_c__AnonStorey.__m__0));
		this.videoDropdowns.Add(_addDropdown_c__AnonStorey.dropdown);
		return _addDropdown_c__AnonStorey.dropdown;
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
		if (this.displayDropdown != null)
		{
			this.displayDropdown.Dropdown.SetValue(this.api.DisplayIndex);
		}
		if (this.fullScreenToggle != null)
		{
			this.qualityDropdown.Dropdown.SetValue(this.api.QualityIndex);
			this.graphicsQualityDropdown.Dropdown.SetValue((int)this.api.GraphicsQuality);
			this.resolutionDropdown.Dropdown.SetValue(this.getResolutionIndex());
			if (this.stageQualityDropdown)
			{
				this.stageQualityDropdown.Dropdown.SetValue((this.api.StageQuality != ThreeTierQualityLevel.Low) ? 1 : 0);
			}
			if (this.materialQualityDropdown)
			{
				this.materialQualityDropdown.Dropdown.SetValue((this.api.MaterialQuality != ThreeTierQualityLevel.Low) ? 1 : 0);
			}
			this.showPerformance.Toggle.SetToggle(this.api.ShowPerformance);
			this.showSystemClock.Toggle.SetToggle(this.api.ShowSystemClock);
			this.fullScreenToggle.Toggle.SetToggle(this.api.FullScreen);
			this.vsyncToggle.Toggle.SetToggle(this.api.Vsync);
			this.postProcessToggle.Toggle.SetToggle(this.api.PostProcessing);
		}
	}

	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.syncButtonNavigation();
	}

	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && base.allowInteraction() && this.windowDisplay.GetWindowCount() == 0)
		{
			foreach (VideoDropdownElement current in this.videoDropdowns)
			{
				if (current.Dropdown.IsOpen())
				{
					current.Dropdown.AutoSelectElement();
					return;
				}
			}
			if (this.buttonList.CurrentSelection == null)
			{
				this.buttonList.AutoSelect(this.buttonList.GetButtons()[0]);
			}
		}
	}

	private void selectQuality()
	{
		this.buttonList.Lock();
		this.qualityDropdown.Dropdown.Open();
		this.syncButtonNavigation();
	}

	private void selectGraphicsQuality()
	{
		this.buttonList.Lock();
		this.graphicsQualityDropdown.Dropdown.Open();
		this.syncButtonNavigation();
	}

	private void selectResolution()
	{
		this.buttonList.Lock();
		this.resolutionDropdown.Dropdown.Open();
		this.syncButtonNavigation();
	}

	private void selectDisplay()
	{
		this.buttonList.Lock();
		this.displayDropdown.Dropdown.Open();
		this.syncButtonNavigation();
	}

	private void selectStageQuality()
	{
		this.buttonList.Lock();
		if (this.stageQualityDropdown)
		{
			this.stageQualityDropdown.Dropdown.Open();
		}
		this.syncButtonNavigation();
	}

	private void selectMaterialQuality()
	{
		this.buttonList.Lock();
		this.materialQualityDropdown.Dropdown.Open();
		this.syncButtonNavigation();
	}

	private void setQuality(int value)
	{
		this.qualityDropdown.Dropdown.Close();
		this.api.QualityIndex = value;
	}

	private void setResolution(int value)
	{
		this.resolutionDropdown.Dropdown.Close();
		this.api.Resolution = this.possibleResolutions[value];
	}

	private void setDisplay(int value)
	{
		this.displayDropdown.Dropdown.Close();
		this.api.DisplayIndex = value;
	}

	private void setStageQuality(int value)
	{
		if (this.stageQualityDropdown)
		{
			this.stageQualityDropdown.Dropdown.Close();
		}
		this.api.StageQuality = ((value != 0) ? ThreeTierQualityLevel.High : ThreeTierQualityLevel.Low);
	}

	private void setGraphicsQuality(int value)
	{
		this.graphicsQualityDropdown.Dropdown.Close();
		this.api.GraphicsQuality = (ThreeTierQualityLevel)value;
	}

	private void setMaterialQuality(int value)
	{
		this.materialQualityDropdown.Dropdown.Close();
		this.api.MaterialQuality = ((value != 0) ? ThreeTierQualityLevel.High : ThreeTierQualityLevel.Low);
	}

	private void toggleMSAA()
	{
		this.api.MultiSampleAntiAliasing = !this.api.MultiSampleAntiAliasing;
	}

	private void toggleVsync()
	{
		this.api.Vsync = !this.api.Vsync;
	}

	private void toggleVisibleInBackground()
	{
		this.api.VisibleInBackground = !this.api.VisibleInBackground;
	}

	private void toggleShowPerformance()
	{
		this.api.ShowPerformance = !this.api.ShowPerformance;
	}

	private void toggleShowSystemClock()
	{
		this.api.ShowSystemClock = !this.api.ShowSystemClock;
	}

	private void toggleFullScreen()
	{
		this.api.FullScreen = !this.api.FullScreen;
	}

	private void togglePostProcess()
	{
		this.api.PostProcessing = !this.api.PostProcessing;
	}

	private void toggleMotionBlur()
	{
		this.api.MotionBlur = !this.api.MotionBlur;
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

	private string[] getQualityOptions()
	{
		int num = QualitySettings.names.Length;
		string[] array = new string[num];
		for (int i = 0; i < array.Length; i++)
		{
			string key = string.Format("ui.video.quality.{0}", i);
			array[i] = base.localization.GetText(key);
		}
		return array;
	}

	private string[] getGraphicsQualityOptions()
	{
		string[] array = new string[3];
		for (int i = 0; i < array.Length; i++)
		{
			string key = string.Format("ui.video.graphicsQuality.{0}", i);
			array[i] = base.localization.GetText(key);
		}
		return array;
	}

	private string[] getResolutionOptions()
	{
		string[] array = new string[this.possibleResolutions.Count];
		for (int i = 0; i < this.possibleResolutions.Count; i++)
		{
			WD_Resolution wD_Resolution = this.possibleResolutions[i];
			array[i] = base.localization.GetText("ui.video.resolution.option", wD_Resolution.width.ToString(), wD_Resolution.height.ToString());
		}
		return array;
	}

	private string[] getLowHighQualityOptions()
	{
		return new string[]
		{
			base.localization.GetText(string.Format("ui.video.threeTierQuality.{0}", 0)),
			base.localization.GetText(string.Format("ui.video.threeTierQuality.{0}", 2))
		};
	}

	private string[] getDisplayOptions()
	{
		int num = Display.displays.Length;
		string[] array = new string[num];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = base.localization.GetText("ui.video.display.option", (i + 1).ToString());
		}
		return array;
	}

	private int getResolutionIndex()
	{
		for (int i = 0; i < this.possibleResolutions.Count; i++)
		{
			WD_Resolution wD_Resolution = this.possibleResolutions[i];
			if (this.api.Resolution.height == wD_Resolution.height && this.api.Resolution.width == wD_Resolution.width)
			{
				return i;
			}
		}
		return -1;
	}

	public override bool OnCancelPressed()
	{
		foreach (VideoDropdownElement current in this.videoDropdowns)
		{
			if (current.Dropdown.IsOpen())
			{
				current.Dropdown.Close();
				return true;
			}
		}
		return base.OnCancelPressed();
	}

	public override void OnYButtonPressed()
	{
		base.OnYButtonPressed();
		this.attemptReset();
	}

	private void onDropdownClosed(VideoDropdownElement dropdown)
	{
		this.buttonList.Unlock();
		this.buttonList.AutoSelect(dropdown.Dropdown.Button);
	}

	private void _attemptReset_m__0()
	{
		this.api.Reset();
		base.audioManager.PlayMenuSound(SoundKey.settings_settingsReset, 0f);
	}
}
