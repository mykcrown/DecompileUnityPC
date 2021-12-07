// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class SettingsTabElement : ClientBehavior
{
	public InputInstructions SelectInstructionsPrefab;

	public Transform SelectInstructionsAnchor;

	public InputInstructions ResetInstructionsPrefab;

	public Transform ResetInstructionsAnchor;

	public InputInstructions SaveInstructionsPrefab;

	public Transform SaveInstructionsAnchor;

	public UnbindText SaveTextPrefab;

	public Transform SaveTextAnchor;

	private InputInstructions selectInstructions;

	private InputInstructions resetInstructions;

	private InputInstructions saveInstructions;

	protected UnbindText SaveText;

	public TabDefinition Def;

	private Func<SettingsTab, bool> _allowInteraction;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public ISettingsTabsModel settingsTabsModel
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public UIManager uiManager
	{
		get;
		set;
	}

	public Func<SettingsTab, bool> AllowInteraction
	{
		set
		{
			this._allowInteraction = value;
		}
	}

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
			WavedashUIButton expr_C0 = this.resetInstructions.MouseInstuctionsButton;
			expr_C0.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(expr_C0.OnPointerClickEvent, new Action<InputEventData>(this.ResetClicked));
			this.resetInstructions.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
		}
		if (this.SaveInstructionsAnchor != null && this.SaveInstructionsPrefab != null)
		{
			this.saveInstructions = UnityEngine.Object.Instantiate<InputInstructions>(this.SaveInstructionsPrefab);
			this.saveInstructions.transform.SetParent(this.SaveInstructionsAnchor, false);
			WavedashUIButton expr_152 = this.saveInstructions.MouseInstuctionsButton;
			expr_152.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(expr_152.OnPointerClickEvent, new Action<InputEventData>(this.SaveClicked));
			this.saveInstructions.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
		}
		if (this.SaveTextAnchor != null && this.SaveTextPrefab != null)
		{
			this.SaveText = UnityEngine.Object.Instantiate<UnbindText>(this.SaveTextPrefab, this.SaveTextAnchor);
			this.SaveText.transform.SetParent(this.SaveTextAnchor, false);
		}
	}

	protected bool allowInteraction()
	{
		return this._allowInteraction((SettingsTab)this.Def.id);
	}

	public virtual bool OnRightTriggerPressed()
	{
		return false;
	}

	public virtual bool OnLeftTriggerPressed()
	{
		return false;
	}

	public virtual bool OnCancelPressed()
	{
		return false;
	}

	public virtual bool OnRightStickLeft()
	{
		return false;
	}

	public virtual bool OnRightStickRight()
	{
		return false;
	}

	public virtual bool OnDPadLeft()
	{
		return false;
	}

	public virtual bool OnDPadRight()
	{
		return false;
	}

	public virtual void OnDrawComplete()
	{
	}

	public virtual void OnActivate()
	{
	}

	public virtual void OnDeactivate()
	{
	}

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

	public virtual void OnLeft()
	{
	}

	public virtual void OnRight()
	{
	}

	public virtual void OnUp()
	{
	}

	public virtual void OnDown()
	{
	}

	public virtual void OnYButtonPressed()
	{
	}

	public virtual void OnXButtonPressed()
	{
	}

	public virtual void ResetClicked(InputEventData eventData)
	{
	}

	public virtual void SaveClicked(InputEventData eventData)
	{
	}
}
