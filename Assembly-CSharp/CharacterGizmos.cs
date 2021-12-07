using System;
using UnityEngine;

// Token: 0x02000586 RID: 1414
public class CharacterGizmos : MonoBehaviour
{
	// Token: 0x06001FF1 RID: 8177 RVA: 0x000A2244 File Offset: 0x000A0644
	private void Awake()
	{
	}

	// Token: 0x06001FF2 RID: 8178 RVA: 0x000A2246 File Offset: 0x000A0646
	public void OnDrawGizmos()
	{
		if (this.Data != null)
		{
			this.Data.bounds.DrawGizmos(base.transform.position, this.Facing);
		}
	}

	// Token: 0x0400194D RID: 6477
	public CharacterMenusData Data;

	// Token: 0x0400194E RID: 6478
	public HorizontalDirection Facing;
}
