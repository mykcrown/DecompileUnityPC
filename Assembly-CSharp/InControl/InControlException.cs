using System;

namespace InControl
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public class InControlException : Exception
	{
		// Token: 0x06000429 RID: 1065 RVA: 0x00018BDE File Offset: 0x00016FDE
		public InControlException()
		{
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00018BE6 File Offset: 0x00016FE6
		public InControlException(string message) : base(message)
		{
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00018BEF File Offset: 0x00016FEF
		public InControlException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
