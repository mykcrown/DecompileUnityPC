// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace InControl
{
	public interface BindingSourceListener
	{
		void Reset();

		BindingSource Listen(BindingListenOptions listenOptions, InputDevice device);
	}
}
