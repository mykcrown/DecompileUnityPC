// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.framework.api
{
	public interface IInstanceProvider
	{
		T GetInstance<T>();

		object GetInstance(Type key);
	}
}
