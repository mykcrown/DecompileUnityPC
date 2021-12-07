using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000B98 RID: 2968
	[ExecuteInEditMode]
	[RequireComponent(typeof(WaterBase))]
	public class Displace : MonoBehaviour
	{
		// Token: 0x060055CA RID: 21962 RVA: 0x001B62FF File Offset: 0x001B46FF
		public void Awake()
		{
			if (base.enabled)
			{
				this.OnEnable();
			}
			else
			{
				this.OnDisable();
			}
		}

		// Token: 0x060055CB RID: 21963 RVA: 0x001B631D File Offset: 0x001B471D
		public void OnEnable()
		{
			Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
			Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
		}

		// Token: 0x060055CC RID: 21964 RVA: 0x001B6333 File Offset: 0x001B4733
		public void OnDisable()
		{
			Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
			Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
		}
	}
}
