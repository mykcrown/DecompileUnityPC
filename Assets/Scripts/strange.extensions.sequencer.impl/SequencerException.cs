// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.sequencer.api;
using System;

namespace strange.extensions.sequencer.impl
{
	public class SequencerException : Exception
	{
		public SequencerExceptionType type
		{
			get;
			set;
		}

		public SequencerException()
		{
		}

		public SequencerException(string message, SequencerExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}
	}
}
