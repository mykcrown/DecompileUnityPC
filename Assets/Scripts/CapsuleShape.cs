// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class CapsuleShape : MonoBehaviour
{
	private static float CYLINDER_LENGTH = 2f;

	private ICapsuleOwner owner;

	[SerializeField]
	private GameObject sphere1;

	[SerializeField]
	private GameObject sphere2;

	[SerializeField]
	private GameObject cylinder;

	[SerializeField]
	private Material material;

	[SerializeField]
	private float radius = 1f;

	[SerializeField]
	private Transform transform1;

	[SerializeField]
	private Transform transform2;

	private bool isCircle;

	private bool isVisible = true;

	private Vector3 defaultPosition1;

	private Vector3 defaultPosition2;

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

	public Vector3 position1
	{
		get
		{
			return (!(this.transform1 != null)) ? this.defaultPosition1 : this.transform1.position;
		}
	}

	public Vector3 position2
	{
		get
		{
			return (!(this.transform2 != null)) ? this.defaultPosition2 : this.transform2.position;
		}
	}

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

	public void Initialize(GameObject sphere1, GameObject sphere2, GameObject cylinder, Material material)
	{
		this.sphere1 = sphere1;
		this.sphere2 = sphere2;
		this.cylinder = cylinder;
		this.material = material;
	}

	public void SetOwner(ICapsuleOwner owner)
	{
		this.owner = owner;
	}

	public void Load(Transform transform1, Transform transform2, float radius, Color color, bool isCircle)
	{
		this.Load(transform1, transform2, Vector3.zero, Vector3.zero, radius, color, isCircle);
	}

	public void Load(Vector3F position1, Vector3F position2, Fixed radius, Color color, bool isCircle)
	{
		this.Load(null, null, (Vector3)position1, (Vector3)position2, (float)radius, color, isCircle);
	}

	public void Load(Vector3 position1, Vector3 position2, float radius, Color color, bool isCircle)
	{
		this.Load(null, null, position1, position2, radius, color, isCircle);
	}

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

	public void SetPositions(Vector3F position1, Vector3F position2, bool isCircle)
	{
		this.SetPositions((Vector3)position1, (Vector3)position2, isCircle);
	}

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

	public void SetColor(Color color)
	{
		if (this.material == null)
		{
			return;
		}
		this.material.SetColor("_Color", color);
	}

	public void Clear()
	{
		if (this.owner != null)
		{
			this.owner.ReturnCapsule(this);
		}
	}

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
}
