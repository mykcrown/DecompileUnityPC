using System;

namespace BattleServer
{
	// Token: 0x02000797 RID: 1943
	public interface IBufferable
	{
		// Token: 0x06002FEB RID: 12267
		void SetAsBufferable(uint bufferSize);

		// Token: 0x06002FEC RID: 12268
		void ResetBuffer();

		// Token: 0x06002FED RID: 12269
		void DeserializeToBuffer(byte[] msg, uint msgSize);
	}
}
