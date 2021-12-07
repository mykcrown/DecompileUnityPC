using System;
using InControl;

// Token: 0x02000ACD RID: 2765
public class UnbindPlayerControlCommand : GameEvent
{
	// Token: 0x060050AE RID: 20654 RVA: 0x001505D9 File Offset: 0x0014E9D9
	public UnbindPlayerControlCommand(BindingSource binding)
	{
		this.binding = binding;
	}

	// Token: 0x040033F4 RID: 13300
	public BindingSource binding;
}
