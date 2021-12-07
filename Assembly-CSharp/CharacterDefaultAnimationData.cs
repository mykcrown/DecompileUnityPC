using System;

// Token: 0x0200038B RID: 907
[Serializable]
public class CharacterDefaultAnimationData : IGameDataElement
{
	// Token: 0x1700038F RID: 911
	// (get) Token: 0x0600137D RID: 4989 RVA: 0x00070177 File Offset: 0x0006E577
	public bool Enabled
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x0600137E RID: 4990 RVA: 0x0007017A File Offset: 0x0006E57A
	public int ID
	{
		get
		{
			return (int)this.type;
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x0600137F RID: 4991 RVA: 0x00070182 File Offset: 0x0006E582
	public string Key
	{
		get
		{
			return CharacterDefaultAnimationData.ANIM_ID_PREFIX + this.type.ToString();
		}
	}

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06001380 RID: 4992 RVA: 0x0007019F File Offset: 0x0006E59F
	LocalizationData IGameDataElement.Localization
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x000701A2 File Offset: 0x0006E5A2
	public string CreateAnimationId()
	{
		return CharacterDefaultAnimationData.ANIM_ID_PREFIX + this.type.ToString();
	}

	// Token: 0x04000CD7 RID: 3287
	private static string ANIM_ID_PREFIX = "CharacterDefaultAnimationData.";

	// Token: 0x04000CD8 RID: 3288
	public CharacterDefaultAnimationKey type;

	// Token: 0x04000CD9 RID: 3289
	public WavedashAnimationData animationData = new WavedashAnimationData();
}
