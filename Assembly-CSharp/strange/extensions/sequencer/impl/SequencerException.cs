using System;
using strange.extensions.sequencer.api;

namespace strange.extensions.sequencer.impl
{
	// Token: 0x0200027C RID: 636
	public class SequencerException : Exception
	{
		// Token: 0x06000CFE RID: 3326 RVA: 0x000567D4 File Offset: 0x00054BD4
		public SequencerException()
		{
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000567DC File Offset: 0x00054BDC
		public SequencerException(string message, SequencerExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x000567EC File Offset: 0x00054BEC
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x000567F4 File Offset: 0x00054BF4
		public SequencerExceptionType type { get; set; }
	}
}
