// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;

public interface ICustomList
{
	int Count
	{
		get;
	}

	bool IsReadOnly
	{
		get;
	}

	void Clear();

	void RemoveAt(int index);

	IEnumerator ManualGetEnumerator();
}
