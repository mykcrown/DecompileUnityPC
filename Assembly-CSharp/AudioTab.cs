using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000967 RID: 2407
public class AudioTab : SettingsTabElement
{
	// Token: 0x17000F2E RID: 3886
	// (get) Token: 0x0600402C RID: 16428 RVA: 0x00121700 File Offset: 0x0011FB00
	// (set) Token: 0x0600402D RID: 16429 RVA: 0x00121708 File Offset: 0x0011FB08
	[Inject]
	public IAudioTabAPI api { get; set; }

	// Token: 0x17000F2F RID: 3887
	// (get) Token: 0x0600402E RID: 16430 RVA: 0x00121711 File Offset: 0x0011FB11
	// (set) Token: 0x0600402F RID: 16431 RVA: 0x00121719 File Offset: 0x0011FB19
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x06004030 RID: 16432 RVA: 0x00121724 File Offset: 0x0011FB24
	public override void Awake()
	{
		base.Awake();
		if (this.api == null)
		{
			return;
		}
		this.buttonList = base.injector.GetInstance<MenuItemList>();
		this.buttonList.SetNavigationType(MenuItemList.NavigationType.InOrderVertical, 0);
		this.createSlider(new Func<float>(this.getMusicVolume), new Action<float>(this.setMusicVolume), "ui.audio.bgm", true);
		this.sfxSlider = this.createSlider(new Func<float>(this.getSFXVolume), new Action<float>(this.setSFXVolume), "ui.audio.sfx", true);
		this.altSoundToggle = this.addOption(this.Column1, "ALTERNATIVE SFX STYLE", new Action(this.toggleAltSound));
		this.altMenuMusicToggle = this.addOption(this.Column1, "ALTERNATIVE MENU MUSIC", new Action(this.toggleAltMenuMusic));
		this.altBattleMusicToggle = this.addOption(this.Column1, "OST BATTLE MUSIC", new Action(this.toggleAltBattleMusic));
		this.buttonList.Initialize();
		this.sfxSliderSoundRepeatTimer = base.config.soundSettings.soundEffectSliderSetRepeatDelay;
		base.listen(AudioTabAPI.UPDATED, new Action(this.onUpdate));
	}

	// Token: 0x06004031 RID: 16433 RVA: 0x00121854 File Offset: 0x0011FC54
	private void Update()
	{
		if (!this.intialUpdate)
		{
			this.onUpdate();
			this.intialUpdate = true;
		}
		if (this.sfxSlider.IsDragging)
		{
			this.sfxSliderSoundRepeatTimer += Time.deltaTime;
		}
		if (this.sfxSliderSoundRepeatTimer > base.config.soundSettings.soundEffectSliderSetRepeatDelay && this.sfxSliderSoundRepeatTimer > 0f)
		{
			this.sfxSliderSoundRepeatTimer -= base.config.soundSettings.soundEffectSliderSetRepeatDelay;
			base.audioManager.PlaySFX(SoundKey.settings_soundEffectSliderSet);
		}
	}

	// Token: 0x06004032 RID: 16434 RVA: 0x001218F4 File Offset: 0x0011FCF4
	private PercentSlider createSlider(Func<float> getter, Action<float> setter, string text, bool isEnabled)
	{
		PercentSlider slider;
		if (isEnabled)
		{
			slider = UnityEngine.Object.Instantiate<PercentSlider>(this.PercentSliderPrefab, this.SliderAnchor);
		}
		else
		{
			slider = UnityEngine.Object.Instantiate<PercentSlider>(this.DisabledSliderPrefab, this.SliderAnchor);
		}
		slider.Initialize(getter, setter, this.buttonList);
		slider.Title.text = base.localization.GetText(text);
		if (isEnabled)
		{
			MenuItemButton button = slider.Button;
			button.Submit = (Action<MenuItemButton, InputEventData>)Delegate.Combine(button.Submit, new Action<MenuItemButton, InputEventData>(delegate(MenuItemButton MenuItemButton, InputEventData InputEventData)
			{
				if (slider == this.sfxSlider)
				{
					this.audioManager.PlaySFX(SoundKey.settings_soundEffectSliderSet);
				}
			}));
		}
		else
		{
			slider.Button.DisableType = ButtonAnimator.VisualDisableType.None;
			this.buttonList.SetButtonEnabled(slider.Button, false);
		}
		this.sliders[slider.Button] = slider;
		return slider;
	}

	// Token: 0x06004033 RID: 16435 RVA: 0x001219FC File Offset: 0x0011FDFC
	private VideoOptionToggle addOption(Transform parent, string text, Action callback)
	{
		VideoOptionToggle component = UnityEngine.Object.Instantiate<VideoOptionToggle>(this.TogglePrefab).GetComponent<VideoOptionToggle>();
		component.transform.SetParent(parent);
		component.Title.text = text;
		this.buttonList.AddButton(component.Toggle.Button, callback);
		return component;
	}

	// Token: 0x06004034 RID: 16436 RVA: 0x00121A4A File Offset: 0x0011FE4A
	private float getMasterVolume()
	{
		return this.api.MasterVolume;
	}

	// Token: 0x06004035 RID: 16437 RVA: 0x00121A57 File Offset: 0x0011FE57
	private float getMusicVolume()
	{
		return this.api.MusicVolume;
	}

	// Token: 0x06004036 RID: 16438 RVA: 0x00121A64 File Offset: 0x0011FE64
	private float getSFXVolume()
	{
		return this.api.SoundsEffectsVolume;
	}

	// Token: 0x06004037 RID: 16439 RVA: 0x00121A71 File Offset: 0x0011FE71
	private float getCharacterVolume()
	{
		return this.api.CharacterAnnouncerVolume;
	}

	// Token: 0x06004038 RID: 16440 RVA: 0x00121A7E File Offset: 0x0011FE7E
	private void setMasterVolume(float volume)
	{
		this.api.MasterVolume = volume;
	}

	// Token: 0x06004039 RID: 16441 RVA: 0x00121A8C File Offset: 0x0011FE8C
	private void setMusicVolume(float volume)
	{
		this.api.MusicVolume = volume;
	}

	// Token: 0x0600403A RID: 16442 RVA: 0x00121A9A File Offset: 0x0011FE9A
	private void setSFXVolume(float volume)
	{
		this.api.SoundsEffectsVolume = volume;
	}

	// Token: 0x0600403B RID: 16443 RVA: 0x00121AA8 File Offset: 0x0011FEA8
	private void toggleAltSound()
	{
		this.api.UseAltSounds = !this.api.UseAltSounds;
	}

	// Token: 0x0600403C RID: 16444 RVA: 0x00121AC3 File Offset: 0x0011FEC3
	private void toggleAltMenuMusic()
	{
		this.api.UseAltMenuMusic = !this.api.UseAltMenuMusic;
	}

	// Token: 0x0600403D RID: 16445 RVA: 0x00121ADE File Offset: 0x0011FEDE
	private void toggleAltBattleMusic()
	{
		this.api.UseAltBattleMusic = !this.api.UseAltBattleMusic;
	}

	// Token: 0x0600403E RID: 16446 RVA: 0x00121AF9 File Offset: 0x0011FEF9
	private void setCharacterVolume(float volume)
	{
		this.api.CharacterAnnouncerVolume = volume;
	}

	// Token: 0x0600403F RID: 16447 RVA: 0x00121B07 File Offset: 0x0011FF07
	public override void ResetClicked(InputEventData eventData)
	{
		base.ResetClicked(eventData);
		this.attemptReset();
	}

	// Token: 0x06004040 RID: 16448 RVA: 0x00121B18 File Offset: 0x0011FF18
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

	// Token: 0x06004041 RID: 16449 RVA: 0x00121B93 File Offset: 0x0011FF93
	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.syncButtonNavigation();
	}

	// Token: 0x06004042 RID: 16450 RVA: 0x00121BA4 File Offset: 0x0011FFA4
	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && this.windowDisplay.GetWindowCount() == 0 && this.buttonList.CurrentSelection == null && base.allowInteraction())
		{
			foreach (MenuItemButton menuItemButton in this.buttonList.GetButtons())
			{
				if (menuItemButton.ButtonEnabled)
				{
					this.buttonList.AutoSelect(menuItemButton);
					break;
				}
			}
		}
	}

	// Token: 0x06004043 RID: 16451 RVA: 0x00121C3C File Offset: 0x0012003C
	public override void OnLeft()
	{
		if (this.buttonList.CurrentSelection != null)
		{
			this.tickSlider(false);
		}
		else
		{
			base.OnLeft();
		}
	}

	// Token: 0x06004044 RID: 16452 RVA: 0x00121C66 File Offset: 0x00120066
	public override void OnRight()
	{
		if (this.buttonList.CurrentSelection != null)
		{
			this.tickSlider(true);
		}
		else
		{
			base.OnLeft();
		}
	}

	// Token: 0x06004045 RID: 16453 RVA: 0x00121C90 File Offset: 0x00120090
	private void tickSlider(bool isPositive)
	{
		if (this.sliders.ContainsKey(this.buttonList.CurrentSelection))
		{
			PercentSlider percentSlider = this.sliders[this.buttonList.CurrentSelection];
			if (percentSlider.Button == this.buttonList.CurrentSelection)
			{
				int num = (!isPositive) ? -1 : 1;
				float obj = percentSlider.PercentGetter() + (float)num * 0.01f;
				percentSlider.PercentSettter(obj);
				if (percentSlider == this.sfxSlider)
				{
					this.sfxSliderSoundRepeatTimer += Mathf.Max(Time.deltaTime, 0.01f);
				}
			}
		}
	}

	// Token: 0x06004046 RID: 16454 RVA: 0x00121D46 File Offset: 0x00120146
	public override void OnYButtonPressed()
	{
		base.OnYButtonPressed();
		this.attemptReset();
	}

	// Token: 0x06004047 RID: 16455 RVA: 0x00121D54 File Offset: 0x00120154
	private void onUpdate()
	{
		foreach (PercentSlider percentSlider in this.sliders.Values)
		{
			percentSlider.SetValue(percentSlider.PercentGetter());
		}
		this.altSoundToggle.Toggle.SetToggle(this.api.UseAltSounds);
		this.altMenuMusicToggle.Toggle.SetToggle(this.api.UseAltMenuMusic);
		this.altBattleMusicToggle.Toggle.SetToggle(this.api.UseAltBattleMusic);
	}

	// Token: 0x04002B54 RID: 11092
	public PercentSlider PercentSliderPrefab;

	// Token: 0x04002B55 RID: 11093
	public PercentSlider DisabledSliderPrefab;

	// Token: 0x04002B56 RID: 11094
	public VideoOptionToggle TogglePrefab;

	// Token: 0x04002B57 RID: 11095
	public Transform SliderAnchor;

	// Token: 0x04002B58 RID: 11096
	public Transform Column1;

	// Token: 0x04002B59 RID: 11097
	private PercentSlider sfxSlider;

	// Token: 0x04002B5A RID: 11098
	private VideoOptionToggle altSoundToggle;

	// Token: 0x04002B5B RID: 11099
	private VideoOptionToggle altMenuMusicToggle;

	// Token: 0x04002B5C RID: 11100
	private VideoOptionToggle altBattleMusicToggle;

	// Token: 0x04002B5D RID: 11101
	private MenuItemList buttonList;

	// Token: 0x04002B5E RID: 11102
	private Dictionary<MenuItemButton, PercentSlider> sliders = new Dictionary<MenuItemButton, PercentSlider>();

	// Token: 0x04002B5F RID: 11103
	private float sfxSliderSoundRepeatTimer;

	// Token: 0x04002B60 RID: 11104
	private const float SLIDER_CONTROLLER_SPEED = 0.01f;

	// Token: 0x04002B61 RID: 11105
	private bool intialUpdate;
}
