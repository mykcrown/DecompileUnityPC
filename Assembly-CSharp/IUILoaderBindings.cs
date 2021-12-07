using System;

// Token: 0x02000A64 RID: 2660
public interface IUILoaderBindings
{
	// Token: 0x06004D20 RID: 19744
	void AddBinding<T, U>(DataRequirementLevel level = DataRequirementLevel.REQUIRED) where U : IDataDependency;

	// Token: 0x06004D21 RID: 19745
	DataRequirement[] GetBindings(Type theType);

	// Token: 0x06004D22 RID: 19746
	void AddBinding<T>(SoundBundleKey key, bool preloadIndividualSounds);

	// Token: 0x06004D23 RID: 19747
	UILoadBindings.SoundBundleDef[] GetSoundBindings(Type theType);
}
