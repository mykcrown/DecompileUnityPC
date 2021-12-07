using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200090B RID: 2315
public class CreditsScene : UIScene
{
	// Token: 0x17000E61 RID: 3681
	// (get) Token: 0x06003C25 RID: 15397 RVA: 0x00116CC4 File Offset: 0x001150C4
	// (set) Token: 0x06003C26 RID: 15398 RVA: 0x00116CCC File Offset: 0x001150CC
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000E62 RID: 3682
	// (get) Token: 0x06003C27 RID: 15399 RVA: 0x00116CD5 File Offset: 0x001150D5
	// (set) Token: 0x06003C28 RID: 15400 RVA: 0x00116CDD File Offset: 0x001150DD
	[Inject]
	public IItemLoader itemLoader { get; set; }

	// Token: 0x06003C29 RID: 15401 RVA: 0x00116CE8 File Offset: 0x001150E8
	public void Init(CreditsData credits)
	{
		this.allHolograms = this.equipmentModel.GetCharacterItems(CharacterID.Any, EquipmentTypes.HOLOGRAM);
		this.hologramIndicies = new int[this.allHolograms.Length];
		for (int i = 0; i < this.hologramIndicies.Length; i++)
		{
			this.hologramIndicies[i] = i;
		}
		this.shuffleIndices(this.hologramIndicies);
		this.hologramIndex = 0;
		this.hologramInstantiateTime = this.nextHologramTime();
		this.resetCredits();
		this.instantiateCredits(credits);
	}

	// Token: 0x06003C2A RID: 15402 RVA: 0x00116D6C File Offset: 0x0011516C
	private HologramData getRandomHologram()
	{
		int num = this.hologramIndicies[this.hologramIndex];
		this.hologramIndex++;
		if (this.hologramIndex >= this.hologramIndicies.Length)
		{
			this.hologramIndex = 0;
			this.shuffleIndices(this.hologramIndicies);
		}
		EquippableItem item = this.allHolograms[num];
		return this.itemLoader.LoadAsset<HologramData>(item);
	}

	// Token: 0x06003C2B RID: 15403 RVA: 0x00116DD0 File Offset: 0x001151D0
	private float nextHologramTime()
	{
		return Time.time + UnityEngine.Random.Range(2f, 5f);
	}

	// Token: 0x06003C2C RID: 15404 RVA: 0x00116DE8 File Offset: 0x001151E8
	private void instantiateNewHologram()
	{
		HologramData randomHologram = this.getRandomHologram();
		GameObject prefab = base.config.defaultCharacterEffects.hologram.prefab;
		if (randomHologram.hasOverrideVFX && randomHologram.overrideVFX != null)
		{
			prefab = randomHologram.overrideVFX.prefab;
		}
		this.hologramsSpawned++;
		float x = (this.hologramsSpawned % 2 != 0) ? 50f : -50f;
		Vector3 position = new Vector3(x, -23f, 2f);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, position, Quaternion.identity);
		ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			particleSystem.transform.localScale = Vector3.one * 20f;
		}
		HologramDisplay component = gameObject.GetComponent<HologramDisplay>();
		component.SetTexture(randomHologram.texture);
		component.PlayHologram(HologramDisplay.PlayBehavior.Once, 0f);
		this.activeHolograms.Add(component);
	}

	// Token: 0x06003C2D RID: 15405 RVA: 0x00116EF8 File Offset: 0x001152F8
	private void resetCredits()
	{
		IEnumerator enumerator = this.creditsRootStart.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06003C2E RID: 15406 RVA: 0x00116F64 File Offset: 0x00115364
	private void instantiateCredits(CreditsData credits)
	{
		this.creditsRoot = new GameObject("credits root").transform;
		this.creditsRoot.SetParent(this.creditsRootStart);
		float num = this.startScrollPosition;
		foreach (CreditEntryData creditEntryData in credits.entries)
		{
			if (creditEntryData is CreditHeadingData)
			{
				this.createMessageObject((creditEntryData as CreditHeadingData).heading, num, true, 40f);
			}
			else if (creditEntryData is CreditCategoryData)
			{
				this.createMessageObject((creditEntryData as CreditCategoryData).category, num, true, 32f);
			}
			else if (creditEntryData is CreditMessageData)
			{
				this.createMessageObject((creditEntryData as CreditMessageData).message, num, false, 28f);
			}
			else if (creditEntryData is CreditNameData)
			{
				CreditNameData creditNameData = creditEntryData as CreditNameData;
				CreditLine component = UnityEngine.Object.Instantiate<GameObject>(this.creditLinePrefab).GetComponent<CreditLine>();
				component.Set(creditNameData.name, creditNameData.title);
				component.transform.SetParent(this.creditsRoot);
				component.transform.localPosition = new Vector3(0f, num, 0f);
			}
			num -= creditEntryData.spacing;
		}
		this.endScrollPosition = -(num + this.startScrollPosition);
	}

	// Token: 0x06003C2F RID: 15407 RVA: 0x001170E8 File Offset: 0x001154E8
	private void createMessageObject(string message, float yPos, bool underline = true, float fontSize = 40f)
	{
		CreditHeading component = UnityEngine.Object.Instantiate<GameObject>(this.creditHeadingPrefab).GetComponent<CreditHeading>();
		component.Set(message, underline, fontSize);
		component.transform.SetParent(this.creditsRoot);
		component.transform.localPosition = new Vector3(0f, yPos, 0f);
	}

	// Token: 0x06003C30 RID: 15408 RVA: 0x0011713C File Offset: 0x0011553C
	private void Update()
	{
		if (this.creditsRoot == null)
		{
			return;
		}
		if (Time.time >= this.hologramInstantiateTime)
		{
			this.instantiateNewHologram();
			this.hologramInstantiateTime = this.nextHologramTime();
		}
		for (int i = this.activeHolograms.Count - 1; i >= 0; i--)
		{
			HologramDisplay hologramDisplay = this.activeHolograms[i];
			Vector3 position = hologramDisplay.transform.position;
			position.y += 10f * Time.deltaTime;
			hologramDisplay.transform.position = position;
			if (hologramDisplay.transform.position.y > 100f)
			{
				UnityEngine.Object.Destroy(hologramDisplay.gameObject);
				this.activeHolograms.RemoveAt(i);
			}
		}
		float num = 1f;
		if (this.stickReleaseTimer > 0f)
		{
			this.stickReleaseTimer -= Time.deltaTime;
			num = this.scrollMultiplier;
		}
		Vector3 position2 = this.creditsRoot.position;
		position2.y += this.scrollSpeed * Time.deltaTime * num;
		if (position2.y > this.endScrollPosition)
		{
			position2.y = this.startScrollPosition;
		}
		else if (position2.y < this.startScrollPosition)
		{
			position2.y = this.endScrollPosition;
		}
		this.creditsRoot.position = position2;
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x001172B8 File Offset: 0x001156B8
	public void UpdateRightStick(float x, float y)
	{
		if (Mathf.Abs(y) > 0.1f)
		{
			this.stickReleaseTimer = this.stickReleaseTime;
			this.scrollMultiplier = y * 4f;
		}
		else
		{
			this.scrollMultiplier = 0f;
		}
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x001172F4 File Offset: 0x001156F4
	private void shuffleIndices(int[] indices)
	{
		for (int i = indices.Length - 1; i > 0; i--)
		{
			int num = UnityEngine.Random.Range(0, i + 1);
			int num2 = indices[i];
			indices[i] = indices[num];
			indices[num] = num2;
		}
	}

	// Token: 0x0400293C RID: 10556
	public Transform creditsRootStart;

	// Token: 0x0400293D RID: 10557
	public GameObject creditHeadingPrefab;

	// Token: 0x0400293E RID: 10558
	public GameObject creditLinePrefab;

	// Token: 0x0400293F RID: 10559
	public float headingSpacing = 60f;

	// Token: 0x04002940 RID: 10560
	public float creditSpacing = 20f;

	// Token: 0x04002941 RID: 10561
	public float scrollSpeed = 5f;

	// Token: 0x04002942 RID: 10562
	public float startScrollPosition = -50f;

	// Token: 0x04002943 RID: 10563
	public float stickReleaseTime = 2f;

	// Token: 0x04002944 RID: 10564
	private Transform creditsRoot;

	// Token: 0x04002945 RID: 10565
	private float endScrollPosition;

	// Token: 0x04002946 RID: 10566
	private float hologramInstantiateTime;

	// Token: 0x04002947 RID: 10567
	private EquippableItem[] allHolograms;

	// Token: 0x04002948 RID: 10568
	private int hologramsSpawned;

	// Token: 0x04002949 RID: 10569
	private float stickReleaseTimer;

	// Token: 0x0400294A RID: 10570
	private float scrollMultiplier = 1f;

	// Token: 0x0400294B RID: 10571
	private List<HologramDisplay> activeHolograms = new List<HologramDisplay>();

	// Token: 0x0400294C RID: 10572
	private int[] hologramIndicies;

	// Token: 0x0400294D RID: 10573
	private int hologramIndex;
}
