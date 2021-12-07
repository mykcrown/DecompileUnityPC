using System;
using UnityEngine;

namespace InterfaceMovement
{
	// Token: 0x02000042 RID: 66
	public class Button : MonoBehaviour
	{
		// Token: 0x06000249 RID: 585 RVA: 0x00010746 File Offset: 0x0000EB46
		private void Start()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00010754 File Offset: 0x0000EB54
		private void Update()
		{
			bool flag = base.transform.parent.GetComponent<ButtonManager>().focusedButton == this;
			Color color = this.cachedRenderer.material.color;
			color.a = Mathf.MoveTowards(color.a, (!flag) ? 0.5f : 1f, Time.deltaTime * 3f);
			this.cachedRenderer.material.color = color;
		}

		// Token: 0x040001B9 RID: 441
		private Renderer cachedRenderer;

		// Token: 0x040001BA RID: 442
		public Button up;

		// Token: 0x040001BB RID: 443
		public Button down;

		// Token: 0x040001BC RID: 444
		public Button left;

		// Token: 0x040001BD RID: 445
		public Button right;
	}
}
