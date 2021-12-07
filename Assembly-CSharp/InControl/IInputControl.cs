using System;

namespace InControl
{
	// Token: 0x02000069 RID: 105
	public interface IInputControl
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003B4 RID: 948
		bool HasChanged { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060003B5 RID: 949
		bool IsPressed { get; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003B6 RID: 950
		bool WasPressed { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003B7 RID: 951
		bool WasReleased { get; }

		// Token: 0x060003B8 RID: 952
		void ClearInputState();
	}
}
