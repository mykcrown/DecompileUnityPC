using System;
using UnityEngine;

namespace MultiplayerWithBindingsExample
{
	// Token: 0x02000048 RID: 72
	public class Player : MonoBehaviour
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00010DAC File Offset: 0x0000F1AC
		// (set) Token: 0x06000265 RID: 613 RVA: 0x00010DB4 File Offset: 0x0000F1B4
		public PlayerActions Actions { get; set; }

		// Token: 0x06000266 RID: 614 RVA: 0x00010DBD File Offset: 0x0000F1BD
		private void OnDisable()
		{
			if (this.Actions != null)
			{
				this.Actions.Destroy();
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00010DD5 File Offset: 0x0000F1D5
		private void Start()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00010DE4 File Offset: 0x0000F1E4
		private void Update()
		{
			if (this.Actions == null)
			{
				this.cachedRenderer.material.color = new Color(1f, 1f, 1f, 0.2f);
			}
			else
			{
				this.cachedRenderer.material.color = this.GetColorFromInput();
				base.transform.Rotate(Vector3.down, 500f * Time.deltaTime * this.Actions.Rotate.X, Space.World);
				base.transform.Rotate(Vector3.right, 500f * Time.deltaTime * this.Actions.Rotate.Y, Space.World);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00010E9C File Offset: 0x0000F29C
		private Color GetColorFromInput()
		{
			if (this.Actions.Green)
			{
				return Color.green;
			}
			if (this.Actions.Red)
			{
				return Color.red;
			}
			if (this.Actions.Blue)
			{
				return Color.blue;
			}
			if (this.Actions.Yellow)
			{
				return Color.yellow;
			}
			return Color.white;
		}

		// Token: 0x040001C7 RID: 455
		private Renderer cachedRenderer;
	}
}
