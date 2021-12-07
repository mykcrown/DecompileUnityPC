using System;
using System.Collections.Generic;

// Token: 0x02000A5D RID: 2653
public class ActivateCursorModuleCommand : GameEvent
{
	// Token: 0x06004D07 RID: 19719 RVA: 0x001458D7 File Offset: 0x00143CD7
	public ActivateCursorModuleCommand(List<IPlayerCursor> cursors)
	{
		this.cursors = cursors;
	}

	// Token: 0x04003294 RID: 12948
	public List<IPlayerCursor> cursors;
}
