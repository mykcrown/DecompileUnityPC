using System;
using System.Collections;

// Token: 0x02000A96 RID: 2710
public interface IUserManager
{
	// Token: 0x170012DB RID: 4827
	// (get) Token: 0x06004F81 RID: 20353
	ILoggedInUser PrimaryUser { get; }

	// Token: 0x06004F82 RID: 20354
	IGuestUser AddGuest();

	// Token: 0x06004F83 RID: 20355
	void RemoveGuest(int guestIndex);

	// Token: 0x06004F84 RID: 20356
	IGuestUser GetGuest(int guestIndex);

	// Token: 0x06004F85 RID: 20357
	void UpdateFromMessage(Hashtable data);

	// Token: 0x06004F86 RID: 20358
	void LogOutPrimaryUser();

	// Token: 0x170012DC RID: 4828
	// (get) Token: 0x06004F87 RID: 20359
	IUser[] Users { get; }
}
