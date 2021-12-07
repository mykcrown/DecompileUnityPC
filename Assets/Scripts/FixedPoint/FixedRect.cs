// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace FixedPoint
{
	[Serializable]
	public struct FixedRect
	{
		public Vector2F position;

		public Vector2F dimensions;

		public Fixed Left
		{
			get
			{
				return this.position.x;
			}
		}

		public Fixed Right
		{
			get
			{
				return this.position.x + this.dimensions.x;
			}
		}

		public Fixed Top
		{
			get
			{
				return this.position.y;
			}
		}

		public Fixed Bottom
		{
			get
			{
				return this.position.y - this.dimensions.y;
			}
		}

		public Vector2F TopLeft
		{
			get
			{
				return new Vector2F(this.Left, this.Top);
			}
		}

		public Vector2F TopRight
		{
			get
			{
				return new Vector2F(this.Right, this.Top);
			}
		}

		public Vector2F BottomLeft
		{
			get
			{
				return new Vector2F(this.Left, this.Bottom);
			}
		}

		public Vector2F BottomRight
		{
			get
			{
				return new Vector2F(this.Right, this.Bottom);
			}
		}

		public Vector2F Center
		{
			get
			{
				return new Vector2F(this.Left + this.Width / 2, this.Top + this.Height / 2);
			}
		}

		public Fixed Width
		{
			get
			{
				return this.dimensions.x;
			}
		}

		public Fixed Height
		{
			get
			{
				return this.dimensions.y;
			}
		}

		public FixedRect(Fixed x, Fixed y, Fixed width, Fixed height)
		{
			this.position = new Vector2F(x, y);
			this.dimensions = new Vector2F(width, height);
		}

		public FixedRect(Vector2F position, Vector2F dimensions)
		{
			this.position = position;
			this.dimensions = dimensions;
		}

		public FixedRect(Vector2F position, Fixed width, Fixed height)
		{
			this.position = position;
			this.dimensions = new Vector2F(width, height);
		}

		public static explicit operator FixedRect(Rect rect)
		{
			return new FixedRect((Fixed)((double)rect.xMin), (Fixed)((double)rect.yMin), (Fixed)((double)rect.width), (Fixed)((double)rect.height));
		}

		public static explicit operator Rect(FixedRect rect)
		{
			return new Rect((float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
		}

		public void Set(Fixed x, Fixed y, Fixed width, Fixed height)
		{
			this.position.Set(x, y);
			this.dimensions.Set(width, height);
		}

		public bool Overlaps(FixedRect other)
		{
			return other.Left <= this.Right && other.Right >= this.Left && other.Top >= this.Bottom && other.Bottom <= this.Top;
		}

		public bool ContainsPoint(Vector2F point)
		{
			return point.x >= this.Left && point.x <= this.Right && point.y <= this.Top && point.y >= this.Bottom;
		}

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
			for (int i = 0; i < points.Length; i++)
			{
				Vector2F vector2F = points[i];
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
	}
}
