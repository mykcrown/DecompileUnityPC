// Decompile from assembly: Assembly-CSharp.dll

using System;

public class StoreTabElement : ClientBehavior
{
	public TabDefinition Def;

	private Func<StoreTab, bool> _allowInteraction;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IStoreTabsModel storeTabsModel
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

	[Inject]
	public IStoreAPI storeAPI
	{
		get;
		set;
	}

	public Func<StoreTab, bool> AllowInteraction
	{
		set
		{
			this._allowInteraction = value;
		}
	}

	protected bool allowInteraction()
	{
		return this.storeAPI.Mode == StoreMode.NORMAL && this._allowInteraction((StoreTab)this.Def.id);
	}

	public virtual bool OnCancelPressed()
	{
		return false;
	}

	public virtual bool OnBackButtonClicked()
	{
		return false;
	}

	public virtual bool OnRightTriggerPressed()
	{
		return false;
	}

	public virtual bool OnLeftTriggerPressed()
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

	public virtual void UpdateRightStick(float x, float y)
	{
	}

	public virtual void OnSubmitPressed()
	{
	}

	public virtual void OnZPressed()
	{
	}

	public virtual void OnLeftBumperPressed()
	{
	}

	public virtual void OnRightStickUp()
	{
	}

	public virtual void OnRightStickDown()
	{
	}

	public virtual void OnDrawComplete()
	{
	}

	public virtual void OnActivate()
	{
	}

	public virtual void UpdateMouseMode()
	{
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
}
