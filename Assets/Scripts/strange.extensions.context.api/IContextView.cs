// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.mediation.api;
using System;

namespace strange.extensions.context.api
{
	public interface IContextView : IView
	{
		IContext context
		{
			get;
			set;
		}
	}
}
