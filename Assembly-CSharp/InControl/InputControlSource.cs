using System;

namespace InControl
{
	// Token: 0x0200006C RID: 108
	public interface InputControlSource
	{
		// Token: 0x060003CC RID: 972
		float GetValue(InputDevice inputDevice);

		// Token: 0x060003CD RID: 973
		bool GetState(InputDevice inputDevice);
	}
}
