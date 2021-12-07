// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class InputProfileMap : MonoBehaviour
{
	[Serializable]
	public class InputProfileMapEntry
	{
		public MoveLabel moveLabel;

		public InputProfile inputProfile;
	}

	public InputProfileMap.InputProfileMapEntry[] map;

	public InputProfile Find(MoveLabel label)
	{
		for (int i = this.map.Length - 1; i >= 0; i--)
		{
			if (this.map[i].moveLabel == label)
			{
				return this.map[i].inputProfile;
			}
		}
		return null;
	}
}
