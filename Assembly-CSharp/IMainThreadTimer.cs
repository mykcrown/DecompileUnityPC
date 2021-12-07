using System;

// Token: 0x02000B66 RID: 2918
public interface IMainThreadTimer
{
	// Token: 0x0600546F RID: 21615
	void SetOrReplaceTimeout(int time, Action callback);

	// Token: 0x06005470 RID: 21616
	void SetTimeout(int time, Action callback);

	// Token: 0x06005471 RID: 21617
	void CancelTimeout(Action callback);

	// Token: 0x06005472 RID: 21618
	void UnblockThread(Action callback);

	// Token: 0x06005473 RID: 21619
	void NextFrame(Action callback);

	// Token: 0x06005474 RID: 21620
	void EndOfFrame(Action callback);
}
