// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ILoggedInUser : IUser
{
	IUserCoreData CoreData
	{
		get;
	}

	bool IsLoggedIn
	{
		get;
	}

	bool LogIn(string id, string name);

	bool LogOut();
}
