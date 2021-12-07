using System;
using UnityEngine;

// Token: 0x0200033E RID: 830
public class AvatarData
{
	// Token: 0x0600119A RID: 4506 RVA: 0x00065CAA File Offset: 0x000640AA
	public AvatarData(Avatar humanoidAvatar, Avatar genericAvatar)
	{
		this.humanoidAvatar = humanoidAvatar;
		this.genericAvatar = genericAvatar;
	}

	// Token: 0x04000B3E RID: 2878
	public Avatar humanoidAvatar;

	// Token: 0x04000B3F RID: 2879
	public Avatar genericAvatar;
}
