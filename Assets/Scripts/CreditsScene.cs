// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScene : UIScene
{
	public Transform creditsRootStart;

	public GameObject creditHeadingPrefab;

	public GameObject creditLinePrefab;

	public float headingSpacing = 60f;

	public float creditSpacing = 20f;

	public float scrollSpeed = 5f;

	public float startScrollPosition = -50f;

	public float stickReleaseTime = 2f;

	private Transform creditsRoot;

	private float endScrollPosition;

	private float hologramInstantiateTime;

	private EquippableItem[] allHolograms;

	private int hologramsSpawned;

	private float stickReleaseTimer;

	private float scrollMultiplier = 1f;

	private List<HologramDisplay> activeHolograms = new List<HologramDisplay>();

	private int[] hologramIndicies;

	private int hologramIndex;

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public IItemLoader itemLoader
	{
		get;
		set;
	}

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

	private float nextHologramTime()
	{
		return Time.time + UnityEngine.Random.Range(2f, 5f);
	}

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
		ParticleSystem[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			ParticleSystem particleSystem = array[i];
			particleSystem.transform.localScale = Vector3.one * 20f;
		}
		HologramDisplay component = gameObject.GetComponent<HologramDisplay>();
		component.SetTexture(randomHologram.texture);
		component.PlayHologram(HologramDisplay.PlayBehavior.Once, 0f);
		this.activeHolograms.Add(component);
	}

	private void resetCredits()
	{
		IEnumerator enumerator = this.creditsRootStart.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
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

	private void instantiateCredits(CreditsData credits)
	{
		this.creditsRoot = new GameObject("credits root").transform;
		this.creditsRoot.SetParent(this.creditsRootStart);
		float num = this.startScrollPosition;
		foreach (CreditEntryData current in credits.entries)
		{
			if (current is CreditHeadingData)
			{
				this.createMessageObject((current as CreditHeadingData).heading, num, true, 40f);
			}
			else if (current is CreditCategoryData)
			{
				this.createMessageObject((current as CreditCategoryData).category, num, true, 32f);
			}
			else if (current is CreditMessageData)
			{
				this.createMessageObject((current as CreditMessageData).message, num, false, 28f);
			}
			else if (current is CreditNameData)
			{
				CreditNameData creditNameData = current as CreditNameData;
				CreditLine component = UnityEngine.Object.Instantiate<GameObject>(this.creditLinePrefab).GetComponent<CreditLine>();
				component.Set(creditNameData.name, creditNameData.title);
				component.transform.SetParent(this.creditsRoot);
				component.transform.localPosition = new Vector3(0f, num, 0f);
			}
			num -= current.spacing;
		}
		this.endScrollPosition = -(num + this.startScrollPosition);
	}

	private void createMessageObject(string message, float yPos, bool underline = true, float fontSize = 40f)
	{
		CreditHeading component = UnityEngine.Object.Instantiate<GameObject>(this.creditHeadingPrefab).GetComponent<CreditHeading>();
		component.Set(message, underline, fontSize);
		component.transform.SetParent(this.creditsRoot);
		component.transform.localPosition = new Vector3(0f, yPos, 0f);
	}

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
}
