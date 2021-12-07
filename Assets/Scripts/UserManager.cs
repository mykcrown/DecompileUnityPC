// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;

public class UserManager : IUserManager
{
	public static readonly string SIGNAL_USER_UPDATE = "UserManager.SIGNAL_USER_UPDATE";

	public static readonly string SIGNAL_INVENTORY_UPDATE = "UserManager.SIGNAL_INVENTORY_UPDATE";

	public static readonly string SIGNAL_CURRENCY_UPDATE = "UserManager.SIGNAL_CURRENCY_UPDATE";

	public static int MAX_USERS_PER_CLIENT = 6;

	private User[] users = new User[UserManager.MAX_USERS_PER_CLIENT];

	private int currentGuestId;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	private User primaryUser
	{
		get
		{
			return this.users[0];
		}
	}

	public ILoggedInUser PrimaryUser
	{
		get
		{
			return this.primaryUser;
		}
	}

	public IUser[] Users
	{
		get
		{
			return this.users;
		}
	}

	[PostConstruct]
	public void Init()
	{
		User user = new User();
		this.currentGuestId = 1;
		this.addUser(user);
	}

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

	private bool removeUser(int userIndex)
	{
		if (this.users[userIndex] != null)
		{
			this.users[userIndex] = null;
			return true;
		}
		return false;
	}

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
}
