using System;

// Token: 0x02000981 RID: 2433
public interface ISettingsScreenAPI
{
	// Token: 0x060041D7 RID: 16855
	void SetPlayer(int portId);

	// Token: 0x17000F87 RID: 3975
	// (get) Token: 0x060041D8 RID: 16856
	PlayerInputPort InputPort { get; }
}
