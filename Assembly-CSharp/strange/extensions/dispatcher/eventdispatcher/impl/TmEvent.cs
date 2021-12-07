using System;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.pool.api;

namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	// Token: 0x0200023A RID: 570
	public class TmEvent : IEvent, IPoolable
	{
		// Token: 0x06000B3E RID: 2878 RVA: 0x00053B8F File Offset: 0x00051F8F
		public TmEvent()
		{
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00053B97 File Offset: 0x00051F97
		public TmEvent(object type, IEventDispatcher target, object data)
		{
			this.type = type;
			this.target = target;
			this.data = data;
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00053BB4 File Offset: 0x00051FB4
		// (set) Token: 0x06000B41 RID: 2881 RVA: 0x00053BBC File Offset: 0x00051FBC
		public object type { get; set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x00053BC5 File Offset: 0x00051FC5
		// (set) Token: 0x06000B43 RID: 2883 RVA: 0x00053BCD File Offset: 0x00051FCD
		public IEventDispatcher target { get; set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x00053BD6 File Offset: 0x00051FD6
		// (set) Token: 0x06000B45 RID: 2885 RVA: 0x00053BDE File Offset: 0x00051FDE
		public object data { get; set; }

		// Token: 0x06000B46 RID: 2886 RVA: 0x00053BE7 File Offset: 0x00051FE7
		public void Restore()
		{
			this.type = null;
			this.target = null;
			this.data = null;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00053BFE File Offset: 0x00051FFE
		public void Retain()
		{
			this.retainCount++;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00053C0E File Offset: 0x0005200E
		public void Release()
		{
			this.retainCount--;
			if (this.retainCount == 0)
			{
				this.target.ReleaseEvent(this);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x00053C35 File Offset: 0x00052035
		public bool retain
		{
			get
			{
				return this.retainCount > 0;
			}
		}

		// Token: 0x04000747 RID: 1863
		protected int retainCount;
	}
}
