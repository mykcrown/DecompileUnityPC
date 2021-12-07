// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VideoSettingsUtility : IVideoSettingsUtility
{
	[Inject]
	public IUserVideoSettingsModel userVideoSettings
	{
		get;
		set;
	}

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
