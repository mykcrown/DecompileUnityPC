using System;
using System.Collections.Generic;

// Token: 0x02000476 RID: 1142
public interface IPlayerLookup
{
	// Token: 0x060018C9 RID: 6345
	PlayerReference GetPlayerReference(PlayerNum player);

	// Token: 0x060018CA RID: 6346
	PlayerController GetPlayerController(PlayerNum player);

	// Token: 0x060018CB RID: 6347
	List<PlayerController> GetPlayers();
}
