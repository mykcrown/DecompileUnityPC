using System;
using System.Collections.Generic;

// Token: 0x02000A62 RID: 2658
public class UILoadBindings : IUILoaderBindings
{
	// Token: 0x06004D19 RID: 19737 RVA: 0x001459B4 File Offset: 0x00143DB4
	public void AddBinding<T, U>(DataRequirementLevel level = DataRequirementLevel.REQUIRED) where U : IDataDependency
	{
		List<DataRequirement> list = this.getList(typeof(T));
		list.Add(new DataRequirement
		{
			theType = typeof(U),
			level = level
		});
	}

	// Token: 0x06004D1A RID: 19738 RVA: 0x001459F8 File Offset: 0x00143DF8
	public void AddBinding<T>(SoundBundleKey key, bool preloadIndividualSounds)
	{
		List<UILoadBindings.SoundBundleDef> soundBundleList = this.getSoundBundleList(typeof(T));
		soundBundleList.Add(new UILoadBindings.SoundBundleDef
		{
			key = key,
			preloadIndividualSounds = preloadIndividualSounds
		});
	}

	// Token: 0x06004D1B RID: 19739 RVA: 0x00145A31 File Offset: 0x00143E31
	public DataRequirement[] GetBindings(Type theType)
	{
		return this.getList(theType).ToArray();
	}

	// Token: 0x06004D1C RID: 19740 RVA: 0x00145A3F File Offset: 0x00143E3F
	public UILoadBindings.SoundBundleDef[] GetSoundBindings(Type theType)
	{
		return this.getSoundBundleList(theType).ToArray();
	}

	// Token: 0x06004D1D RID: 19741 RVA: 0x00145A4D File Offset: 0x00143E4D
	private List<DataRequirement> getList(Type theType)
	{
		if (!this.bindings.ContainsKey(theType))
		{
			this.bindings[theType] = new List<DataRequirement>();
		}
		return this.bindings[theType];
	}

	// Token: 0x06004D1E RID: 19742 RVA: 0x00145A7D File Offset: 0x00143E7D
	private List<UILoadBindings.SoundBundleDef> getSoundBundleList(Type theType)
	{
		if (!this.soundBundles.ContainsKey(theType))
		{
			this.soundBundles[theType] = new List<UILoadBindings.SoundBundleDef>();
		}
		return this.soundBundles[theType];
	}

	// Token: 0x0400329B RID: 12955
	private Dictionary<Type, List<DataRequirement>> bindings = new Dictionary<Type, List<DataRequirement>>();

	// Token: 0x0400329C RID: 12956
	private Dictionary<Type, List<UILoadBindings.SoundBundleDef>> soundBundles = new Dictionary<Type, List<UILoadBindings.SoundBundleDef>>();

	// Token: 0x02000A63 RID: 2659
	public class SoundBundleDef
	{
		// Token: 0x0400329D RID: 12957
		public SoundBundleKey key;

		// Token: 0x0400329E RID: 12958
		public bool preloadIndividualSounds;
	}
}
