using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000490 RID: 1168
public class MaterialQualitySwitcher : MonoBehaviour
{
	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x06001961 RID: 6497 RVA: 0x000846BE File Offset: 0x00082ABE
	// (set) Token: 0x06001962 RID: 6498 RVA: 0x000846C6 File Offset: 0x00082AC6
	[Inject]
	public IUserVideoSettingsModel userVideoSettingsModel { get; set; }

	// Token: 0x06001963 RID: 6499 RVA: 0x000846D0 File Offset: 0x00082AD0
	private IEnumerator Start()
	{
		StaticInject.Inject(this);
		while (this.userVideoSettingsModel == null)
		{
			yield return null;
		}
		if (this.userVideoSettingsModel.MaterialQuality == ThreeTierQualityLevel.Low)
		{
			if (this.lowQuality)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.lowQuality;
			}
		}
		else if (this.highQuality)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.highQuality;
		}
		yield break;
	}

	// Token: 0x0400131B RID: 4891
	public Material lowQuality;

	// Token: 0x0400131C RID: 4892
	public Material highQuality;
}
