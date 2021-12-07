using System;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.sequencer.api;

namespace strange.extensions.sequencer.impl
{
	// Token: 0x0200027A RID: 634
	public class SequenceCommand : Command, ISequenceCommand, ICommand
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00056429 File Offset: 0x00054829
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x00056431 File Offset: 0x00054831
		[Inject]
		public ISequencer sequencer { get; set; }

		// Token: 0x06000CEE RID: 3310 RVA: 0x0005643A File Offset: 0x0005483A
		public new void Fail()
		{
			if (this.sequencer != null)
			{
				this.sequencer.Stop(this);
			}
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00056453 File Offset: 0x00054853
		public new virtual void Execute()
		{
			throw new SequencerException("You must override the Execute method in every SequenceCommand", SequencerExceptionType.EXECUTE_OVERRIDE);
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00056460 File Offset: 0x00054860
		public new void Release()
		{
			base.retain = false;
			if (this.sequencer != null)
			{
				this.sequencer.ReleaseCommand(this);
			}
		}
	}
}
