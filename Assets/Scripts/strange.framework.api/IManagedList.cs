// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.framework.api
{
	public interface IManagedList
	{
		object value
		{
			get;
		}

		IManagedList Add(object value);

		IManagedList Add(object[] list);

		IManagedList Remove(object value);

		IManagedList Remove(object[] list);
	}
}
