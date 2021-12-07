using System;
using System.Collections;

// Token: 0x02000A95 RID: 2709
public class UserManager : IUserManager
{
	// Token: 0x170012D7 RID: 4823
	// (get) Token: 0x06004F72 RID: 20338 RVA: 0x0014BEB1 File Offset: 0x0014A2B1
	// (set) Token: 0x06004F73 RID: 20339 RVA: 0x0014BEB9 File Offset: 0x0014A2B9
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x170012D8 RID: 4824
	// (get) Token: 0x06004F74 RID: 20340 RVA: 0x0014BEC2 File Offset: 0x0014A2C2
	private User primaryUser
	{
		get
		{
			return this.users[0];
		}
	}

	// Token: 0x170012D9 RID: 4825
	// (get) Token: 0x06004F75 RID: 20341 RVA: 0x0014BECC File Offset: 0x0014A2CC
	public ILoggedInUser PrimaryUser
	{
		get
		{
			return this.primaryUser;
		}
	}

	// Token: 0x06004F76 RID: 20342 RVA: 0x0014BED4 File Offset: 0x0014A2D4
	[PostConstruct]
	public void Init()
	{
		User user = new User();
		this.currentGuestId = 1;
		this.addUser(user);
	}

	// Token: 0x06004F77 RID: 20343 RVA: 0x0014BEF8 File Offset: 0x0014A2F8
	private bool addUser(User user)
	{
		for (int i = 0; i < this.users.Length; i++)
		{
			if (this.users[i] == null)
			{
				this.users[i] = user;
				user.userIndex = i;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004F78 RID: 20344 RVA: 0x0014BF40 File Offset: 0x0014A340
	private bool removeUser(User user)
	{
		for (int i = 0; i < this.users.Length; i++)
		{
			if (this.users[i] == user)
			{
				return this.removeUser(i);
			}
		}
		return false;
	}

	// Token: 0x06004F79 RID: 20345 RVA: 0x0014BF7D File Offset: 0x0014A37D
	private bool removeUser(int userIndex)
	{
		if (this.users[userIndex] != null)
		{
			this.users[userIndex] = null;
			return true;
		}
		return false;
	}

	// Token: 0x06004F7A RID: 20346 RVA: 0x0014BF98 File Offset: 0x0014A398
	public IGuestUser AddGuest()
	{
		User user = new User();
		if (this.addUser(user))
		{
			user.guestIndex = this.currentGuestId++;
			return user;
		}
		return null;
	}

	// Token: 0x06004F7B RID: 20347 RVA: 0x0014BFD4 File Offset: 0x0014A3D4
	public void RemoveGuest(int guestIndex)
	{
		bool flag = false;
		for (int i = 0; i < this.users.Length; i++)
		{
			if (this.users[i].guestIndex == guestIndex)
			{
				flag = this.removeUser(i);
				break;
			}
		}
		if (flag)
		{
			for (int j = 0; j < this.users.Length; j++)
			{
				if (this.users[j].guestIndex > guestIndex)
				{
					this.users[j].guestIndex--;
				}
			}
			this.currentGuestId--;
		}
	}

	// Token: 0x06004F7C RID: 20348 RVA: 0x0014C074 File Offset: 0x0014A474
	public IGuestUser GetGuest(int guestIndex)
	{
		for (int i = 0; i < this.users.Length; i++)
		{
			if (this.users[i].guestIndex == guestIndex)
			{
				return this.users[i];
			}
		}
		return null;
	}

	// Token: 0x06004F7D RID: 20349 RVA: 0x0014C0B8 File Offset: 0x0014A4B8
	public void UpdateFromMessage(Hashtable data)
	{
		string text = ObjectUtil.Convert<string>(data["userId"]);
		string name = ObjectUtil.Convert<string>(data["username"]);
		if (text == null)
		{
			throw new Exception("Why do we even do this.");
		}
		if (this.primaryUser.LogIn(text, name))
		{
			this.signalBus.Dispatch(UserManager.SIGNAL_USER_UPDATE);
		}
	}

	// Token: 0x06004F7E RID: 20350 RVA: 0x0014C11C File Offset: 0x0014A51C
	public void LogOutPrimaryUser()
	{
		if (this.PrimaryUser.LogOut())
		{
			this.signalBus.Dispatch(UserManager.SIGNAL_USER_UPDATE);
			int num = this.currentGuestId;
			for (int i = num; i >= 1; i--)
			{
				this.RemoveGuest(i);
			}
		}
	}

	// Token: 0x170012DA RID: 4826
	// (get) Token: 0x06004F7F RID: 20351 RVA: 0x0014C169 File Offset: 0x0014A569
	public IUser[] Users
	{
		get
		{
			return this.users;
		}
	}

	// Token: 0x0400339B RID: 13211
	public static readonly string SIGNAL_USER_UPDATE = "UserManager.SIGNAL_USER_UPDATE";

	// Token: 0x0400339C RID: 13212
	public static readonly string SIGNAL_INVENTORY_UPDATE = "UserManager.SIGNAL_INVENTORY_UPDATE";

	// Token: 0x0400339D RID: 13213
	public static readonly string SIGNAL_CURRENCY_UPDATE = "UserManager.SIGNAL_CURRENCY_UPDATE";

	// Token: 0x0400339F RID: 13215
	public static int MAX_USERS_PER_CLIENT = 6;

	// Token: 0x040033A0 RID: 13216
	private User[] users = new User[UserManager.MAX_USERS_PER_CLIENT];

	// Token: 0x040033A1 RID: 13217
	private int currentGuestId;
}
