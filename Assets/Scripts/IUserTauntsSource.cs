// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IUserTauntsSource
{
	Dictionary<CharacterID, Dictionary<TauntSlot, EquipmentID>> GetSourceData();
}
