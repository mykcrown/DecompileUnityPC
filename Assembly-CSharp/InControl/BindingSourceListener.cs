using System;

namespace InControl
{
	// Token: 0x02000053 RID: 83
	public interface BindingSourceListener
	{
		// Token: 0x060002B2 RID: 690
		void Reset();

		// Token: 0x060002B3 RID: 691
		BindingSource Listen(BindingListenOptions listenOptions, InputDevice device);
	}
}
