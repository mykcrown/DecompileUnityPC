// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace InControl
{
	public abstract class InputDeviceManager
	{
		protected List<InputDevice> devices = new List<InputDevice>();

		public abstract void Update(ulong updateTick, float deltaTime);

		public virtual void Destroy()
		{
		}
	}
}
