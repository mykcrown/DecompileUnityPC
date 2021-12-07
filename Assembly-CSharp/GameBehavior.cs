using System;

// Token: 0x02000465 RID: 1125
public class GameBehavior : ClientBehavior
{
	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x06001769 RID: 5993 RVA: 0x0006F35B File Offset: 0x0006D75B
	protected GameManager gameManager
	{
		get
		{
			return base.gameController.currentGame;
		}
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x0600176A RID: 5994 RVA: 0x0006F368 File Offset: 0x0006D768
	protected GameLog log
	{
		get
		{
			return (!(this.gameManager == null)) ? this.gameManager.Log : null;
		}
	}
}
