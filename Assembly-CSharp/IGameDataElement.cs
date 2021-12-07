using System;

// Token: 0x0200041A RID: 1050
public interface IGameDataElement
{
	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x060015ED RID: 5613
	string Key { get; }

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x060015EE RID: 5614
	int ID { get; }

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x060015EF RID: 5615
	bool Enabled { get; }

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x060015F0 RID: 5616
	LocalizationData Localization { get; }
}
