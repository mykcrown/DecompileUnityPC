using System;
using UnityEngine;

// Token: 0x02000439 RID: 1081
public class GeneratedEffect
{
	// Token: 0x06001651 RID: 5713 RVA: 0x00079655 File Offset: 0x00077A55
	public GeneratedEffect(Effect effectData, GameObject effectVisual, ParticleData particleInfo)
	{
		this.EffectController = effectData;
		this.EffectObject = effectVisual;
		this.ParticleInfo = particleInfo;
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06001652 RID: 5714 RVA: 0x00079672 File Offset: 0x00077A72
	// (set) Token: 0x06001653 RID: 5715 RVA: 0x0007967A File Offset: 0x00077A7A
	public Effect EffectController { get; private set; }

	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06001654 RID: 5716 RVA: 0x00079683 File Offset: 0x00077A83
	// (set) Token: 0x06001655 RID: 5717 RVA: 0x0007968B File Offset: 0x00077A8B
	public GameObject EffectObject { get; private set; }

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06001656 RID: 5718 RVA: 0x00079694 File Offset: 0x00077A94
	// (set) Token: 0x06001657 RID: 5719 RVA: 0x0007969C File Offset: 0x00077A9C
	public ParticleData ParticleInfo { get; private set; }

	// Token: 0x06001658 RID: 5720 RVA: 0x000796A5 File Offset: 0x00077AA5
	public void Expire()
	{
		if (this.EffectController != null)
		{
			this.EffectController.Destroy();
			this.EffectController = null;
		}
		this.EffectObject = null;
		this.ParticleInfo = null;
	}

	// Token: 0x04001123 RID: 4387
	private Effect effectData;

	// Token: 0x04001124 RID: 4388
	private GameObject effectVisual;

	// Token: 0x04001125 RID: 4389
	private ParticleData particleInfo;
}
