using System;
using InControl;
using UnityEngine;

namespace BasicExample
{
	// Token: 0x0200003F RID: 63
	public class BasicExample : MonoBehaviour
	{
		// Token: 0x06000239 RID: 569 RVA: 0x0000F5F4 File Offset: 0x0000D9F4
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			base.transform.Rotate(Vector3.down, 500f * Time.deltaTime * activeDevice.LeftStickX, Space.World);
			base.transform.Rotate(Vector3.right, 500f * Time.deltaTime * activeDevice.LeftStickY, Space.World);
			Color a = (!activeDevice.Action1.IsPressed) ? Color.white : Color.red;
			Color b = (!activeDevice.Action2.IsPressed) ? Color.white : Color.green;
			base.GetComponent<Renderer>().material.color = Color.Lerp(a, b, 0.5f);
		}
	}
}
