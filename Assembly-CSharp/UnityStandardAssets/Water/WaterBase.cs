using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000BA0 RID: 2976
	[ExecuteInEditMode]
	public class WaterBase : MonoBehaviour
	{
		// Token: 0x060055EE RID: 21998 RVA: 0x001B7974 File Offset: 0x001B5D74
		public void UpdateShader()
		{
			if (this.waterQuality > WaterQuality.Medium)
			{
				this.sharedMaterial.shader.maximumLOD = 501;
			}
			else if (this.waterQuality > WaterQuality.Low)
			{
				this.sharedMaterial.shader.maximumLOD = 301;
			}
			else
			{
				this.sharedMaterial.shader.maximumLOD = 201;
			}
			if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.edgeBlend = false;
			}
			if (this.edgeBlend)
			{
				Shader.EnableKeyword("WATER_EDGEBLEND_ON");
				Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
				if (Camera.main)
				{
					Camera.main.depthTextureMode |= DepthTextureMode.Depth;
				}
			}
			else
			{
				Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
				Shader.DisableKeyword("WATER_EDGEBLEND_ON");
			}
		}

		// Token: 0x060055EF RID: 21999 RVA: 0x001B7A4D File Offset: 0x001B5E4D
		public void WaterTileBeingRendered(Transform tr, Camera currentCam)
		{
			if (currentCam && this.edgeBlend)
			{
				currentCam.depthTextureMode |= DepthTextureMode.Depth;
			}
		}

		// Token: 0x060055F0 RID: 22000 RVA: 0x001B7A73 File Offset: 0x001B5E73
		public void Update()
		{
			if (this.sharedMaterial)
			{
				this.UpdateShader();
			}
		}

		// Token: 0x040036A0 RID: 13984
		public Material sharedMaterial;

		// Token: 0x040036A1 RID: 13985
		public WaterQuality waterQuality = WaterQuality.High;

		// Token: 0x040036A2 RID: 13986
		public bool edgeBlend = true;
	}
}
