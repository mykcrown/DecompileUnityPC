// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CollectiblesTab : StoreTabElement
{
	public CollectiblesIntroView IntroViewPrefab;

	public CollectiblesEquipView EquipViewPrefab;

	public NetsukeEquipView NetsukeEquipViewPrefab;

	private CollectiblesIntroView introView;

	private CollectiblesEquipView equipView;

	private NetsukeEquipView netsukeEquipView;

	private CollectiblesTabState currentState;

	[Inject]
	public ICollectiblesTabAPI api
	{
		get;
		set;
	}

	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI equipAPI
	{
		get;
		set;
	}

	public override void Awake()
	{
		base.Awake();
		this.introView = UnityEngine.Object.Instantiate<CollectiblesIntroView>(this.IntroViewPrefab, base.transform);
		this.equipView = UnityEngine.Object.Instantiate<CollectiblesEquipView>(this.EquipViewPrefab, base.transform);
		this.netsukeEquipView = UnityEngine.Object.Instantiate<NetsukeEquipView>(this.NetsukeEquipViewPrefab, base.transform);
		this.introView.AllowInteraction = new Func<bool>(this.isIntroViewFocused);
		this.equipView.AllowInteraction = new Func<bool>(this.isEquipViewFocused);
		this.netsukeEquipView.AllowInteraction = new Func<bool>(this.isNetsukeEquipViewFocused);
		CollectiblesIntroView expr_96 = this.introView;
		expr_96.EquipmentTypeSelected = (Action<EquipmentTypes>)Delegate.Combine(expr_96.EquipmentTypeSelected, new Action<EquipmentTypes>(this._Awake_m__0));
	}

	[PostConstruct]
	public void Init()
	{
		base.listen("CollectiblesTabAPI.UPDATED", new Action(this.onTabStateUpdated));
	}

	private void onTabStateUpdated()
	{
		if (this.currentState != this.api.GetState())
		{
			this.currentState = this.api.GetState();
			if (this.currentState == CollectiblesTabState.EquipView)
			{
				this.introView.ReleaseSelections();
				this.equipView.OnStateActivate();
			}
			else if (this.currentState == CollectiblesTabState.IntroView)
			{
				this.equipView.ReleaseSelections();
				this.introView.IntroPanelView.SyncButtonSelectionToEquipView();
			}
			else if (this.currentState == CollectiblesTabState.NetsukeEquipView)
			{
				this.equipView.ReleaseSelections();
				this.netsukeEquipView.OnStateActivate();
			}
		}
	}

	private bool isIntroViewFocused()
	{
		return base.allowInteraction() && this.api.GetState() == CollectiblesTabState.IntroView;
	}

	private bool isEquipViewFocused()
	{
		return base.allowInteraction() && this.api.GetState() == CollectiblesTabState.EquipView;
	}

	private bool isNetsukeEquipViewFocused()
	{
		return base.allowInteraction() && this.api.GetState() == CollectiblesTabState.NetsukeEquipView;
	}

	public override void OnDrawComplete()
	{
		this.equipView.OnDrawComplete();
		this.introView.OnDrawComplete();
	}

	public override bool OnCancelPressed()
	{
		bool flag = this.equipView.OnCancelPressed();
		if (!flag)
		{
			flag = this.netsukeEquipView.OnCancelPressed();
		}
		if (!flag)
		{
			flag = this.tryReturnToIntroView();
		}
		return flag;
	}

	public override bool OnBackButtonClicked()
	{
		return this.tryReturnToIntroView();
	}

	private bool tryReturnToIntroView()
	{
		if (this.api.GetState() == CollectiblesTabState.EquipView)
		{
			this.api.SetState(CollectiblesTabState.IntroView, false);
			base.audioManager.PlayMenuSound(SoundKey.store_collectiblesEquipViewClosed, 0f);
			return true;
		}
		return false;
	}

	public override void OnLeft()
	{
		this.equipView.OnLeft();
	}

	public override void OnActivate()
	{
		base.OnActivate();
		this.introView.OnActivate();
		this.equipView.OnActivate();
	}

	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.introView.UpdateMouseMode();
		this.equipView.UpdateMouseMode();
		this.netsukeEquipView.UpdateMouseMode();
	}

	public override void OnRight()
	{
		this.equipView.OnRight();
	}

	public override void OnSubmitPressed()
	{
		this.netsukeEquipView.OnSubmitPressed();
	}

	public override void OnYButtonPressed()
	{
		this.equipView.OnYButtonPressed();
	}

	public override bool OnRightTriggerPressed()
	{
		return this.netsukeEquipView.OnRightTriggerPressed();
	}

	public override bool OnLeftTriggerPressed()
	{
		return this.netsukeEquipView.OnLeftTriggerPressed();
	}

	public override void UpdateRightStick(float x, float y)
	{
		this.equipView.UpdateRightStick(x, y);
	}

	private void _Awake_m__0(EquipmentTypes type)
	{
		this.equipAPI.SelectedEquipType = type;
		this.api.SetState(CollectiblesTabState.EquipView, false);
		this.introView.PlayTransition();
		this.equipView.PlayTransition();
		base.audioManager.PlayMenuSound(SoundKey.store_collectiblesEquipViewOpen, 0f);
	}
}
