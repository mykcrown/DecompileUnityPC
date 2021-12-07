using System;
using UnityEngine;

// Token: 0x020009F0 RID: 2544
public class CollectiblesTab : StoreTabElement
{
	// Token: 0x1700115E RID: 4446
	// (get) Token: 0x06004896 RID: 18582 RVA: 0x0013A35D File Offset: 0x0013875D
	// (set) Token: 0x06004897 RID: 18583 RVA: 0x0013A365 File Offset: 0x00138765
	[Inject]
	public ICollectiblesTabAPI api { get; set; }

	// Token: 0x1700115F RID: 4447
	// (get) Token: 0x06004898 RID: 18584 RVA: 0x0013A36E File Offset: 0x0013876E
	// (set) Token: 0x06004899 RID: 18585 RVA: 0x0013A376 File Offset: 0x00138776
	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI equipAPI { get; set; }

	// Token: 0x0600489A RID: 18586 RVA: 0x0013A380 File Offset: 0x00138780
	public override void Awake()
	{
		base.Awake();
		this.introView = UnityEngine.Object.Instantiate<CollectiblesIntroView>(this.IntroViewPrefab, base.transform);
		this.equipView = UnityEngine.Object.Instantiate<CollectiblesEquipView>(this.EquipViewPrefab, base.transform);
		this.netsukeEquipView = UnityEngine.Object.Instantiate<NetsukeEquipView>(this.NetsukeEquipViewPrefab, base.transform);
		this.introView.AllowInteraction = new Func<bool>(this.isIntroViewFocused);
		this.equipView.AllowInteraction = new Func<bool>(this.isEquipViewFocused);
		this.netsukeEquipView.AllowInteraction = new Func<bool>(this.isNetsukeEquipViewFocused);
		CollectiblesIntroView collectiblesIntroView = this.introView;
		collectiblesIntroView.EquipmentTypeSelected = (Action<EquipmentTypes>)Delegate.Combine(collectiblesIntroView.EquipmentTypeSelected, new Action<EquipmentTypes>(delegate(EquipmentTypes type)
		{
			this.equipAPI.SelectedEquipType = type;
			this.api.SetState(CollectiblesTabState.EquipView, false);
			this.introView.PlayTransition();
			this.equipView.PlayTransition();
			base.audioManager.PlayMenuSound(SoundKey.store_collectiblesEquipViewOpen, 0f);
		}));
	}

	// Token: 0x0600489B RID: 18587 RVA: 0x0013A444 File Offset: 0x00138844
	[PostConstruct]
	public void Init()
	{
		base.listen("CollectiblesTabAPI.UPDATED", new Action(this.onTabStateUpdated));
	}

	// Token: 0x0600489C RID: 18588 RVA: 0x0013A460 File Offset: 0x00138860
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

	// Token: 0x0600489D RID: 18589 RVA: 0x0013A508 File Offset: 0x00138908
	private bool isIntroViewFocused()
	{
		return base.allowInteraction() && this.api.GetState() == CollectiblesTabState.IntroView;
	}

	// Token: 0x0600489E RID: 18590 RVA: 0x0013A526 File Offset: 0x00138926
	private bool isEquipViewFocused()
	{
		return base.allowInteraction() && this.api.GetState() == CollectiblesTabState.EquipView;
	}

	// Token: 0x0600489F RID: 18591 RVA: 0x0013A544 File Offset: 0x00138944
	private bool isNetsukeEquipViewFocused()
	{
		return base.allowInteraction() && this.api.GetState() == CollectiblesTabState.NetsukeEquipView;
	}

	// Token: 0x060048A0 RID: 18592 RVA: 0x0013A562 File Offset: 0x00138962
	public override void OnDrawComplete()
	{
		this.equipView.OnDrawComplete();
		this.introView.OnDrawComplete();
	}

	// Token: 0x060048A1 RID: 18593 RVA: 0x0013A57C File Offset: 0x0013897C
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

	// Token: 0x060048A2 RID: 18594 RVA: 0x0013A5B5 File Offset: 0x001389B5
	public override bool OnBackButtonClicked()
	{
		return this.tryReturnToIntroView();
	}

	// Token: 0x060048A3 RID: 18595 RVA: 0x0013A5BD File Offset: 0x001389BD
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

	// Token: 0x060048A4 RID: 18596 RVA: 0x0013A5F2 File Offset: 0x001389F2
	public override void OnLeft()
	{
		this.equipView.OnLeft();
	}

	// Token: 0x060048A5 RID: 18597 RVA: 0x0013A600 File Offset: 0x00138A00
	public override void OnActivate()
	{
		base.OnActivate();
		this.introView.OnActivate();
		this.equipView.OnActivate();
	}

	// Token: 0x060048A6 RID: 18598 RVA: 0x0013A61E File Offset: 0x00138A1E
	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.introView.UpdateMouseMode();
		this.equipView.UpdateMouseMode();
		this.netsukeEquipView.UpdateMouseMode();
	}

	// Token: 0x060048A7 RID: 18599 RVA: 0x0013A647 File Offset: 0x00138A47
	public override void OnRight()
	{
		this.equipView.OnRight();
	}

	// Token: 0x060048A8 RID: 18600 RVA: 0x0013A654 File Offset: 0x00138A54
	public override void OnSubmitPressed()
	{
		this.netsukeEquipView.OnSubmitPressed();
	}

	// Token: 0x060048A9 RID: 18601 RVA: 0x0013A662 File Offset: 0x00138A62
	public override void OnYButtonPressed()
	{
		this.equipView.OnYButtonPressed();
	}

	// Token: 0x060048AA RID: 18602 RVA: 0x0013A66F File Offset: 0x00138A6F
	public override bool OnRightTriggerPressed()
	{
		return this.netsukeEquipView.OnRightTriggerPressed();
	}

	// Token: 0x060048AB RID: 18603 RVA: 0x0013A67C File Offset: 0x00138A7C
	public override bool OnLeftTriggerPressed()
	{
		return this.netsukeEquipView.OnLeftTriggerPressed();
	}

	// Token: 0x060048AC RID: 18604 RVA: 0x0013A689 File Offset: 0x00138A89
	public override void UpdateRightStick(float x, float y)
	{
		this.equipView.UpdateRightStick(x, y);
	}

	// Token: 0x04002FFE RID: 12286
	public CollectiblesIntroView IntroViewPrefab;

	// Token: 0x04002FFF RID: 12287
	public CollectiblesEquipView EquipViewPrefab;

	// Token: 0x04003000 RID: 12288
	public NetsukeEquipView NetsukeEquipViewPrefab;

	// Token: 0x04003001 RID: 12289
	private CollectiblesIntroView introView;

	// Token: 0x04003002 RID: 12290
	private CollectiblesEquipView equipView;

	// Token: 0x04003003 RID: 12291
	private NetsukeEquipView netsukeEquipView;

	// Token: 0x04003004 RID: 12292
	private CollectiblesTabState currentState;
}
