using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008ED RID: 2285
public class MoreOptionsWindow : ClientBehavior
{
	// Token: 0x17000E0B RID: 3595
	// (get) Token: 0x06003A6E RID: 14958 RVA: 0x001119C5 File Offset: 0x0010FDC5
	// (set) Token: 0x06003A6F RID: 14959 RVA: 0x001119CD File Offset: 0x0010FDCD
	[Inject]
	public IMainOptionsCalculator optionsCalculator { get; set; }

	// Token: 0x17000E0C RID: 3596
	// (get) Token: 0x06003A70 RID: 14960 RVA: 0x001119D6 File Offset: 0x0010FDD6
	// (set) Token: 0x06003A71 RID: 14961 RVA: 0x001119DE File Offset: 0x0010FDDE
	[Inject]
	public OptionsProfileAPI optionsProfileAPI { get; set; }

	// Token: 0x17000E0D RID: 3597
	// (get) Token: 0x06003A72 RID: 14962 RVA: 0x001119E7 File Offset: 0x0010FDE7
	// (set) Token: 0x06003A73 RID: 14963 RVA: 0x001119EF File Offset: 0x0010FDEF
	public Action<bool> OnCloseRequest { get; set; }

	// Token: 0x06003A74 RID: 14964 RVA: 0x001119F8 File Offset: 0x0010FDF8
	public void Init()
	{
		base.listen(OptionsProfileManager.PROFILES_UPDATED, new Action(this.updateProfiles));
		this.updateProfiles();
	}

	// Token: 0x06003A75 RID: 14965 RVA: 0x00111A17 File Offset: 0x0010FE17
	public void OnStartPressed(IPlayerCursor cursor)
	{
		this.close(false);
	}

	// Token: 0x06003A76 RID: 14966 RVA: 0x00111A20 File Offset: 0x0010FE20
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
			foreach (OptionsProfile profile in all)
			{
				this.addProfileGui(profile);
			}
			this.NoSavedDisplay.gameObject.SetActive(all.Length == 0);
			this.addNewProfileButton();
		}
		foreach (OptionProfileGUI optionProfileGUI in this.currentProfileDisplay)
		{
			optionProfileGUI.CheckMark.SetActive(this.optionsProfileAPI.IsCurrentlySelected(optionProfileGUI.profile.id));
		}
	}

	// Token: 0x06003A77 RID: 14967 RVA: 0x00111B38 File Offset: 0x0010FF38
	private void addNewProfileButton()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.NewProfileButtonPrefab);
		gameObject.transform.SetParent(this.currentProfileList.transform, false);
		this.newProfileEntry = gameObject.GetComponent<CreateNewOptionsProfile>();
		base.injector.Inject(this.newProfileEntry);
	}

	// Token: 0x06003A78 RID: 14968 RVA: 0x00111B88 File Offset: 0x0010FF88
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

	// Token: 0x06003A79 RID: 14969 RVA: 0x00111C34 File Offset: 0x00110034
	private void addProfileGui(OptionsProfile profile)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.OptionProfilePrefab);
		OptionProfileGUI component = gameObject.GetComponent<OptionProfileGUI>();
		base.injector.Inject(component);
		gameObject.transform.SetParent(this.currentProfileList.transform, false);
		this.currentProfileDisplay.Add(component);
		component.Load(profile);
	}

	// Token: 0x06003A7A RID: 14970 RVA: 0x00111C8C File Offset: 0x0011008C
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

	// Token: 0x06003A7B RID: 14971 RVA: 0x00111D16 File Offset: 0x00110116
	public void Close()
	{
		this.close(true);
	}

	// Token: 0x06003A7C RID: 14972 RVA: 0x00111D1F File Offset: 0x0011011F
	public void Save()
	{
		this.optionsProfileAPI.SaveAndSwitchToDefault(delegate(SaveOptionsProfileResult result)
		{
			this.close(false);
		});
	}

	// Token: 0x06003A7D RID: 14973 RVA: 0x00111D38 File Offset: 0x00110138
	public void Cancel()
	{
		this.close(true);
	}

	// Token: 0x06003A7E RID: 14974 RVA: 0x00111D41 File Offset: 0x00110141
	private void close(bool revertChanges)
	{
		if (this.OnCloseRequest != null)
		{
			this.OnCloseRequest(revertChanges);
		}
	}

	// Token: 0x06003A7F RID: 14975 RVA: 0x00111D5A File Offset: 0x0011015A
	public void OnOpened()
	{
		this.isClosed = false;
		this.updateProfiles();
		this.updateOptions();
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x00111D70 File Offset: 0x00110170
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
		foreach (OptionGUI optionGUI in this.currentOptions)
		{
			optionGUI.Removed();
		}
		if (this.currentProfileDisplay != null)
		{
			foreach (OptionProfileGUI optionProfileGUI in this.currentProfileDisplay)
			{
				optionProfileGUI.Removed();
			}
		}
	}

	// Token: 0x06003A81 RID: 14977 RVA: 0x00111E64 File Offset: 0x00110264
	public void UpdatePayload(GameLoadPayload gamePayload, GameMode rules, GameRules mode)
	{
		this.gamePayload = gamePayload;
		this.rules = rules;
		this.mode = mode;
		this.updateOptions();
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x00111E84 File Offset: 0x00110284
	private void updateOptions()
	{
		MoreOptionsList allOptions = this.optionsCalculator.GetAllOptions(this.rules, this.mode);
		if (!this.isEquivalentOptions(this.currentOptions, allOptions.All))
		{
			foreach (OptionGUI optionGUI in this.currentOptions)
			{
				UnityEngine.Object.Destroy(optionGUI.gameObject);
			}
			this.currentOptions.Clear();
			foreach (OptionDescription desc in allOptions.All)
			{
				OptionGUI component = UnityEngine.Object.Instantiate<GameObject>(this.OptionPrefab).GetComponent<OptionGUI>();
				base.injector.Inject(component);
				component.StaticSize = true;
				component.StaticWidth = this.OptionWidth;
				component.LoadFromDesc(desc);
				component.UpdatePayload(this.gamePayload);
				this.currentOptions.Add(component);
				component.transform.SetParent(this.MainList.transform, false);
			}
		}
		else
		{
			foreach (OptionGUI optionGUI2 in this.currentOptions)
			{
				optionGUI2.UpdatePayload(this.gamePayload);
			}
		}
	}

	// Token: 0x06003A83 RID: 14979 RVA: 0x0011202C File Offset: 0x0011042C
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

	// Token: 0x17000E0E RID: 3598
	// (get) Token: 0x06003A84 RID: 14980 RVA: 0x00112080 File Offset: 0x00110480
	// (set) Token: 0x06003A85 RID: 14981 RVA: 0x00112088 File Offset: 0x00110488
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

	// Token: 0x0400283B RID: 10299
	public GameObject OptionPrefab;

	// Token: 0x0400283C RID: 10300
	public GameObject OptionProfilePrefab;

	// Token: 0x0400283D RID: 10301
	public GameObject NewProfileButtonPrefab;

	// Token: 0x0400283E RID: 10302
	public VerticalLayoutGroup MainList;

	// Token: 0x0400283F RID: 10303
	public VerticalLayoutGroup ProfileList;

	// Token: 0x04002840 RID: 10304
	public VerticalLayoutGroup ProfileListEmpty;

	// Token: 0x04002841 RID: 10305
	public TextMeshProUGUI NoSavedDisplay;

	// Token: 0x04002842 RID: 10306
	public float OptionWidth = 282f;

	// Token: 0x04002843 RID: 10307
	public CursorTargetButton CloseButton;

	// Token: 0x04002844 RID: 10308
	public CursorTargetButton SaveButton;

	// Token: 0x04002845 RID: 10309
	public CursorTargetButton CancelButton;

	// Token: 0x04002846 RID: 10310
	private VerticalLayoutGroup currentProfileList;

	// Token: 0x04002848 RID: 10312
	private List<OptionGUI> currentOptions = new List<OptionGUI>();

	// Token: 0x04002849 RID: 10313
	private float alpha;

	// Token: 0x0400284A RID: 10314
	private bool isClosed = true;

	// Token: 0x0400284B RID: 10315
	private GameLoadPayload gamePayload;

	// Token: 0x0400284C RID: 10316
	private GameMode rules;

	// Token: 0x0400284D RID: 10317
	private GameRules mode;

	// Token: 0x0400284E RID: 10318
	private CreateNewOptionsProfile newProfileEntry;

	// Token: 0x0400284F RID: 10319
	private List<OptionProfileGUI> currentProfileDisplay;
}
