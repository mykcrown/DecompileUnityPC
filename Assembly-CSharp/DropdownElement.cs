using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000937 RID: 2359
public class DropdownElement : MonoBehaviour
{
	// Token: 0x17000ED2 RID: 3794
	// (get) Token: 0x06003E4C RID: 15948 RVA: 0x0011C5DE File Offset: 0x0011A9DE
	// (set) Token: 0x06003E4D RID: 15949 RVA: 0x0011C5E6 File Offset: 0x0011A9E6
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000ED3 RID: 3795
	// (get) Token: 0x06003E4E RID: 15950 RVA: 0x0011C5EF File Offset: 0x0011A9EF
	// (set) Token: 0x06003E4F RID: 15951 RVA: 0x0011C5F7 File Offset: 0x0011A9F7
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x06003E50 RID: 15952 RVA: 0x0011C600 File Offset: 0x0011AA00
	public void Initialize(string[] optionNames, Transform dropdownContainer, string defaultName = null)
	{
		this.value = 0;
		this.isOpen = false;
		this.OptionList = this.injector.GetInstance<MenuItemList>();
		this.OptionList.SetNavigationType(MenuItemList.NavigationType.InOrderVertical, 0);
		this.dropdownContainer = dropdownContainer;
		WavedashUIButton shroud = this.Shroud;
		shroud.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(shroud.OnPointerClickEvent, new Action<InputEventData>(this.onShroudClicked));
		this.optionNames = optionNames;
		this.options = new List<DropdownOption>();
		for (int i = 0; i < optionNames.Length; i++)
		{
			DropdownOption dropdownOption = UnityEngine.Object.Instantiate<DropdownOption>(this.DropdownOptionPrefab);
			dropdownOption.transform.SetParent(this.DropdownOptions.transform);
			int selectedValue = i;
			this.OptionList.AddButton(dropdownOption.Button, delegate()
			{
				this.onOptionSelected(selectedValue);
			});
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

	// Token: 0x06003E51 RID: 15953 RVA: 0x0011C785 File Offset: 0x0011AB85
	public void SetValue(int value)
	{
		this.value = value;
		this.onUpdate();
	}

	// Token: 0x06003E52 RID: 15954 RVA: 0x0011C794 File Offset: 0x0011AB94
	public bool IsOpen()
	{
		return this.isOpen;
	}

	// Token: 0x06003E53 RID: 15955 RVA: 0x0011C79C File Offset: 0x0011AB9C
	public void Open()
	{
		this.isOpen = true;
		this.audioManager.PlayMenuSound(SoundKey.generic_dropdownOpen, 0f);
		this.onUpdate();
	}

	// Token: 0x06003E54 RID: 15956 RVA: 0x0011C7BC File Offset: 0x0011ABBC
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

	// Token: 0x06003E55 RID: 15957 RVA: 0x0011C808 File Offset: 0x0011AC08
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

	// Token: 0x06003E56 RID: 15958 RVA: 0x0011C87C File Offset: 0x0011AC7C
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

	// Token: 0x06003E57 RID: 15959 RVA: 0x0011C9A6 File Offset: 0x0011ADA6
	private void onOptionSelected(int newValue)
	{
		if (this.ValueSelected != null)
		{
			this.ValueSelected(newValue);
		}
	}

	// Token: 0x06003E58 RID: 15960 RVA: 0x0011C9BF File Offset: 0x0011ADBF
	private void onShroudClicked(InputEventData eventData)
	{
		this.Close();
	}

	// Token: 0x04002A4E RID: 10830
	public MenuItemButton Button;

	// Token: 0x04002A4F RID: 10831
	public TextMeshProUGUI ButtonText;

	// Token: 0x04002A50 RID: 10832
	public VerticalLayoutGroup DropdownOptions;

	// Token: 0x04002A51 RID: 10833
	public DropdownOption DropdownOptionPrefab;

	// Token: 0x04002A52 RID: 10834
	public WavedashUIButton Shroud;

	// Token: 0x04002A53 RID: 10835
	public MenuItemList OptionList;

	// Token: 0x04002A54 RID: 10836
	public Action<int> ValueSelected;

	// Token: 0x04002A55 RID: 10837
	public Action OnDropdownClosed;

	// Token: 0x04002A56 RID: 10838
	private string[] optionNames;

	// Token: 0x04002A57 RID: 10839
	private Transform dropdownContainer;

	// Token: 0x04002A58 RID: 10840
	private string defaultName;

	// Token: 0x04002A59 RID: 10841
	private int value;

	// Token: 0x04002A5A RID: 10842
	private bool isOpen;

	// Token: 0x04002A5B RID: 10843
	private List<DropdownOption> options;
}
