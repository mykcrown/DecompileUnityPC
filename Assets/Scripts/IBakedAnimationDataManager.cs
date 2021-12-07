// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IBakedAnimationDataManager
{
	void Set(string characterName, BakedAnimationData data);

	BakedAnimationData Get(string characterName);

	void OnApplicationQuit();

	void Clear();
}
