using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x02000294 RID: 660
public class ClickToOpenLoot : MonoBehaviour, IUnboxingFlow
{
	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x00057A3E File Offset: 0x00055E3E
	// (set) Token: 0x06000DD8 RID: 3544 RVA: 0x00057A46 File Offset: 0x00055E46
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x00057A4F File Offset: 0x00055E4F
	// (set) Token: 0x06000DDA RID: 3546 RVA: 0x00057A57 File Offset: 0x00055E57
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06000DDB RID: 3547 RVA: 0x00057A60 File Offset: 0x00055E60
	// (set) Token: 0x06000DDC RID: 3548 RVA: 0x00057A68 File Offset: 0x00055E68
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x06000DDD RID: 3549 RVA: 0x00057A71 File Offset: 0x00055E71
	[PostConstruct]
	public void OnPostConstruct()
	{
		this.soundRef[ClickToOpenLoot.LootSoundKey.CRYSTAL_DROP] = SoundKey.unboxing_crystalDrop;
		this.soundRef[ClickToOpenLoot.LootSoundKey.PORTAL_APPEAR] = SoundKey.unboxing_portalAppear;
		this.soundRef[ClickToOpenLoot.LootSoundKey.BLUE_FLASH_SIDE] = SoundKey.unboxing_blueFlashSide;
		this.soundRef[ClickToOpenLoot.LootSoundKey.BLUE_FLASH_CENTER] = SoundKey.unboxing_blueFlashCenter;
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x00057AAB File Offset: 0x00055EAB
	private void Start()
	{
		this.setup();
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x00057AB3 File Offset: 0x00055EB3
	private void OnEnable()
	{
		this.setup();
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x00057ABC File Offset: 0x00055EBC
	private void setup()
	{
		if (!this.isSetup && base.gameObject.activeInHierarchy && this.theLootbox != null)
		{
			this.lootboxAnim = this.theLootbox.GetComponent<Animator>();
			this.initialLootboxPosition = this.theLootbox.transform.localPosition;
			this.isSetup = true;
		}
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x00057B23 File Offset: 0x00055F23
	public void ResetScene()
	{
		this.killLootboxTween();
		this.theLootbox.transform.localPosition = this.initialLootboxPosition;
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x00057B41 File Offset: 0x00055F41
	public void OnEnterScreen()
	{
		base.StartCoroutine(this.InitializeBoxForFirstOpen());
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x00057B50 File Offset: 0x00055F50
	private bool isPortalActive(int index)
	{
		return this.portals.ContainsKey(index) && this.portals[index] != null;
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x00057B78 File Offset: 0x00055F78
	public void HighlightPortal(int index)
	{
		if (this.isPortalActive(index))
		{
			VFXUnboxingPortal vfxunboxingPortal = this.portals[index];
			vfxunboxingPortal.SetMouseover(true);
		}
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x00057BA8 File Offset: 0x00055FA8
	public void UnhighlightPortal(int index)
	{
		if (this.isPortalActive(index))
		{
			VFXUnboxingPortal vfxunboxingPortal = this.portals[index];
			vfxunboxingPortal.SetMouseover(false);
		}
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x00057BD5 File Offset: 0x00055FD5
	private void playRaritySound(int index)
	{
		this.audioManager.PlayMenuSound(this.getRaritySound(index), 0f);
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x00057BF0 File Offset: 0x00055FF0
	private void tweenPortalColor(int index, float delay)
	{
		if (this.isPortalActive(index))
		{
			VFXUnboxingPortal portal = this.portals[index];
			portal.ScaleTweenTime = this.portalMouseoverTweenTime;
			Color rarityColor = this.getRarityColor(this.rarityList[index]);
			Color color = default(Color);
			color.a = rarityColor.a;
			color.r = 1f;
			color.g = 1f;
			color.b = 1f;
			portal.color = color;
			DOTween.To(() => portal.color, delegate(Color valueIn)
			{
				portal.color = valueIn;
			}, rarityColor, this.portalColorTweenTime).SetEase(Ease.InCubic).SetDelay(delay);
			this.timer.SetTimeout((int)(delay * 1000f), delegate
			{
				this.playRaritySound(index);
			});
		}
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x00057D08 File Offset: 0x00056108
	private IEnumerator playAfterDelay(ClickToOpenLoot.DelayedEffect effect)
	{
		yield return new WaitForSeconds(effect.delay);
		if (effect.sound != ClickToOpenLoot.LootSoundKey.NONE)
		{
			this.audioManager.PlayMenuSound(this.soundRef[effect.sound], 0f);
		}
		GameObject go = null;
		if (effect.type == ClickToOpenLoot.DelayedEffectType.NORMAL)
		{
			if (effect.vfx != null)
			{
				go = UnityEngine.Object.Instantiate<GameObject>(effect.vfx, (!(effect.transform == null)) ? effect.transform.position : base.transform.position, Quaternion.identity);
				go.transform.SetParent(this.vfxContainer.transform, true);
				VFXUnboxingPortal component = go.GetComponent<VFXUnboxingPortal>();
				if (component != null)
				{
					this.portals[effect.itemIndex] = component;
					float delay = 0f;
					int itemIndex = effect.itemIndex;
					if (itemIndex != 0)
					{
						if (itemIndex != 1)
						{
							if (itemIndex == 2)
							{
								delay = this.timeUntilColorTweenRight;
							}
						}
						else
						{
							delay = this.timeUntilColorTweenCenter;
						}
					}
					else
					{
						delay = this.timeUntilColorTweenLeft;
					}
					this.tweenPortalColor(effect.itemIndex, delay);
				}
			}
		}
		else
		{
			go = this.dynamicObjects[effect.itemIndex];
			go.SetLayer(LayerMask.NameToLayer(Layers.Foreground_Lighting));
			go.SetActive(true);
			go.transform.localPosition = Vector3.zero;
			go.transform.SetParent(effect.transform, false);
			Vector3 localScale = go.transform.localScale;
			go.transform.localScale = Vector3.zero;
			DOTween.To(() => go.transform.localScale, delegate(Vector3 valueIn)
			{
				go.transform.localScale = valueIn;
			}, localScale, this.scaleItemTweenTime).SetEase(this.scaleItemEase);
		}
		if (go != null)
		{
			if (!effect.destroyOnNewBox)
			{
				UnityEngine.Object.Destroy(go, (effect.overrideDestroyTime != 0f) ? effect.overrideDestroyTime : this.destroyEffectObjTime);
			}
			else
			{
				this.destroyList.Add(go);
			}
		}
		if (effect.type == ClickToOpenLoot.DelayedEffectType.DYNAMIC_ITEM)
		{
			this.showRevealedItem(effect.itemIndex);
		}
		yield break;
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x00057D2A File Offset: 0x0005612A
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

	// Token: 0x06000DEA RID: 3562 RVA: 0x00057D60 File Offset: 0x00056160
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

	// Token: 0x06000DEB RID: 3563 RVA: 0x00057DCE File Offset: 0x000561CE
	public void SetShowRevealedItemCallback(Action<int> callback)
	{
		this.showRevealedItem = callback;
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x00057DD7 File Offset: 0x000561D7
	public void SetCamera(Camera theCamera)
	{
		this.theCamera = theCamera;
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x00057DE0 File Offset: 0x000561E0
	public void StartDisplay(List<GameObject> dynamicObjects, List<EquipmentRarity> rarityList)
	{
		this.dynamicObjects = dynamicObjects;
		this.rarityList = rarityList;
		this.lootBoxOpenFlow();
	}

	// Token: 0x06000DEE RID: 3566 RVA: 0x00057DF8 File Offset: 0x000561F8
	public void EndDisplay()
	{
		this.cleanup();
		foreach (GameObject obj in this.destroyList)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.theLootbox.gameObject.SetActive(true);
		this.killLootboxTween();
		this.lootboxTween = DOTween.To(() => this.theLootbox.transform.position, delegate(Vector3 valueIn)
		{
			this.theLootbox.transform.position = valueIn;
		}, this.lootboxTarget.transform.position, this.resetLootboxDuration).SetEase(this.lootAppearEase).SetDelay(this.resetLootboxTweenTime).OnComplete(new TweenCallback(this.killLootboxTween));
	}

	// Token: 0x06000DEF RID: 3567 RVA: 0x00057ED0 File Offset: 0x000562D0
	private void killLootboxTween()
	{
		TweenUtil.Destroy(ref this.lootboxTween);
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x00057EE0 File Offset: 0x000562E0
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

	// Token: 0x06000DF1 RID: 3569 RVA: 0x00057F3C File Offset: 0x0005633C
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
		DOTween.To(() => this.theCamera.transform.position, delegate(Vector3 valueIn)
		{
			this.theCamera.transform.position = valueIn;
		}, this.cameraTarget.transform.position, this.cameraTweenDuration).SetEase(this.cameraDollyEase).SetDelay(this.cameraMoveDelayTime);
		foreach (ClickToOpenLoot.DelayedEffect effect in this.vfxObjects)
		{
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

	// Token: 0x06000DF2 RID: 3570 RVA: 0x000580E9 File Offset: 0x000564E9
	private void InitializeGlowForPillars()
	{
		this.glowPillarAnim.SetBool("Initialize", true);
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x000580FC File Offset: 0x000564FC
	private void UnGlowPillars()
	{
		this.glowPillarAnim.SetBool("Initialize", false);
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x0005810F File Offset: 0x0005650F
	private void InitializeGlowForCenterRing()
	{
		this.centerRingAnim.SetBool("Initialize", true);
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x00058122 File Offset: 0x00056522
	private void UnGlowCenterRing()
	{
		this.centerRingAnim.SetBool("Initialize", false);
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x00058135 File Offset: 0x00056535
	private void InitializeGlowForMidRing()
	{
		this.midRingAnim.SetBool("Initialize", true);
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x00058148 File Offset: 0x00056548
	private void UnGlowMidRing()
	{
		this.midRingAnim.SetBool("Initialize", false);
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x0005815B File Offset: 0x0005655B
	private void InitializeBeams()
	{
		this.beamGlowAnim.SetBool("Initialize", true);
	}

	// Token: 0x06000DF9 RID: 3577 RVA: 0x0005816E File Offset: 0x0005656E
	private void UnGlowBeams()
	{
		this.beamGlowAnim.SetBool("Initialize", false);
	}

	// Token: 0x06000DFA RID: 3578 RVA: 0x00058181 File Offset: 0x00056581
	private void BackgroundLightsON()
	{
		this.backgroundLightAnim01.SetBool("Initialize", true);
		this.backgroundLightAnim02.SetBool("Initialize", true);
	}

	// Token: 0x06000DFB RID: 3579 RVA: 0x000581A5 File Offset: 0x000565A5
	private void BackgroundLightsOFF()
	{
		this.backgroundLightAnim01.SetBool("Initialize", false);
		this.backgroundLightAnim02.SetBool("Initialize", false);
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x000581CC File Offset: 0x000565CC
	private IEnumerator InitializeBoxForFirstOpen()
	{
		this.killLootboxTween();
		this.lootboxTween = DOTween.To(() => this.theLootbox.transform.position, delegate(Vector3 valueIn)
		{
			this.theLootbox.transform.position = valueIn;
		}, this.lootboxTarget.transform.position, this.firstLootboxTweenDuration).SetEase(this.lootAppearEase).SetDelay(0f).OnComplete(new TweenCallback(this.killLootboxTween));
		yield return new WaitForSeconds(this.firstLootboxTweenDuration);
		yield break;
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x000581E7 File Offset: 0x000565E7
	public void OnOpenFinished()
	{
		base.StartCoroutine(this.RecycleBoxForNextOpen());
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x000581F8 File Offset: 0x000565F8
	private IEnumerator RecycleBoxForNextOpen()
	{
		this.theLootbox.transform.localPosition = this.initialLootboxPosition;
		this.theLootbox.gameObject.SetActive(false);
		yield return new WaitForSeconds(this.waitForRecycledLootboxDuration);
		yield break;
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x00058213 File Offset: 0x00056613
	private void OnDestroy()
	{
		this.showRevealedItem = null;
	}

	// Token: 0x040007F9 RID: 2041
	public Color legendaryColor = new Color(1f, 0.73f, 0f);

	// Token: 0x040007FA RID: 2042
	public Color rareColor = new Color(0f, 0.55f, 1f);

	// Token: 0x040007FB RID: 2043
	public Color uncommonColor = new Color(0.3f, 0.62f, 0.24f);

	// Token: 0x040007FC RID: 2044
	public Color commonColor = new Color(1f, 1f, 1f);

	// Token: 0x040007FD RID: 2045
	public Transform cameraTarget;

	// Token: 0x040007FE RID: 2046
	public Transform theLootbox;

	// Token: 0x040007FF RID: 2047
	public Transform lootboxTarget;

	// Token: 0x04000800 RID: 2048
	public Animator lootboxAnim;

	// Token: 0x04000801 RID: 2049
	public Animator glowPillarAnim;

	// Token: 0x04000802 RID: 2050
	public float glowPillarAfterOpenTime = 0.5f;

	// Token: 0x04000803 RID: 2051
	public float unglowPillarAfterOpenTime = 1.5f;

	// Token: 0x04000804 RID: 2052
	public Animator centerRingAnim;

	// Token: 0x04000805 RID: 2053
	public float glowCenterRingAfterOpenTime = 0.5f;

	// Token: 0x04000806 RID: 2054
	public float unglowCenterRingAfterOpenTime = 1.5f;

	// Token: 0x04000807 RID: 2055
	public Animator midRingAnim;

	// Token: 0x04000808 RID: 2056
	public float glowMidRingAfterOpenTime = 0.5f;

	// Token: 0x04000809 RID: 2057
	public float unglowMidRingAfterOpenTime = 1.5f;

	// Token: 0x0400080A RID: 2058
	public Animator beamGlowAnim;

	// Token: 0x0400080B RID: 2059
	public float glowBeamsAfterOpenTime = 0.5f;

	// Token: 0x0400080C RID: 2060
	public float unglowBeamsAfterOpenTime = 1.5f;

	// Token: 0x0400080D RID: 2061
	public Animator backgroundLightAnim01;

	// Token: 0x0400080E RID: 2062
	public Animator backgroundLightAnim02;

	// Token: 0x0400080F RID: 2063
	public float backgroundLights_ONTime = 0.5f;

	// Token: 0x04000810 RID: 2064
	public float backgroundLights_OFFTime = 1.5f;

	// Token: 0x04000811 RID: 2065
	public float timeUntilColorTweenLeft;

	// Token: 0x04000812 RID: 2066
	public float timeUntilColorTweenCenter = 0.5f;

	// Token: 0x04000813 RID: 2067
	public float timeUntilColorTweenRight;

	// Token: 0x04000814 RID: 2068
	public float portalColorTweenTime = 0.3f;

	// Token: 0x04000815 RID: 2069
	public float portalMouseoverTweenTime = 0.07f;

	// Token: 0x04000816 RID: 2070
	private float destroyEffectObjTime = 4f;

	// Token: 0x04000817 RID: 2071
	public float cameraMoveDelayTime;

	// Token: 0x04000818 RID: 2072
	public float cameraTweenDuration;

	// Token: 0x04000819 RID: 2073
	public float firstLootboxTweenDuration;

	// Token: 0x0400081A RID: 2074
	public float waitForRecycledLootboxDuration;

	// Token: 0x0400081B RID: 2075
	public float resetLootboxTweenTime;

	// Token: 0x0400081C RID: 2076
	public float resetLootboxDuration;

	// Token: 0x0400081D RID: 2077
	public float scaleItemTweenTime;

	// Token: 0x0400081E RID: 2078
	private Ease scaleItemEase = Ease.OutFlash;

	// Token: 0x0400081F RID: 2079
	private Vector3 startingLootboxLocalPosition;

	// Token: 0x04000820 RID: 2080
	private Action<int> showRevealedItem;

	// Token: 0x04000821 RID: 2081
	private Ease lootAppearEase = Ease.OutFlash;

	// Token: 0x04000822 RID: 2082
	private Ease cameraDollyEase = Ease.OutFlash;

	// Token: 0x04000823 RID: 2083
	public ClickToOpenLoot.DelayedEffect[] vfxObjects;

	// Token: 0x04000824 RID: 2084
	private Tweener lootboxTween;

	// Token: 0x04000825 RID: 2085
	private List<GameObject> destroyList = new List<GameObject>();

	// Token: 0x04000826 RID: 2086
	private Camera theCamera;

	// Token: 0x04000827 RID: 2087
	private Transform vfxContainer;

	// Token: 0x04000828 RID: 2088
	private List<GameObject> dynamicObjects;

	// Token: 0x04000829 RID: 2089
	private List<EquipmentRarity> rarityList;

	// Token: 0x0400082A RID: 2090
	private Dictionary<int, VFXUnboxingPortal> portals = new Dictionary<int, VFXUnboxingPortal>();

	// Token: 0x0400082B RID: 2091
	private bool isSetup;

	// Token: 0x0400082C RID: 2092
	private Vector3 initialLootboxPosition;

	// Token: 0x0400082D RID: 2093
	private Dictionary<ClickToOpenLoot.LootSoundKey, SoundKey> soundRef = new Dictionary<ClickToOpenLoot.LootSoundKey, SoundKey>();

	// Token: 0x02000295 RID: 661
	[Serializable]
	public class DelayedEffect
	{
		// Token: 0x0400082E RID: 2094
		public ClickToOpenLoot.DelayedEffectType type;

		// Token: 0x0400082F RID: 2095
		public GameObject vfx;

		// Token: 0x04000830 RID: 2096
		public Transform transform;

		// Token: 0x04000831 RID: 2097
		public float delay;

		// Token: 0x04000832 RID: 2098
		public float overrideDestroyTime;

		// Token: 0x04000833 RID: 2099
		public bool destroyOnNewBox;

		// Token: 0x04000834 RID: 2100
		public int itemIndex;

		// Token: 0x04000835 RID: 2101
		public ClickToOpenLoot.LootSoundKey sound;
	}

	// Token: 0x02000296 RID: 662
	[Serializable]
	public enum LootSoundKey
	{
		// Token: 0x04000837 RID: 2103
		NONE,
		// Token: 0x04000838 RID: 2104
		CRYSTAL_DROP,
		// Token: 0x04000839 RID: 2105
		PORTAL_APPEAR,
		// Token: 0x0400083A RID: 2106
		BLUE_FLASH_SIDE,
		// Token: 0x0400083B RID: 2107
		BLUE_FLASH_CENTER
	}

	// Token: 0x02000297 RID: 663
	public enum DelayedEffectType
	{
		// Token: 0x0400083D RID: 2109
		NORMAL,
		// Token: 0x0400083E RID: 2110
		DYNAMIC_ITEM
	}
}
