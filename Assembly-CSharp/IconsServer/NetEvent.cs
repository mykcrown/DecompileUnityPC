using System;

namespace IconsServer
{
	// Token: 0x020007FB RID: 2043
	public abstract class NetEvent
	{
		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x0600329B RID: 12955
		public abstract EEvents Type { get; }

		// Token: 0x0600329C RID: 12956 RVA: 0x000F3174 File Offset: 0x000F1574
		public TimeSpan GetAge()
		{
			return DateTime.Now.Subtract(this.addTime);
		}

		// Token: 0x04002378 RID: 9080
		private DateTime addTime = DateTime.Now;
	}
}
