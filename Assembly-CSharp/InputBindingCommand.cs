using System;
using InControl;
using UnityEngine;

// Token: 0x02000A54 RID: 2644
public class InputBindingCommand : GameEvent
{
	// Token: 0x06004CFE RID: 19710 RVA: 0x0014583A File Offset: 0x00143C3A
	public InputBindingCommand(PlayerAction action, BindingSource binding, InputBindingActionType type, GameObject source)
	{
		this.action = action;
		this.binding = binding;
		this.type = type;
		this.source = source;
	}

	// Token: 0x04003288 RID: 12936
	public PlayerAction action;

	// Token: 0x04003289 RID: 12937
	public BindingSource binding;

	// Token: 0x0400328A RID: 12938
	public InputBindingActionType type;

	// Token: 0x0400328B RID: 12939
	public GameObject source;
}
