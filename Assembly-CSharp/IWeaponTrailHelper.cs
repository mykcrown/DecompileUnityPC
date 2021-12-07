using System;
using System.Collections.Generic;
using Xft;

// Token: 0x02000532 RID: 1330
public interface IWeaponTrailHelper
{
	// Token: 0x06001CCE RID: 7374
	void UpdateWeaponTrailMap(Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap, int frame, MoveData moveData, IBodyOwner body, Func<XWeaponTrail> intantiator);

	// Token: 0x06001CCF RID: 7375
	void ClearWeaponTrails(Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap);
}
