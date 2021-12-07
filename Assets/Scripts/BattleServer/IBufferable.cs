// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace BattleServer
{
	public interface IBufferable
	{
		void SetAsBufferable(uint bufferSize);

		void ResetBuffer();

		void DeserializeToBuffer(byte[] msg, uint msgSize);
	}
}
