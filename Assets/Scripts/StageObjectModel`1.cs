// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public abstract class StageObjectModel<T> : RollbackStateTyped<T>, ISituationalValidation where T : StageObjectModel<T>
{
	[IgnoreRollback(IgnoreRollbackType.Todo)]
	public bool shouldValidate;

	public virtual bool ShouldValidate
	{
		get
		{
			return this.shouldValidate;
		}
	}

	public override void CopyTo(T target)
	{
		target.shouldValidate = this.shouldValidate;
	}
}
