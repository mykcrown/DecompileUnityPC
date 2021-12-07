// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoreOptionsWindow : ClientBehavior
{
	public GameObject OptionPrefab;

	public GameObject OptionProfilePrefab;

	public GameObject NewProfileButtonPrefab;

	public VerticalLayoutGroup MainList;

	public VerticalLayoutGroup ProfileList;

	public VerticalLayoutGroup ProfileListEmpty;

	public TextMeshProUGUI NoSavedDisplay;

	public float OptionWidth = 282f;

	public CursorTargetButton CloseButton;

	public CursorTargetButton SaveButton;

	public CursorTargetButton CancelButton;

	private VerticalLayoutGroup currentProfileList;

	private List<OptionGUI> currentOptions = new List<OptionGUI>();

	private float alpha;

	private bool isClosed = true;

	private GameLoadPayload gamePayload;

	private GameMode rules;

	private GameRules mode;

	private CreateNewOptionsProfile newProfileEntry;

	private List<OptionProfileGUI> currentProfileDisplay;

	[Inject]
	public IMainOptionsCalculator optionsCalculator
	{
		get;
		set;
	}

	[Inject]
	public OptionsProfileAPI optionsProfileAPI
	{
		get;
		set;
	}

	public Action<bool> OnCloseRequest
	{
		get;
		set;
	}

	public float Alpha
	{
		get
		{
			return this.alpha;
		}
		set
		{
			this.alpha = value;
			base.GetComponent<CanvasGroup>().alpha = this.alpha;
		}
	}

	public void Init()
	{
		base.listen(OptionsProfileManager.PROFILES_UPDATED, new Action(this.updateProfiles));
		this.updateProfiles();
	}

	public void OnStartPressed(IPlayerCursor cursor)
	{
		this.close(false);
	}

	private void updateProfiles()
	{
		if (this.isClosed)
		{
			return;
		}
		OptionsProfile[] all = this.optionsProfileAPI.GetAll();
		if (!this.isEquivalent(all))
		{
			if (this.currentProfileDisplay == null)
			{
				this.currentProfileDisplay = new List<OptionProfileGUI>();
			}
			this.currentProfileList = ((all.Length != 0) ? this.ProfileList : this.ProfileListEmpty);
			this.clearAllProfiles();
			OptionsProfile[] array = all;
			for (int i = 0; i < array.Length; i++)
			{
				OptionsProfile profile = array[i];
				this.addProfileGui(profile);
			}
			this.NoSavedDisplay.gameObject.SetActive(all.Length == 0);
			this.addNewProfileButton();
		}
		foreach (OptionProfileGUI current in this.currentProfileDisplay)
		{
			current.CheckMark.SetActive(this.optionsProfileAPI.IsCurrentlySelected(current.profile.id));
		}
	}

	private void addNewProfileButton()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.NewProfileButtonPrefab);
		gameObject.transform.SetParent(this.currentProfileList.transform, false);
		this.newProfileEntry = gameObject.GetComponent<CreateNewOptionsProfile>();
		base.injector.Inject(this.newProfileEntry);
	}

	private void clearAllProfiles()
	{
		while (this.ProfileList.transform.childCount > 0)
		{
			Transform child = this.ProfileList.transform.GetChild(0);
			child.transform.SetParent(null);
			UnityEngine.Object.DestroyImmediate(child.gameObject);
		}
		while (this.ProfileListEmpty.transform.childCount > 0)
		{
			Transform child2 = this.ProfileListEmpty.transform.GetChild(0);
			child2.transform.SetParent(null);
			UnityEngine.Object.DestroyImmediate(child2.gameObject);
		}
		if (this.currentProfileDisplay != null)
		{
			this.currentProfileDisplay.Clear();
		}
	}

	private void addProfileGui(OptionsProfile profile)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.OptionProfilePrefab);
		OptionProfileGUI component = gameObject.GetComponent<OptionProfileGUI>();
		base.injector.Inject(component);
		gameObject.transform.SetParent(this.currentProfileList.transform, false);
		this.currentProfileDisplay.Add(component);
		component.Load(profile);
	}

	private bool isEquivalent(OptionsProfile[] newList)
	{
		if (this.currentProfileDisplay == null)
		{
			return false;
		}
		if (newList.Length != this.currentProfileDisplay.Count)
		{
			return false;
		}
		for (int i = 0; i < newList.Length; i++)
		{
			OptionsProfile optionsProfile = newList[i];
			OptionsProfile profile = this.currentProfileDisplay[i].profile;
			if (optionsProfile.name != profile.name)
			{
				return false;
			}
			if (optionsProfile.id != profile.id)
			{
				return false;
			}
		}
		return true;
	}

	public void Close()
	{
		this.close(true);
	}

	public void Save()
	{
		this.optionsProfileAPI.SaveAndSwitchToDefault(new Action<SaveOptionsProfileResult>(this._Save_m__0));
	}

	public void Cancel()
	{
		this.close(true);
	}

	private void close(bool revertChanges)
	{
		if (this.OnCloseRequest != null)
		{
			this.OnCloseRequest(revertChanges);
		}
	}

	public void OnOpened()
	{
		this.isClosed = false;
		this.updateProfiles();
		this.updateOptions();
	}

	public void OnClosed()
	{
		this.isClosed = true;
		this.CloseButton.Removed();
		this.SaveButton.Removed();
		this.CancelButton.Removed();
		if (this.newProfileEntry != null)
		{
			this.newProfileEntry.Removed();
		}
		foreach (OptionGUI current in this.currentOptions)
		{
			current.Removed();
		}
		if (this.currentProfileDisplay != null)
		{
			foreach (OptionProfileGUI current2 in this.currentProfileDisplay)
			{
				current2.Removed();
			}
		}
	}

	public void UpdatePayload(GameLoadPayload gamePayload, GameMode rules, GameRules mode)
	{
		this.gamePayload = gamePayload;
		this.rules = rules;
		this.mode = mode;
		this.updateOptions();
	}

	private void updateOptions()
	{
		MoreOptionsList allOptions = this.optionsCalculator.GetAllOptions(this.rules, this.mode);
		if (!this.isEquivalentOptions(this.currentOptions, allOptions.All))
		{
			foreach (OptionGUI current in this.currentOptions)
			{
				UnityEngine.Object.Destroy(current.gameObject);
			}
			this.currentOptions.Clear();
			foreach (OptionDescription current2 in allOptions.All)
			{
				OptionGUI component = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab).GetComponent<OptionGUI>();
				base.injector.Inject(component);
				component.StaticSize = true;
				component.StaticWidth = this.OptionWidth;
				component.LoadFromDesc(current2);
				component.UpdatePayload(this.gamePayload);
				this.currentOptions.Add(component);
				component.transform.SetParent(this.MainList.transform, false);
			}
		}
		else
		{
			foreach (OptionGUI current3 in this.currentOptions)
			{
				current3.UpdatePayload(this.gamePayload);
			}
		}
	}

	private bool isEquivalentOptions(List<OptionGUI> existing, List<OptionDescription> newList)
	{
		if (existing.Count != newList.Count)
		{
			return false;
		}
		for (int i = existing.Count - 1; i >= 0; i--)
		{
			if (existing[i].Desc != newList[i])
			{
				return false;
			}
		}
		return true;
	}

	private void _Save_m__0(SaveOptionsProfileResult result)
	{
		this.close(false);
	}
}
