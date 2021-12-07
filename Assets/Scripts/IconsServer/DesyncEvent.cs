// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public class DesyncEvent : ServerEvent
	{
		public int desyncFrame;

		public DesyncEvent(int desyncFrame)
		{
			this.desyncFrame = desyncFrame;
		}
	}
}
