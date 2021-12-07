using System;
using strange.extensions.mediation.api;

namespace strange.extensions.mediation.impl
{
	// Token: 0x02000260 RID: 608
	public class MediationException : Exception
	{
		// Token: 0x06000C32 RID: 3122 RVA: 0x000559B9 File Offset: 0x00053DB9
		public MediationException()
		{
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x000559C1 File Offset: 0x00053DC1
		public MediationException(string message, MediationExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x000559D1 File Offset: 0x00053DD1
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x000559D9 File Offset: 0x00053DD9
		public MediationExceptionType type { get; set; }
	}
}
