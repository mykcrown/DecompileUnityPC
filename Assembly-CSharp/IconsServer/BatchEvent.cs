using System;
using network;
using UnityEngine;

namespace IconsServer
{
	// Token: 0x020007E9 RID: 2025
	public abstract class BatchEvent : ServerEvent, ICloneable
	{
		// Token: 0x06003203 RID: 12803 RVA: 0x000F2647 File Offset: 0x000F0A47
		public virtual void UpdateNetMessage(ref INetMsg message)
		{
			Debug.LogError("Unsupported message");
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x000F2653 File Offset: 0x000F0A53
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}
	}
}
