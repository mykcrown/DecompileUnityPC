// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace InControl
{
	public class UnityMouseButtonSource : InputControlSource
	{
		public int ButtonId;

		public UnityMouseButtonSource()
		{
		}

		public UnityMouseButtonSource(int buttonId)
		{
			this.ButtonId = buttonId;
		}

		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		public bool GetState(InputDevice inputDevice)
		{
			return Input.GetMouseButton(this.ButtonId);
		}
	}
}
