using System;

// Token: 0x02000648 RID: 1608
[Serializable]
public abstract class StageObjectModel<T> : RollbackStateTyped<T>, ISituationalValidation where T : StageObjectModel<T>
{
	// Token: 0x170009AE RID: 2478
	// (get) Token: 0x06002764 RID: 10084 RVA: 0x000BE4FB File Offset: 0x000BC8FB
	public virtual bool ShouldValidate
	{
		get
		{
			return this.shouldValidate;
		}
	}

	// Token: 0x06002765 RID: 10085 RVA: 0x000BE503 File Offset: 0x000BC903
	public override void CopyTo(T target)
	{
		target.shouldValidate = this.shouldValidate;
	}

	// Token: 0x04001CD7 RID: 7383
	[IgnoreRollback(IgnoreRollbackType.Todo)]
	public bool shouldValidate;
}
