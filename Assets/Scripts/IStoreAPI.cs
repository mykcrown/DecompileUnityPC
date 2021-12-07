// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IStoreAPI : IDataDependency
{
	StoreMode Mode
	{
		get;
		set;
	}

	int Port
	{
		get;
		set;
	}

	string PortDisplay
	{
		get;
	}

	void OnScreenOpened();
}
