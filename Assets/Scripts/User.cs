// Decompile from assembly: Assembly-CSharp.dll

using System;

public class User : IUser, ILoggedInUser, IGuestUser
{
	public LocalProfile localProfile = new LocalProfile();

	public LoggedInUserData userData;

	public int userIndex = -1;

	public int guestIndex;

	public ILocalProfile LocalProfile
	{
		get
		{
			return this.localProfile;
		}
	}

	public IUserCoreData CoreData
	{
		get
		{
			return this.userData.coreData;
		}
	}

	public bool IsGuest
	{
		get
		{
			return this.guestIndex > 0;
		}
	}

	public int GuestIndex
	{
		get
		{
			return this.guestIndex;
		}
	}

	public string NameText
	{
		get
		{
			if (this.IsGuest)
			{
				return string.Format("Guest ({0})", this.guestIndex);
			}
			if (!this.IsLoggedIn || string.IsNullOrEmpty(this.CoreData.Username))
			{
				return "Null";
			}
			return this.CoreData.Username;
		}
	}

	public string IdText
	{
		get
		{
			if (this.IsGuest)
			{
				return "Guest";
			}
			if (!this.IsLoggedIn || string.IsNullOrEmpty(this.CoreData.Id))
			{
				return "Null";
			}
			return this.CoreData.Id;
		}
	}

	public bool IsLoggedIn
	{
		get
		{
			return !this.IsGuest && this.userData != null;
		}
	}

	public bool LogIn(string id, string name)
	{
		if (this.IsGuest)
		{
			return false;
		}
		if (this.userData == null)
		{
			this.userData = new LoggedInUserData();
		}
		this.userData.coreData.id = id;
		this.userData.coreData.username = name;
		return true;
	}

	public bool LogOut()
	{
		if (this.IsGuest)
		{
			return false;
		}
		this.userData = null;
		return true;
	}
}
