using System;
using FixedPoint;

// Token: 0x02000354 RID: 852
public class AnnouncementStatTracker : IRollbackStateOwner
{
	// Token: 0x1700031E RID: 798
	// (get) Token: 0x060011F5 RID: 4597 RVA: 0x0006778B File Offset: 0x00065B8B
	// (set) Token: 0x060011F6 RID: 4598 RVA: 0x00067793 File Offset: 0x00065B93
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x060011F7 RID: 4599 RVA: 0x0006779C File Offset: 0x00065B9C
	public void Init(AnnouncementStatData data, Action<string> triggerAnnouncement, PlayerNum trackedPlayer = PlayerNum.All)
	{
		this.data = data;
		this.triggerAnnouncement = triggerAnnouncement;
		this.trackedPlayer = trackedPlayer;
	}

	// Token: 0x060011F8 RID: 4600 RVA: 0x000677B4 File Offset: 0x00065BB4
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

	// Token: 0x060011F9 RID: 4601 RVA: 0x000678F9 File Offset: 0x00065CF9
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<AnnouncementStatModel>(ref this.model);
		return true;
	}

	// Token: 0x060011FA RID: 4602 RVA: 0x00067909 File Offset: 0x00065D09
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.rollbackStatePooling.Clone<AnnouncementStatModel>(this.model));
	}

	// Token: 0x04000B86 RID: 2950
	private AnnouncementStatData data;

	// Token: 0x04000B87 RID: 2951
	private Action<string> triggerAnnouncement;

	// Token: 0x04000B88 RID: 2952
	private PlayerNum trackedPlayer = PlayerNum.All;

	// Token: 0x04000B89 RID: 2953
	private AnnouncementStatModel model = new AnnouncementStatModel();
}
