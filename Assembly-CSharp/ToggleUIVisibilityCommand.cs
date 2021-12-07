using System;

// Token: 0x02000AD3 RID: 2771
public class ToggleUIVisibilityCommand : GameEvent
{
	// Token: 0x060050B4 RID: 20660 RVA: 0x00150642 File Offset: 0x0014EA42
	public ToggleUIVisibilityCommand(bool visibility)
	{
		this.visibility = visibility;
	}

	// Token: 0x040033FC RID: 13308
	public bool visibility;
}
