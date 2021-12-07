using System;
using System.Collections.Generic;
using UnityEngine;
using Xft;

// Token: 0x02000531 RID: 1329
public class WeaponTrailHelper : IWeaponTrailHelper
{
	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x06001CC8 RID: 7368 RVA: 0x000944A5 File Offset: 0x000928A5
	// (set) Token: 0x06001CC9 RID: 7369 RVA: 0x000944AD File Offset: 0x000928AD
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x06001CCA RID: 7370 RVA: 0x000944B6 File Offset: 0x000928B6
	// (set) Token: 0x06001CCB RID: 7371 RVA: 0x000944BE File Offset: 0x000928BE
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x06001CCC RID: 7372 RVA: 0x000944C8 File Offset: 0x000928C8
	public void UpdateWeaponTrailMap(Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap, int frame, MoveData moveData, IBodyOwner body, Func<XWeaponTrail> intantiator)
	{
		foreach (WeaponTrailData weaponTrailData in moveData.weaponTrails)
		{
			if (weaponTrailData.startFrame == frame)
			{
				XWeaponTrail xweaponTrail = intantiator();
				if (weaponTrailData.bodyPart2 == BodyPart.none)
				{
					Debug.LogError("Tried to make a weapon trail without a secondary body part");
				}
				if (!weaponTrailData.useOffsets || weaponTrailData.bodyPartOffset1 == 0)
				{
					xweaponTrail.PointStart = (IBoneFloatTransform)body.GetBoneTransform(weaponTrailData.bodyPart);
				}
				else
				{
					xweaponTrail.PointStart = (IBoneFloatTransform)body.GetBoneOffsetTransform(weaponTrailData.bodyPart, weaponTrailData.bodyPart2, weaponTrailData.bodyPartOffset1);
				}
				IBoneFloatTransform boneFloatTransform;
				if (!weaponTrailData.useOffsets || weaponTrailData.bodyPartOffset2 == 0)
				{
					boneFloatTransform = (IBoneFloatTransform)body.GetBoneTransform(weaponTrailData.bodyPart2);
				}
				else
				{
					boneFloatTransform = (IBoneFloatTransform)body.GetBoneOffsetTransform(weaponTrailData.bodyPart2, weaponTrailData.bodyPart, weaponTrailData.bodyPartOffset2);
				}
				if (boneFloatTransform != null)
				{
					xweaponTrail.PointEnd = boneFloatTransform;
					xweaponTrail.MaxFrame = weaponTrailData.frameLength;
					xweaponTrail.Granularity = weaponTrailData.granularity;
					if (weaponTrailData.overrideMaterial != null)
					{
						xweaponTrail.MyMaterial = weaponTrailData.overrideMaterial;
					}
					else
					{
						xweaponTrail.MyMaterial = this.gameDataManager.ConfigData.defaultCharacterEffects.weaponTrailMaterial;
					}
					xweaponTrail.MyColor = weaponTrailData.color;
					xweaponTrail.Activate();
					if (weaponTrailMap.ContainsKey(weaponTrailData))
					{
						weaponTrailMap[weaponTrailData].Deactivate();
					}
					weaponTrailMap[weaponTrailData] = xweaponTrail;
				}
			}
			else if (weaponTrailData.endFrame == frame)
			{
				if (weaponTrailMap.ContainsKey(weaponTrailData))
				{
					XWeaponTrail xweaponTrail2 = weaponTrailMap[weaponTrailData];
					if (weaponTrailData.fadeFrames == 0)
					{
						xweaponTrail2.Deactivate();
					}
					else
					{
						xweaponTrail2.StopSmoothly(weaponTrailData.fadeFrames);
					}
				}
				else
				{
					Debug.LogWarning("Missing weapon trail!");
				}
			}
		}
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x000946C4 File Offset: 0x00092AC4
	public void ClearWeaponTrails(Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap)
	{
		List<XWeaponTrail> list = new List<XWeaponTrail>();
		foreach (KeyValuePair<WeaponTrailData, XWeaponTrail> keyValuePair in weaponTrailMap)
		{
			XWeaponTrail value = keyValuePair.Value;
			if (value != null && !value.Equals(null))
			{
				list.Add(value);
			}
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			list[i].Deactivate();
		}
		weaponTrailMap.Clear();
	}
}
