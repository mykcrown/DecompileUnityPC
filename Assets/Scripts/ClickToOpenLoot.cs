// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ClickToOpenLoot : MonoBehaviour, IUnboxingFlow
{
	[Serializable]
	public class DelayedEffect
	{
		public ClickToOpenLoot.DelayedEffectType type;

		public GameObject vfx;

		public Transform transform;

		public float delay;

		public float overrideDestroyTime;

		public bool destroyOnNewBox;

		public int itemIndex;

		public ClickToOpenLoot.LootSoundKey sound;
	}

	[Serializable]
	public enum LootSoundKey
	{
		NONE,
		CRYSTAL_DROP,
		PORTAL_APPEAR,
		BLUE_FLASH_SIDE,
		BLUE_FLASH_CENTER
	}

	public enum DelayedEffectType
	{
		NORMAL,
		DYNAMIC_ITEM
	}

	private sealed class _tweenPortalColor_c__AnonStorey4
	{
		internal int index;

		internal ClickToOpenLoot _this;
	}

	private sealed class _tweenPortalColor_c__AnonStorey3
	{
		internal VFXUnboxingPortal portal;

		internal ClickToOpenLoot._tweenPortalColor_c__AnonStorey4 __f__ref_4;

		internal Color __m__0()
		{
			return this.portal.color;
		}

		internal void __m__1(Color valueIn)
		{
			this.portal.color = valueIn;
		}

		internal void __m__2()
		{
			this.__f__ref_4._this.playRaritySound(this.__f__ref_4.index);
		}
	}

	private sealed class _playAfterDelay_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		private sealed class _playAfterDelay_c__AnonStorey5
		{
			internal GameObject go;

			internal ClickToOpenLoot._playAfterDelay_c__Iterator0 __f__ref_0;

			internal Vector3 __m__0()
			{
				return this.go.transform.localScale;
			}

			internal void __m__1(Vector3 valueIn)
			{
				this.go.transform.localScale = valueIn;
			}
		}

		internal ClickToOpenLoot.DelayedEffect effect;

		internal ClickToOpenLoot _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		private ClickToOpenLoot._playAfterDelay_c__Iterator0._playAfterDelay_c__AnonStorey5 _locvar1;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _playAfterDelay_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._locvar1 = new ClickToOpenLoot._playAfterDelay_c__Iterator0._playAfterDelay_c__AnonStorey5();
				this._locvar1.__f__ref_0 = this;
				this._current = new WaitForSeconds(this.effect.delay);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this.effect.sound != ClickToOpenLoot.LootSoundKey.NONE)
				{
					this._this.audioManager.PlayMenuSound(this._this.soundRef[this.effect.sound], 0f);
				}
				this._locvar1.go = null;
				if (this.effect.type == ClickToOpenLoot.DelayedEffectType.NORMAL)
				{
					if (this.effect.vfx != null)
					{
						this._locvar1.go = UnityEngine.Object.Instantiate<GameObject>(this.effect.vfx, (!(this.effect.transform == null)) ? this.effect.transform.position : this._this.transform.position, Quaternion.identity);
						this._locvar1.go.transform.SetParent(this._this.vfxContainer.transform, true);
						VFXUnboxingPortal component = this._locvar1.go.GetComponent<VFXUnboxingPortal>();
						if (component != null)
						{
							this._this.portals[this.effect.itemIndex] = component;
							float delay = 0f;
							int itemIndex = this.effect.itemIndex;
							if (itemIndex != 0)
							{
								if (itemIndex != 1)
								{
									if (itemIndex == 2)
									{
										delay = this._this.timeUntilColorTweenRight;
									}
								}
								else
								{
									delay = this._this.timeUntilColorTweenCenter;
								}
							}
							else
							{
								delay = this._this.timeUntilColorTweenLeft;
							}
							this._this.tweenPortalColor(this.effect.itemIndex, delay);
						}
					}
				}
				else
				{
					this._locvar1.go = this._this.dynamicObjects[this.effect.itemIndex];
					this._locvar1.go.SetLayer(LayerMask.NameToLayer(Layers.Foreground_Lighting));
					this._locvar1.go.SetActive(true);
					this._locvar1.go.transform.localPosition = Vector3.zero;
					this._locvar1.go.transform.SetParent(this.effect.transform, false);
					Vector3 localScale = this._locvar1.go.transform.localScale;
					this._locvar1.go.transform.localScale = Vector3.zero;
					DOTween.To(new DOGetter<Vector3>(this._locvar1.__m__0), new DOSetter<Vector3>(this._locvar1.__m__1), localScale, this._this.scaleItemTweenTime).SetEase(this._this.scaleItemEase);
				}
				if (this._locvar1.go != null)
				{
					if (!this.effect.destroyOnNewBox)
					{
						UnityEngine.Object.Destroy(this._locvar1.go, (this.effect.overrideDestroyTime != 0f) ? this.effect.overrideDestroyTime : this._this.destroyEffectObjTime);
					}
					else
					{
						this._this.destroyList.Add(this._locvar1.go);
					}
				}
				if (this.effect.type == ClickToOpenLoot.DelayedEffectType.DYNAMIC_ITEM)
				{
					this._this.showRevealedItem(this.effect.itemIndex);
				}
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _InitializeBoxForFirstOpen_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ClickToOpenLoot _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _InitializeBoxForFirstOpen_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.killLootboxTween();
				this._this.lootboxTween = DOTween.To(new DOGetter<Vector3>(this.__m__0), new DOSetter<Vector3>(this.__m__1), this._this.lootboxTarget.transform.position, this._this.firstLootboxTweenDuration).SetEase(this._this.lootAppearEase).SetDelay(0f).OnComplete(new TweenCallback(this._this.killLootboxTween));
				this._current = new WaitForSeconds(this._this.firstLootboxTweenDuration);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}

		internal Vector3 __m__0()
		{
			return this._this.theLootbox.transform.position;
		}

		internal void __m__1(Vector3 valueIn)
		{
			this._this.theLootbox.transform.position = valueIn;
		}
	}

	private sealed class _RecycleBoxForNextOpen_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ClickToOpenLoot _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _RecycleBoxForNextOpen_c__Iterator2()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.theLootbox.transform.localPosition = this._this.initialLootboxPosition;
				this._this.theLootbox.gameObject.SetActive(false);
				this._current = new WaitForSeconds(this._this.waitForRecycledLootboxDuration);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public Color legendaryColor = new Color(1f, 0.73f, 0f);

	public Color rareColor = new Color(0f, 0.55f, 1f);

	public Color uncommonColor = new Color(0.3f, 0.62f, 0.24f);

	public Color commonColor = new Color(1f, 1f, 1f);

	public Transform cameraTarget;

	public Transform theLootbox;

	public Transform lootboxTarget;

	public Animator lootboxAnim;

	public Animator glowPillarAnim;

	public float glowPillarAfterOpenTime = 0.5f;

	public float unglowPillarAfterOpenTime = 1.5f;

	public Animator centerRingAnim;

	public float glowCenterRingAfterOpenTime = 0.5f;

	public float unglowCenterRingAfterOpenTime = 1.5f;

	public Animator midRingAnim;

	public float glowMidRingAfterOpenTime = 0.5f;

	public float unglowMidRingAfterOpenTime = 1.5f;

	public Animator beamGlowAnim;

	public float glowBeamsAfterOpenTime = 0.5f;

	public float unglowBeamsAfterOpenTime = 1.5f;

	public Animator backgroundLightAnim01;

	public Animator backgroundLightAnim02;

	public float backgroundLights_ONTime = 0.5f;

	public float backgroundLights_OFFTime = 1.5f;

	public float timeUntilColorTweenLeft;

	public float timeUntilColorTweenCenter = 0.5f;

	public float timeUntilColorTweenRight;

	public float portalColorTweenTime = 0.3f;

	public float portalMouseoverTweenTime = 0.07f;

	private float destroyEffectObjTime = 4f;

	public float cameraMoveDelayTime;

	public float cameraTweenDuration;

	public float firstLootboxTweenDuration;

	public float waitForRecycledLootboxDuration;

	public float resetLootboxTweenTime;

	public float resetLootboxDuration;

	public float scaleItemTweenTime;

	private Ease scaleItemEase = Ease.OutFlash;

	private Vector3 startingLootboxLocalPosition;

	private Action<int> showRevealedItem;

	private Ease lootAppearEase = Ease.OutFlash;

	private Ease cameraDollyEase = Ease.OutFlash;

	public ClickToOpenLoot.DelayedEffect[] vfxObjects;

	private Tweener lootboxTween;

	private List<GameObject> destroyList = new List<GameObject>();

	private Camera theCamera;

	private Transform vfxContainer;

	private List<GameObject> dynamicObjects;

	private List<EquipmentRarity> rarityList;

	private Dictionary<int, VFXUnboxingPortal> portals = new Dictionary<int, VFXUnboxingPortal>();

	private bool isSetup;

	private Vector3 initialLootboxPosition;

	private Dictionary<ClickToOpenLoot.LootSoundKey, SoundKey> soundRef = new Dictionary<ClickToOpenLoot.LootSoundKey, SoundKey>();

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
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

	[PostConstruct]
	public void OnPostConstruct()
	{
		this.soundRef[ClickToOpenLoot.LootSoundKey.CRYSTAL_DROP] = SoundKey.unboxing_crystalDrop;
		this.soundRef[ClickToOpenLoot.LootSoundKey.PORTAL_APPEAR] = SoundKey.unboxing_portalAppear;
		this.soundRef[ClickToOpenLoot.LootSoundKey.BLUE_FLASH_SIDE] = SoundKey.unboxing_blueFlashSide;
		this.soundRef[ClickToOpenLoot.LootSoundKey.BLUE_FLASH_CENTER] = SoundKey.unboxing_blueFlashCenter;
	}

	private void Start()
	{
		this.setup();
	}

	private void OnEnable()
	{
		this.setup();
	}

	private void setup()
	{
		if (!this.isSetup && base.gameObject.activeInHierarchy && this.theLootbox != null)
		{
			this.lootboxAnim = this.theLootbox.GetComponent<Animator>();
			this.initialLootboxPosition = this.theLootbox.transform.localPosition;
			this.isSetup = true;
		}
	}

	public void ResetScene()
	{
		this.killLootboxTween();
		this.theLootbox.transform.localPosition = this.initialLootboxPosition;
	}

	public void OnEnterScreen()
	{
		base.StartCoroutine(this.InitializeBoxForFirstOpen());
	}

	private bool isPortalActive(int index)
	{
		return this.portals.ContainsKey(index) && this.portals[index] != null;
	}

	public void HighlightPortal(int index)
	{
		if (this.isPortalActive(index))
		{
			VFXUnboxingPortal vFXUnboxingPortal = this.portals[index];
			vFXUnboxingPortal.SetMouseover(true);
		}
	}

	public void UnhighlightPortal(int index)
	{
		if (this.isPortalActive(index))
		{
			VFXUnboxingPortal vFXUnboxingPortal = this.portals[index];
			vFXUnboxingPortal.SetMouseover(false);
		}
	}

	private void playRaritySound(int index)
	{
		this.audioManager.PlayMenuSound(this.getRaritySound(index), 0f);
	}

	private void tweenPortalColor(int index, float delay)
	{
		ClickToOpenLoot._tweenPortalColor_c__AnonStorey4 _tweenPortalColor_c__AnonStorey = new ClickToOpenLoot._tweenPortalColor_c__AnonStorey4();
		_tweenPortalColor_c__AnonStorey.index = index;
		_tweenPortalColor_c__AnonStorey._this = this;
		if (this.isPortalActive(_tweenPortalColor_c__AnonStorey.index))
		{
			ClickToOpenLoot._tweenPortalColor_c__AnonStorey3 _tweenPortalColor_c__AnonStorey2 = new ClickToOpenLoot._tweenPortalColor_c__AnonStorey3();
			_tweenPortalColor_c__AnonStorey2.__f__ref_4 = _tweenPortalColor_c__AnonStorey;
			_tweenPortalColor_c__AnonStorey2.portal = this.portals[_tweenPortalColor_c__AnonStorey.index];
			_tweenPortalColor_c__AnonStorey2.portal.ScaleTweenTime = this.portalMouseoverTweenTime;
			Color rarityColor = this.getRarityColor(this.rarityList[_tweenPortalColor_c__AnonStorey.index]);
			Color color = default(Color);
			color.a = rarityColor.a;
			color.r = 1f;
			color.g = 1f;
			color.b = 1f;
			_tweenPortalColor_c__AnonStorey2.portal.color = color;
			DOTween.To(new DOGetter<Color>(_tweenPortalColor_c__AnonStorey2.__m__0), new DOSetter<Color>(_tweenPortalColor_c__AnonStorey2.__m__1), rarityColor, this.portalColorTweenTime).SetEase(Ease.InCubic).SetDelay(delay);
			this.timer.SetTimeout((int)(delay * 1000f), new Action(_tweenPortalColor_c__AnonStorey2.__m__2));
		}
	}

	private IEnumerator playAfterDelay(ClickToOpenLoot.DelayedEffect effect)
	{
		ClickToOpenLoot._playAfterDelay_c__Iterator0 _playAfterDelay_c__Iterator = new ClickToOpenLoot._playAfterDelay_c__Iterator0();
		_playAfterDelay_c__Iterator.effect = effect;
		_playAfterDelay_c__Iterator._this = this;
		return _playAfterDelay_c__Iterator;
	}

	private Color getRarityColor(EquipmentRarity rarity)
	{
		switch (rarity)
		{
		case EquipmentRarity.UNCOMMON:
			return this.uncommonColor;
		case EquipmentRarity.RARE:
			return this.rareColor;
		case EquipmentRarity.LEGENDARY:
			return this.legendaryColor;
		default:
			return this.commonColor;
		}
	}

	private SoundKey getRaritySound(int index)
	{
		EquipmentRarity equipmentRarity = this.rarityList[index];
		if (index == 1)
		{
			switch (equipmentRarity)
			{
			case EquipmentRarity.UNCOMMON:
				return SoundKey.unboxing_itemCenterUncommon;
			case EquipmentRarity.RARE:
				return SoundKey.unboxing_itemCenterRare;
			case EquipmentRarity.LEGENDARY:
				return SoundKey.unboxing_itemCenterLegendary;
			}
			return SoundKey.unboxing_itemCenterCommon;
		}
		switch (equipmentRarity)
		{
		case EquipmentRarity.UNCOMMON:
			return SoundKey.unboxing_itemSideUncommon;
		case EquipmentRarity.RARE:
			return SoundKey.unboxing_itemSideRare;
		case EquipmentRarity.LEGENDARY:
			return SoundKey.unboxing_itemSideLegendary;
		}
		return SoundKey.unboxing_itemSideCommon;
	}

	public void SetShowRevealedItemCallback(Action<int> callback)
	{
		this.showRevealedItem = callback;
	}

	public void SetCamera(Camera theCamera)
	{
		this.theCamera = theCamera;
	}

	public void StartDisplay(List<GameObject> dynamicObjects, List<EquipmentRarity> rarityList)
	{
		this.dynamicObjects = dynamicObjects;
		this.rarityList = rarityList;
		this.lootBoxOpenFlow();
	}

	public void EndDisplay()
	{
		this.cleanup();
		foreach (GameObject current in this.destroyList)
		{
			UnityEngine.Object.Destroy(current);
		}
		this.theLootbox.gameObject.SetActive(true);
		this.killLootboxTween();
		this.lootboxTween = DOTween.To(new DOGetter<Vector3>(this._EndDisplay_m__0), new DOSetter<Vector3>(this._EndDisplay_m__1), this.lootboxTarget.transform.position, this.resetLootboxDuration).SetEase(this.lootAppearEase).SetDelay(this.resetLootboxTweenTime).OnComplete(new TweenCallback(this.killLootboxTween));
	}

	private void killLootboxTween()
	{
		TweenUtil.Destroy(ref this.lootboxTween);
	}

	private void cleanup()
	{
		if (this.vfxContainer != null)
		{
			while (this.vfxContainer.childCount > 0)
			{
				Transform child = this.vfxContainer.GetChild(0);
				child.transform.SetParent(null, false);
				UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}
	}

	private void lootBoxOpenFlow()
	{
		this.theLootbox.gameObject.SetActive(true);
		this.killLootboxTween();
		this.theLootbox.transform.position = this.lootboxTarget.transform.position;
		if (this.vfxContainer != null)
		{
			UnityEngine.Object.DestroyImmediate(this.vfxContainer.gameObject);
		}
		this.vfxContainer = new GameObject("VFXContainer").transform;
		this.vfxContainer.SetParent(base.transform, false);
		this.lootboxAnim.SetBool("IsOpen", true);
		DOTween.To(new DOGetter<Vector3>(this._lootBoxOpenFlow_m__2), new DOSetter<Vector3>(this._lootBoxOpenFlow_m__3), this.cameraTarget.transform.position, this.cameraTweenDuration).SetEase(this.cameraDollyEase).SetDelay(this.cameraMoveDelayTime);
		ClickToOpenLoot.DelayedEffect[] array = this.vfxObjects;
		for (int i = 0; i < array.Length; i++)
		{
			ClickToOpenLoot.DelayedEffect effect = array[i];
			base.StartCoroutine(this.playAfterDelay(effect));
		}
		base.Invoke("InitializeGlowForPillars", this.glowPillarAfterOpenTime);
		base.Invoke("UnGlowPillars", this.unglowPillarAfterOpenTime);
		base.Invoke("InitializeGlowForCenterRing", this.glowCenterRingAfterOpenTime);
		base.Invoke("UnGlowCenterRing", this.unglowCenterRingAfterOpenTime);
		base.Invoke("InitializeGlowForMidRing", this.glowMidRingAfterOpenTime);
		base.Invoke("UnGlowMidRing", this.unglowMidRingAfterOpenTime);
		base.Invoke("InitializeBeams", this.glowBeamsAfterOpenTime);
		base.Invoke("UnGlowBeams", this.unglowBeamsAfterOpenTime);
		base.Invoke("BackgroundLightsOFF", this.backgroundLights_OFFTime);
	}

	private void InitializeGlowForPillars()
	{
		this.glowPillarAnim.SetBool("Initialize", true);
	}

	private void UnGlowPillars()
	{
		this.glowPillarAnim.SetBool("Initialize", false);
	}

	private void InitializeGlowForCenterRing()
	{
		this.centerRingAnim.SetBool("Initialize", true);
	}

	private void UnGlowCenterRing()
	{
		this.centerRingAnim.SetBool("Initialize", false);
	}

	private void InitializeGlowForMidRing()
	{
		this.midRingAnim.SetBool("Initialize", true);
	}

	private void UnGlowMidRing()
	{
		this.midRingAnim.SetBool("Initialize", false);
	}

	private void InitializeBeams()
	{
		this.beamGlowAnim.SetBool("Initialize", true);
	}

	private void UnGlowBeams()
	{
		this.beamGlowAnim.SetBool("Initialize", false);
	}

	private void BackgroundLightsON()
	{
		this.backgroundLightAnim01.SetBool("Initialize", true);
		this.backgroundLightAnim02.SetBool("Initialize", true);
	}

	private void BackgroundLightsOFF()
	{
		this.backgroundLightAnim01.SetBool("Initialize", false);
		this.backgroundLightAnim02.SetBool("Initialize", false);
	}

	private IEnumerator InitializeBoxForFirstOpen()
	{
		ClickToOpenLoot._InitializeBoxForFirstOpen_c__Iterator1 _InitializeBoxForFirstOpen_c__Iterator = new ClickToOpenLoot._InitializeBoxForFirstOpen_c__Iterator1();
		_InitializeBoxForFirstOpen_c__Iterator._this = this;
		return _InitializeBoxForFirstOpen_c__Iterator;
	}

	public void OnOpenFinished()
	{
		base.StartCoroutine(this.RecycleBoxForNextOpen());
	}

	private IEnumerator RecycleBoxForNextOpen()
	{
		ClickToOpenLoot._RecycleBoxForNextOpen_c__Iterator2 _RecycleBoxForNextOpen_c__Iterator = new ClickToOpenLoot._RecycleBoxForNextOpen_c__Iterator2();
		_RecycleBoxForNextOpen_c__Iterator._this = this;
		return _RecycleBoxForNextOpen_c__Iterator;
	}

	private void OnDestroy()
	{
		this.showRevealedItem = null;
	}

	private Vector3 _EndDisplay_m__0()
	{
		return this.theLootbox.transform.position;
	}

	private void _EndDisplay_m__1(Vector3 valueIn)
	{
		this.theLootbox.transform.position = valueIn;
	}

	private Vector3 _lootBoxOpenFlow_m__2()
	{
		return this.theCamera.transform.position;
	}

	private void _lootBoxOpenFlow_m__3(Vector3 valueIn)
	{
		this.theCamera.transform.position = valueIn;
	}
}
