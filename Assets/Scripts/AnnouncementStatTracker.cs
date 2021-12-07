// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class AnnouncementStatTracker : IRollbackStateOwner
{
	private AnnouncementStatData data;

	private Action<string> triggerAnnouncement;

	private PlayerNum trackedPlayer = PlayerNum.All;

	private AnnouncementStatModel model = new AnnouncementStatModel();

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public void Init(AnnouncementStatData data, Action<string> triggerAnnouncement, PlayerNum trackedPlayer = PlayerNum.All)
	{
		this.data = data;
		this.triggerAnnouncement = triggerAnnouncement;
		this.trackedPlayer = trackedPlayer;
	}

	public bool RecordStat(int quantity, PlayerNum player, bool canAnnounce, int frame)
	{
		if (this.trackedPlayer != PlayerNum.All && player != this.trackedPlayer)
		{
			return false;
		}
		bool result = false;
		StatTriggerMode triggerMode = this.data.triggerMode;
		if (triggerMode != StatTriggerMode.EachOccurrance)
		{
			if (triggerMode != StatTriggerMode.QuantityWithinInterval)
			{
				if (triggerMode == StatTriggerMode.ValueThreshold)
				{
					if (quantity > this.data.statQuantityRequired)
					{
						this.triggerAnnouncement(this.data.announcement);
						result = true;
					}
				}
			}
			else
			{
				Fixed @fixed = (Fixed)((double)((float)frame / WTime.fps));
				if (@fixed - this.model.lastRecordedSeconds > (Fixed)((double)this.data.intervalSeconds))
				{
					this.model.currentQuantity = 0;
				}
				this.model.currentQuantity += quantity;
				if (this.model.currentQuantity >= this.data.statQuantityRequired)
				{
					this.triggerAnnouncement(this.data.announcement);
					this.model.currentQuantity = 0;
					result = true;
				}
				this.model.lastRecordedSeconds = @fixed;
			}
		}
		else
		{
			this.triggerAnnouncement(this.data.announcement);
			result = true;
		}
		return result;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<AnnouncementStatModel>(ref this.model);
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<AnnouncementStatModel>(this.model));
	}
}
