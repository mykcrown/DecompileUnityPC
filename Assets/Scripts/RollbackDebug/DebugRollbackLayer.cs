// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace RollbackDebug
{
	public class DebugRollbackLayer : RollbackLayer, IDebugRollbackLayer
	{
		private FrameController frameController;

		int IDebugRollbackLayer.CurrentGameFrame
		{
			get
			{
				return this.currentFrame;
			}
		}

		int IDebugRollbackLayer.ActiveRollbackFrame
		{
			get
			{
				return base.client.Frame;
			}
		}

		public int RollbackBufferSize
		{
			get
			{
				return RollbackStatePoolContainer.ROLLBACK_FRAMES;
			}
		}

		void IDebugRollbackLayer.AdvanceFrame()
		{
			this.frameController.AdvanceFrame();
		}

		bool IDebugRollbackLayer.Rollback(int framesBack)
		{
			base.ForceRollback(base.client.Frame - framesBack);
			bool flag;
			base.Idle(20, out flag, 1);
			return true;
		}

		public RollbackStateContainer GetBufferedState(int frame)
		{
			if (this.currentFrame - frame > this.RollbackBufferSize)
			{
				return null;
			}
			return base.rollbackStatePool.GetRollbackState(frame);
		}

		public void Init(RollbackSettings settings, FrameController frameController, IRollbackLayerDebugger debugger, RollbackDebugSettings debugSettings)
		{
			base.Init(settings, debugger);
			debugger.Initialize(base.client, debugSettings);
			debugger.LoadRollbackLayer(this);
			this.frameController = frameController;
		}
	}
}
