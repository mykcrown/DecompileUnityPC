// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class ComboState : IRollbackStateOwner
{
	private ComboStateModel model = new ComboStateModel();

	[Inject]
	public ICombatCalculator combatCalculator
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public Fixed Damage
	{
		get
		{
			return this.model.Damage;
		}
	}

	public int Count
	{
		get
		{
			return this.model.Count;
		}
	}

	public bool IsRecovered
	{
		get
		{
			return this.model.IsRecovered;
		}
	}

	public int FramesRecovered
	{
		get
		{
			return this.model.FramesRecovered;
		}
	}

	public bool IsActive
	{
		get
		{
			return this.model.IsActive;
		}
	}

	public int WindowFrames
	{
		get
		{
			return this.model.WindowFrames;
		}
	}

	public void Setup(int framesTilInactive)
	{
		this.model.Damage = 0;
		this.model.Count = 0;
		this.model.IsRecovered = false;
		this.model.FramesRecovered = 0;
		this.model.IsActive = false;
		this.model.WindowFrames = framesTilInactive;
	}

	public void UpdateFrame(HitData hit, IHitOwner hitOwner, bool isRecovered = false, bool hasLostControl = false)
	{
		if (hit != null)
		{
			this.RecordHit(hit, hitOwner);
		}
		if (!this.IsActive)
		{
			return;
		}
		if (isRecovered)
		{
			this.RecordRecovered();
		}
		if (hasLostControl)
		{
			this.RecordLostControl();
		}
		this.AdvanceFrame();
	}

	public void Clear()
	{
		this.model.IsActive = false;
		this.model.Damage = 0;
		this.model.Count = 0;
		this.model.FramesRecovered = 0;
		this.model.IsRecovered = false;
	}

	public void ForceRecordHit(HitData hit, IHitOwner hitOwner)
	{
		this.RecordHit(hit, hitOwner);
	}

	private void RecordHit(HitData hit, IHitOwner hitOwner)
	{
		if (!this.IsActive)
		{
			this.model.Count = 1;
			this.model.Damage = 0;
		}
		else
		{
			this.model.Count++;
		}
		this.model.IsActive = true;
		this.model.IsRecovered = false;
		this.model.FramesRecovered = 0;
		this.model.Damage += this.combatCalculator.CalculateModifiedDamage(hit, hitOwner);
	}

	private void RecordRecovered()
	{
		if (!this.IsRecovered)
		{
			this.model.FramesRecovered = 0;
		}
		this.model.IsRecovered = true;
	}

	private void RecordLostControl()
	{
		if (this.IsRecovered)
		{
			this.model.IsRecovered = false;
			this.model.FramesRecovered = 0;
		}
	}

	private void AdvanceFrame()
	{
		if (this.IsRecovered)
		{
			this.model.FramesRecovered++;
		}
		if (this.FramesRecovered >= this.WindowFrames)
		{
			this.model.IsActive = false;
		}
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ComboStateModel>(ref this.model);
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<ComboStateModel>(this.model));
	}
}
