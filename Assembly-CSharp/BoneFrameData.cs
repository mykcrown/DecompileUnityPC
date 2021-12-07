using System;
using FixedPoint;

// Token: 0x02000342 RID: 834
public struct BoneFrameData
{
	// Token: 0x060011AB RID: 4523 RVA: 0x00066285 File Offset: 0x00064685
	public BoneFrameData(Vector3F position, QuaternionF rotation)
	{
		this = default(BoneFrameData);
		this.position = position;
		this.rotation = rotation;
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x060011AC RID: 4524 RVA: 0x0006629C File Offset: 0x0006469C
	// (set) Token: 0x060011AD RID: 4525 RVA: 0x000662A4 File Offset: 0x000646A4
	public Vector3F position { get; private set; }

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x060011AE RID: 4526 RVA: 0x000662AD File Offset: 0x000646AD
	// (set) Token: 0x060011AF RID: 4527 RVA: 0x000662B5 File Offset: 0x000646B5
	public QuaternionF rotation { get; private set; }
}
