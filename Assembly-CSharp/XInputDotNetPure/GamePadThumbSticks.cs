using System;
using UnityEngine;

namespace XInputDotNetPure
{
	// Token: 0x020001E3 RID: 483
	public struct GamePadThumbSticks
	{
		// Token: 0x060008B8 RID: 2232 RVA: 0x0004CBC1 File Offset: 0x0004AFC1
		internal GamePadThumbSticks(GamePadThumbSticks.StickValue left, GamePadThumbSticks.StickValue right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0004CBD1 File Offset: 0x0004AFD1
		public GamePadThumbSticks.StickValue Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0004CBD9 File Offset: 0x0004AFD9
		public GamePadThumbSticks.StickValue Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x0400061D RID: 1565
		private GamePadThumbSticks.StickValue left;

		// Token: 0x0400061E RID: 1566
		private GamePadThumbSticks.StickValue right;

		// Token: 0x020001E4 RID: 484
		public struct StickValue
		{
			// Token: 0x060008BB RID: 2235 RVA: 0x0004CBE1 File Offset: 0x0004AFE1
			internal StickValue(float x, float y)
			{
				this.vector = new Vector2(x, y);
			}

			// Token: 0x1700018D RID: 397
			// (get) Token: 0x060008BC RID: 2236 RVA: 0x0004CBF0 File Offset: 0x0004AFF0
			public float X
			{
				get
				{
					return this.vector.x;
				}
			}

			// Token: 0x1700018E RID: 398
			// (get) Token: 0x060008BD RID: 2237 RVA: 0x0004CBFD File Offset: 0x0004AFFD
			public float Y
			{
				get
				{
					return this.vector.y;
				}
			}

			// Token: 0x1700018F RID: 399
			// (get) Token: 0x060008BE RID: 2238 RVA: 0x0004CC0A File Offset: 0x0004B00A
			public Vector2 Vector
			{
				get
				{
					return this.vector;
				}
			}

			// Token: 0x0400061F RID: 1567
			private Vector2 vector;
		}
	}
}
