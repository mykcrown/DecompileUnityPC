using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x0200034C RID: 844
[Serializable]
public class AnnouncementConfigData
{
	// Token: 0x17000317 RID: 791
	// (get) Token: 0x060011D2 RID: 4562 RVA: 0x00066B80 File Offset: 0x00064F80
	public static List<string> StaticAnnouncementIDs
	{
		get
		{
			List<string> list = new List<string>();
			FieldInfo[] fields = typeof(AnnouncementType).GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.GetValue(null) is string)
				{
					list.Add((string)fieldInfo.GetValue(null));
				}
			}
			return list;
		}
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00066BE8 File Offset: 0x00064FE8
	public List<string> AllAnnouncementIDs
	{
		get
		{
			List<string> list = new List<string>();
			list.AddRange(AnnouncementConfigData.StaticAnnouncementIDs);
			list.AddRange(this.CustomAnnouncementIDs);
			return list;
		}
	}

	// Token: 0x04000B59 RID: 2905
	public int SaltyRunbackSeconds = 5;

	// Token: 0x04000B5A RID: 2906
	public int MinMsBetweenAnnouncements = 100;

	// Token: 0x04000B5B RID: 2907
	public bool SubtitlesEnabled = true;

	// Token: 0x04000B5C RID: 2908
	public bool AnnouncementsEnabled = true;

	// Token: 0x04000B5D RID: 2909
	public float AnnouncementsVolume = 1f;

	// Token: 0x04000B5E RID: 2910
	public List<AnnouncementDataBankElement> AnnouncementDataBank;

	// Token: 0x04000B5F RID: 2911
	public List<AnnouncementStatData> AnnouncementStats;

	// Token: 0x04000B60 RID: 2912
	public List<string> CustomAnnouncementIDs;
}
