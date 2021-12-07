// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct BattleSettingTypeComparer : IEqualityComparer<BattleSettingType>
{
	public bool Equals(BattleSettingType x, BattleSettingType y)
	{
		return x == y;
	}

	public int GetHashCode(BattleSettingType obj)
	{
		return (int)obj;
	}
}
