using System;

// Token: 0x02000B52 RID: 2898
public interface IPoolListener
{
	// Token: 0x0600540B RID: 21515
	void OnAcquired();

	// Token: 0x0600540C RID: 21516
	void OnReleased();

	// Token: 0x0600540D RID: 21517
	void OnCooledOff();
}
