// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StoreScene3D : UIScene
{
	public enum ItemType
	{
		NONE,
		CHARACTER_EQUIP,
		COLLECTIBLE
	}

	public Transform CharacterEquipDisplayContainer;

	public Transform CollectiblesEquipDisplayContainer;

	public Transform NetsukeEquipPedestalContainer;

	public GameObject BlueBackground;

	public Light DirectionalLight;

	public GameObject CharacterContainer;

	public Transform MainScene;

	public Transform ScrollPoint;

	public float LootSceneZ = -550f;

	private Tweener mainTween;

	private Tweener cameraTween;

	private Vector3 mainTweenTarget;

	private IUISceneCharacter currentCharacter;

	private CharacterID currentID;

	private HologramDisplay currentHologram;

	private VoiceTauntDisplay currentVoiceTaunt;

	private Item3DPreview currentVoiceTauntPreview;

	private StoreMode mode;

	private Transform attachToUI;

	private Transform attachCharactersTo;

	private Transform attachCharacterItemsTo;

	private Transform attachCollectiblesTo;

	private Vector3 cameraPoint;

	private PlayerNetsukeDisplay netsukeSelectorDisplay;

	private bool shadowsOn = true;

	private Tweener shadowTween;

	private bool use3dGallery;

	private float netsukeSelectorTargetAngle;

	private float _netsukeSelectorRotate;

	private Dictionary<StoreScene3D.ItemType, SpinData> spinData = new Dictionary<StoreScene3D.ItemType, SpinData>();

	private Dictionary<StoreScene3D.ItemType, EquippableItem> currentItem = new Dictionary<StoreScene3D.ItemType, EquippableItem>();

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager
	{
		get;
		set;
	}

	[Inject]
	public ICreate3DItemDisplay createItemDisplay
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
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

	public float netsukeSelectorRotate
	{
		get
		{
			return this._netsukeSelectorRotate;
		}
		set
		{
			if (this._netsukeSelectorRotate != value)
			{
				this._netsukeSelectorRotate = value;
				this.netsukeSelectorDisplay.transform.localRotation = Quaternion.Euler(0f, this._netsukeSelectorRotate, 0f);
			}
		}
	}

	public bool IsCharacterSwapping
	{
		get
		{
			return this.currentCharacter != null && this.currentCharacter.IsCharacterSwapping;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.cameraPoint = this.myCamera.transform.localPosition;
	}

	public void Set3DScene(bool show)
	{
		this.use3dGallery = show;
		this.MainScene.gameObject.SetActive(show);
	}

	public void Attach(Transform uiAnchor)
	{
		this.attachToUI = uiAnchor;
		this.updatePosition();
	}

	private void updatePosition()
	{
		if (this.attachToUI != null && this.mode == StoreMode.NORMAL)
		{
			Vector3 position = this.attachToUI.position;
			position.z = Mathf.Abs(this.myCamera.transform.position.z - this.ScrollPoint.transform.position.z);
			Vector3 position2 = this.myCamera.ScreenToWorldPoint(position);
			position2.z = this.ScrollPoint.transform.position.z;
			this.ScrollPoint.transform.position = position2;
		}
	}

	public void OnSceneTransitionComplete()
	{
		if (this.mode == StoreMode.UNBOXING)
		{
			this.audioManager.PlayMenuSound(SoundKey.unboxing_openUnboxingScene, 0f);
		}
	}

	public void UpdateMode(StoreMode mode)
	{
		this.mode = mode;
		Vector3 localPosition = this.MainScene.localPosition;
		if (mode == StoreMode.UNBOXING)
		{
			localPosition.z = this.LootSceneZ;
		}
		else
		{
			localPosition.z = 0f;
		}
		if (this.use3dGallery)
		{
			this.tweenStoreScene(0.75f, localPosition);
		}
		else
		{
			this.tweenStoreScene(0f, localPosition);
			this.BlueBackground.SetActive(mode == StoreMode.NORMAL);
			this.MainScene.gameObject.SetActive(mode != StoreMode.NORMAL);
		}
	}

	private void tweenStoreScene(float time, Vector3 target)
	{
		if (this.mainTweenTarget != target)
		{
			this.killMainTween();
			this.mainTweenTarget = target;
			this.mainTween = DOTween.To(new DOGetter<Vector3>(this._tweenStoreScene_m__0), new DOSetter<Vector3>(this._tweenStoreScene_m__1), target, time).SetEase(Ease.OutQuad).OnComplete(new TweenCallback(this.killMainTween));
		}
		this.killCameraTween();
		this.cameraTween = DOTween.To(new DOGetter<Vector3>(this._tweenStoreScene_m__2), new DOSetter<Vector3>(this._tweenStoreScene_m__3), this.cameraPoint, time).SetEase(Ease.OutQuad).OnComplete(new TweenCallback(this.killCameraTween));
	}

	private void killMainTween()
	{
		TweenUtil.Destroy(ref this.mainTween);
	}

	private void killCameraTween()
	{
		TweenUtil.Destroy(ref this.cameraTween);
	}

	public void AttachCharactersTo(Transform attachCharactersTo, Transform attachCharacterItemsTo)
	{
		this.attachCharactersTo = attachCharactersTo;
		this.attachCharacterItemsTo = attachCharacterItemsTo;
	}

	public void AttachCollectiblesTo(Transform attachTo)
	{
		this.attachCollectiblesTo = attachTo;
	}

	private SpinData getSpinData(StoreScene3D.ItemType type)
	{
		if (!this.spinData.ContainsKey(type))
		{
			this.spinData[type] = new SpinData();
		}
		return this.spinData[type];
	}

	public void SetShadowMode(bool on, bool instant = false)
	{
		if (this.shadowsOn != on)
		{
			this.shadowsOn = on;
			float endValue = (float)((!this.shadowsOn) ? 0 : 1);
			float duration = 0.4f;
			this.killShadowTween();
			if (instant)
			{
				this.DirectionalLight.shadowStrength = 0f;
			}
			else
			{
				this.shadowTween = DOTween.To(new DOGetter<float>(this._SetShadowMode_m__4), new DOSetter<float>(this._SetShadowMode_m__5), endValue, duration).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killShadowTween));
			}
		}
	}

	private void killShadowTween()
	{
		TweenUtil.Destroy(ref this.shadowTween);
	}

	public void Show3DNetsukeSelector(GameObject prefab, Transform attachTo)
	{
		this.netsukeSelectorDisplay = UnityEngine.Object.Instantiate<GameObject>(prefab).GetComponent<PlayerNetsukeDisplay>();
		this.netsukeSelectorDisplay.transform.SetParent(this.NetsukeEquipPedestalContainer, false);
		this.netsukeSelectorDisplay.Attach(attachTo, this.myCamera);
	}

	public void SetNetsukeSelectorRotate(float rotate)
	{
		this.netsukeSelectorTargetAngle = rotate;
	}

	public void SetNetsukeOn3DSelector(Netsuke netsuke, int i)
	{
		this.netsukeSelectorDisplay.AddNetsuke(netsuke, i);
	}

	public void FadeOutNetsukeSelector(float value)
	{
		this.netsukeSelectorDisplay.Alpha = value;
	}

	public void ScaleNetsukeSelector(Vector3 value)
	{
		this.netsukeSelectorDisplay.Scale = value;
	}

	public void ShowNetsuke(EquippableItem item)
	{
		PlayerNetsukeDisplay component = this.createItemDisplay.CreateDisplay(item).obj.GetComponent<PlayerNetsukeDisplay>();
		this.clearCollectiblesDisplay();
		component.transform.SetParent(this.CollectiblesEquipDisplayContainer, false);
		component.Attach(this.attachCollectiblesTo, this.myCamera);
		this.setCurrentItem(component.transform, item, StoreScene3D.ItemType.COLLECTIBLE);
	}

	public void FadeOutCollectible(float alpha)
	{
		SpinData spinData = this.getSpinData(StoreScene3D.ItemType.COLLECTIBLE);
		if (spinData.currentItem != null)
		{
			PlayerNetsukeDisplay component = spinData.currentItem.gameObject.GetComponent<PlayerNetsukeDisplay>();
			if (component != null)
			{
				component.Alpha = alpha;
			}
		}
	}

	public void ScaleCollectible(Vector3 scale)
	{
		SpinData spinData = this.getSpinData(StoreScene3D.ItemType.COLLECTIBLE);
		if (spinData.currentItem != null)
		{
			PlayerNetsukeDisplay component = spinData.currentItem.gameObject.GetComponent<PlayerNetsukeDisplay>();
			if (component != null)
			{
				component.Scale = scale;
			}
		}
	}

	public void ShowPlayerToken(EquippableItem item)
	{
		PlayerTokenDisplay component = this.createItemDisplay.CreateDisplay(item).obj.GetComponent<PlayerTokenDisplay>();
		this.clearCollectiblesDisplay();
		component.transform.SetParent(this.CollectiblesEquipDisplayContainer, false);
		component.Attach(this.attachCollectiblesTo, this.myCamera);
		this.setCurrentItem(component.transform, item, StoreScene3D.ItemType.COLLECTIBLE);
	}

	public void ShowPlayerCardIcon(EquippableItem item)
	{
		PlayerCardIconDisplay component = this.createItemDisplay.CreateDisplay(item).obj.GetComponent<PlayerCardIconDisplay>();
		this.clearCollectiblesDisplay();
		component.transform.SetParent(this.CollectiblesEquipDisplayContainer, false);
		component.Attach(this.attachCollectiblesTo, this.myCamera);
		this.setCurrentItem(component.transform, item, StoreScene3D.ItemType.COLLECTIBLE);
	}

	private void setCurrentItem(Transform display, EquippableItem item, StoreScene3D.ItemType type)
	{
		SpinData spinData = this.getSpinData(type);
		spinData.currentItem = display;
		spinData.spinTarget = Quaternion.identity;
		this.currentItem[type] = item;
	}

	public void HideCollectiblesObject()
	{
		this.clearCollectiblesDisplay();
	}

	private void clearCollectiblesDisplay()
	{
		while (this.CollectiblesEquipDisplayContainer.childCount > 0)
		{
			GameObject gameObject = this.CollectiblesEquipDisplayContainer.GetChild(0).gameObject;
			gameObject.SetActive(false);
			gameObject.transform.SetParent(null, false);
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
		this.setCurrentItem(null, null, StoreScene3D.ItemType.COLLECTIBLE);
	}

	public void SpinItemManual(StoreScene3D.ItemType type, float value)
	{
		SpinData spinData = this.getSpinData(type);
		Transform transform = spinData.currentItem;
		if (transform != null)
		{
			spinData.spinToTarget = false;
			Vector3 eulerAngles = transform.localRotation.eulerAngles;
			eulerAngles.y -= base.config.storeSettings.analogSpinSpeed * Time.deltaTime * value;
			this.normalizeAngle(ref eulerAngles.y);
			transform.localRotation = Quaternion.Euler(eulerAngles);
		}
	}

	public void SpinItem(StoreScene3D.ItemType type)
	{
		SpinData spinData = this.getSpinData(type);
		Transform transform = spinData.currentItem;
		if (transform != null)
		{
			if (!spinData.spinToTarget)
			{
				spinData.spinToTarget = true;
				spinData.spinTarget = transform.localRotation;
			}
			Vector3 eulerAngles = spinData.spinTarget.eulerAngles;
			eulerAngles.y -= 90f;
			this.normalizeAngle(ref eulerAngles.y);
			spinData.spinTarget = Quaternion.Euler(eulerAngles);
		}
	}

	public void OnItemClick(StoreScene3D.ItemType type)
	{
		SpinData spinData = this.getSpinData(type);
		if (spinData.currentItem != null)
		{
			BaseItem3DPreviewDisplay component = spinData.currentItem.GetComponent<BaseItem3DPreviewDisplay>();
			if (component != null)
			{
				component.OnClick();
			}
		}
	}

	private void normalizeAngle(ref float angle)
	{
		angle = Mathf.DeltaAngle(0f, angle);
		if (angle < 0f)
		{
			angle += 360f;
		}
	}

	private void Update()
	{
		foreach (KeyValuePair<StoreScene3D.ItemType, SpinData> current in this.spinData)
		{
			SpinData value = current.Value;
			if (value.spinToTarget && value.currentItem != null)
			{
				value.currentItem.localRotation = Quaternion.Lerp(value.currentItem.localRotation, value.spinTarget, Time.deltaTime * base.config.storeSettings.clickSpinSpeed);
			}
		}
		if (this.netsukeSelectorDisplay != null)
		{
			this.netsukeSelectorRotate = Mathf.Lerp(this.netsukeSelectorRotate, this.netsukeSelectorTargetAngle, Time.deltaTime * base.config.storeSettings.netsukeSelectorSpinSpeed);
		}
		this.updatePosition();
	}

	public void ShowPlatform(EquippableItem item)
	{
		if (this.currentItem.ContainsKey(StoreScene3D.ItemType.CHARACTER_EQUIP) && this.currentItem[StoreScene3D.ItemType.CHARACTER_EQUIP] == item)
		{
			return;
		}
		RespawnPlatform component;
		if (item == null)
		{
			component = this.createItemDisplay.CreateDefault(EquipmentTypes.PLATFORM).obj.GetComponent<RespawnPlatform>();
		}
		else
		{
			component = this.createItemDisplay.CreateDisplay(item).obj.GetComponent<RespawnPlatform>();
		}
		this.clearCharacterDisplay();
		component.transform.SetParent(this.CharacterEquipDisplayContainer, false);
		component.Attach(this.attachCharacterItemsTo, this.myCamera);
		this.setCurrentItem(component.transform, item, StoreScene3D.ItemType.CHARACTER_EQUIP);
	}

	public void ShowHologram(EquippableItem item)
	{
		if (this.currentItem.ContainsKey(StoreScene3D.ItemType.CHARACTER_EQUIP) && this.currentItem[StoreScene3D.ItemType.CHARACTER_EQUIP] == item)
		{
			return;
		}
		if (item == null)
		{
			UnityEngine.Debug.LogError("Attempted to show a null Hologram.");
			return;
		}
		HologramDisplay component = this.createItemDisplay.CreateDisplay(item).obj.GetComponent<HologramDisplay>();
		this.clearCharacterDisplay();
		component.transform.SetParent(this.CharacterEquipDisplayContainer, false);
		component.Attach(this.attachCharacterItemsTo, this.myCamera);
		this.currentHologram = component;
		this.setCurrentItem(component.transform, item, StoreScene3D.ItemType.CHARACTER_EQUIP);
	}

	public void ReplayHologram()
	{
		if (this.currentHologram != null)
		{
			this.currentHologram.Replay();
		}
	}

	public void ShowVoiceTaunt(EquippableItem item)
	{
		if (this.currentItem.ContainsKey(StoreScene3D.ItemType.CHARACTER_EQUIP) && this.currentItem[StoreScene3D.ItemType.CHARACTER_EQUIP] == item)
		{
			return;
		}
		if (item == null)
		{
			UnityEngine.Debug.LogError("Attempted to show a null voice taunt.");
			return;
		}
		Item3DPreview item3DPreview = this.createItemDisplay.CreateDisplay(item);
		VoiceTauntDisplay component = item3DPreview.obj.GetComponent<VoiceTauntDisplay>();
		base.timer.CancelTimeout(new Action(this.playVoicePreview));
		this.clearCharacterDisplay();
		component.transform.SetParent(this.CharacterEquipDisplayContainer, false);
		component.Attach(this.attachCharacterItemsTo, this.myCamera);
		if (this.uiManager.CurrentInputModule.IsMouseMode)
		{
			item3DPreview.PlayPreview();
		}
		else
		{
			base.timer.SetTimeout((int)(base.config.storeSettings.voiceTauntPlayDelay * 1000f), new Action(this.playVoicePreview));
		}
		this.currentVoiceTaunt = component;
		this.currentVoiceTauntPreview = item3DPreview;
		this.setCurrentItem(component.transform, item, StoreScene3D.ItemType.CHARACTER_EQUIP);
	}

	private void playVoicePreview()
	{
		if (this.currentVoiceTauntPreview != null)
		{
			this.currentVoiceTauntPreview.PlayPreview();
		}
	}

	public void ReplayVoiceTaunt()
	{
		if (this.currentVoiceTaunt != null)
		{
			this.currentVoiceTaunt.Replay();
		}
	}

	public void SetTypeAligner(CharacterMenusData characterMenusData, CharacterMenusData.UICharacterAdjustments aligner)
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.RemoveAligners();
			this.currentCharacter.AddAligner(characterMenusData.galleryAdjustments);
			if (aligner != null)
			{
				this.currentCharacter.AddAligner(aligner);
			}
		}
	}

	public void ShowCharacter(CharacterMenusData characterMenusData, SkinDefinition skinDef, EquippableItem item, int characterIndex)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.currentID != characterMenusData.characterID)
		{
			this.clearCharacterDisplay();
			IUISceneCharacter character = this.uiSceneCharacterManager.GetCharacter(characterMenusData.characterID, skinDef);
			character.Activate(this.CharacterContainer.transform);
			character.ChangeFrontCharIndex(characterIndex);
			character.InstantSyncFrontCharacter();
			List<WavedashAnimationData> allDefaultAnimations = this.characterDataHelper.GetAllDefaultAnimations(characterMenusData.characterDefinition, CharacterDefaultAnimationKey.STORE_IDLE);
			character.SetDefaultAnimations(UISceneAnimRequestHelper.GetAnimRequests(allDefaultAnimations));
			character.Attach(this.attachCharactersTo, this.myCamera);
			character.AddAligner(characterMenusData.galleryAdjustments);
			this.currentCharacter = character;
			this.currentID = characterMenusData.characterID;
		}
		this.currentCharacter.SetSkin(skinDef);
		this.setCurrentItem((this.currentCharacter as Component).transform, item, StoreScene3D.ItemType.CHARACTER_EQUIP);
	}

	public int GetClickedCharacterIndex(Vector2 position)
	{
		if (this.currentCharacter != null)
		{
			return this.currentCharacter.GetClickedCharacterIndex(position, this.myCamera);
		}
		return -1;
	}

	public IUISceneCharacter GetCurrentCharacter()
	{
		return this.currentCharacter;
	}

	public void PlayCharacterAnimations(List<UISceneCharacterAnimRequest> requests)
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.PlayAnimations(requests);
		}
	}

	public void PlayTransition(List<UISceneCharacterAnimRequest> requests)
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.PlayTransition(requests);
		}
	}

	public void ChangeFrontCharIndex(int value)
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.ChangeFrontCharIndex(value);
		}
	}

	public void InstantSyncFrontCharacter()
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.InstantSyncFrontCharacter();
		}
	}

	public void SetDefaultAnimations(List<UISceneCharacterAnimRequest> requests)
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.SetDefaultAnimations(requests);
		}
	}

	public void HideCharacterEquipObject()
	{
		this.clearCharacterDisplay();
	}

	private void clearCharacterDisplay()
	{
		this.removeCharacterObject();
		this.removeCharacter();
		this.setCurrentItem(null, null, StoreScene3D.ItemType.CHARACTER_EQUIP);
	}

	private void removeCharacter()
	{
		while (this.CharacterContainer.transform.childCount > 0)
		{
			GameObject gameObject = this.CharacterContainer.transform.GetChild(0).gameObject;
			gameObject.SetActive(false);
			gameObject.transform.SetParent(null, false);
			this.uiSceneCharacterManager.ReleaseCharacter(gameObject.GetComponent<IUISceneCharacter>());
		}
		this.currentCharacter = null;
		this.currentID = CharacterID.None;
	}

	private void removeCharacterObject()
	{
		while (this.CharacterEquipDisplayContainer.childCount > 0)
		{
			GameObject gameObject = this.CharacterEquipDisplayContainer.GetChild(0).gameObject;
			gameObject.SetActive(false);
			gameObject.transform.SetParent(null, false);
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
	}

	public void SetMode(UIAssetDisplayMode mode)
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.SetMode(mode);
		}
	}

	private Vector3 _tweenStoreScene_m__0()
	{
		return this.MainScene.localPosition;
	}

	private void _tweenStoreScene_m__1(Vector3 x)
	{
		this.MainScene.localPosition = x;
	}

	private Vector3 _tweenStoreScene_m__2()
	{
		return this.myCamera.transform.localPosition;
	}

	private void _tweenStoreScene_m__3(Vector3 x)
	{
		this.myCamera.transform.localPosition = x;
	}

	private float _SetShadowMode_m__4()
	{
		return this.DirectionalLight.shadowStrength;
	}

	private void _SetShadowMode_m__5(float x)
	{
		this.DirectionalLight.shadowStrength = x;
	}
}
