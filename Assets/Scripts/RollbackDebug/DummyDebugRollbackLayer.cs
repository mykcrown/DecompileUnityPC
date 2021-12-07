// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	public class DummyDebugRollbackLayer : IDebugRollbackLayer
	{
		private int currentGameFrame = 10;

		private int activeRollbackFrame = 7;

		private int bufferSize = 4;

		private Dictionary<int, DummyRollbackStateContainer> rollbackBuffer = new Dictionary<int, DummyRollbackStateContainer>();

		int IDebugRollbackLayer.RollbackBufferSize
		{
			get
			{
				return this.bufferSize;
			}
		}

		bool IDebugRollbackLayer.IsRollingBack
		{
			get
			{
				return this.activeRollbackFrame < this.currentGameFrame;
			}
		}

		int IDebugRollbackLayer.CurrentGameFrame
		{
			get
			{
				return this.currentGameFrame;
			}
		}

		int IDebugRollbackLayer.ActiveRollbackFrame
		{
			get
			{
				return this.activeRollbackFrame;
			}
		}

		public Dictionary<int, DummyRollbackStateContainer> Buffer
		{
			get
			{
				return this.rollbackBuffer;
			}
		}

		public DummyDebugRollbackLayer()
		{
			for (int i = this.currentGameFrame - this.bufferSize; i <= this.currentGameFrame; i++)
			{
				this.generateRollbackBuffer(i);
			}
		}

		private void generateRollbackBuffer(int frame)
		{
			this.rollbackBuffer[frame] = new DummyRollbackStateContainer((DummyRollbackStateContainer.Mode)(frame % 5));
		}

		void IDebugRollbackLayer.AdvanceFrame()
		{
			if (this.activeRollbackFrame < this.currentGameFrame)
			{
				this.activeRollbackFrame++;
			}
			else
			{
				this.activeRollbackFrame++;
				this.currentGameFrame++;
				this.generateRollbackBuffer(this.currentGameFrame);
			}
		}

		bool IDebugRollbackLayer.Rollback(int frame)
		{
			if (frame > this.bufferSize)
			{
				return false;
			}
			this.activeRollbackFrame -= frame;
			return true;
		}

		public RollbackStateContainer GetBufferedState(int frame)
		{
			if (!this.rollbackBuffer.ContainsKey(frame))
			{
				return null;
			}
			return this.rollbackBuffer[frame];
		}
	}
}
