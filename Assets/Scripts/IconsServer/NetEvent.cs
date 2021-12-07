// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace IconsServer
{
	public abstract class NetEvent
	{
		private DateTime addTime = DateTime.Now;

		public abstract EEvents Type
		{
			get;
		}

		public TimeSpan GetAge()
		{
			return DateTime.Now.Subtract(this.addTime);
		}
	}
}
