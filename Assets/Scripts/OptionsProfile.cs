// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class OptionsProfile
{
	public BattleSettings settings;

	public bool isDefault;

	public string id = "__default";

	public string name;

	public OptionsProfile Clone()
	{
		return new OptionsProfile
		{
			settings = this.settings.Clone(),
			isDefault = this.isDefault,
			id = this.id,
			name = this.name
		};
	}
}
