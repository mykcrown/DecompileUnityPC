using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000BA1 RID: 2977
	[ExecuteInEditMode]
	public class WaterTile : MonoBehaviour
	{
		// Token: 0x060055F2 RID: 22002 RVA: 0x001B7A93 File Offset: 0x001B5E93
		public void Start()
		{
			this.AcquireComponents();
		}

		// Token: 0x060055F3 RID: 22003 RVA: 0x001B7A9C File Offset: 0x001B5E9C
		private void AcquireComponents()
		{
			if (!this.reflection)
			{
				if (base.transform.parent)
				{
					this.reflection = base.transform.parent.GetComponent<PlanarReflection>();
				}
				else
				{
					this.reflection = base.transform.GetComponent<PlanarReflection>();
				}
			}
			if (!this.waterBase)
			{
				if (base.transform.parent)
				{
					this.waterBase = base.transform.parent.GetComponent<WaterBase>();
				}
				else
				{
					this.waterBase = base.transform.GetComponent<WaterBase>();
				}
			}
		}

		// Token: 0x060055F4 RID: 22004 RVA: 0x001B7B4C File Offset: 0x001B5F4C
		public void OnWillRenderObject()
		{
			if (this.reflection)
			{
				this.reflection.WaterTileBeingRendered(base.transform, Camera.current);
			}
			if (this.waterBase)
			{
				this.waterBase.WaterTileBeingRendered(base.transform, Camera.current);
			}
		}

		// Token: 0x040036A3 RID: 13987
		public PlanarReflection reflection;

		// Token: 0x040036A4 RID: 13988
		public WaterBase waterBase;
	}
}
