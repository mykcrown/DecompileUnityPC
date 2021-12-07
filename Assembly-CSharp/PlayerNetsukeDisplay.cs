using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000765 RID: 1893
public class PlayerNetsukeDisplay : BaseItem3DPreviewDisplay
{
	// Token: 0x06002EC8 RID: 11976 RVA: 0x000EBF89 File Offset: 0x000EA389
	private void Awake()
	{
		this.initMap();
		this.recursiveSetMaterials(this.RotateContainer.transform, this.fadeMaterials, 3000);
	}

	// Token: 0x06002EC9 RID: 11977 RVA: 0x000EBFAD File Offset: 0x000EA3AD
	public void SpinDirection(int dir)
	{
		this.spinIndex += dir;
		this.targetRotate = (float)(this.spinIndex * 120);
	}

	// Token: 0x17000B5B RID: 2907
	// (get) Token: 0x06002ECA RID: 11978 RVA: 0x000EBFCD File Offset: 0x000EA3CD
	// (set) Token: 0x06002ECB RID: 11979 RVA: 0x000EBFD5 File Offset: 0x000EA3D5
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

	// Token: 0x06002ECC RID: 11980 RVA: 0x000EC00C File Offset: 0x000EA40C
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

	// Token: 0x06002ECD RID: 11981 RVA: 0x000EC08C File Offset: 0x000EA48C
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

	// Token: 0x06002ECE RID: 11982 RVA: 0x000EC0D8 File Offset: 0x000EA4D8
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
			foreach (GameObject gameObject in this.containers)
			{
				if (gameObject)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x000EC148 File Offset: 0x000EA548
	public void AddNetsuke(Netsuke netsuke, int index)
	{
		this.AddManual((!(netsuke == null)) ? netsuke.gameObject : null, index, netsuke);
	}

	// Token: 0x06002ED0 RID: 11984 RVA: 0x000EC16C File Offset: 0x000EA56C
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

	// Token: 0x06002ED1 RID: 11985 RVA: 0x000EC2DB File Offset: 0x000EA6DB
	public override void Attach(Transform attachTo, Camera usingCamera)
	{
		base.Attach(attachTo, usingCamera);
		this.updateRotation();
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x000EC2EC File Offset: 0x000EA6EC
	private void updateAlpha(float alpha)
	{
		foreach (Material material in this.fadeMaterials)
		{
			this.setMaterialAlpha(material, alpha);
		}
		foreach (KeyValuePair<int, PlayerNetsukeDisplay.NetsukeDisplay> keyValuePair in this.displays)
		{
			if (keyValuePair.Value != null)
			{
				foreach (Material material2 in keyValuePair.Value.fadeMaterials)
				{
					this.setMaterialAlpha(material2, alpha);
				}
			}
		}
	}

	// Token: 0x06002ED3 RID: 11987 RVA: 0x000EC3F0 File Offset: 0x000EA7F0
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

	// Token: 0x17000B5C RID: 2908
	// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x000EC598 File Offset: 0x000EA998
	// (set) Token: 0x06002ED5 RID: 11989 RVA: 0x000EC5A0 File Offset: 0x000EA9A0
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

	// Token: 0x17000B5D RID: 2909
	// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x000EC5BC File Offset: 0x000EA9BC
	// (set) Token: 0x06002ED7 RID: 11991 RVA: 0x000EC5C4 File Offset: 0x000EA9C4
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

	// Token: 0x06002ED8 RID: 11992 RVA: 0x000EC5EF File Offset: 0x000EA9EF
	protected override void Update()
	{
		base.Update();
		this.updateRotation();
	}

	// Token: 0x06002ED9 RID: 11993 RVA: 0x000EC5FD File Offset: 0x000EA9FD
	private void updateRotation()
	{
		this.lookAtCamera();
		this.rotationOffset = Mathf.Lerp(this.rotationOffset, this.targetRotate, Time.deltaTime * 5f);
	}

	// Token: 0x17000B5E RID: 2910
	// (get) Token: 0x06002EDA RID: 11994 RVA: 0x000EC627 File Offset: 0x000EAA27
	private bool isMultiItemMode
	{
		get
		{
			return this.Container3 != null;
		}
	}

	// Token: 0x06002EDB RID: 11995 RVA: 0x000EC638 File Offset: 0x000EAA38
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
			foreach (KeyValuePair<int, PlayerNetsukeDisplay.NetsukeDisplay> keyValuePair in this.displays)
			{
				if (keyValuePair.Value != null)
				{
					this.objectLookAtCamera(keyValuePair.Value.rotateContainer);
				}
			}
		}
	}

	// Token: 0x06002EDC RID: 11996 RVA: 0x000EC768 File Offset: 0x000EAB68
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

	// Token: 0x040020C8 RID: 8392
	private float yRotate;

	// Token: 0x040020C9 RID: 8393
	public Transform RotateContainer;

	// Token: 0x040020CA RID: 8394
	public GameObject Container1;

	// Token: 0x040020CB RID: 8395
	public GameObject Container2;

	// Token: 0x040020CC RID: 8396
	public GameObject Container3;

	// Token: 0x040020CD RID: 8397
	private Dictionary<int, PlayerNetsukeDisplay.NetsukeDisplay> displays = new Dictionary<int, PlayerNetsukeDisplay.NetsukeDisplay>();

	// Token: 0x040020CE RID: 8398
	private List<Material> fadeMaterials = new List<Material>();

	// Token: 0x040020CF RID: 8399
	private Dictionary<Material, float> originalShaderMode = new Dictionary<Material, float>();

	// Token: 0x040020D0 RID: 8400
	private float _alpha = 1f;

	// Token: 0x040020D1 RID: 8401
	private Vector3 _scale = new Vector3(1f, 1f, 1f);

	// Token: 0x040020D2 RID: 8402
	private GameObject[] containers;

	// Token: 0x040020D3 RID: 8403
	private float targetRotate;

	// Token: 0x040020D4 RID: 8404
	private float _rotationOffset;

	// Token: 0x040020D5 RID: 8405
	private int spinIndex;

	// Token: 0x040020D6 RID: 8406
	private const float spinSpeed = 5f;

	// Token: 0x02000766 RID: 1894
	private class NetsukeDisplay
	{
		// Token: 0x040020D7 RID: 8407
		public int index;

		// Token: 0x040020D8 RID: 8408
		public GameObject gameObj;

		// Token: 0x040020D9 RID: 8409
		public Transform rotateContainer;

		// Token: 0x040020DA RID: 8410
		public Netsuke data;

		// Token: 0x040020DB RID: 8411
		public List<Material> fadeMaterials;
	}
}
