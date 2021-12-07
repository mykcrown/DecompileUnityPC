// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TrueClouds
{
	[ExecuteInEditMode]
	internal class CloudPointLight : MonoBehaviour
	{
		public float Start;

		public float Range = 10f;

		public Color Color = Color.white;

		public float ShadowIntensity = 0.2f;

		private static Shader SHADER;

		private static int START_ID = -1;

		private static int RANGE_ID = -1;

		private static int COLOR_ID = -1;

		private static int SHADOW_INTENSITY_ID = -1;

		private Material _material;

		private Transform _transform;

		private GameObject _light;

		private Transform _lightTransform;

		private void OnValidate()
		{
			this.ValidateHasGoodLayer();
			this.ValidateDistances();
		}

		private void ValidateHasGoodLayer()
		{
			CloudCamera[] components = base.GetComponents<CloudCamera>();
			if (components.Length == 0)
			{
				return;
			}
			if (components.All(new Func<CloudCamera, bool>(this._ValidateHasGoodLayer_m__0)))
			{
				UnityEngine.Debug.LogWarning("This light has a layer that is not rendered by any of the Cloud Cameras", base.gameObject);
			}
		}

		private void ValidateDistances()
		{
			this.Start = Mathf.Max(0f, this.Start);
			this.Range = Mathf.Max(this.Range, this.Start);
		}

		private void Awake()
		{
			if (CloudPointLight.SHADER == null)
			{
				this.InitShaderAndIDs();
			}
			this._transform = base.transform;
			this._light = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			this._light.layer = base.gameObject.layer;
			this._light.hideFlags = HideFlags.HideAndDontSave;
			this._material = new Material(CloudPointLight.SHADER);
			this._light.GetComponent<Renderer>().sharedMaterial = this._material;
			this._lightTransform = this._light.transform;
		}

		private void OnDisable()
		{
			this._light.SetActive(false);
		}

		private void OnEnable()
		{
			this._light.SetActive(true);
		}

		private void OnDestroy()
		{
			if (Application.isEditor && !Application.isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(this._light);
			}
			else
			{
				UnityEngine.Object.Destroy(this._light);
			}
		}

		private void Update()
		{
			if (CloudPointLight.SHADER == null)
			{
				this.InitShaderAndIDs();
			}
			this._material.SetFloat(CloudPointLight.START_ID, this.Start);
			this._material.SetFloat(CloudPointLight.RANGE_ID, this.Range);
			this._material.SetColor(CloudPointLight.COLOR_ID, this.Color);
			this._material.SetFloat(CloudPointLight.SHADOW_INTENSITY_ID, this.ShadowIntensity);
			float num = this.Range * 2f * 1.1f;
			this._lightTransform.localScale = new Vector3(num, num, num);
			this._lightTransform.position = this._transform.position;
		}

		private void InitShaderAndIDs()
		{
			CloudPointLight.SHADER = Shader.Find("Hidden/Clouds/PointLight");
			CloudPointLight.START_ID = Shader.PropertyToID("_Start");
			CloudPointLight.RANGE_ID = Shader.PropertyToID("_MaxDistance");
			CloudPointLight.COLOR_ID = Shader.PropertyToID("_TintColor");
			CloudPointLight.SHADOW_INTENSITY_ID = Shader.PropertyToID("_ShadowIntensity");
		}

		private void OnDrawGizmosSelected()
		{
			Color yellow = Color.yellow;
			yellow.a = 0.7f;
			Gizmos.color = yellow;
			Gizmos.DrawSphere(base.transform.position, this.Start);
			yellow.a = 0.3f;
			Gizmos.color = yellow;
			Gizmos.DrawSphere(base.transform.position, this.Range);
		}

		private bool _ValidateHasGoodLayer_m__0(CloudCamera camera)
		{
			return (camera.LightMask & base.gameObject.layer) == 0;
		}
	}
}
