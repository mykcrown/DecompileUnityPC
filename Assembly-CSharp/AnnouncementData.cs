using System;
using UnityEngine.Serialization;

// Token: 0x0200034D RID: 845
[Serializable]
public class AnnouncementData
{
	// Token: 0x04000B61 RID: 2913
	public bool isEmpty;

	// Token: 0x04000B62 RID: 2914
	public string subtitle;

	// Token: 0x04000B63 RID: 2915
	[FormerlySerializedAs("_sound")]
	public AudioData sound;

	// Token: 0x04000B64 RID: 2916
	public int weight;

	// Token: 0x04000B65 RID: 2917
	public Priority priority = Priority.Low;
}
