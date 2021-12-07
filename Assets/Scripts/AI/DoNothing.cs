// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public class DoNothing : Leaf
	{
		public DoNothing(int frameDuration) : base(frameDuration)
		{
		}

		public override NodeResult TickFrame()
		{
			if (this.currentFrame == this.frameDuration)
			{
				return base.resultSuccess;
			}
			return base.resultRunning;
		}
	}
}
