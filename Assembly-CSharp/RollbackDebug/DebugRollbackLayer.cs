using System;

namespace RollbackDebug
{
	// Token: 0x0200084E RID: 2126
	public class DebugRollbackLayer : RollbackLayer, IDebugRollbackLayer
	{
		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x0600352C RID: 13612 RVA: 0x000FBE9F File Offset: 0x000FA29F
		public int RollbackBufferSize
		{
			get
			{
				return RollbackStatePoolContainer.ROLLBACK_FRAMES;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x0600352D RID: 13613 RVA: 0x000FBEA6 File Offset: 0x000FA2A6
		int IDebugRollbackLayer.CurrentGameFrame
		{
			get
			{
				return this.currentFrame;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x0600352E RID: 13614 RVA: 0x000FBEAE File Offset: 0x000FA2AE
		int IDebugRollbackLayer.ActiveRollbackFrame
		{
			get
			{
				return base.client.Frame;
			}
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x000FBEBB File Offset: 0x000FA2BB
		void IDebugRollbackLayer.AdvanceFrame()
		{
			this.frameController.AdvanceFrame();
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x000FBEC8 File Offset: 0x000FA2C8
		bool IDebugRollbackLayer.Rollback(int framesBack)
		{
			base.ForceRollback(base.client.Frame - framesBack);
			bool flag;
			base.Idle(20, out flag, 1);
			return true;
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x000FBEF5 File Offset: 0x000FA2F5
		public RollbackStateContainer GetBufferedState(int frame)
		{
			if (this.currentFrame - frame > this.RollbackBufferSize)
			{
				return null;
			}
			return base.rollbackStatePool.GetRollbackState(frame);
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x000FBF18 File Offset: 0x000FA318
		public void Init(RollbackSettings settings, FrameController frameController, IRollbackLayerDebugger debugger, RollbackDebugSettings debugSettings)
		{
			base.Init(settings, debugger);
			debugger.Initialize(base.client, debugSettings);
			debugger.LoadRollbackLayer(this);
			this.frameController = frameController;
		}

		// Token: 0x04002498 RID: 9368
		private FrameController frameController;
	}
}
