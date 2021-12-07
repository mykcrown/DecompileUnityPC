using System;

// Token: 0x02000874 RID: 2164
public interface IRollbackInput : IResetable
{
	// Token: 0x17000D1B RID: 3355
	// (get) Token: 0x060035F5 RID: 13813
	// (set) Token: 0x060035F6 RID: 13814
	InputValuesSnapshot values { get; set; }

	// Token: 0x17000D1C RID: 3356
	// (get) Token: 0x060035F7 RID: 13815
	// (set) Token: 0x060035F8 RID: 13816
	int frame { get; set; }

	// Token: 0x17000D1D RID: 3357
	// (get) Token: 0x060035F9 RID: 13817
	// (set) Token: 0x060035FA RID: 13818
	int playerID { get; set; }

	// Token: 0x060035FB RID: 13819
	bool Equals(IRollbackInput other);
}
