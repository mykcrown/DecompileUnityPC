// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IGameDataElement
{
	string Key
	{
		get;
	}

	int ID
	{
		get;
	}

	bool Enabled
	{
		get;
	}

	LocalizationData Localization
	{
		get;
	}
}
