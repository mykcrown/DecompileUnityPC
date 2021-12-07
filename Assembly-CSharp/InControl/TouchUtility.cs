﻿using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000137 RID: 311
	public static class TouchUtility
	{
		// Token: 0x06000712 RID: 1810 RVA: 0x0002EE2C File Offset: 0x0002D22C
		public static Vector2 AnchorToViewPoint(TouchControlAnchor touchControlAnchor)
		{
			switch (touchControlAnchor)
			{
			case TouchControlAnchor.TopLeft:
				return new Vector2(0f, 1f);
			case TouchControlAnchor.CenterLeft:
				return new Vector2(0f, 0.5f);
			case TouchControlAnchor.BottomLeft:
				return new Vector2(0f, 0f);
			case TouchControlAnchor.TopCenter:
				return new Vector2(0.5f, 1f);
			case TouchControlAnchor.Center:
				return new Vector2(0.5f, 0.5f);
			case TouchControlAnchor.BottomCenter:
				return new Vector2(0.5f, 0f);
			case TouchControlAnchor.TopRight:
				return new Vector2(1f, 1f);
			case TouchControlAnchor.CenterRight:
				return new Vector2(1f, 0.5f);
			case TouchControlAnchor.BottomRight:
				return new Vector2(1f, 0f);
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0002EEFD File Offset: 0x0002D2FD
		public static Vector2 RoundVector(Vector2 vector)
		{
			return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
		}
	}
}
