using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x02000A22 RID: 2594
public class StoreScene3D : UIScene
{
	// Token: 0x170011F9 RID: 4601
	// (get) Token: 0x06004B6E RID: 19310 RVA: 0x00141CF3 File Offset: 0x001400F3
	// (set) Token: 0x06004B6F RID: 19311 RVA: 0x00141CFB File Offset: 0x001400FB
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x170011FA RID: 4602
	// (get) Token: 0x06004B70 RID: 19312 RVA: 0x00141D04 File Offset: 0x00140104
	// (set) Token: 0x06004B71 RID: 19313 RVA: 0x00141D0C File Offset: 0x0014010C
	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager { get; set; }

	// Token: 0x170011FB RID: 4603
	// (get) Token: 0x06004B72 RID: 19314 RVA: 0x00141D15 File Offset: 0x00140115
	// (set) Token: 0x06004B73 RID: 19315 RVA: 0x00141D1D File Offset: 0x0014011D
	[Inject]
	public ICreate3DItemDisplay createItemDisplay { get; set; }

	// Token: 0x170011FC RID: 4604
	// (get) Token: 0x06004B74 RID: 19316 RVA: 0x00141D26 File Offset: 0x00140126
	// (set) Token: 0x06004B75 RID: 19317 RVA: 0x00141D2E File Offset: 0x0014012E
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x170011FD RID: 4605
	// (get) Token: 0x06004B76 RID: 19318 RVA: 0x00141D37 File Offset: 0x00140137
	// (set) Token: 0x06004B77 RID: 19319 RVA: 0x00141D3F File Offset: 0x0014013F
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x06004B78 RID: 19320 RVA: 0x00141D48 File Offset: 0x00140148
	protected override void Awake()
	{
		base.Awake();
		this.cameraPoint = this.myCamera.transform.localPosition;
	}

	// Token: 0x06004B79 RID: 19321 RVA: 0x00141D66 File Offset: 0x00140166
	public void Set3DScene(bool show)
	{
		this.use3dGallery = show;
		this.MainScene.gameObject.SetActive(show);
	}

	// Token: 0x06004B7A RID: 19322 RVA: 0x00141D80 File Offset: 0x00140180
	public void Attach(Transform uiAnchor)
	{
		this.attachToUI = uiAnchor;
		this.updatePosition();
	}

	// Token: 0x06004B7B RID: 19323 RVA: 0x00141D90 File Offset: 0x00140190
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

	// Token: 0x06004B7C RID: 19324 RVA: 0x00141E40 File Offset: 0x00140240
	public void OnSceneTransitionComplete()
	{
		if (this.mode == StoreMode.UNBOXING)
		{
			this.audioManager.PlayMenuSound(SoundKey.unboxing_openUnboxingScene, 0f);
		}
	}

	// Token: 0x06004B7D RID: 19325 RVA: 0x00141E60 File Offset: 0x00140260
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

	// Token: 0x06004B7E RID: 19326 RVA: 0x00141EF4 File Offset: 0x001402F4
	private void tweenStoreScene(float time, Vector3 target)
	{
		if (this.mainTweenTarget != target)
		{
			this.killMainTween();
			this.mainTweenTarget = target;
			this.mainTween = DOTween.To(() => this.MainScene.localPosition, delegate(Vector3 x)
			{
				this.MainScene.localPosition = x;
			}, target, time).SetEase(Ease.OutQuad).OnComplete(new TweenCallback(this.killMainTween));
		}
		this.killCameraTween();
		this.cameraTween = DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, this.cameraPoint, time).SetEase(Ease.OutQuad).OnComplete(new TweenCallback(this.killCameraTween));
	}

	// Token: 0x06004B7F RID: 19327 RVA: 0x00141FA2 File Offset: 0x001403A2
	private void killMainTween()
	{
		TweenUtil.Destroy(ref this.mainTween);
	}

	// Token: 0x06004B80 RID: 19328 RVA: 0x00141FAF File Offset: 0x001403AF
	private void killCameraTween()
	{
		TweenUtil.Destroy(ref this.cameraTween);
	}

	// Token: 0x06004B81 RID: 19329 RVA: 0x00141FBC File Offset: 0x001403BC
	public void AttachCharactersTo(Transform attachCharactersTo, Transform attachCharacterItemsTo)
	{
		this.attachCharactersTo = attachCharactersTo;
		this.attachCharacterItemsTo = attachCharacterItemsTo;
	}

	// Token: 0x06004B82 RID: 19330 RVA: 0x00141FCC File Offset: 0x001403CC
	public void AttachCollectiblesTo(Transform attachTo)
	{
		this.attachCollectiblesTo = attachTo;
	}

	// Token: 0x06004B83 RID: 19331 RVA: 0x00141FD5 File Offset: 0x001403D5
	private SpinData getSpinData(StoreScene3D.ItemType type)
	{
		if (!this.spinData.ContainsKey(type))
		{
			this.spinData[type] = new SpinData();
		}
		return this.spinData[type];
	}

	// Token: 0x06004B84 RID: 19332 RVA: 0x00142008 File Offset: 0x00140408
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
				this.shadowTween = DOTween.To(() => this.DirectionalLight.shadowStrength, delegate(float x)
				{
					this.DirectionalLight.shadowStrength = x;
				}, endValue, duration).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.killShadowTween));
			}
		}
	}

	// Token: 0x06004B85 RID: 19333 RVA: 0x0014209F File Offset: 0x0014049F
	private void killShadowTween()
	{
		TweenUtil.Destroy(ref this.shadowTween);
	}

	// Token: 0x06004B86 RID: 19334 RVA: 0x001420AC File Offset: 0x001404AC
	public void Show3DNetsukeSelector(GameObject prefab, Transform attachTo)
	{
		this.netsukeSelectorDisplay = UnityEngine.Object.Instantiate<GameObject>(prefab).GetComponent<PlayerNetsukeDisplay>();
		this.netsukeSelectorDisplay.transform.SetParent(this.NetsukeEquipPedestalContainer, false);
		this.netsukeSelectorDisplay.Attach(attachTo, this.myCamera);
	}

	// Token: 0x06004B87 RID: 19335 RVA: 0x001420E8 File Offset: 0x001404E8
	public void SetNetsukeSelectorRotate(float rotate)
	{
		this.netsukeSelectorTargetAngle = rotate;
	}

	// Token: 0x170011FE RID: 4606
	// (get) Token: 0x06004B88 RID: 19336 RVA: 0x001420F1 File Offset: 0x001404F1
	// (set) Token: 0x06004B89 RID: 19337 RVA: 0x001420F9 File Offset: 0x001404F9
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

	// Token: 0x06004B8A RID: 19338 RVA: 0x00142133 File Offset: 0x00140533
	public void SetNetsukeOn3DSelector(Netsuke netsuke, int i)
	{
		this.netsukeSelectorDisplay.AddNetsuke(netsuke, i);
	}

	// Token: 0x06004B8B RID: 19339 RVA: 0x00142142 File Offset: 0x00140542
	public void FadeOutNetsukeSelector(float value)
	{
		this.netsukeSelectorDisplay.Alpha = value;
	}

	// Token: 0x06004B8C RID: 19340 RVA: 0x00142150 File Offset: 0x00140550
	public void ScaleNetsukeSelector(Vector3 value)
	{
		this.netsukeSelectorDisplay.Scale = value;
	}

	// Token: 0x06004B8D RID: 19341 RVA: 0x00142160 File Offset: 0x00140560
	public void ShowNetsuke(EquippableItem item)
	{
		PlayerNetsukeDisplay component = this.createItemDisplay.CreateDisplay(item).obj.GetComponent<PlayerNetsukeDisplay>();
		this.clearCollectiblesDisplay();
		component.transform.SetParent(this.CollectiblesEquipDisplayContainer, false);
		component.Attach(this.attachCollectiblesTo, this.myCamera);
		this.setCurrentItem(component.transform, item, StoreScene3D.ItemType.COLLECTIBLE);
	}

	// Token: 0x06004B8E RID: 19342 RVA: 0x001421BC File Offset: 0x001405BC
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

	// Token: 0x06004B8F RID: 19343 RVA: 0x00142208 File Offset: 0x00140608
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

	// Token: 0x06004B90 RID: 19344 RVA: 0x00142254 File Offset: 0x00140654
	public void ShowPlayerToken(EquippableItem item)
	{
		PlayerTokenDisplay component = this.createItemDisplay.CreateDisplay(item).obj.GetComponent<PlayerTokenDisplay>();
		this.clearCollectiblesDisplay();
		component.transform.SetParent(this.CollectiblesEquipDisplayContainer, false);
		component.Attach(this.attachCollectiblesTo, this.myCamera);
		this.setCurrentItem(component.transform, item, StoreScene3D.ItemType.COLLECTIBLE);
	}

	// Token: 0x06004B91 RID: 19345 RVA: 0x001422B0 File Offset: 0x001406B0
	public void ShowPlayerCardIcon(EquippableItem item)
	{
		PlayerCardIconDisplay component = this.createItemDisplay.CreateDisplay(item).obj.GetComponent<PlayerCardIconDisplay>();
		this.clearCollectiblesDisplay();
		component.transform.SetParent(this.CollectiblesEquipDisplayContainer, false);
		component.Attach(this.attachCollectiblesTo, this.myCamera);
		this.setCurrentItem(component.transform, item, StoreScene3D.ItemType.COLLECTIBLE);
	}

	// Token: 0x06004B92 RID: 19346 RVA: 0x0014230C File Offset: 0x0014070C
	private void setCurrentItem(Transform display, EquippableItem item, StoreScene3D.ItemType type)
	{
		SpinData spinData = this.getSpinData(type);
		spinData.currentItem = display;
		spinData.spinTarget = Quaternion.identity;
		this.currentItem[type] = item;
	}

	// Token: 0x06004B93 RID: 19347 RVA: 0x00142340 File Offset: 0x00140740
	public void HideCollectiblesObject()
	{
		this.clearCollectiblesDisplay();
	}

	// Token: 0x06004B94 RID: 19348 RVA: 0x00142348 File Offset: 0x00140748
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

	// Token: 0x06004B95 RID: 19349 RVA: 0x001423A0 File Offset: 0x001407A0
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

	// Token: 0x06004B96 RID: 19350 RVA: 0x00142420 File Offset: 0x00140820
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

	// Token: 0x06004B97 RID: 19351 RVA: 0x001424A0 File Offset: 0x001408A0
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

	// Token: 0x06004B98 RID: 19352 RVA: 0x001424E4 File Offset: 0x001408E4
	private void normalizeAngle(ref float angle)
	{
		angle = Mathf.DeltaAngle(0f, angle);
		if (angle < 0f)
		{
			angle += 360f;
		}
	}

	// Token: 0x06004B99 RID: 19353 RVA: 0x0014250C File Offset: 0x0014090C
	private void Update()
	{
		foreach (KeyValuePair<StoreScene3D.ItemType, SpinData> keyValuePair in this.spinData)
		{
			SpinData value = keyValuePair.Value;
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

	// Token: 0x06004B9A RID: 19354 RVA: 0x00142600 File Offset: 0x00140A00
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

	// Token: 0x06004B9B RID: 19355 RVA: 0x001426A4 File Offset: 0x00140AA4
	public void ShowHologram(EquippableItem item)
	{
		if (this.currentItem.ContainsKey(StoreScene3D.ItemType.CHARACTER_EQUIP) && this.currentItem[StoreScene3D.ItemType.CHARACTER_EQUIP] == item)
		{
			return;
		}
		if (item == null)
		{
			Debug.LogError("Attempted to show a null Hologram.");
			return;
		}
		HologramDisplay component = this.createItemDisplay.CreateDisplay(item).obj.GetComponent<HologramDisplay>();
		this.clearCharacterDisplay();
		component.transform.SetParent(this.CharacterEquipDisplayContainer, false);
		component.Attach(this.attachCharacterItemsTo, this.myCamera);
		this.currentHologram = component;
		this.setCurrentItem(component.transform, item, StoreScene3D.ItemType.CHARACTER_EQUIP);
	}

	// Token: 0x06004B9C RID: 19356 RVA: 0x0014273C File Offset: 0x00140B3C
	public void ReplayHologram()
	{
		if (this.currentHologram != null)
		{
			this.currentHologram.Replay();
		}
	}

	// Token: 0x06004B9D RID: 19357 RVA: 0x0014275C File Offset: 0x00140B5C
	public void ShowVoiceTaunt(EquippableItem item)
	{
		if (this.currentItem.ContainsKey(StoreScene3D.ItemType.CHARACTER_EQUIP) && this.currentItem[StoreScene3D.ItemType.CHARACTER_EQUIP] == item)
		{
			return;
		}
		if (item == null)
		{
			Debug.LogError("Attempted to show a null voice taunt.");
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

	// Token: 0x06004B9E RID: 19358 RVA: 0x00142862 File Offset: 0x00140C62
	private void playVoicePreview()
	{
		if (this.currentVoiceTauntPreview != null)
		{
			this.currentVoiceTauntPreview.PlayPreview();
		}
	}

	// Token: 0x06004B9F RID: 19359 RVA: 0x0014287A File Offset: 0x00140C7A
	public void ReplayVoiceTaunt()
	{
		if (this.currentVoiceTaunt != null)
		{
			this.currentVoiceTaunt.Replay();
		}
	}

	// Token: 0x06004BA0 RID: 19360 RVA: 0x00142898 File Offset: 0x00140C98
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

	// Token: 0x06004BA1 RID: 19361 RVA: 0x001428D4 File Offset: 0x00140CD4
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

	// Token: 0x06004BA2 RID: 19362 RVA: 0x001429AF File Offset: 0x00140DAF
	public int GetClickedCharacterIndex(Vector2 position)
	{
		if (this.currentCharacter != null)
		{
			return this.currentCharacter.GetClickedCharacterIndex(position, this.myCamera);
		}
		return -1;
	}

	// Token: 0x06004BA3 RID: 19363 RVA: 0x001429D0 File Offset: 0x00140DD0
	public IUISceneCharacter GetCurrentCharacter()
	{
		return this.currentCharacter;
	}

	// Token: 0x06004BA4 RID: 19364 RVA: 0x001429D8 File Offset: 0x00140DD8
	public void PlayCharacterAnimations(List<UISceneCharacterAnimRequest> requests)
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.PlayAnimations(requests);
		}
	}

	// Token: 0x06004BA5 RID: 19365 RVA: 0x001429F1 File Offset: 0x00140DF1
	public void PlayTransition(List<UISceneCharacterAnimRequest> requests)
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.PlayTransition(requests);
		}
	}

	// Token: 0x06004BA6 RID: 19366 RVA: 0x00142A0A File Offset: 0x00140E0A
	public void ChangeFrontCharIndex(int value)
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.ChangeFrontCharIndex(value);
		}
	}

	// Token: 0x06004BA7 RID: 19367 RVA: 0x00142A23 File Offset: 0x00140E23
	public void InstantSyncFrontCharacter()
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.InstantSyncFrontCharacter();
		}
	}

	// Token: 0x06004BA8 RID: 19368 RVA: 0x00142A3B File Offset: 0x00140E3B
	public void SetDefaultAnimations(List<UISceneCharacterAnimRequest> requests)
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.SetDefaultAnimations(requests);
		}
	}

	// Token: 0x06004BA9 RID: 19369 RVA: 0x00142A54 File Offset: 0x00140E54
	public void HideCharacterEquipObject()
	{
		this.clearCharacterDisplay();
	}

	// Token: 0x06004BAA RID: 19370 RVA: 0x00142A5C File Offset: 0x00140E5C
	private void clearCharacterDisplay()
	{
		this.removeCharacterObject();
		this.removeCharacter();
		this.setCurrentItem(null, null, StoreScene3D.ItemType.CHARACTER_EQUIP);
	}

	// Token: 0x06004BAB RID: 19371 RVA: 0x00142A74 File Offset: 0x00140E74
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

	// Token: 0x06004BAC RID: 19372 RVA: 0x00142AE8 File Offset: 0x00140EE8
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

	// Token: 0x06004BAD RID: 19373 RVA: 0x00142B37 File Offset: 0x00140F37
	public void SetMode(UIAssetDisplayMode mode)
	{
		if (this.currentCharacter != null)
		{
			this.currentCharacter.SetMode(mode);
		}
	}

	// Token: 0x170011FF RID: 4607
	// (get) Token: 0x06004BAE RID: 19374 RVA: 0x00142B50 File Offset: 0x00140F50
	public bool IsCharacterSwapping
	{
		get
		{
			return this.currentCharacter != null && this.currentCharacter.IsCharacterSwapping;
		}
	}

	// Token: 0x04003191 RID: 12689
	public Transform CharacterEquipDisplayContainer;

	// Token: 0x04003192 RID: 12690
	public Transform CollectiblesEquipDisplayContainer;

	// Token: 0x04003193 RID: 12691
	public Transform NetsukeEquipPedestalContainer;

	// Token: 0x04003194 RID: 12692
	public GameObject BlueBackground;

	// Token: 0x04003195 RID: 12693
	public Light DirectionalLight;

	// Token: 0x04003196 RID: 12694
	public GameObject CharacterContainer;

	// Token: 0x04003197 RID: 12695
	public Transform MainScene;

	// Token: 0x04003198 RID: 12696
	public Transform ScrollPoint;

	// Token: 0x04003199 RID: 12697
	public float LootSceneZ = -550f;

	// Token: 0x0400319A RID: 12698
	private Tweener mainTween;

	// Token: 0x0400319B RID: 12699
	private Tweener cameraTween;

	// Token: 0x0400319C RID: 12700
	private Vector3 mainTweenTarget;

	// Token: 0x0400319D RID: 12701
	private IUISceneCharacter currentCharacter;

	// Token: 0x0400319E RID: 12702
	private CharacterID currentID;

	// Token: 0x0400319F RID: 12703
	private HologramDisplay currentHologram;

	// Token: 0x040031A0 RID: 12704
	private VoiceTauntDisplay currentVoiceTaunt;

	// Token: 0x040031A1 RID: 12705
	private Item3DPreview currentVoiceTauntPreview;

	// Token: 0x040031A2 RID: 12706
	private StoreMode mode;

	// Token: 0x040031A3 RID: 12707
	private Transform attachToUI;

	// Token: 0x040031A4 RID: 12708
	private Transform attachCharactersTo;

	// Token: 0x040031A5 RID: 12709
	private Transform attachCharacterItemsTo;

	// Token: 0x040031A6 RID: 12710
	private Transform attachCollectiblesTo;

	// Token: 0x040031A7 RID: 12711
	private Vector3 cameraPoint;

	// Token: 0x040031A8 RID: 12712
	private PlayerNetsukeDisplay netsukeSelectorDisplay;

	// Token: 0x040031A9 RID: 12713
	private bool shadowsOn = true;

	// Token: 0x040031AA RID: 12714
	private Tweener shadowTween;

	// Token: 0x040031AB RID: 12715
	private bool use3dGallery;

	// Token: 0x040031AC RID: 12716
	private float netsukeSelectorTargetAngle;

	// Token: 0x040031AD RID: 12717
	private float _netsukeSelectorRotate;

	// Token: 0x040031AE RID: 12718
	private Dictionary<StoreScene3D.ItemType, SpinData> spinData = new Dictionary<StoreScene3D.ItemType, SpinData>();

	// Token: 0x040031AF RID: 12719
	private Dictionary<StoreScene3D.ItemType, EquippableItem> currentItem = new Dictionary<StoreScene3D.ItemType, EquippableItem>();

	// Token: 0x02000A23 RID: 2595
	public enum ItemType
	{
		// Token: 0x040031B1 RID: 12721
		NONE,
		// Token: 0x040031B2 RID: 12722
		CHARACTER_EQUIP,
		// Token: 0x040031B3 RID: 12723
		COLLECTIBLE
	}
}
