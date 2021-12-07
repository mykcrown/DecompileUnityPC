using System;
using UnityEngine;

// Token: 0x02000609 RID: 1545
public class SkinDefinition : ScriptableObject, IDefaultableData
{
	// Token: 0x17000966 RID: 2406
	// (get) Token: 0x06002618 RID: 9752 RVA: 0x000BC1B7 File Offset: 0x000BA5B7
	public int ID
	{
		get
		{
			return string.IsNullOrEmpty(this.uniqueKey) ? this.skinName.GetHashCode() : this.uniqueKey.GetHashCode();
		}
	}

	// Token: 0x17000967 RID: 2407
	// (get) Token: 0x06002619 RID: 9753 RVA: 0x000BC1E4 File Offset: 0x000BA5E4
	public bool IsDefaultData
	{
		get
		{
			return this.isDefault;
		}
	}

	// Token: 0x04001BEB RID: 7147
	public string skinName;

	// Token: 0x04001BEC RID: 7148
	public bool enabled = true;

	// Token: 0x04001BED RID: 7149
	public bool demoEnabled;

	// Token: 0x04001BEE RID: 7150
	public string uniqueKey;

	// Token: 0x04001BEF RID: 7151
	public bool isDefault;

	// Token: 0x04001BF0 RID: 7152
	public int priority = 1000;

	// Token: 0x04001BF1 RID: 7153
	public string dataFile;

	// Token: 0x04001BF2 RID: 7154
	public string sourceFilePath;
}
