using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200098A RID: 2442
public class VideoTab : SettingsTabElement
{
	// Token: 0x17000F94 RID: 3988
	// (get) Token: 0x06004214 RID: 16916 RVA: 0x00127242 File Offset: 0x00125642
	// (set) Token: 0x06004215 RID: 16917 RVA: 0x0012724A File Offset: 0x0012564A
	[Inject]
	public IVideoTabAPI api { get; set; }

	// Token: 0x17000F95 RID: 3989
	// (get) Token: 0x06004216 RID: 16918 RVA: 0x00127253 File Offset: 0x00125653
	// (set) Token: 0x06004217 RID: 16919 RVA: 0x0012725B File Offset: 0x0012565B
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x06004218 RID: 16920 RVA: 0x00127264 File Offset: 0x00125664
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

	// Token: 0x06004219 RID: 16921 RVA: 0x00127590 File Offset: 0x00125990
	private void addSpacer(Transform parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.SpacerPrefab);
		gameObject.transform.SetParent(parent);
	}

	// Token: 0x0600421A RID: 16922 RVA: 0x001275B8 File Offset: 0x001259B8
	private VideoDropdownElement addDropdown(Transform parent, string text, Action selectCallback, Action<int> valueSelectedCallback)
	{
		VideoDropdownElement dropdown = UnityEngine.Object.Instantiate<VideoDropdownElement>(this.DropdownPrefab, parent);
		base.injector.Inject(dropdown.Dropdown);
		dropdown.Title.text = text;
		this.buttonList.AddButton(dropdown.Dropdown.Button, selectCallback);
		DropdownElement dropdown3 = dropdown.Dropdown;
		dropdown3.ValueSelected = (Action<int>)Delegate.Combine(dropdown3.ValueSelected, valueSelectedCallback);
		DropdownElement dropdown2 = dropdown.Dropdown;
		dropdown2.OnDropdownClosed = (Action)Delegate.Combine(dropdown2.OnDropdownClosed, new Action(delegate()
		{
			this.onDropdownClosed(dropdown);
		}));
		this.videoDropdowns.Add(dropdown);
		return dropdown;
	}

	// Token: 0x0600421B RID: 16923 RVA: 0x0012768C File Offset: 0x00125A8C
	private VideoOptionToggle addOption(Transform parent, string text, Action callback)
	{
		VideoOptionToggle component = UnityEngine.Object.Instantiate<VideoOptionToggle>(this.TogglePrefab).GetComponent<VideoOptionToggle>();
		component.transform.SetParent(parent);
		component.Title.text = text;
		this.buttonList.AddButton(component.Toggle.Button, callback);
		return component;
	}

	// Token: 0x0600421C RID: 16924 RVA: 0x001276DC File Offset: 0x00125ADC
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

	// Token: 0x0600421D RID: 16925 RVA: 0x00127867 File Offset: 0x00125C67
	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.syncButtonNavigation();
	}

	// Token: 0x0600421E RID: 16926 RVA: 0x00127878 File Offset: 0x00125C78
	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && base.allowInteraction() && this.windowDisplay.GetWindowCount() == 0)
		{
			foreach (VideoDropdownElement videoDropdownElement in this.videoDropdowns)
			{
				if (videoDropdownElement.Dropdown.IsOpen())
				{
					videoDropdownElement.Dropdown.AutoSelectElement();
					return;
				}
			}
			if (this.buttonList.CurrentSelection == null)
			{
				this.buttonList.AutoSelect(this.buttonList.GetButtons()[0]);
			}
		}
	}

	// Token: 0x0600421F RID: 16927 RVA: 0x00127950 File Offset: 0x00125D50
	private void selectQuality()
	{
		this.buttonList.Lock();
		this.qualityDropdown.Dropdown.Open();
		this.syncButtonNavigation();
	}

	// Token: 0x06004220 RID: 16928 RVA: 0x00127973 File Offset: 0x00125D73
	private void selectGraphicsQuality()
	{
		this.buttonList.Lock();
		this.graphicsQualityDropdown.Dropdown.Open();
		this.syncButtonNavigation();
	}

	// Token: 0x06004221 RID: 16929 RVA: 0x00127996 File Offset: 0x00125D96
	private void selectResolution()
	{
		this.buttonList.Lock();
		this.resolutionDropdown.Dropdown.Open();
		this.syncButtonNavigation();
	}

	// Token: 0x06004222 RID: 16930 RVA: 0x001279B9 File Offset: 0x00125DB9
	private void selectDisplay()
	{
		this.buttonList.Lock();
		this.displayDropdown.Dropdown.Open();
		this.syncButtonNavigation();
	}

	// Token: 0x06004223 RID: 16931 RVA: 0x001279DC File Offset: 0x00125DDC
	private void selectStageQuality()
	{
		this.buttonList.Lock();
		if (this.stageQualityDropdown)
		{
			this.stageQualityDropdown.Dropdown.Open();
		}
		this.syncButtonNavigation();
	}

	// Token: 0x06004224 RID: 16932 RVA: 0x00127A0F File Offset: 0x00125E0F
	private void selectMaterialQuality()
	{
		this.buttonList.Lock();
		this.materialQualityDropdown.Dropdown.Open();
		this.syncButtonNavigation();
	}

	// Token: 0x06004225 RID: 16933 RVA: 0x00127A32 File Offset: 0x00125E32
	private void setQuality(int value)
	{
		this.qualityDropdown.Dropdown.Close();
		this.api.QualityIndex = value;
	}

	// Token: 0x06004226 RID: 16934 RVA: 0x00127A50 File Offset: 0x00125E50
	private void setResolution(int value)
	{
		this.resolutionDropdown.Dropdown.Close();
		this.api.Resolution = this.possibleResolutions[value];
	}

	// Token: 0x06004227 RID: 16935 RVA: 0x00127A79 File Offset: 0x00125E79
	private void setDisplay(int value)
	{
		this.displayDropdown.Dropdown.Close();
		this.api.DisplayIndex = value;
	}

	// Token: 0x06004228 RID: 16936 RVA: 0x00127A97 File Offset: 0x00125E97
	private void setStageQuality(int value)
	{
		if (this.stageQualityDropdown)
		{
			this.stageQualityDropdown.Dropdown.Close();
		}
		this.api.StageQuality = ((value != 0) ? ThreeTierQualityLevel.High : ThreeTierQualityLevel.Low);
	}

	// Token: 0x06004229 RID: 16937 RVA: 0x00127AD1 File Offset: 0x00125ED1
	private void setGraphicsQuality(int value)
	{
		this.graphicsQualityDropdown.Dropdown.Close();
		this.api.GraphicsQuality = (ThreeTierQualityLevel)value;
	}

	// Token: 0x0600422A RID: 16938 RVA: 0x00127AEF File Offset: 0x00125EEF
	private void setMaterialQuality(int value)
	{
		this.materialQualityDropdown.Dropdown.Close();
		this.api.MaterialQuality = ((value != 0) ? ThreeTierQualityLevel.High : ThreeTierQualityLevel.Low);
	}

	// Token: 0x0600422B RID: 16939 RVA: 0x00127B19 File Offset: 0x00125F19
	private void toggleMSAA()
	{
		this.api.MultiSampleAntiAliasing = !this.api.MultiSampleAntiAliasing;
	}

	// Token: 0x0600422C RID: 16940 RVA: 0x00127B34 File Offset: 0x00125F34
	private void toggleVsync()
	{
		this.api.Vsync = !this.api.Vsync;
	}

	// Token: 0x0600422D RID: 16941 RVA: 0x00127B4F File Offset: 0x00125F4F
	private void toggleVisibleInBackground()
	{
		this.api.VisibleInBackground = !this.api.VisibleInBackground;
	}

	// Token: 0x0600422E RID: 16942 RVA: 0x00127B6A File Offset: 0x00125F6A
	private void toggleShowPerformance()
	{
		this.api.ShowPerformance = !this.api.ShowPerformance;
	}

	// Token: 0x0600422F RID: 16943 RVA: 0x00127B85 File Offset: 0x00125F85
	private void toggleShowSystemClock()
	{
		this.api.ShowSystemClock = !this.api.ShowSystemClock;
	}

	// Token: 0x06004230 RID: 16944 RVA: 0x00127BA0 File Offset: 0x00125FA0
	private void toggleFullScreen()
	{
		this.api.FullScreen = !this.api.FullScreen;
	}

	// Token: 0x06004231 RID: 16945 RVA: 0x00127BBB File Offset: 0x00125FBB
	private void togglePostProcess()
	{
		this.api.PostProcessing = !this.api.PostProcessing;
	}

	// Token: 0x06004232 RID: 16946 RVA: 0x00127BD6 File Offset: 0x00125FD6
	private void toggleMotionBlur()
	{
		this.api.MotionBlur = !this.api.MotionBlur;
	}

	// Token: 0x06004233 RID: 16947 RVA: 0x00127BF1 File Offset: 0x00125FF1
	public override void ResetClicked(InputEventData eventData)
	{
		base.ResetClicked(eventData);
		this.attemptReset();
	}

	// Token: 0x06004234 RID: 16948 RVA: 0x00127C00 File Offset: 0x00126000
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

	// Token: 0x06004235 RID: 16949 RVA: 0x00127C7C File Offset: 0x0012607C
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

	// Token: 0x06004236 RID: 16950 RVA: 0x00127CD0 File Offset: 0x001260D0
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

	// Token: 0x06004237 RID: 16951 RVA: 0x00127D1C File Offset: 0x0012611C
	private string[] getResolutionOptions()
	{
		string[] array = new string[this.possibleResolutions.Count];
		for (int i = 0; i < this.possibleResolutions.Count; i++)
		{
			WD_Resolution wd_Resolution = this.possibleResolutions[i];
			array[i] = base.localization.GetText("ui.video.resolution.option", wd_Resolution.width.ToString(), wd_Resolution.height.ToString());
		}
		return array;
	}

	// Token: 0x06004238 RID: 16952 RVA: 0x00127D9C File Offset: 0x0012619C
	private string[] getLowHighQualityOptions()
	{
		return new string[]
		{
			base.localization.GetText(string.Format("ui.video.threeTierQuality.{0}", 0)),
			base.localization.GetText(string.Format("ui.video.threeTierQuality.{0}", 2))
		};
	}

	// Token: 0x06004239 RID: 16953 RVA: 0x00127DF0 File Offset: 0x001261F0
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

	// Token: 0x0600423A RID: 16954 RVA: 0x00127E48 File Offset: 0x00126248
	private int getResolutionIndex()
	{
		for (int i = 0; i < this.possibleResolutions.Count; i++)
		{
			WD_Resolution wd_Resolution = this.possibleResolutions[i];
			if (this.api.Resolution.height == wd_Resolution.height && this.api.Resolution.width == wd_Resolution.width)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600423B RID: 16955 RVA: 0x00127EC0 File Offset: 0x001262C0
	public override bool OnCancelPressed()
	{
		foreach (VideoDropdownElement videoDropdownElement in this.videoDropdowns)
		{
			if (videoDropdownElement.Dropdown.IsOpen())
			{
				videoDropdownElement.Dropdown.Close();
				return true;
			}
		}
		return base.OnCancelPressed();
	}

	// Token: 0x0600423C RID: 16956 RVA: 0x00127F40 File Offset: 0x00126340
	public override void OnYButtonPressed()
	{
		base.OnYButtonPressed();
		this.attemptReset();
	}

	// Token: 0x0600423D RID: 16957 RVA: 0x00127F4E File Offset: 0x0012634E
	private void onDropdownClosed(VideoDropdownElement dropdown)
	{
		this.buttonList.Unlock();
		this.buttonList.AutoSelect(dropdown.Dropdown.Button);
	}

	// Token: 0x04002C59 RID: 11353
	public VerticalLayoutGroup Column1;

	// Token: 0x04002C5A RID: 11354
	public VerticalLayoutGroup Column2;

	// Token: 0x04002C5B RID: 11355
	public VideoDropdownElement DropdownPrefab;

	// Token: 0x04002C5C RID: 11356
	public VideoOptionToggle TogglePrefab;

	// Token: 0x04002C5D RID: 11357
	public GameObject SpacerPrefab;

	// Token: 0x04002C5E RID: 11358
	public Transform DropdownContainer;

	// Token: 0x04002C5F RID: 11359
	private MenuItemList buttonList;

	// Token: 0x04002C60 RID: 11360
	private VideoDropdownElement qualityDropdown;

	// Token: 0x04002C61 RID: 11361
	private VideoDropdownElement resolutionDropdown;

	// Token: 0x04002C62 RID: 11362
	private VideoDropdownElement displayDropdown;

	// Token: 0x04002C63 RID: 11363
	private VideoDropdownElement stageQualityDropdown;

	// Token: 0x04002C64 RID: 11364
	private VideoDropdownElement materialQualityDropdown;

	// Token: 0x04002C65 RID: 11365
	private VideoDropdownElement graphicsQualityDropdown;

	// Token: 0x04002C66 RID: 11366
	private VideoOptionToggle visibleInBackgroundToggle;

	// Token: 0x04002C67 RID: 11367
	private VideoOptionToggle fullScreenToggle;

	// Token: 0x04002C68 RID: 11368
	private VideoOptionToggle showPerformance;

	// Token: 0x04002C69 RID: 11369
	private VideoOptionToggle showSystemClock;

	// Token: 0x04002C6A RID: 11370
	private VideoOptionToggle vsyncToggle;

	// Token: 0x04002C6B RID: 11371
	private VideoOptionToggle postProcessToggle;

	// Token: 0x04002C6C RID: 11372
	private List<VideoDropdownElement> videoDropdowns;

	// Token: 0x04002C6D RID: 11373
	private List<WD_Resolution> possibleResolutions = new List<WD_Resolution>();
}
