using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Xft
{
	// Token: 0x0200001F RID: 31
	public class XWeaponTrail : MonoBehaviour, ITickable, IExpirable, IRollbackStateOwner, IPooledGameObjectListener
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000117 RID: 279 RVA: 0x0000B592 File Offset: 0x00009992
		// (set) Token: 0x06000118 RID: 280 RVA: 0x0000B59A File Offset: 0x0000999A
		[Inject]
		public IRollbackStatePooling rollbackStatePooling { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000B5A3 File Offset: 0x000099A3
		public Vector3 CurHeadPos
		{
			get
			{
				return (this.PointStart.position + this.PointEnd.position) / 2f;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600011A RID: 282 RVA: 0x0000B5CA File Offset: 0x000099CA
		public float TrailWidth
		{
			get
			{
				return this.mTrailWidth;
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000B5D4 File Offset: 0x000099D4
		public void Init()
		{
			if (this.model.inited)
			{
				return;
			}
			this.mElemPool = new XWeaponTrail.ElementPool(this.MaxFrame);
			this.model.snapShotList = new List<XWeaponTrail.Element>();
			this.mTrailWidth = (this.PointStart.position - this.PointEnd.position).magnitude;
			this.model.inited = true;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000B648 File Offset: 0x00009A48
		public void Activate()
		{
			this.Init();
			base.gameObject.SetActive(true);
			this.InitOriginalElements();
			this.InitMeshObj();
			this.InitSpline();
			this.model.isExpired = false;
			this.model.fadeT = 1f;
			this.model.fading = false;
			this.model.fadeFrames = 60;
			this.model.fadeElapsedFrames = 0;
			for (int i = 0; i < this.model.snapShotList.Count; i++)
			{
				this.model.snapShotList[i].PointStart = this.PointStart.position;
				this.model.snapShotList[i].PointEnd = this.PointEnd.position;
				this.mSpline.ControlPoints[i].Position = this.model.snapShotList[i].Pos;
				this.mSpline.ControlPoints[i].Normal = this.model.snapShotList[i].PointEnd - this.model.snapShotList[i].PointStart;
			}
			this.RefreshSpline();
			this.UpdateVertex();
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000B79C File Offset: 0x00009B9C
		public void Deactivate()
		{
			if (base.gameObject != null && !base.gameObject.Equals(null))
			{
				base.gameObject.DestroySafe();
			}
			this.model.fading = false;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000B7D7 File Offset: 0x00009BD7
		public void StopSmoothly(int fadeFrames)
		{
			this.model.fading = true;
			this.model.fadeFrames = fadeFrames;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600011F RID: 287 RVA: 0x0000B7F1 File Offset: 0x00009BF1
		public virtual bool IsExpired
		{
			get
			{
				return this.model.isExpired;
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000B7FE File Offset: 0x00009BFE
		public void TickFrame()
		{
			if (!this.model.inited)
			{
				return;
			}
			this.UpdateHeadElem();
			this.RecordCurElem();
			this.RefreshSpline();
			this.UpdateFade();
			this.UpdateVertex();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000B82F File Offset: 0x00009C2F
		public bool ExportState(ref RollbackStateContainer container)
		{
			return container.WriteState(this.rollbackStatePooling.Clone<XWeaponTrailModel>(this.model));
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000B849 File Offset: 0x00009C49
		public bool LoadState(RollbackStateContainer container)
		{
			container.ReadState<XWeaponTrailModel>(ref this.model);
			return true;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000B859 File Offset: 0x00009C59
		void IPooledGameObjectListener.OnAcquired()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000B867 File Offset: 0x00009C67
		void IPooledGameObjectListener.OnReleased()
		{
			base.gameObject.SetActive(false);
			this.model.isExpired = true;
			this.model.fading = false;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000B88D File Offset: 0x00009C8D
		void IPooledGameObjectListener.OnCooledOff()
		{
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000B88F File Offset: 0x00009C8F
		private void LateUpdate()
		{
			if (!this.model.inited || this.model.isExpired)
			{
				return;
			}
			this.mVertexPool.LateUpdate();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000B8BD File Offset: 0x00009CBD
		private void onSceneWasLoaded(Scene scene, LoadSceneMode mode)
		{
			this.model.inited = false;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000B8CB File Offset: 0x00009CCB
		private void OnDestroy()
		{
			if (this.mVertexPool != null)
			{
				this.mVertexPool.Destroy();
			}
			SceneManager.sceneLoaded -= this.onSceneWasLoaded;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000B8F4 File Offset: 0x00009CF4
		private void Awake()
		{
			if (!GameAssetPreloader.InProgress && GameController.IsMatchRunning && SystemBoot.mode != SystemBoot.Mode.StagePreview)
			{
				Debug.LogError("Instantiated weapon trail outside of preload time. Please add me to GameAssetPreloader! " + base.name);
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000B92A File Offset: 0x00009D2A
		private void Start()
		{
			SceneManager.sceneLoaded += this.onSceneWasLoaded;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000B940 File Offset: 0x00009D40
		private void OnDrawGizmos()
		{
			if (this.PointEnd == null || this.PointStart == null)
			{
				return;
			}
			float magnitude = (this.PointStart.position - this.PointEnd.position).magnitude;
			if (magnitude < Mathf.Epsilon)
			{
				return;
			}
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(this.PointStart.position, magnitude * 0.04f);
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(this.PointEnd.position, magnitude * 0.04f);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000B9D8 File Offset: 0x00009DD8
		private void InitSpline()
		{
			this.mSpline.Granularity = this.Granularity;
			this.mSpline.Clear();
			for (int i = 0; i < this.MaxFrame; i++)
			{
				this.mSpline.AddControlPoint(this.CurHeadPos, this.PointStart.position - this.PointEnd.position);
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000BA48 File Offset: 0x00009E48
		private void RefreshSpline()
		{
			for (int i = 0; i < this.model.snapShotList.Count; i++)
			{
				this.mSpline.ControlPoints[i].Position = this.model.snapShotList[i].Pos;
				this.mSpline.ControlPoints[i].Normal = this.model.snapShotList[i].PointEnd - this.model.snapShotList[i].PointStart;
			}
			this.mSpline.RefreshSpline();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000BAF4 File Offset: 0x00009EF4
		private void UpdateVertex()
		{
			VertexPool pool = this.mVertexSegment.Pool;
			for (int i = 0; i < this.Granularity; i++)
			{
				int num = this.mVertexSegment.VertStart + i * 3;
				float num2 = (float)i / (float)this.Granularity;
				float tl = num2 * this.model.fadeT;
				Vector2 zero = Vector2.zero;
				Vector3 vector = this.mSpline.InterpolateByLen(tl);
				Vector3 vector2 = this.mSpline.InterpolateNormalByLen(tl);
				Vector3 vector3 = vector + vector2.normalized * this.mTrailWidth * 0.5f;
				Vector3 vector4 = vector - vector2.normalized * this.mTrailWidth * 0.5f;
				pool.Vertices[num] = vector3;
				pool.Colors[num] = this.MyColor;
				zero.x = 0f;
				zero.y = num2;
				pool.UVs[num] = zero;
				pool.Vertices[num + 1] = vector;
				pool.Colors[num + 1] = this.MyColor;
				zero.x = 0.5f;
				zero.y = num2;
				pool.UVs[num + 1] = zero;
				pool.Vertices[num + 2] = vector4;
				pool.Colors[num + 2] = this.MyColor;
				zero.x = 1f;
				zero.y = num2;
				pool.UVs[num + 2] = zero;
			}
			this.mVertexSegment.Pool.UVChanged = true;
			this.mVertexSegment.Pool.VertChanged = true;
			this.mVertexSegment.Pool.ColorChanged = true;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000BCF0 File Offset: 0x0000A0F0
		private void UpdateIndices()
		{
			VertexPool pool = this.mVertexSegment.Pool;
			for (int i = 0; i < this.Granularity - 1; i++)
			{
				int num = this.mVertexSegment.VertStart + i * 3;
				int num2 = this.mVertexSegment.VertStart + (i + 1) * 3;
				int num3 = this.mVertexSegment.IndexStart + i * 12;
				pool.Indices[num3] = num2;
				pool.Indices[num3 + 1] = num2 + 1;
				pool.Indices[num3 + 2] = num;
				pool.Indices[num3 + 3] = num2 + 1;
				pool.Indices[num3 + 4] = num + 1;
				pool.Indices[num3 + 5] = num;
				pool.Indices[num3 + 6] = num2 + 1;
				pool.Indices[num3 + 7] = num2 + 2;
				pool.Indices[num3 + 8] = num + 1;
				pool.Indices[num3 + 9] = num2 + 2;
				pool.Indices[num3 + 10] = num + 2;
				pool.Indices[num3 + 11] = num + 1;
			}
			pool.IndiceChanged = true;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000BE00 File Offset: 0x0000A200
		private void UpdateHeadElem()
		{
			this.model.snapShotList[0].PointStart = this.PointStart.position;
			this.model.snapShotList[0].PointEnd = this.PointEnd.position;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000BE50 File Offset: 0x0000A250
		private void UpdateFade()
		{
			if (!this.model.fading)
			{
				return;
			}
			this.model.fadeElapsedFrames++;
			float num = (float)this.model.fadeElapsedFrames / (float)this.model.fadeFrames;
			this.model.fadeT = 1f - num;
			if (this.model.fadeT < 0f)
			{
				this.Deactivate();
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000BEC8 File Offset: 0x0000A2C8
		private void RecordCurElem()
		{
			XWeaponTrail.Element element = this.mElemPool.Get();
			element.PointStart = this.PointStart.position;
			element.PointEnd = this.PointEnd.position;
			if (this.model.snapShotList.Count < this.MaxFrame)
			{
				this.model.snapShotList.Insert(1, element);
			}
			else
			{
				this.mElemPool.Release(this.model.snapShotList[this.model.snapShotList.Count - 1]);
				this.model.snapShotList.RemoveAt(this.model.snapShotList.Count - 1);
				this.model.snapShotList.Insert(1, element);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000BF98 File Offset: 0x0000A398
		private void InitOriginalElements()
		{
			this.model.snapShotList.Clear();
			this.model.snapShotList.Add(new XWeaponTrail.Element(this.PointStart.position, this.PointEnd.position));
			this.model.snapShotList.Add(new XWeaponTrail.Element(this.PointStart.position, this.PointEnd.position));
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000C00B File Offset: 0x0000A40B
		private void InitMeshObj()
		{
			this.mVertexPool = new VertexPool(this.MyMaterial, this);
			this.mVertexSegment = this.mVertexPool.GetVertices(this.Granularity * 3, (this.Granularity - 1) * 12);
			this.UpdateIndices();
		}

		// Token: 0x040000F0 RID: 240
		private XWeaponTrailModel model = new XWeaponTrailModel();

		// Token: 0x040000F1 RID: 241
		public static string Version = "1.1.1";

		// Token: 0x040000F2 RID: 242
		public bool UseWith2D;

		// Token: 0x040000F3 RID: 243
		public string SortingLayerName;

		// Token: 0x040000F4 RID: 244
		public int SortingOrder;

		// Token: 0x040000F5 RID: 245
		public IBoneFloatTransform PointStart;

		// Token: 0x040000F6 RID: 246
		public IBoneFloatTransform PointEnd;

		// Token: 0x040000F7 RID: 247
		public int MaxFrame = 14;

		// Token: 0x040000F8 RID: 248
		public int Granularity = 60;

		// Token: 0x040000F9 RID: 249
		public float Fps = 60f;

		// Token: 0x040000FA RID: 250
		public Color MyColor = Color.white;

		// Token: 0x040000FB RID: 251
		public Material MyMaterial;

		// Token: 0x040000FC RID: 252
		protected float mTrailWidth;

		// Token: 0x040000FD RID: 253
		protected XWeaponTrail.Element mHeadElem = new XWeaponTrail.Element();

		// Token: 0x040000FE RID: 254
		protected XWeaponTrail.ElementPool mElemPool;

		// Token: 0x040000FF RID: 255
		protected Spline mSpline = new Spline();

		// Token: 0x04000100 RID: 256
		protected GameObject mMeshObj;

		// Token: 0x04000101 RID: 257
		protected VertexPool mVertexPool;

		// Token: 0x04000102 RID: 258
		protected VertexPool.VertexSegment mVertexSegment;

		// Token: 0x02000020 RID: 32
		public class Element
		{
			// Token: 0x06000136 RID: 310 RVA: 0x0000C055 File Offset: 0x0000A455
			public Element(Vector3 start, Vector3 end)
			{
				this.PointStart = start;
				this.PointEnd = end;
			}

			// Token: 0x06000137 RID: 311 RVA: 0x0000C06B File Offset: 0x0000A46B
			public Element()
			{
			}

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x06000138 RID: 312 RVA: 0x0000C073 File Offset: 0x0000A473
			public Vector3 Pos
			{
				get
				{
					return (this.PointStart + this.PointEnd) / 2f;
				}
			}

			// Token: 0x04000103 RID: 259
			public Vector3 PointStart;

			// Token: 0x04000104 RID: 260
			public Vector3 PointEnd;
		}

		// Token: 0x02000021 RID: 33
		public class ElementPool
		{
			// Token: 0x06000139 RID: 313 RVA: 0x0000C090 File Offset: 0x0000A490
			public ElementPool(int preCount)
			{
				for (int i = 0; i < preCount; i++)
				{
					XWeaponTrail.Element item = new XWeaponTrail.Element();
					this._stack.Push(item);
					this.CountAll++;
				}
			}

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x0600013A RID: 314 RVA: 0x0000C0E0 File Offset: 0x0000A4E0
			// (set) Token: 0x0600013B RID: 315 RVA: 0x0000C0E8 File Offset: 0x0000A4E8
			public int CountAll { get; private set; }

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x0600013C RID: 316 RVA: 0x0000C0F1 File Offset: 0x0000A4F1
			public int CountActive
			{
				get
				{
					return this.CountAll - this.CountInactive;
				}
			}

			// Token: 0x17000013 RID: 19
			// (get) Token: 0x0600013D RID: 317 RVA: 0x0000C100 File Offset: 0x0000A500
			public int CountInactive
			{
				get
				{
					return this._stack.Count;
				}
			}

			// Token: 0x0600013E RID: 318 RVA: 0x0000C110 File Offset: 0x0000A510
			public XWeaponTrail.Element Get()
			{
				XWeaponTrail.Element result;
				if (this._stack.Count == 0)
				{
					result = new XWeaponTrail.Element();
					this.CountAll++;
				}
				else
				{
					result = this._stack.Pop();
				}
				return result;
			}

			// Token: 0x0600013F RID: 319 RVA: 0x0000C153 File Offset: 0x0000A553
			public void Release(XWeaponTrail.Element element)
			{
				if (this._stack.Count > 0 && object.ReferenceEquals(this._stack.Peek(), element))
				{
					Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
				}
				this._stack.Push(element);
			}

			// Token: 0x04000105 RID: 261
			private readonly Stack<XWeaponTrail.Element> _stack = new Stack<XWeaponTrail.Element>();
		}
	}
}
