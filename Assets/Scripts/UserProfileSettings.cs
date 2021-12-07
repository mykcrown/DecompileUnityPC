// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class UserProfileSettings : ScriptableObject
{
	public int minNameLength = 4;

	public int minPWLength = 8;

	public int maxNameLength = 16;

	public int maxOptionProfileNameLength = 18;

	public int maxOptionProfiles = 4;
}
