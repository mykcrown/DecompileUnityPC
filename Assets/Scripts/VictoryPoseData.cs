// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class VictoryPoseData : ScriptableObject, IDefaultableData
{
	public CharacterID character;

	public bool isDefault;

	public string localizedNameWhenIsDefault;

	public float minDuration = 3f;

	public float maxDuration = 3f;

	public MoveData moveData;

	public MoveData loopingMoveData;

	public MoveData partnerMoveData;

	public MoveData partnerLoopingMoveData;

	public bool IsDefaultData
	{
		get
		{
			return this.isDefault;
		}
	}
}
