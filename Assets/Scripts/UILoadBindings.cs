// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class UILoadBindings : IUILoaderBindings
{
	public class SoundBundleDef
	{
		public SoundBundleKey key;

		public bool preloadIndividualSounds;
	}

	private Dictionary<Type, List<DataRequirement>> bindings = new Dictionary<Type, List<DataRequirement>>();

	private Dictionary<Type, List<UILoadBindings.SoundBundleDef>> soundBundles = new Dictionary<Type, List<UILoadBindings.SoundBundleDef>>();

	public void AddBinding<T, U>(DataRequirementLevel level = DataRequirementLevel.REQUIRED) where U : IDataDependency
	{
		List<DataRequirement> list = this.getList(typeof(T));
		list.Add(new DataRequirement
		{
			theType = typeof(U),
			level = level
		});
	}

	public void AddBinding<T>(SoundBundleKey key, bool preloadIndividualSounds)
	{
		List<UILoadBindings.SoundBundleDef> soundBundleList = this.getSoundBundleList(typeof(T));
		soundBundleList.Add(new UILoadBindings.SoundBundleDef
		{
			key = key,
			preloadIndividualSounds = preloadIndividualSounds
		});
	}

	public DataRequirement[] GetBindings(Type theType)
	{
		return this.getList(theType).ToArray();
	}

	public UILoadBindings.SoundBundleDef[] GetSoundBindings(Type theType)
	{
		return this.getSoundBundleList(theType).ToArray();
	}

	private List<DataRequirement> getList(Type theType)
	{
		if (!this.bindings.ContainsKey(theType))
		{
			this.bindings[theType] = new List<DataRequirement>();
		}
		return this.bindings[theType];
	}

	private List<UILoadBindings.SoundBundleDef> getSoundBundleList(Type theType)
	{
		if (!this.soundBundles.ContainsKey(theType))
		{
			this.soundBundles[theType] = new List<UILoadBindings.SoundBundleDef>();
		}
		return this.soundBundles[theType];
	}
}
