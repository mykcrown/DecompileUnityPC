using System;
using System.Collections.Generic;

// Token: 0x02000513 RID: 1299
[Serializable]
public class Hit : RollbackStateTyped<Hit>
{
	// Token: 0x06001BFB RID: 7163 RVA: 0x00073331 File Offset: 0x00071731
	public Hit()
	{
		this.init();
	}

	// Token: 0x06001BFC RID: 7164 RVA: 0x00073358 File Offset: 0x00071758
	public Hit(HitData data)
	{
		this.data = data;
		for (int i = 0; i < data.hitBoxes.Count; i++)
		{
			HitBoxState hitBoxState = new HitBoxState();
			hitBoxState.Load(data.hitBoxes[i]);
			this.hitBoxes.Add(hitBoxState);
		}
		this.init();
	}

	// Token: 0x06001BFD RID: 7165 RVA: 0x000733D0 File Offset: 0x000717D0
	private void init()
	{
		for (int i = 0; i < this.hitBoxes.Capacity; i++)
		{
			this.hitBoxesPool.Add(new HitBoxState());
		}
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x0007340C File Offset: 0x0007180C
	public override void CopyTo(Hit targetIn)
	{
		targetIn.data = this.data;
		targetIn.hitBoxes.Clear();
		for (int i = 0; i < this.hitBoxes.Count; i++)
		{
			targetIn.hitBoxes.Add(targetIn.hitBoxesPool[i]);
			this.hitBoxes[i].CopyTo(targetIn.hitBoxes[i]);
		}
	}

	// Token: 0x06001BFF RID: 7167 RVA: 0x00073480 File Offset: 0x00071880
	public override object Clone()
	{
		Hit hit = new Hit();
		this.CopyTo(hit);
		return hit;
	}

	// Token: 0x040016BB RID: 5819
	[IgnoreRollback(IgnoreRollbackType.Static)]
	[IgnoreCopyValidation]
	[NonSerialized]
	public HitData data;

	// Token: 0x040016BC RID: 5820
	[IsClonedManually]
	[IgnoreCopyValidation]
	public List<HitBoxState> hitBoxes = new List<HitBoxState>(16);

	// Token: 0x040016BD RID: 5821
	[IgnoreRollback(IgnoreRollbackType.Static)]
	[IgnoreCopyValidation]
	[NonSerialized]
	private List<HitBoxState> hitBoxesPool = new List<HitBoxState>();
}
