// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioTab : SettingsTabElement
{
	private sealed class _createSlider_c__AnonStorey0
	{
		internal PercentSlider slider;

		internal AudioTab _this;

		internal void __m__0(MenuItemButton MenuItemButton, InputEventData InputEventData)
		{
			if (this.slider == this._this.sfxSlider)
			{
				this._this.audioManager.PlaySFX(SoundKey.settings_soundEffectSliderSet);
			}
		}
	}

	public PercentSlider PercentSliderPrefab;

	public PercentSlider DisabledSliderPrefab;

	public VideoOptionToggle TogglePrefab;

	public Transform SliderAnchor;

	public Transform Column1;

	private PercentSlider sfxSlider;

	private VideoOptionToggle altSoundToggle;

	private VideoOptionToggle altMenuMusicToggle;

	private VideoOptionToggle altBattleMusicToggle;

	private MenuItemList buttonList;

	private Dictionary<MenuItemButton, PercentSlider> sliders = new Dictionary<MenuItemButton, PercentSlider>();

	private float sfxSliderSoundRepeatTimer;

	private const float SLIDER_CONTROLLER_SPEED = 0.01f;

	private bool intialUpdate;

	[Inject]
	public IAudioTabAPI api
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

	private PercentSlider createSlider(Func<float> getter, Action<float> setter, string text, bool isEnabled)
	{
		AudioTab._createSlider_c__AnonStorey0 _createSlider_c__AnonStorey = new AudioTab._createSlider_c__AnonStorey0();
		_createSlider_c__AnonStorey._this = this;
		if (isEnabled)
		{
			_createSlider_c__AnonStorey.slider = UnityEngine.Object.Instantiate<PercentSlider>(this.PercentSliderPrefab, this.SliderAnchor);
		}
		else
		{
			_createSlider_c__AnonStorey.slider = UnityEngine.Object.Instantiate<PercentSlider>(this.DisabledSliderPrefab, this.SliderAnchor);
		}
		_createSlider_c__AnonStorey.slider.Initialize(getter, setter, this.buttonList);
		_createSlider_c__AnonStorey.slider.Title.text = base.localization.GetText(text);
		if (isEnabled)
		{
			MenuItemButton expr_88 = _createSlider_c__AnonStorey.slider.Button;
			expr_88.Submit = (Action<MenuItemButton, InputEventData>)Delegate.Combine(expr_88.Submit, new Action<MenuItemButton, InputEventData>(_createSlider_c__AnonStorey.__m__0));
		}
		else
		{
			_createSlider_c__AnonStorey.slider.Button.DisableType = ButtonAnimator.VisualDisableType.None;
			this.buttonList.SetButtonEnabled(_createSlider_c__AnonStorey.slider.Button, false);
		}
		this.sliders[_createSlider_c__AnonStorey.slider.Button] = _createSlider_c__AnonStorey.slider;
		return _createSlider_c__AnonStorey.slider;
	}

	private VideoOptionToggle addOption(Transform parent, string text, Action callback)
	{
		VideoOptionToggle component = UnityEngine.Object.Instantiate<VideoOptionToggle>(this.TogglePrefab).GetComponent<VideoOptionToggle>();
		component.transform.SetParent(parent);
		component.Title.text = text;
		this.buttonList.AddButton(component.Toggle.Button, callback);
		return component;
	}

	private float getMasterVolume()
	{
		return this.api.MasterVolume;
	}

	private float getMusicVolume()
	{
		return this.api.MusicVolume;
	}

	private float getSFXVolume()
	{
		return this.api.SoundsEffectsVolume;
	}

	private float getCharacterVolume()
	{
		return this.api.CharacterAnnouncerVolume;
	}

	private void setMasterVolume(float volume)
	{
		this.api.MasterVolume = volume;
	}

	private void setMusicVolume(float volume)
	{
		this.api.MusicVolume = volume;
	}

	private void setSFXVolume(float volume)
	{
		this.api.SoundsEffectsVolume = volume;
	}

	private void toggleAltSound()
	{
		this.api.UseAltSounds = !this.api.UseAltSounds;
	}

	private void toggleAltMenuMusic()
	{
		this.api.UseAltMenuMusic = !this.api.UseAltMenuMusic;
	}

	private void toggleAltBattleMusic()
	{
		this.api.UseAltBattleMusic = !this.api.UseAltBattleMusic;
	}

	private void setCharacterVolume(float volume)
	{
		this.api.CharacterAnnouncerVolume = volume;
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

	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.syncButtonNavigation();
	}

	private void syncButtonNavigation()
	{
		if (!(base.uiManager.CurrentInputModule as UIInputModule).IsMouseMode && this.windowDisplay.GetWindowCount() == 0 && this.buttonList.CurrentSelection == null && base.allowInteraction())
		{
			MenuItemButton[] buttons = this.buttonList.GetButtons();
			for (int i = 0; i < buttons.Length; i++)
			{
				MenuItemButton menuItemButton = buttons[i];
				if (menuItemButton.ButtonEnabled)
				{
					this.buttonList.AutoSelect(menuItemButton);
					break;
				}
			}
		}
	}

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

	private void tickSlider(bool isPositive)
	{
		if (this.sliders.ContainsKey(this.buttonList.CurrentSelection))
		{
			PercentSlider percentSlider = this.sliders[this.buttonList.CurrentSelection];
			if (percentSlider.Button == this.buttonList.CurrentSelection)
			{
				int num = (!isPositive) ? (-1) : 1;
				float obj = percentSlider.PercentGetter() + (float)num * 0.01f;
				percentSlider.PercentSettter(obj);
				if (percentSlider == this.sfxSlider)
				{
					this.sfxSliderSoundRepeatTimer += Mathf.Max(Time.deltaTime, 0.01f);
				}
			}
		}
	}

	public override void OnYButtonPressed()
	{
		base.OnYButtonPressed();
		this.attemptReset();
	}

	private void onUpdate()
	{
		foreach (PercentSlider current in this.sliders.Values)
		{
			current.SetValue(current.PercentGetter());
		}
		this.altSoundToggle.Toggle.SetToggle(this.api.UseAltSounds);
		this.altMenuMusicToggle.Toggle.SetToggle(this.api.UseAltMenuMusic);
		this.altBattleMusicToggle.Toggle.SetToggle(this.api.UseAltBattleMusic);
	}

	private void _attemptReset_m__0()
	{
		this.api.Reset();
		base.audioManager.PlayMenuSound(SoundKey.settings_settingsReset, 0f);
	}
}
