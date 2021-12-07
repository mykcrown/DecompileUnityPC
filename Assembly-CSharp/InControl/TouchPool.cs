using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000132 RID: 306
	public class TouchPool
	{
		// Token: 0x060006E6 RID: 1766 RVA: 0x0002E3D8 File Offset: 0x0002C7D8
		public TouchPool(int capacity)
		{
			this.freeTouches = new List<Touch>(capacity);
			for (int i = 0; i < capacity; i++)
			{
				this.freeTouches.Add(new Touch());
			}
			this.usedTouches = new List<Touch>(capacity);
			this.Touches = new ReadOnlyCollection<Touch>(this.usedTouches);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0002E436 File Offset: 0x0002C836
		public TouchPool() : this(16)
		{
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0002E440 File Offset: 0x0002C840
		public Touch FindOrCreateTouch(int fingerId)
		{
			int count = this.usedTouches.Count;
			Touch touch;
			for (int i = 0; i < count; i++)
			{
				touch = this.usedTouches[i];
				if (touch.fingerId == fingerId)
				{
					return touch;
				}
			}
			touch = this.NewTouch();
			touch.fingerId = fingerId;
			this.usedTouches.Add(touch);
			return touch;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0002E4A4 File Offset: 0x0002C8A4
		public Touch FindTouch(int fingerId)
		{
			int count = this.usedTouches.Count;
			for (int i = 0; i < count; i++)
			{
				Touch touch = this.usedTouches[i];
				if (touch.fingerId == fingerId)
				{
					return touch;
				}
			}
			return null;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0002E4EC File Offset: 0x0002C8EC
		private Touch NewTouch()
		{
			int count = this.freeTouches.Count;
			if (count > 0)
			{
				Touch result = this.freeTouches[count - 1];
				this.freeTouches.RemoveAt(count - 1);
				return result;
			}
			return new Touch();
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0002E530 File Offset: 0x0002C930
		public void FreeTouch(Touch touch)
		{
			touch.Reset();
			this.freeTouches.Add(touch);
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0002E544 File Offset: 0x0002C944
		public void FreeEndedTouches()
		{
			int count = this.usedTouches.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				Touch touch = this.usedTouches[i];
				if (touch.phase == TouchPhase.Ended)
				{
					this.usedTouches.RemoveAt(i);
				}
			}
		}

		// Token: 0x0400055C RID: 1372
		public readonly ReadOnlyCollection<Touch> Touches;

		// Token: 0x0400055D RID: 1373
		private List<Touch> usedTouches;

		// Token: 0x0400055E RID: 1374
		private List<Touch> freeTouches;
	}
}
