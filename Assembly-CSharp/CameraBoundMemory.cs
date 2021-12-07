using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000374 RID: 884
internal class CameraBoundMemory
{
	// Token: 0x0600130C RID: 4876 RVA: 0x0006E197 File Offset: 0x0006C597
	public void Reset()
	{
		this.list.Clear();
	}

	// Token: 0x04000C73 RID: 3187
	public List<Rect> list = new List<Rect>();
}
