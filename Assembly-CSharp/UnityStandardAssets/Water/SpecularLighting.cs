using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000B9C RID: 2972
	[RequireComponent(typeof(WaterBase))]
	[ExecuteInEditMode]
	public class SpecularLighting : MonoBehaviour
	{
		// Token: 0x060055E1 RID: 21985 RVA: 0x001B6C17 File Offset: 0x001B5017
		public void Start()
		{
			this.m_WaterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
		}

		// Token: 0x060055E2 RID: 21986 RVA: 0x001B6C3C File Offset: 0x001B503C
		public void Update()
		{
			if (!this.m_WaterBase)
			{
				this.m_WaterBase = (WaterBase)base.gameObject.GetComponent(typeof(WaterBase));
			}
			if (this.specularLight && this.m_WaterBase.sharedMaterial)
			{
				this.m_WaterBase.sharedMaterial.SetVector("_WorldLightDir", this.specularLight.transform.forward);
			}
		}

		// Token: 0x04003688 RID: 13960
		public Transform specularLight;

		// Token: 0x04003689 RID: 13961
		private WaterBase m_WaterBase;
	}
}
