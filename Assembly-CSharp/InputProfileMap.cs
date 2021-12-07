using System;
using UnityEngine;

// Token: 0x02000208 RID: 520
public class InputProfileMap : MonoBehaviour
{
	// Token: 0x060009C1 RID: 2497 RVA: 0x00050C34 File Offset: 0x0004F034
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

	// Token: 0x040006E9 RID: 1769
	public InputProfileMap.InputProfileMapEntry[] map;

	// Token: 0x02000209 RID: 521
	[Serializable]
	public class InputProfileMapEntry
	{
		// Token: 0x040006EA RID: 1770
		public MoveLabel moveLabel;

		// Token: 0x040006EB RID: 1771
		public InputProfile inputProfile;
	}
}
