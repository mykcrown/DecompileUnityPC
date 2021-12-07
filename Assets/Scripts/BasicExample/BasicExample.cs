// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using UnityEngine;

namespace BasicExample
{
	public class BasicExample : MonoBehaviour
	{
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
