using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

// Token: 0x020009F5 RID: 2549
public class NetsukeEquipView : ClientBehavior
{
	// Token: 0x17001165 RID: 4453
	// (get) Token: 0x060048C4 RID: 18628 RVA: 0x0013A878 File Offset: 0x00138C78
	// (set) Token: 0x060048C5 RID: 18629 RVA: 0x0013A880 File Offset: 0x00138C80
	[Inject]
	public ICollectiblesTabAPI collectiblesTabAPI { get; set; }

	// Token: 0x17001166 RID: 4454
	// (get) Token: 0x060048C6 RID: 18630 RVA: 0x0013A889 File Offset: 0x00138C89
	// (set) Token: 0x060048C7 RID: 18631 RVA: 0x0013A891 File Offset: 0x00138C91
	[Inject]
	public INetsukeEquipViewAPI api { get; set; }

	// Token: 0x17001167 RID: 4455
	// (get) Token: 0x060048C8 RID: 18632 RVA: 0x0013A89A File Offset: 0x00138C9A
	// (set) Token: 0x060048C9 RID: 18633 RVA: 0x0013A8A2 File Offset: 0x00138CA2
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17001168 RID: 4456
	// (get) Token: 0x060048CA RID: 18634 RVA: 0x0013A8AB File Offset: 0x00138CAB
	// (set) Token: 0x060048CB RID: 18635 RVA: 0x0013A8B3 File Offset: 0x00138CB3
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x17001169 RID: 4457
	// (get) Token: 0x060048CC RID: 18636 RVA: 0x0013A8BC File Offset: 0x00138CBC
	// (set) Token: 0x060048CD RID: 18637 RVA: 0x0013A8C4 File Offset: 0x00138CC4
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x1700116A RID: 4458
	// (get) Token: 0x060048CE RID: 18638 RVA: 0x0013A8CD File Offset: 0x00138CCD
	// (set) Token: 0x060048CF RID: 18639 RVA: 0x0013A8D5 File Offset: 0x00138CD5
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x1700116B RID: 4459
	// (get) Token: 0x060048D0 RID: 18640 RVA: 0x0013A8DE File Offset: 0x00138CDE
	// (set) Token: 0x060048D1 RID: 18641 RVA: 0x0013A8E6 File Offset: 0x00138CE6
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x1700116C RID: 4460
	// (get) Token: 0x060048D2 RID: 18642 RVA: 0x0013A8EF File Offset: 0x00138CEF
	// (set) Token: 0x060048D3 RID: 18643 RVA: 0x0013A8F7 File Offset: 0x00138CF7
	[Inject]
	public ISelectionManager selectionManager { get; set; }

	// Token: 0x060048D4 RID: 18644 RVA: 0x0013A900 File Offset: 0x00138D00
	[PostConstruct]
	public void Init()
	{
		this.storeScene = base.uiAdapter.GetUIScene<StoreScene3D>();
		this.storeScene.Show3DNetsukeSelector(this.Selector3DPrefab, this.ItemDisplay3D);
		this.storeScene.FadeOutNetsukeSelector(0f);
		this.menu = base.injector.GetInstance<MenuItemList>();
		this.menu.SetNavigationType(MenuItemList.NavigationType.InOrderHorizontal, 0);
		this.menu.AddButton(this.DoneButton, new Action(this.onDoneButton));
		this.menu.AddButton(this.CancelButton, new Action(this.onCancelButton));
		this.MouseButtonTurnLeft.InteractableButton.Unselectable = true;
		this.MouseButtonTurnRight.InteractableButton.Unselectable = true;
		this.MouseButtonCenter.InteractableButton.Unselectable = true;
		WavedashUIButton interactableButton = this.MouseButtonTurnLeft.InteractableButton;
		interactableButton.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(interactableButton.OnPointerClickEvent, new Action<InputEventData>(this.mouseButtonTurnLeft));
		WavedashUIButton interactableButton2 = this.MouseButtonTurnRight.InteractableButton;
		interactableButton2.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(interactableButton2.OnPointerClickEvent, new Action<InputEventData>(this.mouseButtonTurnRight));
		WavedashUIButton interactableButton3 = this.MouseButtonCenter.InteractableButton;
		interactableButton3.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(interactableButton3.OnPointerClickEvent, new Action<InputEventData>(this.mouseButtonCenter));
		this.menu.Initialize();
		this.addInputInstructions();
		this.animateArrow();
		base.listen("CollectiblesTabAPI.UPDATED", new Action(this.onUpdated));
		base.listen("StoreTabSelectionModel.STORE_TAB_UPDATED", new Action(this.onUpdated));
		base.listen(NetsukeEquipViewAPI.UPDATED, new Action(this.onUpdated));
		base.listen(UserGlobalEquippedModel.UPDATED, new Action(this.onUpdated));
		this.onUpdated();
	}

	// Token: 0x060048D5 RID: 18645 RVA: 0x0013AAD0 File Offset: 0x00138ED0
	private void animateArrow()
	{
		Vector3 localPosition = this.Arrow.localPosition;
		localPosition.y -= 10f;
		DOTween.To(() => this.Arrow.localPosition, delegate(Vector3 valueIn)
		{
			this.Arrow.localPosition = valueIn;
		}, localPosition, 2f).SetEase(Ease.InOutSine).OnComplete(new TweenCallback(this.animateArrowBack));
	}

	// Token: 0x060048D6 RID: 18646 RVA: 0x0013AB38 File Offset: 0x00138F38
	private void animateArrowBack()
	{
		Vector3 localPosition = this.Arrow.localPosition;
		localPosition.y += 10f;
		DOTween.To(() => this.Arrow.localPosition, delegate(Vector3 valueIn)
		{
			this.Arrow.localPosition = valueIn;
		}, localPosition, 2f).SetEase(Ease.InOutSine).OnComplete(new TweenCallback(this.animateArrow));
	}

	// Token: 0x060048D7 RID: 18647 RVA: 0x0013ABA0 File Offset: 0x00138FA0
	private void addInputInstructions()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.RotateNavigationPrefabLeft);
		gameObject.transform.SetParent(this.RotateNavigationAnchorLeft, false);
		this.rotateInstructionsLeft = gameObject.GetComponent<InputInstructions>();
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.RotateNavigationPrefabRight);
		gameObject2.transform.SetParent(this.RotateNavigationAnchorRight, false);
		this.rotateInstructionsRight = gameObject2.GetComponent<InputInstructions>();
		this.backButton = UnityEngine.Object.Instantiate<GameObject>(this.BackButtonPrefab).GetComponent<InputInstructions>();
		this.backButton.transform.SetParent(this.BackButtonAnchor, false);
		this.UpdateMouseMode();
	}

	// Token: 0x060048D8 RID: 18648 RVA: 0x0013AC34 File Offset: 0x00139034
	private void mouseButtonTurnLeft(InputEventData eventData)
	{
		this.api.TurnLeft();
	}

	// Token: 0x060048D9 RID: 18649 RVA: 0x0013AC41 File Offset: 0x00139041
	private void mouseButtonTurnRight(InputEventData eventData)
	{
		this.api.TurnRight();
	}

	// Token: 0x060048DA RID: 18650 RVA: 0x0013AC4E File Offset: 0x0013904E
	private void mouseButtonCenter(InputEventData eventData)
	{
		this.api.EquipNetsuke(this.api.SelectedItem, this.api.SelectedIndex);
	}

	// Token: 0x060048DB RID: 18651 RVA: 0x0013AC71 File Offset: 0x00139071
	private void onDoneButton()
	{
		this.api.SaveEdit();
		this.collectiblesTabAPI.SetState(CollectiblesTabState.EquipView, false);
	}

	// Token: 0x060048DC RID: 18652 RVA: 0x0013AC8B File Offset: 0x0013908B
	private void onCancelButton()
	{
		this.api.DiscardEdit();
		this.collectiblesTabAPI.SetState(CollectiblesTabState.EquipView, false);
	}

	// Token: 0x060048DD RID: 18653 RVA: 0x0013ACA8 File Offset: 0x001390A8
	private void onUpdated()
	{
		if (this.ZoomAnimator.gameObject.activeInHierarchy)
		{
			this.ZoomAnimator.SetBool("initialize", true);
			if (this.ZoomAnimator.gameObject.activeInHierarchy)
			{
				if (this.collectiblesTabAPI.GetState() == CollectiblesTabState.NetsukeEquipView)
				{
					this.ZoomAnimator.SetBool("visible", true);
					if (this.previousTabState != this.collectiblesTabAPI.GetState())
					{
						this.activatedThisFrame = true;
						this.api.BeginEdit();
						this.showBeforeTransition();
					}
				}
				else
				{
					this.ZoomAnimator.SetBool("visible", false);
					this.hideAfterTransition();
				}
			}
		}
		this.previousTabState = this.collectiblesTabAPI.GetState();
		if (this.collectiblesTabAPI.GetState() == CollectiblesTabState.NetsukeEquipView)
		{
			this.update3dSelector();
			this.updateRotation();
			this.updateNetsukeName();
		}
	}

	// Token: 0x060048DE RID: 18654 RVA: 0x0013AD90 File Offset: 0x00139190
	private void LateUpdate()
	{
		if (this.NetsukeFadeout != this._netsukeFadeout)
		{
			this._netsukeFadeout = this.NetsukeFadeout;
			this.storeScene.FadeOutNetsukeSelector(this._netsukeFadeout);
		}
		if (this.UIContainer.localScale != this._transformScale)
		{
			this._transformScale = this.UIContainer.localScale;
			this.storeScene.ScaleNetsukeSelector(this._transformScale);
		}
		this.activatedThisFrame = false;
	}

	// Token: 0x060048DF RID: 18655 RVA: 0x0013AE10 File Offset: 0x00139210
	private void updateNetsukeName()
	{
		EquippableItem equippedNetsuke = this.api.GetEquippedNetsuke(this.api.SelectedIndex);
		if (equippedNetsuke == null)
		{
			this.ItemTitle.text = this.localization.GetText("ui.store.netsuke.empty");
		}
		else if (equippedNetsuke.backupNameText == null)
		{
			this.ItemTitle.text = "~ERROR NO NAME~";
		}
		else
		{
			this.ItemTitle.text = equippedNetsuke.backupNameText.ToUpper();
		}
	}

	// Token: 0x060048E0 RID: 18656 RVA: 0x0013AE90 File Offset: 0x00139290
	private void update3dSelector()
	{
		for (int i = 0; i < UserGlobalEquippedModel.NETSUKE_SLOTS; i++)
		{
			EquippableItem equippedNetsuke = this.api.GetEquippedNetsuke(i);
			if (equippedNetsuke == null)
			{
				this.storeScene.SetNetsukeOn3DSelector(null, i);
			}
			else
			{
				Netsuke netsukeFromItem = this.equipmentModel.GetNetsukeFromItem(equippedNetsuke.id);
				this.storeScene.SetNetsukeOn3DSelector(netsukeFromItem, i);
			}
		}
	}

	// Token: 0x060048E1 RID: 18657 RVA: 0x0013AEF8 File Offset: 0x001392F8
	private void updateRotation()
	{
		float netsukeSelectorRotate = (float)(this.api.SpinIndex * 120);
		this.storeScene.SetNetsukeSelectorRotate(netsukeSelectorRotate);
	}

	// Token: 0x060048E2 RID: 18658 RVA: 0x0013AF24 File Offset: 0x00139324
	private void showBeforeTransition()
	{
		base.transform.localPosition = Vector3.zero;
		this.storeScene.SetShadowMode(false, true);
		this.timer.CancelTimeout(new Action(this.onHideComplete));
		this.timer.SetTimeout(210, new Action(this.onShowComplete));
	}

	// Token: 0x060048E3 RID: 18659 RVA: 0x0013AF81 File Offset: 0x00139381
	private void onShowComplete()
	{
		this.storeScene.SetShadowMode(true, false);
	}

	// Token: 0x060048E4 RID: 18660 RVA: 0x0013AF90 File Offset: 0x00139390
	private void hideAfterTransition()
	{
		this.timer.CancelTimeout(new Action(this.onShowComplete));
		this.timer.SetTimeout(150, new Action(this.onHideComplete));
	}

	// Token: 0x060048E5 RID: 18661 RVA: 0x0013AFC5 File Offset: 0x001393C5
	private void onHideComplete()
	{
		if (base.transform != null)
		{
			base.transform.localPosition = new Vector3(1000000f, 0f, 0f);
		}
	}

	// Token: 0x060048E6 RID: 18662 RVA: 0x0013AFF7 File Offset: 0x001393F7
	protected bool allowInteraction()
	{
		return this._allowInteraction != null && this._allowInteraction();
	}

	// Token: 0x1700116D RID: 4461
	// (set) Token: 0x060048E7 RID: 18663 RVA: 0x0013B011 File Offset: 0x00139411
	public Func<bool> AllowInteraction
	{
		set
		{
			this._allowInteraction = value;
		}
	}

	// Token: 0x060048E8 RID: 18664 RVA: 0x0013B01A File Offset: 0x0013941A
	public void OnStateActivate()
	{
		if (this.allowInteraction())
		{
			this.syncButtonNavigation();
		}
	}

	// Token: 0x060048E9 RID: 18665 RVA: 0x0013B030 File Offset: 0x00139430
	public void UpdateMouseMode()
	{
		if (this.allowInteraction())
		{
			this.syncButtonNavigation();
		}
		if (this.rotateInstructionsLeft != null)
		{
			this.rotateInstructionsLeft.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
			this.rotateInstructionsRight.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
			this.backButton.SetControlMode(this.uiManager.CurrentInputModule.CurrentMode);
			this.backButton.MouseInstuctions.gameObject.SetActive(!this.uiManager.CurrentInputModule.IsMouseMode);
		}
		this.MouseButtonsContainer.SetActive(this.uiManager.CurrentInputModule.IsMouseMode);
	}

	// Token: 0x060048EA RID: 18666 RVA: 0x0013B0F3 File Offset: 0x001394F3
	public bool OnLeftTriggerPressed()
	{
		if (this.allowInteraction())
		{
			this.api.TurnLeft();
			return true;
		}
		return false;
	}

	// Token: 0x060048EB RID: 18667 RVA: 0x0013B10E File Offset: 0x0013950E
	public bool OnRightTriggerPressed()
	{
		if (this.allowInteraction())
		{
			this.api.TurnRight();
			return true;
		}
		return false;
	}

	// Token: 0x060048EC RID: 18668 RVA: 0x0013B129 File Offset: 0x00139529
	public bool OnSubmitPressed()
	{
		if (this.activatedThisFrame)
		{
			return true;
		}
		if (this.allowInteraction())
		{
			this.api.EquipNetsuke(this.api.SelectedItem, this.api.SelectedIndex);
			return true;
		}
		return false;
	}

	// Token: 0x060048ED RID: 18669 RVA: 0x0013B167 File Offset: 0x00139567
	public bool OnCancelPressed()
	{
		if (!this.allowInteraction())
		{
			return false;
		}
		this.api.SaveEdit();
		this.collectiblesTabAPI.SetState(CollectiblesTabState.EquipView, false);
		return true;
	}

	// Token: 0x060048EE RID: 18670 RVA: 0x0013B18F File Offset: 0x0013958F
	private bool isButtonMode()
	{
		return !this.uiManager.CurrentInputModule.IsMouseMode && this.windowDisplay.GetWindowCount() == 0;
	}

	// Token: 0x060048EF RID: 18671 RVA: 0x0013B1B7 File Offset: 0x001395B7
	private void syncButtonNavigation()
	{
		if (this.isButtonMode())
		{
			if (this.menu.CurrentSelection != null)
			{
				this.menu.RemoveSelection();
			}
			else
			{
				this.selectionManager.Select(null);
			}
		}
	}

	// Token: 0x060048F0 RID: 18672 RVA: 0x0013B1F6 File Offset: 0x001395F6
	public override void OnDestroy()
	{
		this.timer.CancelTimeout(new Action(this.onHideComplete));
		this.timer.CancelTimeout(new Action(this.onShowComplete));
		base.OnDestroy();
	}

	// Token: 0x0400301D RID: 12317
	public Animator ZoomAnimator;

	// Token: 0x0400301E RID: 12318
	public GameObject Selector3DPrefab;

	// Token: 0x0400301F RID: 12319
	public Transform ItemDisplay3D;

	// Token: 0x04003020 RID: 12320
	public Transform UIContainer;

	// Token: 0x04003021 RID: 12321
	public Transform Arrow;

	// Token: 0x04003022 RID: 12322
	public GameObject MouseButtonsContainer;

	// Token: 0x04003023 RID: 12323
	public MenuItemButton CancelButton;

	// Token: 0x04003024 RID: 12324
	public MenuItemButton DoneButton;

	// Token: 0x04003025 RID: 12325
	public MenuItemButton MouseButtonTurnLeft;

	// Token: 0x04003026 RID: 12326
	public MenuItemButton MouseButtonTurnRight;

	// Token: 0x04003027 RID: 12327
	public MenuItemButton MouseButtonCenter;

	// Token: 0x04003028 RID: 12328
	public TextMeshProUGUI ItemTitle;

	// Token: 0x04003029 RID: 12329
	public Transform RotateNavigationAnchorLeft;

	// Token: 0x0400302A RID: 12330
	public Transform RotateNavigationAnchorRight;

	// Token: 0x0400302B RID: 12331
	public Transform BackButtonAnchor;

	// Token: 0x0400302C RID: 12332
	public GameObject BackButtonPrefab;

	// Token: 0x0400302D RID: 12333
	public GameObject RotateNavigationPrefabLeft;

	// Token: 0x0400302E RID: 12334
	public GameObject RotateNavigationPrefabRight;

	// Token: 0x0400302F RID: 12335
	private StoreScene3D storeScene;

	// Token: 0x04003030 RID: 12336
	private InputInstructions rotateInstructionsLeft;

	// Token: 0x04003031 RID: 12337
	private InputInstructions rotateInstructionsRight;

	// Token: 0x04003032 RID: 12338
	private InputInstructions backButton;

	// Token: 0x04003033 RID: 12339
	private MenuItemList menu;

	// Token: 0x04003034 RID: 12340
	private Func<bool> _allowInteraction;

	// Token: 0x04003035 RID: 12341
	public float NetsukeFadeout;

	// Token: 0x04003036 RID: 12342
	private float _netsukeFadeout;

	// Token: 0x04003037 RID: 12343
	private Vector3 _transformScale = new Vector3(1f, 1f, 1f);

	// Token: 0x04003038 RID: 12344
	private CollectiblesTabState previousTabState;

	// Token: 0x04003039 RID: 12345
	private bool activatedThisFrame;
}
