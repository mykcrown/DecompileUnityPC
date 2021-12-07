// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.framework.api
{
	public interface ISemiBinding : IManagedList
	{
		Enum constraint
		{
			get;
			set;
		}

		bool uniqueValues
		{
			get;
			set;
		}
	}
}
