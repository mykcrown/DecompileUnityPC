// Decompile from assembly: Assembly-CSharp.dll

using System;

public class StoreTabWithEquipModule : ClientBehavior
{
	public EquipmentSelectorModule EquipModule;

	private Func<bool> _allowInteraction;

	[Inject]
	public ILocalization localization
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

	public virtual IEquipmentSelectorModule module
	{
		get;
		set;
	}

	public Func<bool> AllowInteraction
	{
		set
		{
			this._allowInteraction = value;
		}
	}

	public virtual void Init()
	{
		base.injector.Inject(this.EquipModule);
		this.module = this.EquipModule;
		base.listen(UIManager.WINDOW_CLOSED, new Action(this.onWindowClosed));
	}

	protected bool allowInteraction()
	{
		return this._allowInteraction();
	}

	protected virtual bool isEquipViewActive()
	{
		return true;
	}

	public virtual void OnActivate()
	{
		this.module.ForceRedraws();
		if (this.isEquipViewActive())
		{
			this.module.BeginMenuFocus();
		}
	}

	public virtual void UpdateMouseMode()
	{
		this.module.OnMouseModeUpdate();
		if (this.isEquipModuleFocused())
		{
			this.module.SyncButtonModeSelection();
		}
	}

	protected bool isMouseMode()
	{
		return (this.uiManager.CurrentInputModule as UIInputModule).IsMouseMode;
	}

	public void OnRight()
	{
		if (this.isEquipModuleFocused())
		{
			this.module.OnRight();
		}
	}

	public void OnYButtonPressed()
	{
		if (this.isEquipModuleFocused())
		{
			this.module.OnYButton();
		}
	}

	public virtual void OnZPressed()
	{
	}

	public virtual void OnRightStickUp()
	{
	}

	public virtual void OnRightStickDown()
	{
	}

	public virtual void UpdateRightStick(float x, float y)
	{
	}

	private void onWindowClosed()
	{
		if (this.allowInteraction())
		{
			this.module.SyncButtonModeSelection();
		}
	}

	protected virtual bool isEquipModuleFocused()
	{
		return this.allowInteraction();
	}
}
