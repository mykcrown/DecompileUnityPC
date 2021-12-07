// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.extensions.command.api
{
	public interface ICommandBinder : IBinder
	{
		void ReactTo(object trigger);

		void ReactTo(object trigger, object data);

		void ReleaseCommand(ICommand command);

		void Stop(object key);

		ICommandBinding Bind<T>();

		ICommandBinding Bind(object value);

		ICommandBinding GetBinding<T>();
	}
}
