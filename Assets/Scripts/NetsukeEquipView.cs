// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class NetsukeEquipView : ClientBehavior
{
	public Animator ZoomAnimator;

	public GameObject Selector3DPrefab;

	public Transform ItemDisplay3D;

	public Transform UIContainer;

	public Transform Arrow;

	public GameObject MouseButtonsContainer;

	public MenuItemButton CancelButton;

	public MenuItemButton DoneButton;

	public MenuItemButton MouseButtonTurnLeft;

	public MenuItemButton MouseButtonTurnRight;

	public MenuItemButton MouseButtonCenter;

	public TextMeshProUGUI ItemTitle;

	public Transform RotateNavigationAnchorLeft;

	public Transform RotateNavigationAnchorRight;

	public Transform BackButtonAnchor;

	public GameObject BackButtonPrefab;

	public GameObject RotateNavigationPrefabLeft;

	public GameObject RotateNavigationPrefabRight;

	private StoreScene3D storeScene;

	private InputInstructions rotateInstructionsLeft;

	private InputInstructions rotateInstructionsRight;

	private InputInstructions backButton;

	private MenuItemList menu;

	private Func<bool> _allowInteraction;

	public float NetsukeFadeout;

	private float _netsukeFadeout;

	private Vector3 _transformScale = new Vector3(1f, 1f, 1f);

	private CollectiblesTabState previousTabState;

	private bool activatedThisFrame;

	[Inject]
	public ICollectiblesTabAPI collectiblesTabAPI
	{
		get;
		set;
	}

	[Inject]
	public INetsukeEquipViewAPI api
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
	public UIManager uiManager
	{
		get;
		set;
	}

	[Inject]
	public IWindowDisplay windowDisplay
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public ISelectionManager selectionManager
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
		WavedashUIButton expr_CE = this.MouseButtonTurnLeft.InteractableButton;
		expr_CE.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(expr_CE.OnPointerClickEvent, new Action<InputEventData>(this.mouseButtonTurnLeft));
		WavedashUIButton expr_FA = this.MouseButtonTurnRight.InteractableButton;
		expr_FA.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(expr_FA.OnPointerClickEvent, new Action<InputEventData>(this.mouseButtonTurnRight));
		WavedashUIButton expr_126 = this.MouseButtonCenter.InteractableButton;
		expr_126.OnPointerClickEvent = (Action<InputEventData>)Delegate.Combine(expr_126.OnPointerClickEvent, new Action<InputEventData>(this.mouseButtonCenter));
		this.menu.Initialize();
		this.addInputInstructions();
		this.animateArrow();
		base.listen("CollectiblesTabAPI.UPDATED", new Action(this.onUpdated));
		base.listen("StoreTabSelectionModel.STORE_TAB_UPDATED", new Action(this.onUpdated));
		base.listen(NetsukeEquipViewAPI.UPDATED, new Action(this.onUpdated));
		base.listen(UserGlobalEquippedModel.UPDATED, new Action(this.onUpdated));
		this.onUpdated();
	}

	private void animateArrow()
	{
		Vector3 localPosition = this.Arrow.localPosition;
		localPosition.y -= 10f;
		DOTween.To(new DOGetter<Vector3>(this._animateArrow_m__0), new DOSetter<Vector3>(this._animateArrow_m__1), localPosition, 2f).SetEase(Ease.InOutSine).OnComplete(new TweenCallback(this.animateArrowBack));
	}

	private void animateArrowBack()
	{
		Vector3 localPosition = this.Arrow.localPosition;
		localPosition.y += 10f;
		DOTween.To(new DOGetter<Vector3>(this._animateArrowBack_m__2), new DOSetter<Vector3>(this._animateArrowBack_m__3), localPosition, 2f).SetEase(Ease.InOutSine).OnComplete(new TweenCallback(this.animateArrow));
	}

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

	private void mouseButtonTurnLeft(InputEventData eventData)
	{
		this.api.TurnLeft();
	}

	private void mouseButtonTurnRight(InputEventData eventData)
	{
		this.api.TurnRight();
	}

	private void mouseButtonCenter(InputEventData eventData)
	{
		this.api.EquipNetsuke(this.api.SelectedItem, this.api.SelectedIndex);
	}

	private void onDoneButton()
	{
		this.api.SaveEdit();
		this.collectiblesTabAPI.SetState(CollectiblesTabState.EquipView, false);
	}

	private void onCancelButton()
	{
		this.api.DiscardEdit();
		this.collectiblesTabAPI.SetState(CollectiblesTabState.EquipView, false);
	}

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

	private void updateRotation()
	{
		float netsukeSelectorRotate = (float)(this.api.SpinIndex * 120);
		this.storeScene.SetNetsukeSelectorRotate(netsukeSelectorRotate);
	}

	private void showBeforeTransition()
	{
		base.transform.localPosition = Vector3.zero;
		this.storeScene.SetShadowMode(false, true);
		this.timer.CancelTimeout(new Action(this.onHideComplete));
		this.timer.SetTimeout(210, new Action(this.onShowComplete));
	}

	private void onShowComplete()
	{
		this.storeScene.SetShadowMode(true, false);
	}

	private void hideAfterTransition()
	{
		this.timer.CancelTimeout(new Action(this.onShowComplete));
		this.timer.SetTimeout(150, new Action(this.onHideComplete));
	}

	private void onHideComplete()
	{
		if (base.transform != null)
		{
			base.transform.localPosition = new Vector3(1000000f, 0f, 0f);
		}
	}

	protected bool allowInteraction()
	{
		return this._allowInteraction != null && this._allowInteraction();
	}

	public void OnStateActivate()
	{
		if (this.allowInteraction())
		{
			this.syncButtonNavigation();
		}
	}

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

	public bool OnLeftTriggerPressed()
	{
		if (this.allowInteraction())
		{
			this.api.TurnLeft();
			return true;
		}
		return false;
	}

	public bool OnRightTriggerPressed()
	{
		if (this.allowInteraction())
		{
			this.api.TurnRight();
			return true;
		}
		return false;
	}

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

	private bool isButtonMode()
	{
		return !this.uiManager.CurrentInputModule.IsMouseMode && this.windowDisplay.GetWindowCount() == 0;
	}

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

	public override void OnDestroy()
	{
		this.timer.CancelTimeout(new Action(this.onHideComplete));
		this.timer.CancelTimeout(new Action(this.onShowComplete));
		base.OnDestroy();
	}

	private Vector3 _animateArrow_m__0()
	{
		return this.Arrow.localPosition;
	}

	private void _animateArrow_m__1(Vector3 valueIn)
	{
		this.Arrow.localPosition = valueIn;
	}

	private Vector3 _animateArrowBack_m__2()
	{
		return this.Arrow.localPosition;
	}

	private void _animateArrowBack_m__3(Vector3 valueIn)
	{
		this.Arrow.localPosition = valueIn;
	}
}
