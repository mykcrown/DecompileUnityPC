// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class MaterialAnimationKeyValueList : List<KeyValuePair<string, MaterialAnimationData>>
{
	public MaterialAnimationKeyValueList()
	{
	}

	public MaterialAnimationKeyValueList(IEnumerable<KeyValuePair<string, MaterialAnimationData>> rhs) : base(rhs)
	{
	}
}
