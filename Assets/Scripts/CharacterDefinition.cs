// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class CharacterDefinition : ScriptableObject
{
	public CharacterID characterID;

	public bool enabled;

	public bool demoEnabled = true;

	public bool isRandom;

	public bool isPartner;

	public CharacterDefinition totemPartner;

	public string characterName;

	public string defaultVictoryPose;

	public int ordinal
	{
		get;
		set;
	}
}
