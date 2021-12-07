// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class Hit : RollbackStateTyped<Hit>
{
	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static)]
	[NonSerialized]
	public HitData data;

	[IgnoreCopyValidation, IsClonedManually]
	public List<HitBoxState> hitBoxes = new List<HitBoxState>(16);

	[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.Static)]
	[NonSerialized]
	private List<HitBoxState> hitBoxesPool = new List<HitBoxState>();

	public Hit()
	{
		this.init();
	}

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

	private void init()
	{
		for (int i = 0; i < this.hitBoxes.Capacity; i++)
		{
			this.hitBoxesPool.Add(new HitBoxState());
		}
	}

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

	public override object Clone()
	{
		Hit hit = new Hit();
		this.CopyTo(hit);
		return hit;
	}
}
