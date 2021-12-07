// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace AI
{
	public interface IDataNode
	{
		FloatDataDictionary FloatData
		{
			get;
		}

		IntDataDictionary IntData
		{
			get;
		}

		FixedDataDictionary FixedData
		{
			get;
		}

		BoolDataDictionary BoolData
		{
			get;
		}
	}
}
