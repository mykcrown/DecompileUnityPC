// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownElement : MonoBehaviour
{
	private sealed class _Initialize_c__AnonStorey0
	{
		internal int selectedValue;

		internal DropdownElement _this;

		internal void __m__0()
		{
			this._this.onOptionSelected(this.selectedValue);
		}
	}

	public MenuItemButton Button;

	public TextMeshProUGUI ButtonText;

	public VerticalLayoutGroup DropdownOptions;

	public DropdownOption DropdownOptionPrefab;

	public WavedashUIButton Shroud;

	public MenuItemList OptionList;

	public Action<int> ValueSelected;

	public Action OnDropdownClosed;

	private string[] optionNames;

	private Transform dropdownContainer;

	private string defaultName;

	private int value;

	private bool isOpen;

	private List<DropdownOption> options;

	[Inject]
	public IDependencyInjection injector
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

	public void Initialize(string[] optionNames, Transform dropdownContainer, string defaultName = null)
	{
		this.value = 0;
		this.isOpen = false;
		this.OptionList = this.injector.GetInstance<MenuItemList>();
		this.OptionList.SetNavigationType(MenuItemList.NavigationType.InOrderVertical, 0);
		this.dropdownContainer = dropdownContainer;
		WavedashUIButton expr_39 = this.Shroud;
		expr_39.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(expr_39.OnPointerClickEvent, new Action<InputEventData>(this.onShroudClicked));
		this.optionNames = optionNames;
		this.options = new List<DropdownOption>();
		for (int i = 0; i < optionNames.Length; i++)
		{
			DropdownElement._Initialize_c__AnonStorey0 _Initialize_c__AnonStorey = new DropdownElement._Initialize_c__AnonStorey0();
			_Initialize_c__AnonStorey._this = this;
			DropdownOption dropdownOption = UnityEngine.Object.Instantiate<DropdownOption>(this.DropdownOptionPrefab);
			dropdownOption.transform.SetParent(this.DropdownOptions.transform);
			_Initialize_c__AnonStorey.selectedValue = i;
			this.OptionList.AddButton(dropdownOption.Button, new Action(_Initialize_c__AnonStorey.__m__0));
			dropdownOption.Text.text = optionNames[i];
			this.options.Add(dropdownOption);
		}
		this.OptionList.Initialize();
		this.DropdownOptions.gameObject.SetActive(false);
		Vector2 sizeDelta = ((RectTransform)this.DropdownOptions.transform).sizeDelta;
		sizeDelta.x = ((RectTransform)base.transform).sizeDelta.x;
		((RectTransform)this.DropdownOptions.transform).sizeDelta = sizeDelta;
		if (defaultName != null)
		{
			this.defaultName = defaultName;
		}
		else
		{
			this.defaultName = "UNSET DEFAULT";
		}
		this.onUpdate();
	}

	public void SetValue(int value)
	{
		this.value = value;
		this.onUpdate();
	}

	public bool IsOpen()
	{
		return this.isOpen;
	}

	public void Open()
	{
		this.isOpen = true;
		this.audioManager.PlayMenuSound(SoundKey.generic_dropdownOpen, 0f);
		this.onUpdate();
	}

	public void Close()
	{
		this.isOpen = false;
		this.audioManager.PlayMenuSound(SoundKey.generic_dropdownClose, 0f);
		this.onUpdate();
		if (this.OnDropdownClosed != null)
		{
			this.OnDropdownClosed();
		}
		this.OptionList.Disable();
	}

	public void AutoSelectElement()
	{
		if (this.OptionList.CurrentSelection == null)
		{
			if (this.value >= 0)
			{
				this.OptionList.AutoSelect(this.options[this.value].Button);
			}
			else
			{
				this.OptionList.AutoSelect(this.options[0].Button);
			}
		}
	}

	private void onUpdate()
	{
		if (this.isOpen)
		{
			this.Shroud.transform.SetParent(this.dropdownContainer, false);
			RectTransform rectTransform = (RectTransform)this.Shroud.transform;
			rectTransform.sizeDelta = new Vector2(10000f, 10000f);
			rectTransform.pivot = Vector2.zero;
			rectTransform.position = Vector2.zero;
			this.DropdownOptions.transform.SetParent(this.dropdownContainer, true);
		}
		this.Shroud.gameObject.SetActive(this.isOpen);
		this.DropdownOptions.gameObject.SetActive(this.isOpen);
		if (this.value < 0)
		{
			this.ButtonText.text = this.defaultName;
		}
		else
		{
			this.ButtonText.text = this.optionNames[this.value];
		}
		for (int i = 0; i < this.options.Count; i++)
		{
			bool active = i == this.value;
			this.options[i].CurrentlySelectedImage.SetActive(active);
		}
	}

	private void onOptionSelected(int newValue)
	{
		if (this.ValueSelected != null)
		{
			this.ValueSelected(newValue);
		}
	}

	private void onShroudClicked(InputEventData eventData)
	{
		this.Close();
	}
}
