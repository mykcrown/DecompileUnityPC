// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class StoreSettings
{
	public bool enableAlignerLiveRotationEdits;

	public float analogSpinSpeed = 5f;

	public float clickSpinSpeed = 5f;

	public float netsukeSelectorSpinSpeed = 5f;

	public GameObject playerTokenPreviewPrefab;

	public GameObject playerCardIconPreviewPrefab;

	public GameObject voiceTauntPreviewPrefab;

	public GameObject unlockTokenDisplayPrefab;

	public float voiceTauntPlayDelay = 0.5f;

	public float hologramLoopInterval = 4.75f;

	public ShaderVariantCollection shaderVariants;

	public GameObject playerTokenTemplate;

	public UnboxingItemDisplayParameters[] unboxingDisplayItems = new UnboxingItemDisplayParameters[0];

	public GalleryDisplayParameters[] galleryDisplayItems = new GalleryDisplayParameters[0];

	public UnboxingItemDisplayParameters GetUnboxingParameters(EquipmentTypes type)
	{
		UnboxingItemDisplayParameters[] array = this.unboxingDisplayItems;
		for (int i = 0; i < array.Length; i++)
		{
			UnboxingItemDisplayParameters unboxingItemDisplayParameters = array[i];
			if (unboxingItemDisplayParameters.itemType == type)
			{
				return unboxingItemDisplayParameters;
			}
		}
		return null;
	}

	public GalleryDisplayParameters GetGalleryParameters(EquipmentTypes type)
	{
		GalleryDisplayParameters[] array = this.galleryDisplayItems;
		for (int i = 0; i < array.Length; i++)
		{
			GalleryDisplayParameters galleryDisplayParameters = array[i];
			if (galleryDisplayParameters.itemType == type)
			{
				return galleryDisplayParameters;
			}
		}
		return null;
	}
}
