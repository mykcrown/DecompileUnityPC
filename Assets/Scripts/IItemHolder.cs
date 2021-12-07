// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IItemHolder
{
	List<IItem> HeldItems
	{
		get;
	}

	void GiveItem(IItem pItem);

	void TakeItem(IItem pItem);
}
