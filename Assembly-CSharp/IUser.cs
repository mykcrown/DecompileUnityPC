using System;

// Token: 0x02000A8D RID: 2701
public interface IUser
{
	// Token: 0x170012BF RID: 4799
	// (get) Token: 0x06004F0B RID: 20235
	ILocalProfile LocalProfile { get; }

	// Token: 0x170012C0 RID: 4800
	// (get) Token: 0x06004F0C RID: 20236
	bool IsGuest { get; }

	// Token: 0x170012C1 RID: 4801
	// (get) Token: 0x06004F0D RID: 20237
	string NameText { get; }

	// Token: 0x170012C2 RID: 4802
	// (get) Token: 0x06004F0E RID: 20238
	string IdText { get; }
}
