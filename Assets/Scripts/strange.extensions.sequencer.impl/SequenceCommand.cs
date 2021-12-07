// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.sequencer.api;
using System;

namespace strange.extensions.sequencer.impl
{
	public class SequenceCommand : Command, ISequenceCommand, ICommand
	{
		[Inject]
		public ISequencer sequencer
		{
			get;
			set;
		}

		public new void Fail()
		{
			if (this.sequencer != null)
			{
				this.sequencer.Stop(this);
			}
		}

		public new virtual void Execute()
		{
			throw new SequencerException("You must override the Execute method in every SequenceCommand", SequencerExceptionType.EXECUTE_OVERRIDE);
		}

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
