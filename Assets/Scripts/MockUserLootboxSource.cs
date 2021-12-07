// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class MockUserLootboxSource : IUserLootboxesSource
{
	public Dictionary<int, int> GetLootBoxes()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		dictionary[2] = 50;
		return dictionary;
	}
}
