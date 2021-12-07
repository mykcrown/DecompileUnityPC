// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;

public interface IUserManager
{
	ILoggedInUser PrimaryUser
	{
		get;
	}

	IUser[] Users
	{
		get;
	}

	IGuestUser AddGuest();

	void RemoveGuest(int guestIndex);

	IGuestUser GetGuest(int guestIndex);

	void UpdateFromMessage(Hashtable data);

	void LogOutPrimaryUser();
}
