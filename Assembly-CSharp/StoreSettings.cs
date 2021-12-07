using System;
using UnityEngine;

// Token: 0x020003FA RID: 1018
[Serializable]
public class StoreSettings
{
	// Token: 0x060015A2 RID: 5538 RVA: 0x0007708C File Offset: 0x0007548C
	public UnboxingItemDisplayParameters GetUnboxingParameters(EquipmentTypes type)
	{
		foreach (UnboxingItemDisplayParameters unboxingItemDisplayParameters in this.unboxingDisplayItems)
		{
			if (unboxingItemDisplayParameters.itemType == type)
			{
				return unboxingItemDisplayParameters;
			}
		}
		return null;
	}

	// Token: 0x060015A3 RID: 5539 RVA: 0x000770C8 File Offset: 0x000754C8
	public GalleryDisplayParameters GetGalleryParameters(EquipmentTypes type)
	{
		foreach (GalleryDisplayParameters galleryDisplayParameters in this.galleryDisplayItems)
		{
			if (galleryDisplayParameters.itemType == type)
			{
				return galleryDisplayParameters;
			}
		}
		return null;
	}

	// Token: 0x04001012 RID: 4114
	public bool enableAlignerLiveRotationEdits;

	// Token: 0x04001013 RID: 4115
	public float analogSpinSpeed = 5f;

	// Token: 0x04001014 RID: 4116
	public float clickSpinSpeed = 5f;

	// Token: 0x04001015 RID: 4117
	public float netsukeSelectorSpinSpeed = 5f;

	// Token: 0x04001016 RID: 4118
	public GameObject playerTokenPreviewPrefab;

	// Token: 0x04001017 RID: 4119
	public GameObject playerCardIconPreviewPrefab;

	// Token: 0x04001018 RID: 4120
	public GameObject voiceTauntPreviewPrefab;

	// Token: 0x04001019 RID: 4121
	public GameObject unlockTokenDisplayPrefab;

	// Token: 0x0400101A RID: 4122
	public float voiceTauntPlayDelay = 0.5f;

	// Token: 0x0400101B RID: 4123
	public float hologramLoopInterval = 4.75f;

	// Token: 0x0400101C RID: 4124
	public ShaderVariantCollection shaderVariants;

	// Token: 0x0400101D RID: 4125
	public GameObject playerTokenTemplate;

	// Token: 0x0400101E RID: 4126
	public UnboxingItemDisplayParameters[] unboxingDisplayItems = new UnboxingItemDisplayParameters[0];

	// Token: 0x0400101F RID: 4127
	public GalleryDisplayParameters[] galleryDisplayItems = new GalleryDisplayParameters[0];
}
