// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.extensions.pool.api
{
	public interface IPoolable
	{
		bool retain
		{
			get;
		}

		void Restore();

		void Retain();

		void Release();
	}
}
