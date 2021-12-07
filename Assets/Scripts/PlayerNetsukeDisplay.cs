// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerNetsukeDisplay : BaseItem3DPreviewDisplay
{
	private class NetsukeDisplay
	{
		public int index;

		public GameObject gameObj;

		public Transform rotateContainer;

		public Netsuke data;

		public List<Material> fadeMaterials;
	}

	private float yRotate;

	public Transform RotateContainer;

	public GameObject Container1;

	public GameObject Container2;

	public GameObject Container3;

	private Dictionary<int, PlayerNetsukeDisplay.NetsukeDisplay> displays = new Dictionary<int, PlayerNetsukeDisplay.NetsukeDisplay>();

	private List<Material> fadeMaterials = new List<Material>();

	private Dictionary<Material, float> originalShaderMode = new Dictionary<Material, float>();

	private float _alpha = 1f;

	private Vector3 _scale = new Vector3(1f, 1f, 1f);

	private GameObject[] containers;

	private float targetRotate;

	private float _rotationOffset;

	private int spinIndex;

	private const float spinSpeed = 5f;

	public float rotationOffset
	{
		get
		{
			return this._rotationOffset;
		}
		set
		{
			if (this._rotationOffset != value)
			{
				this._rotationOffset = value;
				base.transform.localRotation = Quaternion.Euler(0f, this._rotationOffset, 0f);
			}
		}
	}

	public float Alpha
	{
		get
		{
			return this._alpha;
		}
		set
		{
			if (this._alpha != value)
			{
				this._alpha = value;
				this.updateAlpha(value);
			}
		}
	}

	public Vector3 Scale
	{
		get
		{
			return this._scale;
		}
		set
		{
			if (this._scale != value)
			{
				this._scale = value;
				this.RotateContainer.localScale = this._scale;
			}
		}
	}

	private bool isMultiItemMode
	{
		get
		{
			return this.Container3 != null;
		}
	}

	private void Awake()
	{
		this.initMap();
		this.recursiveSetMaterials(this.RotateContainer.transform, this.fadeMaterials, 3000);
	}

	public void SpinDirection(int dir)
	{
		this.spinIndex += dir;
		this.targetRotate = (float)(this.spinIndex * 120);
	}

	private void recursiveSetMaterials(Transform theParent, List<Material> list, int renderQueue)
	{
		Renderer component = theParent.GetComponent<Renderer>();
		if (component != null)
		{
			Material material = component.material;
			Material material2 = UnityEngine.Object.Instantiate<Material>(material);
			material2.SetInt("__RENDER_QUEUE_CUSTOM", renderQueue);
			this.setMaterialAlpha(material2, this._alpha);
			component.material = material2;
			list.Add(material2);
		}
		for (int i = 0; i < theParent.childCount; i++)
		{
			this.recursiveSetMaterials(theParent.GetChild(i), list, renderQueue + 1);
		}
	}

	private void removeShadows(Transform transform)
	{
		Renderer component = transform.GetComponent<Renderer>();
		if (component != null)
		{
			component.shadowCastingMode = ShadowCastingMode.Off;
		}
		for (int i = 0; i < transform.childCount; i++)
		{
			this.removeShadows(transform.GetChild(i));
		}
	}

	private void initMap()
	{
		if (this.containers == null)
		{
			this.containers = new GameObject[]
			{
				this.Container1,
				this.Container2,
				this.Container3
			};
			GameObject[] array = this.containers;
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = array[i];
				if (gameObject)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	public void AddNetsuke(Netsuke netsuke, int index)
	{
		this.AddManual((!(netsuke == null)) ? netsuke.gameObject : null, index, netsuke);
	}

	public void AddManual(GameObject prefab, int index, Netsuke netsuke = null)
	{
		this.initMap();
		GameObject gameObject = this.containers[index];
		PlayerNetsukeDisplay.NetsukeDisplay netsukeDisplay;
		this.displays.TryGetValue(index, out netsukeDisplay);
		if (netsukeDisplay == null || netsukeDisplay.data != netsuke)
		{
			if (netsukeDisplay != null && netsukeDisplay.gameObj != null)
			{
				netsukeDisplay.gameObj.transform.SetParent(null);
				UnityEngine.Object.DestroyImmediate(netsukeDisplay.gameObj);
				netsukeDisplay.rotateContainer.SetParent(null);
				UnityEngine.Object.DestroyImmediate(netsukeDisplay.rotateContainer.gameObject);
			}
			if (prefab != null)
			{
				GameObject gameObject2 = new GameObject("Obj Rotate");
				gameObject2.transform.SetParent(gameObject.transform, false);
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(prefab);
				gameObject3.transform.SetParent(gameObject2.transform, false);
				List<Material> list = new List<Material>();
				this.recursiveSetMaterials(gameObject3.transform, list, 6000 - index * 1000);
				if (this.isMultiItemMode)
				{
					this.objectLookAtCamera(gameObject2.transform);
				}
				PlayerNetsukeDisplay.NetsukeDisplay netsukeDisplay2 = new PlayerNetsukeDisplay.NetsukeDisplay();
				netsukeDisplay2.gameObj = gameObject3;
				netsukeDisplay2.rotateContainer = gameObject2.transform;
				netsukeDisplay2.data = netsuke;
				netsukeDisplay2.index = index;
				netsukeDisplay2.fadeMaterials = list;
				this.displays[index] = netsukeDisplay2;
				gameObject.SetActive(true);
			}
			else
			{
				this.displays[index] = null;
				gameObject.SetActive(false);
			}
		}
	}

	public override void Attach(Transform attachTo, Camera usingCamera)
	{
		base.Attach(attachTo, usingCamera);
		this.updateRotation();
	}

	private void updateAlpha(float alpha)
	{
		foreach (Material current in this.fadeMaterials)
		{
			this.setMaterialAlpha(current, alpha);
		}
		foreach (KeyValuePair<int, PlayerNetsukeDisplay.NetsukeDisplay> current2 in this.displays)
		{
			if (current2.Value != null)
			{
				foreach (Material current3 in current2.Value.fadeMaterials)
				{
					this.setMaterialAlpha(current3, alpha);
				}
			}
		}
	}

	private void setMaterialAlpha(Material material, float alpha)
	{
		if (!this.originalShaderMode.ContainsKey(material))
		{
			float value = 0f;
			if (material.HasProperty("_Mode"))
			{
				value = material.GetFloat("_Mode");
			}
			this.originalShaderMode[material] = value;
		}
		if (alpha >= 0.98f)
		{
			if (this.originalShaderMode[material] == 0f)
			{
				material.SetFloat("_Mode", 0f);
				material.SetInt("_SrcBlend", 1);
				material.SetInt("_DstBlend", 0);
				material.SetInt("_ZWrite", 1);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = -1;
			}
			if (material.HasProperty("_Color"))
			{
				Color color = material.color;
				color.a = 1f;
				material.SetColor("_Color", color);
			}
		}
		else
		{
			Color value2 = default(Color);
			if (material.HasProperty("_Color"))
			{
				value2 = material.color;
				value2.a = alpha;
			}
			material.SetFloat("_Mode", 2f);
			material.SetInt("_SrcBlend", 5);
			material.SetInt("_DstBlend", 10);
			material.SetInt("_ZWrite", 1);
			material.DisableKeyword("_ALPHATEST_ON");
			material.EnableKeyword("_ALPHABLEND_ON");
			material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			material.renderQueue = material.GetInt("__RENDER_QUEUE_CUSTOM");
			if (material.HasProperty("_Color"))
			{
				material.SetColor("_Color", value2);
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		this.updateRotation();
	}

	private void updateRotation()
	{
		this.lookAtCamera();
		this.rotationOffset = Mathf.Lerp(this.rotationOffset, this.targetRotate, Time.deltaTime * 5f);
	}

	private void lookAtCamera()
	{
		Quaternion localRotation = base.transform.localRotation;
		Vector3 up = base.transform.up;
		base.transform.LookAt(2f * Camera.main.transform.position - base.transform.position, up);
		this.yRotate = base.transform.localRotation.eulerAngles.y;
		base.transform.localRotation = localRotation;
		Vector3 eulerAngles = this.RotateContainer.localRotation.eulerAngles;
		eulerAngles.y = this.yRotate;
		this.RotateContainer.localRotation = Quaternion.Euler(eulerAngles);
		if (this.isMultiItemMode)
		{
			foreach (KeyValuePair<int, PlayerNetsukeDisplay.NetsukeDisplay> current in this.displays)
			{
				if (current.Value != null)
				{
					this.objectLookAtCamera(current.Value.rotateContainer);
				}
			}
		}
	}

	private void objectLookAtCamera(Transform transform)
	{
		Quaternion localRotation = transform.localRotation;
		Vector3 up = transform.up;
		transform.LookAt(2f * Camera.main.transform.position - transform.position, up);
		this.yRotate = transform.localRotation.eulerAngles.y;
		transform.localRotation = localRotation;
		Vector3 eulerAngles = transform.localRotation.eulerAngles;
		eulerAngles.y = this.yRotate;
		transform.localRotation = Quaternion.Euler(eulerAngles);
	}
}
