// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUser
{
	ILocalProfile LocalProfile
	{
		get;
	}

	bool IsGuest
	{
		get;
	}

	string NameText
	{
		get;
	}

	string IdText
	{
		get;
	}
}
