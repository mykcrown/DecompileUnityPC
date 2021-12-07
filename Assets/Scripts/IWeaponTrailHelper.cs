// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using Xft;

public interface IWeaponTrailHelper
{
	void UpdateWeaponTrailMap(Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap, int frame, MoveData moveData, IBodyOwner body, Func<XWeaponTrail> intantiator);

	void ClearWeaponTrails(Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap);
}
