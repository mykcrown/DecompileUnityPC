// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;

[Serializable]
public class AnnouncementConfigData
{
	public int SaltyRunbackSeconds = 5;

	public int MinMsBetweenAnnouncements = 100;

	public bool SubtitlesEnabled = true;

	public bool AnnouncementsEnabled = true;

	public float AnnouncementsVolume = 1f;

	public List<AnnouncementDataBankElement> AnnouncementDataBank;

	public List<AnnouncementStatData> AnnouncementStats;

	public List<string> CustomAnnouncementIDs;

	public static List<string> StaticAnnouncementIDs
	{
		get
		{
			List<string> list = new List<string>();
			FieldInfo[] fields = typeof(AnnouncementType).GetFields();
			FieldInfo[] array = fields;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				if (fieldInfo.GetValue(null) is string)
				{
					list.Add((string)fieldInfo.GetValue(null));
				}
			}
			return list;
		}
	}

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
}
