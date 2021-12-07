using System;

// Token: 0x02000A8A RID: 2698
public class User : IUser, ILoggedInUser, IGuestUser
{
	// Token: 0x170012B6 RID: 4790
	// (get) Token: 0x06004EFE RID: 20222 RVA: 0x0014B0B2 File Offset: 0x001494B2
	public ILocalProfile LocalProfile
	{
		get
		{
			return this.localProfile;
		}
	}

	// Token: 0x170012B7 RID: 4791
	// (get) Token: 0x06004EFF RID: 20223 RVA: 0x0014B0BA File Offset: 0x001494BA
	public IUserCoreData CoreData
	{
		get
		{
			return this.userData.coreData;
		}
	}

	// Token: 0x170012B8 RID: 4792
	// (get) Token: 0x06004F00 RID: 20224 RVA: 0x0014B0C7 File Offset: 0x001494C7
	public bool IsGuest
	{
		get
		{
			return this.guestIndex > 0;
		}
	}

	// Token: 0x170012B9 RID: 4793
	// (get) Token: 0x06004F01 RID: 20225 RVA: 0x0014B0D2 File Offset: 0x001494D2
	public int GuestIndex
	{
		get
		{
			return this.guestIndex;
		}
	}

	// Token: 0x170012BA RID: 4794
	// (get) Token: 0x06004F02 RID: 20226 RVA: 0x0014B0DC File Offset: 0x001494DC
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

	// Token: 0x170012BB RID: 4795
	// (get) Token: 0x06004F03 RID: 20227 RVA: 0x0014B13C File Offset: 0x0014953C
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

	// Token: 0x170012BC RID: 4796
	// (get) Token: 0x06004F04 RID: 20228 RVA: 0x0014B18B File Offset: 0x0014958B
	public bool IsLoggedIn
	{
		get
		{
			return !this.IsGuest && this.userData != null;
		}
	}

	// Token: 0x06004F05 RID: 20229 RVA: 0x0014B1A8 File Offset: 0x001495A8
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

	// Token: 0x06004F06 RID: 20230 RVA: 0x0014B1FB File Offset: 0x001495FB
	public bool LogOut()
	{
		if (this.IsGuest)
		{
			return false;
		}
		this.userData = null;
		return true;
	}

	// Token: 0x04003387 RID: 13191
	public LocalProfile localProfile = new LocalProfile();

	// Token: 0x04003388 RID: 13192
	public LoggedInUserData userData;

	// Token: 0x04003389 RID: 13193
	public int userIndex = -1;

	// Token: 0x0400338A RID: 13194
	public int guestIndex;
}
