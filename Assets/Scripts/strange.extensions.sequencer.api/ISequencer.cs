// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.command.api;
using strange.framework.api;
using System;

namespace strange.extensions.sequencer.api
{
	public interface ISequencer : ICommandBinder, IBinder
	{
		void ReleaseCommand(ISequenceCommand command);

		ISequenceBinding Bind<T>();

		ISequenceBinding Bind(object value);
	}
}
