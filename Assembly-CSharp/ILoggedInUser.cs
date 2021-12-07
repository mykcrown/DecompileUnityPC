using System;

// Token: 0x02000A8F RID: 2703
public interface ILoggedInUser : IUser
{
	// Token: 0x170012C5 RID: 4805
	// (get) Token: 0x06004F11 RID: 20241
	IUserCoreData CoreData { get; }

	// Token: 0x170012C6 RID: 4806
	// (get) Token: 0x06004F12 RID: 20242
	bool IsLoggedIn { get; }

	// Token: 0x06004F13 RID: 20243
	bool LogIn(string id, string name);

	// Token: 0x06004F14 RID: 20244
	bool LogOut();
}
