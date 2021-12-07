using System;
using InControl;
using UnityEngine;

namespace TouchExample
{
	// Token: 0x0200004B RID: 75
	public class CubeController : MonoBehaviour
	{
		// Token: 0x0600027B RID: 635 RVA: 0x00011524 File Offset: 0x0000F924
		private void Start()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00011534 File Offset: 0x0000F934
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			if (activeDevice != InputDevice.Null && activeDevice != TouchManager.Device)
			{
				TouchManager.ControlsEnabled = false;
			}
			this.cachedRenderer.material.color = this.GetColorFromActionButtons(activeDevice);
			base.transform.Rotate(Vector3.down, 500f * Time.deltaTime * activeDevice.Direction.X, Space.World);
			base.transform.Rotate(Vector3.right, 500f * Time.deltaTime * activeDevice.Direction.Y, Space.World);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000115CC File Offset: 0x0000F9CC
		private Color GetColorFromActionButtons(InputDevice inputDevice)
		{
			if (inputDevice.Action1)
			{
				return Color.green;
			}
			if (inputDevice.Action2)
			{
				return Color.red;
			}
			if (inputDevice.Action3)
			{
				return Color.blue;
			}
			if (inputDevice.Action4)
			{
				return Color.yellow;
			}
			return Color.white;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00011638 File Offset: 0x0000FA38
		private void OnGUI()
		{
			float num = 10f;
			int touchCount = TouchManager.TouchCount;
			for (int i = 0; i < touchCount; i++)
			{
				InControl.Touch touch = TouchManager.GetTouch(i);
				GUI.Label(new Rect(10f, num, 500f, num + 15f), string.Concat(new object[]
				{
					string.Empty,
					i,
					": fingerId = ",
					touch.fingerId,
					", phase = ",
					touch.phase.ToString(),
					", position = ",
					touch.position
				}));
				num += 20f;
			}
		}

		// Token: 0x040001D7 RID: 471
		private Renderer cachedRenderer;
	}
}
