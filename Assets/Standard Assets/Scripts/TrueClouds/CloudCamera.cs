// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace TrueClouds
{
	[ExecuteInEditMode, RequireComponent(typeof(Camera))]
	public abstract class CloudCamera : MonoBehaviour
	{
		public LayerMask CloudsMask;

		public LayerMask LightMask;

		public LayerMask WorldBlockingMask;

		public int ResolutionDivider = 3;

		public int WorldDepthResolutionDivider = 2;

		public DepthPrecision DepthPrecision = DepthPrecision.Medium;

		public bool LateCut = true;

		public float BlurRadius = 10f;

		public BlurQuality BlurQuality = BlurQuality.High;

		public float LateCutThreshohld;

		public float LateCutPower = 1.5f;

		public bool UseDepthFiltering;

		public float DepthFilteringPower;

		public bool UseNoise;

		public Texture2D Noise;

		public Vector3 Wind = new Vector3(2f, 1f, 3f);

		public float NoiseScale = 1f;

		public float DepthNoiseScale = 1f;

		public float NormalNoisePower = 1f;

		public float DepthNoisePower = 1f;

		public float DisplacementNoisePower = 1f;

		public float NoiseSinTimeScale = 0.2f;

		public float DistanceToClouds = 10f;

		public Transform Light;

		public bool UseRamp;

		public Texture Ramp;

		public Color LightColor = Color.white;

		public Color ShadowColor = new Color(0.6f, 0.72f, 0.84f);

		public float LightEnd = 0.75f;

		public float HaloPower = 3f;

		public float HaloDistance = 0.5f;

		public float FallbackDistance = 1f;

		public Shader blurFastShader;

		public Shader blurShader;

		public Shader blurHQShader;

		public Shader depthBlurShader;

		public Shader depthShader;

		public Shader cloudShader;

		public Shader clearColorShader;

		private RenderTexture _worldDepth;

		private RenderTexture _cloudDepth;

		private RenderTexture _fromRT;

		private RenderTexture _toRT;

		private RenderTexture _cloudMain;

		private RenderTexture _worldBlit;

		private Material _renderMaterial;

		private Material _blurMaterial;

		private Material _depthBlurMaterial;

		private Material _clearColorMaterial;

		private Camera _camera;

		private Camera _tempCamera;

		private static int LIGHT_DIR_ID;

		private static int LIGHT_POS_ID;

		private static int MAIN_COLOR_ID;

		private static int SHADOW_COLOR_ID;

		private static int LIGHT_END_ID;

		private static int WORLD_DEPTH_ID;

		private static int CAMERA_DEPTH_ID;

		private static int NORMALS_ID;

		private static int RAMP_ID;

		private static int NOISE_ID;

		private static int NORMAL_NOISE_POWER_ID;

		private static int DEPTH_NOISE_POWER_ID;

		private static int DISPLACEMENT_NOISE_POWER_ID;

		private static int NOISE_SIN_TIME_ID;

		private static int NOISE_PARAMS_ID;

		private static int FALLBACK_DIST_ID;

		private static int CAMERA_ROTATION_ID;

		private static int NEAR_PLANE_ID;

		private static int FAR_PLANE_ID;

		private static int CAMERA_DIR_LD;

		private static int CAMERA_DIR_RD;

		private static int CAMERA_DIR_LU;

		private static int CAMERA_DIR_RU;

		private static int HALO_POWER_ID;

		private static int HALO_DISTANCE_ID;

		private static int BLUR_SIZE_ID;

		private static int LATE_CUT_THRESHOLD;

		private static int LATE_CUT_POWER;

		private static int DEPTH_FILTERING_POWER;

		private int _lastBlurQuality = -1;

		private int _lastResolutionDivider = -1;

		private int _lastWorldResolutionDivider = -1;

		private Rect _lastScreenRect = Rect.zero;

		protected virtual void Awake()
		{
			this._camera = base.GetComponent<Camera>();
			this._tempCamera = new GameObject("cloud camera")
			{
				hideFlags = HideFlags.HideAndDontSave,
				transform = 
				{
					parent = base.transform,
					localPosition = Vector3.zero,
					localRotation = Quaternion.identity
				}
			}.AddComponent<Camera>();
			this._tempCamera.CopyFrom(this._camera);
			this._tempCamera.enabled = false;
		}

		private void OnEnable()
		{
			this.CleanupRenderTextures();
			this.SetupShaderIDs();
		}

		private void OnDisable()
		{
			this.CleanupRenderTextures();
		}

		private void SetupShaderIDs()
		{
			CloudCamera.LIGHT_DIR_ID = Shader.PropertyToID("_LightDir");
			CloudCamera.LIGHT_POS_ID = Shader.PropertyToID("_LightPos");
			CloudCamera.MAIN_COLOR_ID = Shader.PropertyToID("_MainColor");
			CloudCamera.SHADOW_COLOR_ID = Shader.PropertyToID("_ShadowColor");
			CloudCamera.LIGHT_END_ID = Shader.PropertyToID("_LightEnd");
			CloudCamera.WORLD_DEPTH_ID = Shader.PropertyToID("_WorldDepth");
			CloudCamera.CAMERA_DEPTH_ID = Shader.PropertyToID("_CameraDepth");
			CloudCamera.NORMALS_ID = Shader.PropertyToID("_NormalTex");
			CloudCamera.RAMP_ID = Shader.PropertyToID("_Ramp");
			CloudCamera.NOISE_ID = Shader.PropertyToID("_Noise");
			CloudCamera.NOISE_PARAMS_ID = Shader.PropertyToID("_NoiseParams");
			CloudCamera.NORMAL_NOISE_POWER_ID = Shader.PropertyToID("_NormalNoisePower");
			CloudCamera.NOISE_SIN_TIME_ID = Shader.PropertyToID("_NoiseSinTime");
			CloudCamera.DEPTH_NOISE_POWER_ID = Shader.PropertyToID("_DepthNoisePower");
			CloudCamera.DISPLACEMENT_NOISE_POWER_ID = Shader.PropertyToID("_DisplacementNoisePower");
			CloudCamera.FALLBACK_DIST_ID = Shader.PropertyToID("_FallbackDist");
			CloudCamera.CAMERA_ROTATION_ID = Shader.PropertyToID("_CameraRotation");
			CloudCamera.NEAR_PLANE_ID = Shader.PropertyToID("_NearPlane");
			CloudCamera.FAR_PLANE_ID = Shader.PropertyToID("_FarPlane");
			CloudCamera.CAMERA_DIR_LD = Shader.PropertyToID("_CameraDirLD");
			CloudCamera.CAMERA_DIR_RD = Shader.PropertyToID("_CameraDirRD");
			CloudCamera.CAMERA_DIR_LU = Shader.PropertyToID("_CameraDirLU");
			CloudCamera.CAMERA_DIR_RU = Shader.PropertyToID("_CameraDirRU");
			CloudCamera.HALO_POWER_ID = Shader.PropertyToID("_HaloPower");
			CloudCamera.HALO_DISTANCE_ID = Shader.PropertyToID("_HaloDistance");
			CloudCamera.BLUR_SIZE_ID = Shader.PropertyToID("_BlurSize");
			CloudCamera.LATE_CUT_THRESHOLD = Shader.PropertyToID("_LateCutThreshohld");
			CloudCamera.LATE_CUT_POWER = Shader.PropertyToID("_LateCutPower");
			CloudCamera.DEPTH_FILTERING_POWER = Shader.PropertyToID("_DepthFilteringPower");
		}

		private void SetupRenderTextures()
		{
			this._cloudMain = this.GetTemporaryTexture(this.ResolutionDivider, FilterMode.Bilinear);
			this._worldDepth = this.GetTemporaryTexture(this.WorldDepthResolutionDivider, FilterMode.Bilinear);
			if (this.WorldDepthResolutionDivider != 1)
			{
				this._worldBlit = this.GetTemporaryTexture(this.WorldDepthResolutionDivider, FilterMode.Bilinear);
			}
			this._cloudDepth = this.GetTemporaryTexture(this.ResolutionDivider, FilterMode.Bilinear);
			this._fromRT = this.GetTemporaryTexture(this.ResolutionDivider, FilterMode.Bilinear);
			this._toRT = this.GetTemporaryTexture(this.ResolutionDivider, FilterMode.Bilinear);
			this._lastScreenRect = this._camera.rect;
		}

		private void CleanupRenderTextures()
		{
			this.ReleaseTemporaryTexture(ref this._cloudMain);
			this.ReleaseTemporaryTexture(ref this._worldDepth);
			this.ReleaseTemporaryTexture(ref this._worldBlit);
			this.ReleaseTemporaryTexture(ref this._cloudDepth);
			this.ReleaseTemporaryTexture(ref this._fromRT);
			this.ReleaseTemporaryTexture(ref this._toRT);
			this._lastScreenRect = Rect.zero;
		}

		private void Start()
		{
			this._camera.cullingMask &= ~this.CloudsMask;
			this._camera.cullingMask &= ~this.LightMask;
			this._renderMaterial = new Material(this.cloudShader);
			this.UpdateBlurMaterial();
			this._depthBlurMaterial = new Material(this.depthBlurShader);
			this._clearColorMaterial = new Material(this.clearColorShader);
		}

		private void UpdateBlurMaterial()
		{
			BlurQuality blurQuality = this.BlurQuality;
			if (blurQuality != BlurQuality.Low)
			{
				if (blurQuality != BlurQuality.Medium)
				{
					if (blurQuality == BlurQuality.High)
					{
						this._blurMaterial = new Material(this.blurHQShader);
					}
				}
				else
				{
					this._blurMaterial = new Material(this.blurShader);
				}
			}
			else
			{
				this._blurMaterial = new Material(this.blurFastShader);
			}
			this._lastBlurQuality = (int)this.BlurQuality;
		}

		private RenderTexture GetTemporaryTexture(int divider, FilterMode mode)
		{
			return RenderTexture.GetTemporary((int)this._camera.pixelRect.size.x / divider, (int)this._camera.pixelRect.size.y / divider, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		}

		private void ReleaseTemporaryTexture(ref RenderTexture texture)
		{
			if (texture != null)
			{
				RenderTexture.ReleaseTemporary(texture);
				texture = null;
			}
		}

		protected void RenderClouds(RenderTexture source, RenderTexture destination)
		{
			this.UpdateChangedSettings();
			this._tempCamera.CopyFrom(this._camera);
			this._tempCamera.allowMSAA = false;
			this._tempCamera.allowHDR = false;
			this._tempCamera.renderingPath = RenderingPath.Forward;
			this._tempCamera.depthTextureMode = DepthTextureMode.Depth;
			this._tempCamera.enabled = false;
			this.ApplyBlits(source, destination);
		}

		private void ApplyBlits(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination);
			this._tempCamera.clearFlags = CameraClearFlags.Color;
			this._tempCamera.backgroundColor = Color.white;
			this._tempCamera.rect = new Rect(Vector2.zero, Vector2.one);
			this._worldDepth.DiscardContents();
			this._tempCamera.targetTexture = this._worldDepth;
			this._tempCamera.cullingMask = this.WorldBlockingMask;
			this._tempCamera.RenderWithShader(this.depthShader, "RenderType");
			this._cloudDepth.DiscardContents();
			this._tempCamera.targetTexture = this._cloudDepth;
			this._tempCamera.cullingMask = this.CloudsMask;
			this._tempCamera.RenderWithShader(this.depthShader, "RenderType");
			this._tempCamera.clearFlags = CameraClearFlags.Color;
			this._tempCamera.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 0f);
			this._tempCamera.cullingMask = this.CloudsMask;
			this._cloudMain.DiscardContents();
			this._tempCamera.targetTexture = this._cloudMain;
			this._tempCamera.Render();
			this.DrawGreyBorder(this._cloudMain);
			this._tempCamera.enabled = false;
			this.UpdateShaderValues();
			if (this.LateCut)
			{
				this.SwapTextures(ref this._toRT, ref this._cloudMain);
			}
			else
			{
				this.SwapTextures();
				Graphics.Blit(this._cloudMain, this._toRT, this._renderMaterial, 0);
			}
			if (this.LateCut || this.UseNoise)
			{
				this._depthBlurMaterial.SetTexture(CloudCamera.NORMALS_ID, this._toRT);
				this.SwapTextures(ref this._fromRT, ref this._cloudDepth);
				Graphics.Blit(this._fromRT, this._cloudDepth, this._depthBlurMaterial, 1);
				this.SwapTextures(ref this._fromRT, ref this._cloudDepth);
				Graphics.Blit(this._fromRT, this._cloudDepth, this._depthBlurMaterial, 2);
				this._renderMaterial.SetTexture(CloudCamera.CAMERA_DEPTH_ID, this._cloudDepth);
				this._blurMaterial.SetTexture(CloudCamera.CAMERA_DEPTH_ID, this._cloudDepth);
			}
			this.SwapTextures();
			Graphics.Blit(this._fromRT, this._toRT, this._blurMaterial, 0);
			this.SwapTextures();
			Graphics.Blit(this._fromRT, this._toRT, this._blurMaterial, 1);
			if ((this.LateCut || this.UseNoise) && this.UseNoise)
			{
				this.SwapTextures(ref this._fromRT, ref this._cloudDepth);
				Graphics.Blit(this._fromRT, this._cloudDepth, this._depthBlurMaterial, 0);
				this._renderMaterial.SetTexture(CloudCamera.CAMERA_DEPTH_ID, this._cloudDepth);
			}
			this.SwapTextures();
			Graphics.Blit(this._fromRT, this._toRT, this._renderMaterial, 1);
			Shader.SetGlobalTexture(CloudCamera.CAMERA_DEPTH_ID, this._cloudDepth);
			Shader.SetGlobalTexture(CloudCamera.NORMALS_ID, this._fromRT);
			this._tempCamera.clearFlags = CameraClearFlags.Depth;
			this._tempCamera.targetTexture = this._toRT;
			this._tempCamera.cullingMask = this.LightMask;
			this._tempCamera.Render();
			if (!this.LateCut)
			{
				this.SwapTextures();
				Graphics.Blit(this._fromRT, destination, this._renderMaterial, 4);
			}
			else if (this.WorldDepthResolutionDivider != 1)
			{
				this._worldBlit.DiscardContents();
				this.SwapTextures();
				Graphics.Blit(this._fromRT, this._worldBlit, this._renderMaterial, 3);
				Graphics.Blit(this._worldBlit, destination, this._renderMaterial, 4);
			}
			else
			{
				this.SwapTextures();
				Graphics.Blit(this._fromRT, destination, this._renderMaterial, 2);
			}
		}

		private void DrawGreyBorder(RenderTexture texture)
		{
			Graphics.SetRenderTarget(texture);
			this._clearColorMaterial.SetPass(0);
			GL.LoadPixelMatrix();
			GL.Color(Color.black);
			GL.Begin(2);
			GL.Vertex(new Vector3(1f, 1f));
			GL.Vertex(new Vector3(1f, (float)Screen.height));
			GL.Vertex(new Vector3((float)Screen.width, (float)Screen.height));
			GL.Vertex(new Vector3((float)Screen.width, 1f));
			GL.Vertex(new Vector3(1f, 1f));
			GL.End();
		}

		private void SwapTextures()
		{
			this.SwapTextures(ref this._fromRT, ref this._toRT);
		}

		private void SwapTextures(ref RenderTexture a, ref RenderTexture b)
		{
			a.DiscardContents();
			RenderTexture renderTexture = a;
			a = b;
			b = renderTexture;
		}

		private void SetFeature(string on, string off, bool enable)
		{
			if (enable)
			{
				Shader.DisableKeyword(off);
				Shader.EnableKeyword(on);
			}
			else
			{
				Shader.DisableKeyword(on);
				Shader.EnableKeyword(off);
			}
		}

		private void UpdateShaderValues()
		{
			if (this.Light != null)
			{
				Vector4 value = -this.Light.transform.forward;
				value.w = Mathf.Max(0f, Vector3.Dot(base.transform.forward, -this.Light.transform.forward));
				this._renderMaterial.SetVector(CloudCamera.LIGHT_DIR_ID, value);
				Vector3 point = -(base.transform.worldToLocalMatrix * this.Light.transform.forward);
				point.z = -point.z;
				Vector2 vector = this._camera.projectionMatrix.MultiplyPoint(point);
				vector = vector * 0.5f + new Vector2(0.5f, 0.5f);
				this._renderMaterial.SetVector(CloudCamera.LIGHT_POS_ID, vector);
			}
			bool enable = this.DepthPrecision == DepthPrecision.High;
			this.SetFeature("HIGH_RES_DEPTH", "MEDIUM_RES_DEPTH", enable);
			bool enable2 = this.HaloDistance > 0.01f && this.HaloPower > 0.01f;
			this.SetFeature("HALO_ON", "HALO_OFF", enable2);
			this.SetFeature("LATE_CUT", "EARLY_CUT", this.LateCut);
			this.SetFeature("DEPTH_FILTERING_ON", "DEPTH_FILTERING_OFF", this.UseDepthFiltering);
			this.SetFeature("NOISE_ON", "NOISE_OFF", this.UseNoise);
			this.SetFeature("RAMP_ON", "RAMP_OFF", this.UseRamp);
			this._renderMaterial.SetColor(CloudCamera.MAIN_COLOR_ID, this.LightColor);
			if (this.UseRamp)
			{
				this._renderMaterial.SetTexture(CloudCamera.RAMP_ID, this.Ramp);
			}
			else
			{
				this._renderMaterial.SetColor(CloudCamera.SHADOW_COLOR_ID, this.ShadowColor);
				this._renderMaterial.SetFloat(CloudCamera.LIGHT_END_ID, this.LightEnd);
			}
			this._renderMaterial.SetTexture(CloudCamera.WORLD_DEPTH_ID, this._worldDepth);
			this._renderMaterial.SetTexture(CloudCamera.CAMERA_DEPTH_ID, this._cloudDepth);
			if (this.UseNoise)
			{
				this._renderMaterial.SetTexture(CloudCamera.NOISE_ID, this.Noise);
				this._depthBlurMaterial.SetTexture(CloudCamera.NOISE_ID, this.Noise);
				Vector4 value2 = new Vector4(-this.Wind.x, -this.Wind.y, -this.Wind.z, 1f / (this.NoiseScale * this.DistanceToClouds));
				Vector4 value3 = new Vector4(-this.Wind.x, -this.Wind.y, -this.Wind.z, 1f / (this.DepthNoiseScale * this.DistanceToClouds));
				this._renderMaterial.SetVector(CloudCamera.NOISE_PARAMS_ID, value2);
				this._depthBlurMaterial.SetVector(CloudCamera.NOISE_PARAMS_ID, value3);
				this._renderMaterial.SetFloat(CloudCamera.NORMAL_NOISE_POWER_ID, this.NormalNoisePower * 0.3f);
				this._renderMaterial.SetFloat(CloudCamera.DISPLACEMENT_NOISE_POWER_ID, this.DisplacementNoisePower * 0.07f * this.DistanceToClouds);
				this._depthBlurMaterial.SetFloat(CloudCamera.DEPTH_NOISE_POWER_ID, this.DepthNoisePower * this.DistanceToClouds);
				Vector3 v = new Vector3(Mathf.Sin(Time.time * this.NoiseSinTimeScale * 2f * 3.14159274f), Mathf.Sin((Time.time * this.NoiseSinTimeScale + 0.3333f) * 2f * 3.14159274f), Mathf.Sin((Time.time * this.NoiseSinTimeScale + 0.6666f) * 2f * 3.14159274f));
				this._depthBlurMaterial.SetVector(CloudCamera.NOISE_SIN_TIME_ID, v);
			}
			this._renderMaterial.SetFloat(CloudCamera.FALLBACK_DIST_ID, this.FallbackDistance);
			this._depthBlurMaterial.SetFloat(CloudCamera.FALLBACK_DIST_ID, this.FallbackDistance);
			this._renderMaterial.SetMatrix(CloudCamera.CAMERA_ROTATION_ID, base.transform.localToWorldMatrix);
			this._depthBlurMaterial.SetMatrix(CloudCamera.CAMERA_ROTATION_ID, base.transform.localToWorldMatrix);
			this._renderMaterial.SetFloat(CloudCamera.NEAR_PLANE_ID, this._camera.nearClipPlane);
			this._depthBlurMaterial.SetFloat(CloudCamera.NEAR_PLANE_ID, this._camera.nearClipPlane);
			this._renderMaterial.SetFloat(CloudCamera.FAR_PLANE_ID, this._camera.farClipPlane);
			this._blurMaterial.SetFloat(CloudCamera.FAR_PLANE_ID, this._camera.farClipPlane);
			this._depthBlurMaterial.SetFloat(CloudCamera.FAR_PLANE_ID, this._camera.farClipPlane);
			this._blurMaterial.SetFloat(CloudCamera.LATE_CUT_THRESHOLD, this.LateCutThreshohld);
			this._blurMaterial.SetFloat(CloudCamera.LATE_CUT_POWER, this.LateCutPower);
			float num = this.BlurRadius;
			if (this.UseDepthFiltering)
			{
				num *= Mathf.Pow(this.DistanceToClouds / this._camera.farClipPlane, this.DepthFilteringPower);
				this._blurMaterial.SetFloat(CloudCamera.DEPTH_FILTERING_POWER, this.DepthFilteringPower);
				this._depthBlurMaterial.SetFloat(CloudCamera.DEPTH_FILTERING_POWER, this.DepthFilteringPower);
			}
			this._depthBlurMaterial.SetFloat(CloudCamera.BLUR_SIZE_ID, num);
			this._blurMaterial.SetFloat(CloudCamera.BLUR_SIZE_ID, num);
			Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
			Vector4 value4 = worldToLocalMatrix * CloudCamera.Point(this._camera.ScreenToWorldPoint(new Vector3(0f, 0f, 1f)));
			Vector4 value5 = worldToLocalMatrix * CloudCamera.Point(this._camera.ScreenToWorldPoint(new Vector3((float)this._camera.pixelWidth, 0f, 1f)));
			Vector4 value6 = worldToLocalMatrix * CloudCamera.Point(this._camera.ScreenToWorldPoint(new Vector3(0f, (float)this._camera.pixelHeight, 1f)));
			Vector4 value7 = worldToLocalMatrix * CloudCamera.Point(this._camera.ScreenToWorldPoint(new Vector3((float)this._camera.pixelWidth, (float)this._camera.pixelHeight, 1f)));
			this._renderMaterial.SetVector(CloudCamera.CAMERA_DIR_LD, value4);
			this._depthBlurMaterial.SetVector(CloudCamera.CAMERA_DIR_LD, value4);
			this._renderMaterial.SetVector(CloudCamera.CAMERA_DIR_RD, value5);
			this._depthBlurMaterial.SetVector(CloudCamera.CAMERA_DIR_RD, value5);
			this._renderMaterial.SetVector(CloudCamera.CAMERA_DIR_LU, value6);
			this._depthBlurMaterial.SetVector(CloudCamera.CAMERA_DIR_LU, value6);
			this._renderMaterial.SetVector(CloudCamera.CAMERA_DIR_RU, value7);
			this._depthBlurMaterial.SetVector(CloudCamera.CAMERA_DIR_RU, value7);
			this._renderMaterial.SetFloat(CloudCamera.HALO_POWER_ID, this.HaloPower);
			this._renderMaterial.SetFloat(CloudCamera.HALO_DISTANCE_ID, this.HaloDistance / 2f);
		}

		private static Vector4 Point(Vector3 v)
		{
			return new Vector4(v.x, v.y, v.z, 1f);
		}

		private void UpdateChangedSettings()
		{
			if (this._lastBlurQuality != (int)this.BlurQuality)
			{
				this.UpdateBlurMaterial();
			}
			if (this._lastResolutionDivider != this.ResolutionDivider || this._lastWorldResolutionDivider != this.WorldDepthResolutionDivider || this._lastScreenRect != this._camera.rect)
			{
				this.CleanupRenderTextures();
				this._lastResolutionDivider = this.ResolutionDivider;
				this._lastWorldResolutionDivider = this.WorldDepthResolutionDivider;
				this.SetupRenderTextures();
			}
		}
	}
}
