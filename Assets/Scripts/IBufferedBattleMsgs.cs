// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;

public interface IBufferedBattleMsgs
{
	void Init();

	T GetBufferedNetMessage<T>() where T : INetMsg;
}
