using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000491 RID: 1169
public class ObjectForMaterialQualitySwitcher : MonoBehaviour
{
	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x06001965 RID: 6501 RVA: 0x00084810 File Offset: 0x00082C10
	// (set) Token: 0x06001966 RID: 6502 RVA: 0x00084818 File Offset: 0x00082C18
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel { get; set; }

	// Token: 0x06001967 RID: 6503 RVA: 0x00084824 File Offset: 0x00082C24
	private IEnumerator Start()
	{
		StaticInject.Inject(this);
		while (this.userVideoSettingsModel == null)
		{
			yield return null;
		}
		if (this.lowQuality)
		{
			this.lowQuality.SetActive(this.userVideoSettingsModel.MaterialQuality == ThreeTierQualityLevel.Low);
		}
		if (this.highQuality)
		{
			this.highQuality.SetActive(this.userVideoSettingsModel.MaterialQuality != ThreeTierQualityLevel.Low);
		}
		yield break;
	}

	// Token: 0x0400131E RID: 4894
	public GameObject lowQuality;

	// Token: 0x0400131F RID: 4895
	public GameObject highQuality;
}
