using System;
using UnityEngine;

namespace FixedPoint
{
	// Token: 0x02000B1F RID: 2847
	[Serializable]
	public struct FixedRect
	{
		// Token: 0x060051F4 RID: 20980 RVA: 0x001538E3 File Offset: 0x00151CE3
		public FixedRect(Fixed x, Fixed y, Fixed width, Fixed height)
		{
			this.position = new Vector2F(x, y);
			this.dimensions = new Vector2F(width, height);
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x00153900 File Offset: 0x00151D00
		public FixedRect(Vector2F position, Vector2F dimensions)
		{
			this.position = position;
			this.dimensions = dimensions;
		}

		// Token: 0x060051F6 RID: 20982 RVA: 0x00153910 File Offset: 0x00151D10
		public FixedRect(Vector2F position, Fixed width, Fixed height)
		{
			this.position = position;
			this.dimensions = new Vector2F(width, height);
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x060051F7 RID: 20983 RVA: 0x00153926 File Offset: 0x00151D26
		public Fixed Left
		{
			get
			{
				return this.position.x;
			}
		}

		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x060051F8 RID: 20984 RVA: 0x00153933 File Offset: 0x00151D33
		public Fixed Right
		{
			get
			{
				return this.position.x + this.dimensions.x;
			}
		}

		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x060051F9 RID: 20985 RVA: 0x00153950 File Offset: 0x00151D50
		public Fixed Top
		{
			get
			{
				return this.position.y;
			}
		}

		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x060051FA RID: 20986 RVA: 0x0015395D File Offset: 0x00151D5D
		public Fixed Bottom
		{
			get
			{
				return this.position.y - this.dimensions.y;
			}
		}

		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x060051FB RID: 20987 RVA: 0x0015397A File Offset: 0x00151D7A
		public Vector2F TopLeft
		{
			get
			{
				return new Vector2F(this.Left, this.Top);
			}
		}

		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x060051FC RID: 20988 RVA: 0x0015398D File Offset: 0x00151D8D
		public Vector2F TopRight
		{
			get
			{
				return new Vector2F(this.Right, this.Top);
			}
		}

		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x060051FD RID: 20989 RVA: 0x001539A0 File Offset: 0x00151DA0
		public Vector2F BottomLeft
		{
			get
			{
				return new Vector2F(this.Left, this.Bottom);
			}
		}

		// Token: 0x17001319 RID: 4889
		// (get) Token: 0x060051FE RID: 20990 RVA: 0x001539B3 File Offset: 0x00151DB3
		public Vector2F BottomRight
		{
			get
			{
				return new Vector2F(this.Right, this.Bottom);
			}
		}

		// Token: 0x1700131A RID: 4890
		// (get) Token: 0x060051FF RID: 20991 RVA: 0x001539C6 File Offset: 0x00151DC6
		public Vector2F Center
		{
			get
			{
				return new Vector2F(this.Left + this.Width / 2, this.Top + this.Height / 2);
			}
		}

		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x06005200 RID: 20992 RVA: 0x001539FB File Offset: 0x00151DFB
		public Fixed Width
		{
			get
			{
				return this.dimensions.x;
			}
		}

		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x06005201 RID: 20993 RVA: 0x00153A08 File Offset: 0x00151E08
		public Fixed Height
		{
			get
			{
				return this.dimensions.y;
			}
		}

		// Token: 0x06005202 RID: 20994 RVA: 0x00153A15 File Offset: 0x00151E15
		public static explicit operator FixedRect(Rect rect)
		{
			return new FixedRect((Fixed)((double)rect.xMin), (Fixed)((double)rect.yMin), (Fixed)((double)rect.width), (Fixed)((double)rect.height));
		}

		// Token: 0x06005203 RID: 20995 RVA: 0x00153A50 File Offset: 0x00151E50
		public static explicit operator Rect(FixedRect rect)
		{
			return new Rect((float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
		}

		// Token: 0x06005204 RID: 20996 RVA: 0x00153A87 File Offset: 0x00151E87
		public void Set(Fixed x, Fixed y, Fixed width, Fixed height)
		{
			this.position.Set(x, y);
			this.dimensions.Set(width, height);
		}

		// Token: 0x06005205 RID: 20997 RVA: 0x00153AA4 File Offset: 0x00151EA4
		public bool Overlaps(FixedRect other)
		{
			return other.Left <= this.Right && other.Right >= this.Left && other.Top >= this.Bottom && other.Bottom <= this.Top;
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x00153B0C File Offset: 0x00151F0C
		public bool ContainsPoint(Vector2F point)
		{
			return point.x >= this.Left && point.x <= this.Right && point.y <= this.Top && point.y >= this.Bottom;
		}

		// Token: 0x06005207 RID: 20999 RVA: 0x00153B74 File Offset: 0x00151F74
		public static FixedRect CalculateBounds(params Vector2F[] points)
		{
			if (points == null || points.Length == 0)
			{
				return default(FixedRect);
			}
			Fixed @fixed = Fixed.MaxValue;
			Fixed other = Fixed.MaxValue;
			Fixed fixed2 = -Fixed.MaxValue;
			Fixed fixed3 = -Fixed.MaxValue;
			foreach (Vector2F vector2F in points)
			{
				if (vector2F.x < @fixed)
				{
					@fixed = vector2F.x;
				}
				if (vector2F.x > fixed2)
				{
					fixed2 = vector2F.x;
				}
				if (vector2F.y < other)
				{
					other = vector2F.y;
				}
				if (vector2F.y > fixed3)
				{
					fixed3 = vector2F.y;
				}
			}
			return new FixedRect(new Vector2F(@fixed, fixed3), new Vector2F(fixed2 - @fixed, fixed3 - other));
		}

		// Token: 0x04003497 RID: 13463
		public Vector2F position;

		// Token: 0x04003498 RID: 13464
		public Vector2F dimensions;
	}
}
