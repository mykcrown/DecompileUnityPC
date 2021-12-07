using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200001B RID: 27
	public class SplineControlPoint
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000A2EF File Offset: 0x000086EF
		public SplineControlPoint NextControlPoint
		{
			get
			{
				return this.mSpline.NextControlPoint(this);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000FD RID: 253 RVA: 0x0000A2FD File Offset: 0x000086FD
		public SplineControlPoint PreviousControlPoint
		{
			get
			{
				return this.mSpline.PreviousControlPoint(this);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000A30B File Offset: 0x0000870B
		public Vector3 NextPosition
		{
			get
			{
				return this.mSpline.NextPosition(this);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x060000FF RID: 255 RVA: 0x0000A319 File Offset: 0x00008719
		public Vector3 PreviousPosition
		{
			get
			{
				return this.mSpline.PreviousPosition(this);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000100 RID: 256 RVA: 0x0000A327 File Offset: 0x00008727
		public Vector3 NextNormal
		{
			get
			{
				return this.mSpline.NextNormal(this);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000A335 File Offset: 0x00008735
		public Vector3 PreviousNormal
		{
			get
			{
				return this.mSpline.PreviousNormal(this);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000A343 File Offset: 0x00008743
		public bool IsValid
		{
			get
			{
				return this.NextControlPoint != null;
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000A354 File Offset: 0x00008754
		private Vector3 GetNext2Position()
		{
			SplineControlPoint nextControlPoint = this.NextControlPoint;
			if (nextControlPoint != null)
			{
				return nextControlPoint.NextPosition;
			}
			return this.NextPosition;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000A37C File Offset: 0x0000877C
		private Vector3 GetNext2Normal()
		{
			SplineControlPoint nextControlPoint = this.NextControlPoint;
			if (nextControlPoint != null)
			{
				return nextControlPoint.NextNormal;
			}
			return this.Normal;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000A3A3 File Offset: 0x000087A3
		public Vector3 Interpolate(float localF)
		{
			localF = Mathf.Clamp01(localF);
			return Spline.CatmulRom(this.PreviousPosition, this.Position, this.NextPosition, this.GetNext2Position(), localF);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000A3CB File Offset: 0x000087CB
		public Vector3 InterpolateNormal(float localF)
		{
			localF = Mathf.Clamp01(localF);
			return Spline.CatmulRom(this.PreviousNormal, this.Normal, this.NextNormal, this.GetNext2Normal(), localF);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000A3F3 File Offset: 0x000087F3
		public void Init(Spline owner)
		{
			this.mSpline = owner;
			this.SegmentIndex = -1;
		}

		// Token: 0x040000C7 RID: 199
		public Vector3 Position;

		// Token: 0x040000C8 RID: 200
		public Vector3 Normal;

		// Token: 0x040000C9 RID: 201
		public int ControlPointIndex = -1;

		// Token: 0x040000CA RID: 202
		public int SegmentIndex = -1;

		// Token: 0x040000CB RID: 203
		public float Dist;

		// Token: 0x040000CC RID: 204
		protected Spline mSpline;
	}
}
