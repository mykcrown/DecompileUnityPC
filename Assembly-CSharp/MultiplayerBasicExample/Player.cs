using System;
using InControl;
using UnityEngine;

namespace MultiplayerBasicExample
{
	// Token: 0x02000046 RID: 70
	public class Player : MonoBehaviour
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00010969 File Offset: 0x0000ED69
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00010971 File Offset: 0x0000ED71
		public InputDevice Device { get; set; }

		// Token: 0x06000256 RID: 598 RVA: 0x0001097A File Offset: 0x0000ED7A
		private void Start()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00010988 File Offset: 0x0000ED88
		private void Update()
		{
			if (this.Device == null)
			{
				this.cachedRenderer.material.color = new Color(1f, 1f, 1f, 0.2f);
			}
			else
			{
				this.cachedRenderer.material.color = this.GetColorFromInput();
				base.transform.Rotate(Vector3.down, 500f * Time.deltaTime * this.Device.Direction.X, Space.World);
				base.transform.Rotate(Vector3.right, 500f * Time.deltaTime * this.Device.Direction.Y, Space.World);
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00010A40 File Offset: 0x0000EE40
		private Color GetColorFromInput()
		{
			if (this.Device.Action1)
			{
				return Color.green;
			}
			if (this.Device.Action2)
			{
				return Color.red;
			}
			if (this.Device.Action3)
			{
				return Color.blue;
			}
			if (this.Device.Action4)
			{
				return Color.yellow;
			}
			return Color.white;
		}

		// Token: 0x040001C1 RID: 449
		private Renderer cachedRenderer;
	}
}
