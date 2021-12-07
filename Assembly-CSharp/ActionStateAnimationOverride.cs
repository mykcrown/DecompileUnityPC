using System;

// Token: 0x0200057C RID: 1404
[Serializable]
public class ActionStateAnimationOverride : CharacterAnimation
{
	// Token: 0x06001F8B RID: 8075 RVA: 0x000A1010 File Offset: 0x0009F410
	public ActionStateAnimationOverride(ActionState actionState)
	{
		this.actionState = actionState;
	}

	// Token: 0x040018EF RID: 6383
	public ActionState actionState;
}
