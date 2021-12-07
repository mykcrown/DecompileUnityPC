// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUILoaderBindings
{
	void AddBinding<T, U>(DataRequirementLevel level = DataRequirementLevel.REQUIRED) where U : IDataDependency;

	DataRequirement[] GetBindings(Type theType);

	void AddBinding<T>(SoundBundleKey key, bool preloadIndividualSounds);

	UILoadBindings.SoundBundleDef[] GetSoundBindings(Type theType);
}
