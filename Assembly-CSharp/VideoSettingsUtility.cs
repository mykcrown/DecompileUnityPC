using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02000493 RID: 1171
public class VideoSettingsUtility : IVideoSettingsUtility
{
	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x060019A4 RID: 6564 RVA: 0x00085133 File Offset: 0x00083533
	// (set) Token: 0x060019A5 RID: 6565 RVA: 0x0008513B File Offset: 0x0008353B
	[Inject]
	public IUserVideoSettingsModel userVideoSettings { get; set; }

	// Token: 0x060019A6 RID: 6566 RVA: 0x00085144 File Offset: 0x00083544
	public void ApplyToCamera(Camera camera)
	{
		if (camera != null)
		{
			camera.depthTextureMode = DepthTextureMode.None;
			camera.allowMSAA = false;
			camera.allowDynamicResolution = false;
			camera.useOcclusionCulling = true;
			PostProcessLayer component = camera.GetComponent<PostProcessLayer>();
			if (component != null && component.enabled)
			{
				component.enabled = this.userVideoSettings.PostProcessing;
			}
		}
	}
}
