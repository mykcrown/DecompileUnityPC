// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.extensions.context.api
{
	public interface IContext : IBinder
	{
		IContext Start();

		void Launch();

		IContext AddContext(IContext context);

		IContext RemoveContext(IContext context);

		void AddView(object view);

		void RemoveView(object view);

		object GetContextView();
	}
}
