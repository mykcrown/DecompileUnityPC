// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.command.api;
using strange.framework.api;
using System;

namespace strange.extensions.sequencer.api
{
	public interface ISequenceBinding : ICommandBinding, IBinding
	{
		bool isOneOff
		{
			get;
			set;
		}

		ISequenceBinding Once();

		ISequenceBinding Bind<T>();

		ISequenceBinding Bind(object key);

		ISequenceBinding To<T>();

		ISequenceBinding To(object o);

		ISequenceBinding ToName<T>();

		ISequenceBinding ToName(object o);

		ISequenceBinding Named<T>();

		ISequenceBinding Named(object o);
	}
}
