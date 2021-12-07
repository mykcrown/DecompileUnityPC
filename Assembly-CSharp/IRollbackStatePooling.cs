using System;

// Token: 0x0200088C RID: 2188
public interface IRollbackStatePooling
{
	// Token: 0x060036ED RID: 14061
	void Init();

	// Token: 0x060036EE RID: 14062
	T Clone<T>(T source) where T : RollbackStateTyped<T>;
}
