// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VirtualDeviceExample
{
	public class VirtualDeviceExample : MonoBehaviour
	{
		public GameObject leftObject;

		public GameObject rightObject;

		private VirtualDevice virtualDevice;

		private void OnEnable()
		{
			this.virtualDevice = new VirtualDevice();
			InputManager.OnSetup += new Action(this._OnEnable_m__0);
		}

		private void OnDisable()
		{
			InputManager.DetachDevice(this.virtualDevice);
		}

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

		private void _OnEnable_m__0()
		{
			InputManager.AttachDevice(this.virtualDevice);
		}
	}
}
