// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class OptionsProfileSet
{
	public OptionsProfile defaultProfile;

	public List<OptionsProfile> list = new List<OptionsProfile>();

	public string currentSelectedId;

	public OptionsProfileSet Clone()
	{
		OptionsProfileSet optionsProfileSet = new OptionsProfileSet();
		optionsProfileSet.defaultProfile = this.defaultProfile.Clone();
		foreach (OptionsProfile current in this.list)
		{
			optionsProfileSet.list.Add(current.Clone());
		}
		optionsProfileSet.currentSelectedId = this.currentSelectedId;
		return optionsProfileSet;
	}
}
