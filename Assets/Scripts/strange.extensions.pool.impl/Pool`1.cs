// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.pool.api;
using strange.framework.api;
using System;

namespace strange.extensions.pool.impl
{
	public class Pool<T> : Pool, IPool<T>, IPool, IManagedList
	{
		public Pool()
		{
			base.poolType = typeof(T);
		}

		public new T GetInstance()
		{
			return (T)((object)base.GetInstance());
		}
	}
}
