// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace InControl
{
	public class UnityMouseAxisSource : InputControlSource
	{
		public string MouseAxisQuery;

		public UnityMouseAxisSource()
		{
		}

		public UnityMouseAxisSource(string axis)
		{
			this.MouseAxisQuery = "mouse " + axis;
		}

		public float GetValue(InputDevice inputDevice)
		{
			return Input.GetAxisRaw(this.MouseAxisQuery);
		}

		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}
	}
}
