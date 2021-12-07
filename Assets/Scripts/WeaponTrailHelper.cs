// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Xft;

public class WeaponTrailHelper : IWeaponTrailHelper
{
	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[Inject]
	public DeveloperConfig devConfig
	{
		get;
		set;
	}

	public void UpdateWeaponTrailMap(Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap, int frame, MoveData moveData, IBodyOwner body, Func<XWeaponTrail> intantiator)
	{
		WeaponTrailData[] weaponTrails = moveData.weaponTrails;
		for (int i = 0; i < weaponTrails.Length; i++)
		{
			WeaponTrailData weaponTrailData = weaponTrails[i];
			if (weaponTrailData.startFrame == frame)
			{
				XWeaponTrail xWeaponTrail = intantiator();
				if (weaponTrailData.bodyPart2 == BodyPart.none)
				{
					UnityEngine.Debug.LogError("Tried to make a weapon trail without a secondary body part");
				}
				if (!weaponTrailData.useOffsets || weaponTrailData.bodyPartOffset1 == 0)
				{
					xWeaponTrail.PointStart = (IBoneFloatTransform)body.GetBoneTransform(weaponTrailData.bodyPart);
				}
				else
				{
					xWeaponTrail.PointStart = (IBoneFloatTransform)body.GetBoneOffsetTransform(weaponTrailData.bodyPart, weaponTrailData.bodyPart2, weaponTrailData.bodyPartOffset1);
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
					xWeaponTrail.PointEnd = boneFloatTransform;
					xWeaponTrail.MaxFrame = weaponTrailData.frameLength;
					xWeaponTrail.Granularity = weaponTrailData.granularity;
					if (weaponTrailData.overrideMaterial != null)
					{
						xWeaponTrail.MyMaterial = weaponTrailData.overrideMaterial;
					}
					else
					{
						xWeaponTrail.MyMaterial = this.gameDataManager.ConfigData.defaultCharacterEffects.weaponTrailMaterial;
					}
					xWeaponTrail.MyColor = weaponTrailData.color;
					xWeaponTrail.Activate();
					if (weaponTrailMap.ContainsKey(weaponTrailData))
					{
						weaponTrailMap[weaponTrailData].Deactivate();
					}
					weaponTrailMap[weaponTrailData] = xWeaponTrail;
				}
			}
			else if (weaponTrailData.endFrame == frame)
			{
				if (weaponTrailMap.ContainsKey(weaponTrailData))
				{
					XWeaponTrail xWeaponTrail2 = weaponTrailMap[weaponTrailData];
					if (weaponTrailData.fadeFrames == 0)
					{
						xWeaponTrail2.Deactivate();
					}
					else
					{
						xWeaponTrail2.StopSmoothly(weaponTrailData.fadeFrames);
					}
				}
				else
				{
					UnityEngine.Debug.LogWarning("Missing weapon trail!");
				}
			}
		}
	}

	public void ClearWeaponTrails(Dictionary<WeaponTrailData, XWeaponTrail> weaponTrailMap)
	{
		List<XWeaponTrail> list = new List<XWeaponTrail>();
		foreach (KeyValuePair<WeaponTrailData, XWeaponTrail> current in weaponTrailMap)
		{
			XWeaponTrail value = current.Value;
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
