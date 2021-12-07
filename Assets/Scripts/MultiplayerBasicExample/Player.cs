// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using UnityEngine;

namespace MultiplayerBasicExample
{
	public class Player : MonoBehaviour
	{
		private Renderer cachedRenderer;

		public InputDevice Device
		{
			get;
			set;
		}

		private void Start()
		{
			this.cachedRenderer = base.GetComponent<Renderer>();
		}

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
	}
}
