using System;
using UnityEngine;

namespace WDDebug
{
	// Token: 0x020002EA RID: 746
	public class CharacterProfilerCamera : MonoBehaviour
	{
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0005D9BA File Offset: 0x0005BDBA
		// (set) Token: 0x06000F78 RID: 3960 RVA: 0x0005D9C2 File Offset: 0x0005BDC2
		public GameObject Focus
		{
			get
			{
				return this.focus;
			}
			set
			{
				this.focus = value;
				if (this.focus == null)
				{
					this.renderers = new Renderer[0];
				}
				else
				{
					this.renderers = this.focus.GetComponentsInChildren<Renderer>();
				}
			}
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0005DA00 File Offset: 0x0005BE00
		private void Update()
		{
			if (this.focus == null)
			{
				return;
			}
			this.bounds.center = Vector3.zero;
			this.bounds.size = Vector3.zero;
			foreach (Renderer renderer in this.renderers)
			{
				if (renderer != null)
				{
					this.bounds.Encapsulate(renderer.bounds);
				}
			}
			Vector3 target = new Vector3(this.bounds.center.x, this.bounds.center.y, -this.distance);
			Vector3 vector = base.transform.position;
			vector = Vector3.SmoothDamp(vector, target, ref this.velocity, this.smoothFactor * Time.deltaTime);
			base.transform.position = vector;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0005DAE8 File Offset: 0x0005BEE8
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(this.bounds.center, this.bounds.size);
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(this.bounds.center, 0.25f);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(new Vector3(base.transform.position.x, base.transform.position.y, 0f), 0.3f);
		}

		// Token: 0x04000A29 RID: 2601
		public float distance = 5f;

		// Token: 0x04000A2A RID: 2602
		public float smoothFactor = 0.5f;

		// Token: 0x04000A2B RID: 2603
		private GameObject focus;

		// Token: 0x04000A2C RID: 2604
		private Renderer[] renderers;

		// Token: 0x04000A2D RID: 2605
		private Bounds bounds;

		// Token: 0x04000A2E RID: 2606
		private Vector3 velocity;
	}
}
