// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public class SMsg : NetEvent
	{
		public byte msgId = 255;

		public byte[] buffer;

		public uint bufferSize;

		public override EEvents Type
		{
			get
			{
				return EEvents.Msg;
			}
		}
	}
}
