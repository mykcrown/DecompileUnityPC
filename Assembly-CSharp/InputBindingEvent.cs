using System;
using InControl;

// Token: 0x02000A55 RID: 2645
public class InputBindingEvent : GameEvent
{
	// Token: 0x06004CFF RID: 19711 RVA: 0x0014585F File Offset: 0x00143C5F
	public InputBindingEvent(PlayerAction action, BindingSource binding, InputBindingActionType type)
	{
		this.action = action;
		this.binding = binding;
		this.type = type;
	}

	// Token: 0x0400328C RID: 12940
	public PlayerAction action;

	// Token: 0x0400328D RID: 12941
	public BindingSource binding;

	// Token: 0x0400328E RID: 12942
	public InputBindingActionType type;
}
