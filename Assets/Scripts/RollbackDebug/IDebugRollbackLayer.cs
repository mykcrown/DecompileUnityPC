// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace RollbackDebug
{
	public interface IDebugRollbackLayer
	{
		int RollbackBufferSize
		{
			get;
		}

		bool IsRollingBack
		{
			get;
		}

		int CurrentGameFrame
		{
			get;
		}

		int ActiveRollbackFrame
		{
			get;
		}

		RollbackStateContainer GetBufferedState(int frame);

		void AdvanceFrame();

		bool Rollback(int amount);
	}
}
