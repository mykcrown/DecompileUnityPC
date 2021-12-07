using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Xft
{
	// Token: 0x0200001C RID: 28
	public class VertexPool
	{
		// Token: 0x06000108 RID: 264 RVA: 0x0000A404 File Offset: 0x00008804
		public VertexPool(Material material, XWeaponTrail owner)
		{
			this.VertexTotal = (this.VertexUsed = 0);
			this.VertCountChanged = false;
			this._owner = owner;
			if (owner.UseWith2D)
			{
				this.CreateMeshObj(owner, material);
			}
			else
			{
				this._mesh2d = new Mesh();
			}
			this._material = material;
			this.InitArrays();
			this.IndiceChanged = (this.ColorChanged = (this.UVChanged = (this.UV2Changed = (this.VertChanged = true))));
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000109 RID: 265 RVA: 0x0000A4A4 File Offset: 0x000088A4
		public Mesh MyMesh
		{
			get
			{
				if (!this._owner.UseWith2D)
				{
					return this._mesh2d;
				}
				if (this._meshFilter == null || this._meshFilter.gameObject == null)
				{
					return null;
				}
				return this._meshFilter.sharedMesh;
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000A4FC File Offset: 0x000088FC
		public void RecalculateBounds()
		{
			this.MyMesh.RecalculateBounds();
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000A50C File Offset: 0x0000890C
		private void CreateMeshObj(XWeaponTrail owner, Material material)
		{
			GameObject gameObject = new GameObject("_XWeaponTrailMesh:|material:" + material.name);
			gameObject.layer = owner.gameObject.layer;
			gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			this._meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
			MeshRenderer meshRenderer = (MeshRenderer)gameObject.GetComponent(typeof(MeshRenderer));
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshRenderer.GetComponent<Renderer>().sharedMaterial = material;
			meshRenderer.sortingLayerName = this._owner.SortingLayerName;
			meshRenderer.sortingOrder = this._owner.SortingOrder;
			this._meshFilter.sharedMesh = new Mesh();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000A5EB File Offset: 0x000089EB
		public void Destroy()
		{
			if (!this._owner.UseWith2D)
			{
				UnityEngine.Object.DestroyImmediate(this._mesh2d);
			}
			else
			{
				UnityEngine.Object.Destroy(this._meshFilter.gameObject);
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x0000A620 File Offset: 0x00008A20
		public VertexPool.VertexSegment GetVertices(int vcount, int icount)
		{
			int num = 0;
			int num2 = 0;
			if (this.VertexUsed + vcount >= this.VertexTotal)
			{
				num = (vcount / 108 + 1) * 108;
			}
			if (this.IndexUsed + icount >= this.IndexTotal)
			{
				num2 = (icount / 108 + 1) * 108;
			}
			this.VertexUsed += vcount;
			this.IndexUsed += icount;
			if (num != 0 || num2 != 0)
			{
				this.EnlargeArrays(num, num2);
				this.VertexTotal += num;
				this.IndexTotal += num2;
			}
			return new VertexPool.VertexSegment(this.VertexUsed - vcount, vcount, this.IndexUsed - icount, icount, this);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000A6D1 File Offset: 0x00008AD1
		protected void InitArrays()
		{
			this.Vertices = new Vector3[4];
			this.UVs = new Vector2[4];
			this.Colors = new Color[4];
			this.Indices = new int[6];
			this.VertexTotal = 4;
			this.IndexTotal = 6;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000A714 File Offset: 0x00008B14
		public void EnlargeArrays(int count, int icount)
		{
			Vector3[] vertices = this.Vertices;
			this.Vertices = new Vector3[this.Vertices.Length + count];
			vertices.CopyTo(this.Vertices, 0);
			Vector2[] uvs = this.UVs;
			this.UVs = new Vector2[this.UVs.Length + count];
			uvs.CopyTo(this.UVs, 0);
			Color[] colors = this.Colors;
			this.Colors = new Color[this.Colors.Length + count];
			colors.CopyTo(this.Colors, 0);
			int[] indices = this.Indices;
			this.Indices = new int[this.Indices.Length + icount];
			indices.CopyTo(this.Indices, 0);
			this.VertCountChanged = true;
			this.IndiceChanged = true;
			this.ColorChanged = true;
			this.UVChanged = true;
			this.VertChanged = true;
			this.UV2Changed = true;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000A7F0 File Offset: 0x00008BF0
		public void LateUpdate()
		{
			if (this.VertCountChanged)
			{
				this.MyMesh.Clear();
			}
			this.MyMesh.vertices = this.Vertices;
			if (this.UVChanged)
			{
				this.MyMesh.uv = this.UVs;
			}
			if (this.ColorChanged)
			{
				this.MyMesh.colors = this.Colors;
			}
			if (this.IndiceChanged)
			{
				this.MyMesh.triangles = this.Indices;
			}
			this.ElapsedTime += Time.deltaTime;
			if (this.ElapsedTime > this.BoundsScheduleTime || this.FirstUpdate)
			{
				this.RecalculateBounds();
				this.ElapsedTime = 0f;
			}
			if (this.ElapsedTime > this.BoundsScheduleTime)
			{
				this.FirstUpdate = false;
			}
			this.VertCountChanged = false;
			this.IndiceChanged = false;
			this.ColorChanged = false;
			this.UVChanged = false;
			this.UV2Changed = false;
			this.VertChanged = false;
			if (!this._owner.UseWith2D)
			{
				Graphics.DrawMesh(this.MyMesh, Matrix4x4.identity, this._material, this._owner.gameObject.layer, null, 0, null, false, false);
			}
		}

		// Token: 0x040000CD RID: 205
		public Vector3[] Vertices;

		// Token: 0x040000CE RID: 206
		public int[] Indices;

		// Token: 0x040000CF RID: 207
		public Vector2[] UVs;

		// Token: 0x040000D0 RID: 208
		public Color[] Colors;

		// Token: 0x040000D1 RID: 209
		public bool IndiceChanged;

		// Token: 0x040000D2 RID: 210
		public bool ColorChanged;

		// Token: 0x040000D3 RID: 211
		public bool UVChanged;

		// Token: 0x040000D4 RID: 212
		public bool VertChanged;

		// Token: 0x040000D5 RID: 213
		public bool UV2Changed;

		// Token: 0x040000D6 RID: 214
		protected int VertexTotal;

		// Token: 0x040000D7 RID: 215
		protected int VertexUsed;

		// Token: 0x040000D8 RID: 216
		protected int IndexTotal;

		// Token: 0x040000D9 RID: 217
		protected int IndexUsed;

		// Token: 0x040000DA RID: 218
		public bool FirstUpdate = true;

		// Token: 0x040000DB RID: 219
		protected bool VertCountChanged;

		// Token: 0x040000DC RID: 220
		public const int BlockSize = 108;

		// Token: 0x040000DD RID: 221
		public float BoundsScheduleTime = 1f;

		// Token: 0x040000DE RID: 222
		public float ElapsedTime;

		// Token: 0x040000DF RID: 223
		protected XWeaponTrail _owner;

		// Token: 0x040000E0 RID: 224
		protected MeshFilter _meshFilter;

		// Token: 0x040000E1 RID: 225
		protected Mesh _mesh2d;

		// Token: 0x040000E2 RID: 226
		protected Material _material;

		// Token: 0x0200001D RID: 29
		public class VertexSegment
		{
			// Token: 0x06000111 RID: 273 RVA: 0x0000A939 File Offset: 0x00008D39
			public VertexSegment(int start, int count, int istart, int icount, VertexPool pool)
			{
				this.VertStart = start;
				this.VertCount = count;
				this.IndexCount = icount;
				this.IndexStart = istart;
				this.Pool = pool;
			}

			// Token: 0x06000112 RID: 274 RVA: 0x0000A968 File Offset: 0x00008D68
			public void ClearIndices()
			{
				for (int i = this.IndexStart; i < this.IndexStart + this.IndexCount; i++)
				{
					this.Pool.Indices[i] = 0;
				}
				this.Pool.IndiceChanged = true;
			}

			// Token: 0x040000E3 RID: 227
			public int VertStart;

			// Token: 0x040000E4 RID: 228
			public int IndexStart;

			// Token: 0x040000E5 RID: 229
			public int VertCount;

			// Token: 0x040000E6 RID: 230
			public int IndexCount;

			// Token: 0x040000E7 RID: 231
			public VertexPool Pool;
		}
	}
}
