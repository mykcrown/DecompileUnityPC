// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;

namespace strange.extensions.pool.api
{
	public interface IPool : IManagedList
	{
		IInstanceProvider instanceProvider
		{
			get;
			set;
		}

		Type poolType
		{
			get;
			set;
		}

		int available
		{
			get;
		}

		int size
		{
			get;
			set;
		}

		int instanceCount
		{
			get;
		}

		PoolOverflowBehavior overflowBehavior
		{
			get;
			set;
		}

		PoolInflationType inflationType
		{
			get;
			set;
		}

		object GetInstance();

		void ReturnInstance(object value);

		void Clean();
	}
}
