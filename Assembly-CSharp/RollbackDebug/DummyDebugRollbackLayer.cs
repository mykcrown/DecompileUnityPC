using System;
using System.Collections.Generic;

namespace RollbackDebug
{
	// Token: 0x02000850 RID: 2128
	public class DummyDebugRollbackLayer : IDebugRollbackLayer
	{
		// Token: 0x0600353A RID: 13626 RVA: 0x000FBF40 File Offset: 0x000FA340
		public DummyDebugRollbackLayer()
		{
			for (int i = this.currentGameFrame - this.bufferSize; i <= this.currentGameFrame; i++)
			{
				this.generateRollbackBuffer(i);
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x0600353B RID: 13627 RVA: 0x000FBF9E File Offset: 0x000FA39E
		public Dictionary<int, DummyRollbackStateContainer> Buffer
		{
			get
			{
				return this.rollbackBuffer;
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x0600353C RID: 13628 RVA: 0x000FBFA6 File Offset: 0x000FA3A6
		int IDebugRollbackLayer.RollbackBufferSize
		{
			get
			{
				return this.bufferSize;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x0600353D RID: 13629 RVA: 0x000FBFAE File Offset: 0x000FA3AE
		bool IDebugRollbackLayer.IsRollingBack
		{
			get
			{
				return this.activeRollbackFrame < this.currentGameFrame;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x0600353E RID: 13630 RVA: 0x000FBFBE File Offset: 0x000FA3BE
		int IDebugRollbackLayer.CurrentGameFrame
		{
			get
			{
				return this.currentGameFrame;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x0600353F RID: 13631 RVA: 0x000FBFC6 File Offset: 0x000FA3C6
		int IDebugRollbackLayer.ActiveRollbackFrame
		{
			get
			{
				return this.activeRollbackFrame;
			}
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x000FBFCE File Offset: 0x000FA3CE
		private void generateRollbackBuffer(int frame)
		{
			this.rollbackBuffer[frame] = new DummyRollbackStateContainer((DummyRollbackStateContainer.Mode)(frame % 5));
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x000FBFE4 File Offset: 0x000FA3E4
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

		// Token: 0x06003542 RID: 13634 RVA: 0x000FC03D File Offset: 0x000FA43D
		bool IDebugRollbackLayer.Rollback(int frame)
		{
			if (frame > this.bufferSize)
			{
				return false;
			}
			this.activeRollbackFrame -= frame;
			return true;
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x000FC05C File Offset: 0x000FA45C
		public RollbackStateContainer GetBufferedState(int frame)
		{
			if (!this.rollbackBuffer.ContainsKey(frame))
			{
				return null;
			}
			return this.rollbackBuffer[frame];
		}

		// Token: 0x04002499 RID: 9369
		private int currentGameFrame = 10;

		// Token: 0x0400249A RID: 9370
		private int activeRollbackFrame = 7;

		// Token: 0x0400249B RID: 9371
		private int bufferSize = 4;

		// Token: 0x0400249C RID: 9372
		private Dictionary<int, DummyRollbackStateContainer> rollbackBuffer = new Dictionary<int, DummyRollbackStateContainer>();
	}
}
