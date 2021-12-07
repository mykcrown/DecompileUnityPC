using System;
using InControl;
using UnityEngine;

namespace VirtualDeviceExample
{
	// Token: 0x0200004D RID: 77
	public class VirtualDeviceExample : MonoBehaviour
	{
		// Token: 0x06000287 RID: 647 RVA: 0x0001360A File Offset: 0x00011A0A
		private void OnEnable()
		{
			this.virtualDevice = new VirtualDevice();
			InputManager.OnSetup += delegate()
			{
				InputManager.AttachDevice(this.virtualDevice);
			};
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00013628 File Offset: 0x00011A28
		private void OnDisable()
		{
			InputManager.DetachDevice(this.virtualDevice);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00013638 File Offset: 0x00011A38
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			this.leftObject.transform.Rotate(Vector3.down, 500f * Time.deltaTime * activeDevice.LeftStickX, Space.World);
			this.leftObject.transform.Rotate(Vector3.right, 500f * Time.deltaTime * activeDevice.LeftStickY, Space.World);
			this.rightObject.transform.Rotate(Vector3.down, 500f * Time.deltaTime * activeDevice.RightStickX, Space.World);
			this.rightObject.transform.Rotate(Vector3.right, 500f * Time.deltaTime * activeDevice.RightStickY, Space.World);
			Color color = Color.white;
			if (activeDevice.Action1.IsPressed)
			{
				color = Color.green;
			}
			if (activeDevice.Action2.IsPressed)
			{
				color = Color.red;
			}
			if (activeDevice.Action3.IsPressed)
			{
				color = Color.blue;
			}
			if (activeDevice.Action4.IsPressed)
			{
				color = Color.yellow;
			}
			this.leftObject.GetComponent<Renderer>().material.color = color;
		}

		// Token: 0x040001DE RID: 478
		public GameObject leftObject;

		// Token: 0x040001DF RID: 479
		public GameObject rightObject;

		// Token: 0x040001E0 RID: 480
		private VirtualDevice virtualDevice;
	}
}
