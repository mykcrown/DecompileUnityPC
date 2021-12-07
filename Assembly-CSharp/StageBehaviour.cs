using System;

// Token: 0x0200063C RID: 1596
public abstract class StageBehaviour : GameBehavior, ICloneable
{
	// Token: 0x06002715 RID: 10005 RVA: 0x000BE284 File Offset: 0x000BC684
	public virtual void Init()
	{
	}

	// Token: 0x06002716 RID: 10006
	public abstract void Play(object context = null);

	// Token: 0x06002717 RID: 10007 RVA: 0x000BE286 File Offset: 0x000BC686
	public object Clone()
	{
		return null;
	}

	// Token: 0x04001CA6 RID: 7334
	public int StartDelay;
}
