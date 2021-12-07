using System;
using network;

// Token: 0x0200079B RID: 1947
public interface IBufferedBattleMsgs
{
	// Token: 0x06002FF5 RID: 12277
	void Init();

	// Token: 0x06002FF6 RID: 12278
	T GetBufferedNetMessage<T>() where T : INetMsg;
}
