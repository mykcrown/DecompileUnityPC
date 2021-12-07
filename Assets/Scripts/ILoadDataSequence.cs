// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ILoadDataSequence
{
	void Load(DataRequirement[] list, Action<LoadSequenceResults> callback);
}
