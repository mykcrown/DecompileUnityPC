// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.extensions.mediation.api
{
	public interface IMediationBinding : IBinding
	{
		object abstraction
		{
			get;
		}

		IMediationBinding ToMediator<T>();

		IMediationBinding ToAbstraction<T>();

		IMediationBinding Bind<T>();

		IMediationBinding Bind(object key);

		IMediationBinding To<T>();

		IMediationBinding To(object o);
	}
}
