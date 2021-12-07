using System;
using System.Collections.Generic;
using IconsServer;

namespace P2P
{
	// Token: 0x02000818 RID: 2072
	public class SP2PMatchDetailPlayerDesc
	{
		// Token: 0x040023E0 RID: 9184
		public bool stageLoaded;

		// Token: 0x040023E1 RID: 9185
		public ECharacterType characterID;

		// Token: 0x040023E2 RID: 9186
		public bool isDisconnected;

		// Token: 0x040023E3 RID: 9187
		public ushort characterIndex;

		// Token: 0x040023E4 RID: 9188
		public ushort skinID;

		// Token: 0x040023E5 RID: 9189
		public List<ushort> equippedPlayerItemIds = new List<ushort>();

		// Token: 0x040023E6 RID: 9190
		public List<ushort> equippedCharacterItemIds = new List<ushort>();
	}
}
