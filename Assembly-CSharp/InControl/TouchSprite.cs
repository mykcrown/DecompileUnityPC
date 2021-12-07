using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000134 RID: 308
	[Serializable]
	public class TouchSprite
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x0002E598 File Offset: 0x0002C998
		public TouchSprite()
		{
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0002E608 File Offset: 0x0002CA08
		public TouchSprite(float size)
		{
			this.size = Vector2.one * size;
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0002E686 File Offset: 0x0002CA86
		// (set) Token: 0x060006F0 RID: 1776 RVA: 0x0002E68E File Offset: 0x0002CA8E
		public bool Dirty { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0002E697 File Offset: 0x0002CA97
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0002E69F File Offset: 0x0002CA9F
		public bool Ready { get; set; }

		// Token: 0x060006F3 RID: 1779 RVA: 0x0002E6A8 File Offset: 0x0002CAA8
		public void Create(string gameObjectName, Transform parentTransform, int sortingOrder)
		{
			this.spriteGameObject = this.CreateSpriteGameObject(gameObjectName, parentTransform);
			this.spriteRenderer = this.CreateSpriteRenderer(this.spriteGameObject, this.idleSprite, sortingOrder);
			this.spriteRenderer.color = this.idleColor;
			this.Ready = true;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0002E6F4 File Offset: 0x0002CAF4
		public void Delete()
		{
			this.Ready = false;
			UnityEngine.Object.Destroy(this.spriteGameObject);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0002E708 File Offset: 0x0002CB08
		public void Update()
		{
			this.Update(false);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0002E714 File Offset: 0x0002CB14
		public void Update(bool forceUpdate)
		{
			if (this.Dirty || forceUpdate)
			{
				if (this.spriteRenderer != null)
				{
					this.spriteRenderer.sprite = ((!this.State) ? this.idleSprite : this.busySprite);
				}
				if (this.sizeUnitType == TouchUnitType.Pixels)
				{
					Vector2 a = TouchUtility.RoundVector(this.size);
					this.ScaleSpriteInPixels(this.spriteGameObject, this.spriteRenderer, a);
					this.worldSize = a * TouchManager.PixelToWorld;
				}
				else
				{
					this.ScaleSpriteInPercent(this.spriteGameObject, this.spriteRenderer, this.size);
					if (this.lockAspectRatio)
					{
						this.worldSize = this.size * TouchManager.PercentToWorld;
					}
					else
					{
						this.worldSize = Vector2.Scale(this.size, TouchManager.ViewSize);
					}
				}
				this.Dirty = false;
			}
			if (this.spriteRenderer != null)
			{
				Color color = (!this.State) ? this.idleColor : this.busyColor;
				if (this.spriteRenderer.color != color)
				{
					this.spriteRenderer.color = Utility.MoveColorTowards(this.spriteRenderer.color, color, 5f * Time.deltaTime);
				}
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0002E878 File Offset: 0x0002CC78
		private GameObject CreateSpriteGameObject(string name, Transform parentTransform)
		{
			return new GameObject(name)
			{
				transform = 
				{
					parent = parentTransform,
					localPosition = Vector3.zero,
					localScale = Vector3.one
				},
				layer = parentTransform.gameObject.layer
			};
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002E8CC File Offset: 0x0002CCCC
		private SpriteRenderer CreateSpriteRenderer(GameObject spriteGameObject, Sprite sprite, int sortingOrder)
		{
			SpriteRenderer spriteRenderer = spriteGameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = sprite;
			spriteRenderer.sortingOrder = sortingOrder;
			spriteRenderer.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
			spriteRenderer.sharedMaterial.SetFloat("PixelSnap", 1f);
			return spriteRenderer;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002E91C File Offset: 0x0002CD1C
		private void ScaleSpriteInPixels(GameObject spriteGameObject, SpriteRenderer spriteRenderer, Vector2 size)
		{
			if (spriteGameObject == null || spriteRenderer == null || spriteRenderer.sprite == null)
			{
				return;
			}
			float num = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.bounds.size.x;
			float num2 = TouchManager.PixelToWorld * num;
			float x = num2 * size.x / spriteRenderer.sprite.rect.width;
			float y = num2 * size.y / spriteRenderer.sprite.rect.height;
			spriteGameObject.transform.localScale = new Vector3(x, y);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002E9E4 File Offset: 0x0002CDE4
		private void ScaleSpriteInPercent(GameObject spriteGameObject, SpriteRenderer spriteRenderer, Vector2 size)
		{
			if (spriteGameObject == null || spriteRenderer == null || spriteRenderer.sprite == null)
			{
				return;
			}
			if (this.lockAspectRatio)
			{
				float num = Mathf.Min(TouchManager.ViewSize.x, TouchManager.ViewSize.y);
				float x = num * size.x / spriteRenderer.sprite.bounds.size.x;
				float y = num * size.y / spriteRenderer.sprite.bounds.size.y;
				spriteGameObject.transform.localScale = new Vector3(x, y);
			}
			else
			{
				float x2 = TouchManager.ViewSize.x * size.x / spriteRenderer.sprite.bounds.size.x;
				float y2 = TouchManager.ViewSize.y * size.y / spriteRenderer.sprite.bounds.size.y;
				spriteGameObject.transform.localScale = new Vector3(x2, y2);
			}
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002EB30 File Offset: 0x0002CF30
		public bool Contains(Vector2 testWorldPoint)
		{
			if (this.shape == TouchSpriteShape.Oval)
			{
				float num = (testWorldPoint.x - this.Position.x) / this.worldSize.x;
				float num2 = (testWorldPoint.y - this.Position.y) / this.worldSize.y;
				return num * num + num2 * num2 < 0.25f;
			}
			float num3 = Utility.Abs(testWorldPoint.x - this.Position.x) * 2f;
			float num4 = Utility.Abs(testWorldPoint.y - this.Position.y) * 2f;
			return num3 <= this.worldSize.x && num4 <= this.worldSize.y;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0002EC0C File Offset: 0x0002D00C
		public bool Contains(Touch touch)
		{
			return this.Contains(TouchManager.ScreenToWorldPoint(touch.position));
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002EC24 File Offset: 0x0002D024
		public void DrawGizmos(Vector3 position, Color color)
		{
			if (this.shape == TouchSpriteShape.Oval)
			{
				Utility.DrawOvalGizmo(position, this.WorldSize, color);
			}
			else
			{
				Utility.DrawRectGizmo(position, this.WorldSize, color);
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0002EC5A File Offset: 0x0002D05A
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x0002EC62 File Offset: 0x0002D062
		public bool State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (this.state != value)
				{
					this.state = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0002EC7E File Offset: 0x0002D07E
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x0002EC86 File Offset: 0x0002D086
		public Sprite BusySprite
		{
			get
			{
				return this.busySprite;
			}
			set
			{
				if (this.busySprite != value)
				{
					this.busySprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0002ECA7 File Offset: 0x0002D0A7
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x0002ECAF File Offset: 0x0002D0AF
		public Sprite IdleSprite
		{
			get
			{
				return this.idleSprite;
			}
			set
			{
				if (this.idleSprite != value)
				{
					this.idleSprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000148 RID: 328
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x0002ECD0 File Offset: 0x0002D0D0
		public Sprite Sprite
		{
			set
			{
				if (this.idleSprite != value)
				{
					this.idleSprite = value;
					this.Dirty = true;
				}
				if (this.busySprite != value)
				{
					this.busySprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0002ED10 File Offset: 0x0002D110
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x0002ED18 File Offset: 0x0002D118
		public Color BusyColor
		{
			get
			{
				return this.busyColor;
			}
			set
			{
				if (this.busyColor != value)
				{
					this.busyColor = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x0002ED39 File Offset: 0x0002D139
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x0002ED41 File Offset: 0x0002D141
		public Color IdleColor
		{
			get
			{
				return this.idleColor;
			}
			set
			{
				if (this.idleColor != value)
				{
					this.idleColor = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0002ED62 File Offset: 0x0002D162
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x0002ED6A File Offset: 0x0002D16A
		public TouchSpriteShape Shape
		{
			get
			{
				return this.shape;
			}
			set
			{
				if (this.shape != value)
				{
					this.shape = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0002ED86 File Offset: 0x0002D186
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x0002ED8E File Offset: 0x0002D18E
		public TouchUnitType SizeUnitType
		{
			get
			{
				return this.sizeUnitType;
			}
			set
			{
				if (this.sizeUnitType != value)
				{
					this.sizeUnitType = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x0002EDAA File Offset: 0x0002D1AA
		// (set) Token: 0x0600070E RID: 1806 RVA: 0x0002EDB2 File Offset: 0x0002D1B2
		public Vector2 Size
		{
			get
			{
				return this.size;
			}
			set
			{
				if (this.size != value)
				{
					this.size = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0002EDD3 File Offset: 0x0002D1D3
		public Vector2 WorldSize
		{
			get
			{
				return this.worldSize;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x0002EDDB File Offset: 0x0002D1DB
		// (set) Token: 0x06000711 RID: 1809 RVA: 0x0002EE07 File Offset: 0x0002D207
		public Vector3 Position
		{
			get
			{
				return (!this.spriteGameObject) ? Vector3.zero : this.spriteGameObject.transform.position;
			}
			set
			{
				if (this.spriteGameObject)
				{
					this.spriteGameObject.transform.position = value;
				}
			}
		}

		// Token: 0x04000562 RID: 1378
		[SerializeField]
		private Sprite idleSprite;

		// Token: 0x04000563 RID: 1379
		[SerializeField]
		private Sprite busySprite;

		// Token: 0x04000564 RID: 1380
		[SerializeField]
		private Color idleColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x04000565 RID: 1381
		[SerializeField]
		private Color busyColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04000566 RID: 1382
		[SerializeField]
		private TouchSpriteShape shape;

		// Token: 0x04000567 RID: 1383
		[SerializeField]
		private TouchUnitType sizeUnitType;

		// Token: 0x04000568 RID: 1384
		[SerializeField]
		private Vector2 size = new Vector2(10f, 10f);

		// Token: 0x04000569 RID: 1385
		[SerializeField]
		private bool lockAspectRatio = true;

		// Token: 0x0400056A RID: 1386
		[SerializeField]
		[HideInInspector]
		private Vector2 worldSize;

		// Token: 0x0400056B RID: 1387
		private Transform spriteParentTransform;

		// Token: 0x0400056C RID: 1388
		private GameObject spriteGameObject;

		// Token: 0x0400056D RID: 1389
		private SpriteRenderer spriteRenderer;

		// Token: 0x0400056E RID: 1390
		private bool state;
	}
}
