// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IEquipMethodMap
{
	EquipMethod GetMethod(EquipmentTypes type);

	List<EquipmentTypes> GetTypesWithMethod(EquipMethod method);
}
