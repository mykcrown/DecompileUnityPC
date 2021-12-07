// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class CharacterDefaultAnimationData : IGameDataElement
{
	private static string ANIM_ID_PREFIX = "CharacterDefaultAnimationData.";

	public CharacterDefaultAnimationKey type;

	public WavedashAnimationData animationData = new WavedashAnimationData();

	LocalizationData IGameDataElement.Localization
	{
		get
		{
			return null;
		}
	}

	public bool Enabled
	{
		get
		{
			return true;
		}
	}

	public int ID
	{
		get
		{
			return (int)this.type;
		}
	}

	public string Key
	{
		get
		{
			return CharacterDefaultAnimationData.ANIM_ID_PREFIX + this.type.ToString();
		}
	}

	public string CreateAnimationId()
	{
		return CharacterDefaultAnimationData.ANIM_ID_PREFIX + this.type.ToString();
	}
}
