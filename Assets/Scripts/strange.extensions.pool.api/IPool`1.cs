// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.extensions.pool.api
{
	public interface IPool<T> : IPool, IManagedList
	{
		T GetInstance();
	}
}
