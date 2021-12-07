using System;

// Token: 0x02000B51 RID: 2897
public interface IPooledGameObjectListener
{
	// Token: 0x06005408 RID: 21512
	void OnAcquired();

	// Token: 0x06005409 RID: 21513
	void OnReleased();

	// Token: 0x0600540A RID: 21514
	void OnCooledOff();
}
