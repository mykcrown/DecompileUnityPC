using System;
using UnityEngine;

// Token: 0x02000983 RID: 2435
public class SettingsTabElement : ClientBehavior
{
	// Token: 0x17000F89 RID: 3977
	// (get) Token: 0x060041DF RID: 16863 RVA: 0x001213CC File Offset: 0x0011F7CC
	// (set) Token: 0x060041E0 RID: 16864 RVA: 0x001213D4 File Offset: 0x0011F7D4
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000F8A RID: 3978
	// (get) Token: 0x060041E1 RID: 16865 RVA: 0x001213DD File Offset: 0x0011F7DD
	// (set) Token: 0x060041E2 RID: 16866 RVA: 0x001213E5 File Offset: 0x0011F7E5
	[Inject]
	public ISettingsTabsModel settingsTabsModel { get; set; }

	// Token: 0x17000F8B RID: 3979
	// (get) Token: 0x060041E3 RID: 16867 RVA: 0x001213EE File Offset: 0x0011F7EE
	// (set) Token: 0x060041E4 RID: 16868 RVA: 0x001213F6 File Offset: 0x0011F7F6
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000F8C RID: 3980
	// (get) Token: 0x060041E5 RID: 16869 RVA: 0x001213FF File Offset: 0x0011F7FF
	// (set) Token: 0x060041E6 RID: 16870 RVA: 0x00121407 File Offset: 0x0011F807
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17000F8D RID: 3981
	// (get) Token: 0x060041E7 RID: 16871 RVA: 0x00121410 File Offset: 0x0011F810
	// (set) Token: 0x060041E8 RID: 16872 RVA: 0x00121418 File Offset: 0x0011F818
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x060041E9 RID: 16873 RVA: 0x00121424 File Offset: 0x0011F824
	public override void Awake()
	{
		base.Awake();
		if (this.SelectInstructionsAnchor != null && this.SelectInstructionsPrefab != null)
		{
			this.selectInstructions = UnityEngine.Object.Instantiate<InputInstructions>(this.SelectInstructionsPrefab);
			this.selectInstructions.transform.SetParent(this.SelectInstructionsAnchor, false);
			this.selectInstructions.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
		}
		if (this.ResetInstructionsAnchor != null && this.ResetInstructionsPrefab != null)
		{
			this.resetInstructions = UnityEngine.Object.Instantiate<InputInstructions>(this.ResetInstructionsPrefab);
			this.resetInstructions.transform.SetParent(this.ResetInstructionsAnchor, false);
			WavedashUIButton mouseInstuctionsButton = this.resetInstructions.MouseInstuctionsButton;
			mouseInstuctionsButton.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(mouseInstuctionsButton.OnPointerClickEvent, new Action<InputEventData>(this.ResetClicked));
			this.resetInstructions.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
		}
		if (this.SaveInstructionsAnchor != null && this.SaveInstructionsPrefab != null)
		{
			this.saveInstructions = UnityEngine.Object.Instantiate<InputInstructions>(this.SaveInstructionsPrefab);
			this.saveInstructions.transform.SetParent(this.SaveInstructionsAnchor, false);
			WavedashUIButton mouseInstuctionsButton2 = this.saveInstructions.MouseInstuctionsButton;
			mouseInstuctionsButton2.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(mouseInstuctionsButton2.OnPointerClickEvent, new Action<InputEventData>(this.SaveClicked));
			this.saveInstructions.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
		}
		if (this.SaveTextAnchor != null && this.SaveTextPrefab != null)
		{
			this.SaveText = UnityEngine.Object.Instantiate<UnbindText>(this.SaveTextPrefab, this.SaveTextAnchor);
			this.SaveText.transform.SetParent(this.SaveTextAnchor, false);
		}
	}

	// Token: 0x060041EA RID: 16874 RVA: 0x00121610 File Offset: 0x0011FA10
	protected bool allowInteraction()
	{
		return this._allowInteraction((SettingsTab)this.Def.id);
	}

	// Token: 0x17000F8E RID: 3982
	// (set) Token: 0x060041EB RID: 16875 RVA: 0x00121628 File Offset: 0x0011FA28
	public Func<SettingsTab, bool> AllowInteraction
	{
		set
		{
			this._allowInteraction = value;
		}
	}

	// Token: 0x060041EC RID: 16876 RVA: 0x00121631 File Offset: 0x0011FA31
	public virtual bool OnRightTriggerPressed()
	{
		return false;
	}

	// Token: 0x060041ED RID: 16877 RVA: 0x00121634 File Offset: 0x0011FA34
	public virtual bool OnLeftTriggerPressed()
	{
		return false;
	}

	// Token: 0x060041EE RID: 16878 RVA: 0x00121637 File Offset: 0x0011FA37
	public virtual bool OnCancelPressed()
	{
		return false;
	}

	// Token: 0x060041EF RID: 16879 RVA: 0x0012163A File Offset: 0x0011FA3A
	public virtual bool OnRightStickLeft()
	{
		return false;
	}

	// Token: 0x060041F0 RID: 16880 RVA: 0x0012163D File Offset: 0x0011FA3D
	public virtual bool OnRightStickRight()
	{
		return false;
	}

	// Token: 0x060041F1 RID: 16881 RVA: 0x00121640 File Offset: 0x0011FA40
	public virtual bool OnDPadLeft()
	{
		return false;
	}

	// Token: 0x060041F2 RID: 16882 RVA: 0x00121643 File Offset: 0x0011FA43
	public virtual bool OnDPadRight()
	{
		return false;
	}

	// Token: 0x060041F3 RID: 16883 RVA: 0x00121646 File Offset: 0x0011FA46
	public virtual void OnDrawComplete()
	{
	}

	// Token: 0x060041F4 RID: 16884 RVA: 0x00121648 File Offset: 0x0011FA48
	public virtual void OnActivate()
	{
	}

	// Token: 0x060041F5 RID: 16885 RVA: 0x0012164A File Offset: 0x0011FA4A
	public virtual void OnDeactivate()
	{
	}

	// Token: 0x060041F6 RID: 16886 RVA: 0x0012164C File Offset: 0x0011FA4C
	public virtual void UpdateMouseMode()
	{
		if (this.selectInstructions != null)
		{
			this.selectInstructions.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
		}
		if (this.resetInstructions != null)
		{
			this.resetInstructions.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
		}
		if (this.saveInstructions != null)
		{
			this.saveInstructions.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
		}
	}

	// Token: 0x060041F7 RID: 16887 RVA: 0x001216DD File Offset: 0x0011FADD
	public virtual void OnLeft()
	{
	}

	// Token: 0x060041F8 RID: 16888 RVA: 0x001216DF File Offset: 0x0011FADF
	public virtual void OnRight()
	{
	}

	// Token: 0x060041F9 RID: 16889 RVA: 0x001216E1 File Offset: 0x0011FAE1
	public virtual void OnUp()
	{
	}

	// Token: 0x060041FA RID: 16890 RVA: 0x001216E3 File Offset: 0x0011FAE3
	public virtual void OnDown()
	{
	}

	// Token: 0x060041FB RID: 16891 RVA: 0x001216E5 File Offset: 0x0011FAE5
	public virtual void OnYButtonPressed()
	{
	}

	// Token: 0x060041FC RID: 16892 RVA: 0x001216E7 File Offset: 0x0011FAE7
	public virtual void OnXButtonPressed()
	{
	}

	// Token: 0x060041FD RID: 16893 RVA: 0x001216E9 File Offset: 0x0011FAE9
	public virtual void ResetClicked(InputEventData eventData)
	{
	}

	// Token: 0x060041FE RID: 16894 RVA: 0x001216EB File Offset: 0x0011FAEB
	public virtual void SaveClicked(InputEventData eventData)
	{
	}

	// Token: 0x04002C37 RID: 11319
	public InputInstructions SelectInstructionsPrefab;

	// Token: 0x04002C38 RID: 11320
	public Transform SelectInstructionsAnchor;

	// Token: 0x04002C39 RID: 11321
	public InputInstructions ResetInstructionsPrefab;

	// Token: 0x04002C3A RID: 11322
	public Transform ResetInstructionsAnchor;

	// Token: 0x04002C3B RID: 11323
	public InputInstructions SaveInstructionsPrefab;

	// Token: 0x04002C3C RID: 11324
	public Transform SaveInstructionsAnchor;

	// Token: 0x04002C3D RID: 11325
	public UnbindText SaveTextPrefab;

	// Token: 0x04002C3E RID: 11326
	public Transform SaveTextAnchor;

	// Token: 0x04002C3F RID: 11327
	private InputInstructions selectInstructions;

	// Token: 0x04002C40 RID: 11328
	private InputInstructions resetInstructions;

	// Token: 0x04002C41 RID: 11329
	private InputInstructions saveInstructions;

	// Token: 0x04002C42 RID: 11330
	protected UnbindText SaveText;

	// Token: 0x04002C43 RID: 11331
	public TabDefinition Def;

	// Token: 0x04002C44 RID: 11332
	private Func<SettingsTab, bool> _allowInteraction;
}
