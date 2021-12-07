// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Xft
{
	public class XWeaponTrail : MonoBehaviour, ITickable, IExpirable, IRollbackStateOwner, IPooledGameObjectListener
	{
		public class Element
		{
			public Vector3 PointStart;

			public Vector3 PointEnd;

			public Vector3 Pos
			{
				get
				{
					return (this.PointStart + this.PointEnd) / 2f;
				}
			}

			public Element(Vector3 start, Vector3 end)
			{
				this.PointStart = start;
				this.PointEnd = end;
			}

			public Element()
			{
			}
		}

		public class ElementPool
		{
			private readonly Stack<XWeaponTrail.Element> _stack = new Stack<XWeaponTrail.Element>();

			public int CountAll
			{
				get;
				private set;
			}

			public int CountActive
			{
				get
				{
					return this.CountAll - this.CountInactive;
				}
			}

			public int CountInactive
			{
				get
				{
					return this._stack.Count;
				}
			}

			public ElementPool(int preCount)
			{
				for (int i = 0; i < preCount; i++)
				{
					XWeaponTrail.Element item = new XWeaponTrail.Element();
					this._stack.Push(item);
					this.CountAll++;
				}
			}

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

			public void Release(XWeaponTrail.Element element)
			{
				if (this._stack.Count > 0 && object.ReferenceEquals(this._stack.Peek(), element))
				{
					UnityEngine.Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
				}
				this._stack.Push(element);
			}
		}

		private XWeaponTrailModel model = new XWeaponTrailModel();

		public static string Version = "1.1.1";

		public bool UseWith2D;

		public string SortingLayerName;

		public int SortingOrder;

		public IBoneFloatTransform PointStart;

		public IBoneFloatTransform PointEnd;

		public int MaxFrame = 14;

		public int Granularity = 60;

		public float Fps = 60f;

		public Color MyColor = Color.white;

		public Material MyMaterial;

		protected float mTrailWidth;

		protected XWeaponTrail.Element mHeadElem = new XWeaponTrail.Element();

		protected XWeaponTrail.ElementPool mElemPool;

		protected Spline mSpline = new Spline();

		protected GameObject mMeshObj;

		protected VertexPool mVertexPool;

		protected VertexPool.VertexSegment mVertexSegment;

		[Inject]
		public IRollbackStatePooling rollbackStatePooling
		{
			get;
			set;
		}

		public Vector3 CurHeadPos
		{
			get
			{
				return (this.PointStart.position + this.PointEnd.position) / 2f;
			}
		}

		public float TrailWidth
		{
			get
			{
				return this.mTrailWidth;
			}
		}

		public virtual bool IsExpired
		{
			get
			{
				return this.model.isExpired;
			}
		}

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

		public void Deactivate()
		{
			if (base.gameObject != null && !base.gameObject.Equals(null))
			{
				base.gameObject.DestroySafe();
			}
			this.model.fading = false;
		}

		public void StopSmoothly(int fadeFrames)
		{
			this.model.fading = true;
			this.model.fadeFrames = fadeFrames;
		}

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

		public bool ExportState(ref RollbackStateContainer container)
		{
			return container.WriteState(this.rollbackStatePooling.Clone<XWeaponTrailModel>(this.model));
		}

		public bool LoadState(RollbackStateContainer container)
		{
			container.ReadState<XWeaponTrailModel>(ref this.model);
			return true;
		}

		void IPooledGameObjectListener.OnAcquired()
		{
			base.gameObject.SetActive(true);
		}

		void IPooledGameObjectListener.OnReleased()
		{
			base.gameObject.SetActive(false);
			this.model.isExpired = true;
			this.model.fading = false;
		}

		void IPooledGameObjectListener.OnCooledOff()
		{
		}

		private void LateUpdate()
		{
			if (!this.model.inited || this.model.isExpired)
			{
				return;
			}
			this.mVertexPool.LateUpdate();
		}

		private void onSceneWasLoaded(Scene scene, LoadSceneMode mode)
		{
			this.model.inited = false;
		}

		private void OnDestroy()
		{
			if (this.mVertexPool != null)
			{
				this.mVertexPool.Destroy();
			}
			SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.onSceneWasLoaded);
		}

		private void Awake()
		{
			if (!GameAssetPreloader.InProgress && GameController.IsMatchRunning && SystemBoot.mode != SystemBoot.Mode.StagePreview)
			{
				UnityEngine.Debug.LogError("Instantiated weapon trail outside of preload time. Please add me to GameAssetPreloader! " + base.name);
			}
		}

		private void Start()
		{
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.onSceneWasLoaded);
		}

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

		private void InitSpline()
		{
			this.mSpline.Granularity = this.Granularity;
			this.mSpline.Clear();
			for (int i = 0; i < this.MaxFrame; i++)
			{
				this.mSpline.AddControlPoint(this.CurHeadPos, this.PointStart.position - this.PointEnd.position);
			}
		}

		private void RefreshSpline()
		{
			for (int i = 0; i < this.model.snapShotList.Count; i++)
			{
				this.mSpline.ControlPoints[i].Position = this.model.snapShotList[i].Pos;
				this.mSpline.ControlPoints[i].Normal = this.model.snapShotList[i].PointEnd - this.model.snapShotList[i].PointStart;
			}
			this.mSpline.RefreshSpline();
		}

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

		private void UpdateHeadElem()
		{
			this.model.snapShotList[0].PointStart = this.PointStart.position;
			this.model.snapShotList[0].PointEnd = this.PointEnd.position;
		}

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

		private void InitOriginalElements()
		{
			this.model.snapShotList.Clear();
			this.model.snapShotList.Add(new XWeaponTrail.Element(this.PointStart.position, this.PointEnd.position));
			this.model.snapShotList.Add(new XWeaponTrail.Element(this.PointStart.position, this.PointEnd.position));
		}

		private void InitMeshObj()
		{
			this.mVertexPool = new VertexPool(this.MyMaterial, this);
			this.mVertexSegment = this.mVertexPool.GetVertices(this.Granularity * 3, (this.Granularity - 1) * 12);
			this.UpdateIndices();
		}
	}
}
