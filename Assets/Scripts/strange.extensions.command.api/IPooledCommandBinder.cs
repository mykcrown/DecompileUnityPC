// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.pool.impl;
using System;

namespace strange.extensions.command.api
{
	public interface IPooledCommandBinder
	{
		bool usePooling
		{
			get;
			set;
		}

		Pool<T> GetPool<T>();
	}
}
