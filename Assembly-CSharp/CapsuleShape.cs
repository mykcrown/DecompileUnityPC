using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000386 RID: 902
[ExecuteInEditMode]
public class CapsuleShape : MonoBehaviour
{
	// Token: 0x17000386 RID: 902
	// (get) Token: 0x0600134D RID: 4941 RVA: 0x0006F65E File Offset: 0x0006DA5E
	// (set) Token: 0x0600134E RID: 4942 RVA: 0x0006F666 File Offset: 0x0006DA66
	public float Radius
	{
		get
		{
			return this.radius;
		}
		set
		{
			this.radius = value;
		}
	}

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x0600134F RID: 4943 RVA: 0x0006F66F File Offset: 0x0006DA6F
	public Vector3 position1
	{
		get
		{
			return (!(this.transform1 != null)) ? this.defaultPosition1 : this.transform1.position;
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06001350 RID: 4944 RVA: 0x0006F698 File Offset: 0x0006DA98
	public Vector3 position2
	{
		get
		{
			return (!(this.transform2 != null)) ? this.defaultPosition2 : this.transform2.position;
		}
	}

	// Token: 0x06001351 RID: 4945 RVA: 0x0006F6C1 File Offset: 0x0006DAC1
	public void Initialize(GameObject sphere1, GameObject sphere2, GameObject cylinder, Material material)
	{
		this.sphere1 = sphere1;
		this.sphere2 = sphere2;
		this.cylinder = cylinder;
		this.material = material;
	}

	// Token: 0x06001352 RID: 4946 RVA: 0x0006F6E0 File Offset: 0x0006DAE0
	public void SetOwner(ICapsuleOwner owner)
	{
		this.owner = owner;
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06001354 RID: 4948 RVA: 0x0006F76A File Offset: 0x0006DB6A
	// (set) Token: 0x06001353 RID: 4947 RVA: 0x0006F6EC File Offset: 0x0006DAEC
	public bool Visible
	{
		get
		{
			return this.isVisible;
		}
		set
		{
			if (this.sphere1.activeSelf != value)
			{
				this.sphere1.SetActive(value);
			}
			this.isVisible = value;
			bool flag = !this.isCircle && value;
			if (this.sphere2.activeSelf != flag)
			{
				this.sphere2.SetActive(flag);
			}
			if (this.cylinder.activeSelf != flag)
			{
				this.cylinder.SetActive(flag);
			}
		}
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x0006F772 File Offset: 0x0006DB72
	public void Load(Transform transform1, Transform transform2, float radius, Color color, bool isCircle)
	{
		this.Load(transform1, transform2, Vector3.zero, Vector3.zero, radius, color, isCircle);
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x0006F78B File Offset: 0x0006DB8B
	public void Load(Vector3F position1, Vector3F position2, Fixed radius, Color color, bool isCircle)
	{
		this.Load(null, null, (Vector3)position1, (Vector3)position2, (float)radius, color, isCircle);
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x0006F7AB File Offset: 0x0006DBAB
	public void Load(Vector3 position1, Vector3 position2, float radius, Color color, bool isCircle)
	{
		this.Load(null, null, position1, position2, radius, color, isCircle);
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x0006F7BC File Offset: 0x0006DBBC
	private void Load(Transform transform1, Transform transform2, Vector3 position1, Vector3 position2, float radius, Color color, bool isCircle)
	{
		this.transform1 = transform1;
		this.transform2 = transform2;
		this.defaultPosition1 = position1;
		this.defaultPosition2 = position2;
		this.radius = radius;
		this.SetColor(color);
		this.isCircle = isCircle;
		if (isCircle)
		{
			transform2 = transform1;
			this.sphere2.SetActive(false);
			this.cylinder.SetActive(false);
		}
		else
		{
			this.sphere2.SetActive(this.isVisible);
			this.cylinder.SetActive(this.isVisible);
		}
		if (transform1 != null && transform2 != null)
		{
			this.sphere1.transform.SetParent(transform1, false);
			this.cylinder.transform.SetParent(transform1, false);
			this.sphere2.transform.SetParent(transform2, false);
			this.sphere1.transform.localPosition = Vector3.zero;
			this.cylinder.transform.localPosition = Vector3.zero;
			this.sphere2.transform.localPosition = Vector3.zero;
		}
		else
		{
			this.sphere1.transform.SetParent(base.transform, false);
			this.cylinder.transform.SetParent(base.transform, false);
			this.sphere2.transform.SetParent(base.transform, false);
			this.sphere1.transform.position = this.defaultPosition1;
			this.cylinder.transform.position = this.defaultPosition1;
			this.sphere2.transform.position = this.defaultPosition2;
		}
		this.Update();
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x0006F966 File Offset: 0x0006DD66
	public void SetPositions(Vector3F position1, Vector3F position2, bool isCircle)
	{
		this.SetPositions((Vector3)position1, (Vector3)position2, isCircle);
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x0006F97C File Offset: 0x0006DD7C
	public void SetPositions(Vector3 position1, Vector3 position2, bool isCircle)
	{
		this.defaultPosition1 = position1;
		this.defaultPosition2 = position2;
		this.isCircle = isCircle;
		bool flag = this.isVisible && !isCircle;
		if (this.sphere2.activeSelf != flag)
		{
			this.sphere2.SetActive(flag);
		}
		if (this.cylinder.activeSelf != flag)
		{
			this.cylinder.SetActive(flag);
		}
		this.sphere1.transform.position = this.defaultPosition1;
		this.cylinder.transform.position = this.defaultPosition1;
		this.sphere2.transform.position = this.defaultPosition2;
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x0006FA2D File Offset: 0x0006DE2D
	public void SetColor(Color color)
	{
		if (this.material == null)
		{
			return;
		}
		this.material.SetColor("_Color", color);
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x0006FA52 File Offset: 0x0006DE52
	public void Clear()
	{
		if (this.owner != null)
		{
			this.owner.ReturnCapsule(this);
		}
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x0006FA6C File Offset: 0x0006DE6C
	public void EditorDestroy()
	{
		if (this.sphere1 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.sphere1);
		}
		if (this.sphere2 != null)
		{
			UnityEngine.Object.DestroyImmediate(this.sphere2);
		}
		if (this.cylinder != null)
		{
			UnityEngine.Object.DestroyImmediate(this.cylinder);
		}
		if (base.gameObject != null)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x0006FAEC File Offset: 0x0006DEEC
	private void Update()
	{
		Vector3 a = Vector3.one * this.radius * 2f;
		if (this.sphere1.transform.parent.transform.lossyScale.x != 0f)
		{
			this.sphere1.transform.localScale = a / this.sphere1.transform.parent.transform.lossyScale.x;
			this.sphere2.transform.localScale = a / this.sphere2.transform.parent.transform.lossyScale.x;
			Vector3 forward = this.position2 - this.position1;
			if (forward.sqrMagnitude != 0f)
			{
				Quaternion rotation = Quaternion.LookRotation(forward);
				this.cylinder.transform.rotation = rotation;
				this.cylinder.transform.localScale = new Vector3(this.radius * 2f, this.radius * 2f, forward.magnitude / CapsuleShape.CYLINDER_LENGTH) / this.cylinder.transform.parent.transform.lossyScale.x;
			}
		}
	}

	// Token: 0x04000CBF RID: 3263
	private static float CYLINDER_LENGTH = 2f;

	// Token: 0x04000CC0 RID: 3264
	private ICapsuleOwner owner;

	// Token: 0x04000CC1 RID: 3265
	[SerializeField]
	private GameObject sphere1;

	// Token: 0x04000CC2 RID: 3266
	[SerializeField]
	private GameObject sphere2;

	// Token: 0x04000CC3 RID: 3267
	[SerializeField]
	private GameObject cylinder;

	// Token: 0x04000CC4 RID: 3268
	[SerializeField]
	private Material material;

	// Token: 0x04000CC5 RID: 3269
	[SerializeField]
	private float radius = 1f;

	// Token: 0x04000CC6 RID: 3270
	[SerializeField]
	private Transform transform1;

	// Token: 0x04000CC7 RID: 3271
	[SerializeField]
	private Transform transform2;

	// Token: 0x04000CC8 RID: 3272
	private bool isCircle;

	// Token: 0x04000CC9 RID: 3273
	private bool isVisible = true;

	// Token: 0x04000CCA RID: 3274
	private Vector3 defaultPosition1;

	// Token: 0x04000CCB RID: 3275
	private Vector3 defaultPosition2;
}
