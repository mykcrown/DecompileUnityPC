using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000B9D RID: 2973
	[ExecuteInEditMode]
	public class Water : MonoBehaviour
	{
		// Token: 0x060055E4 RID: 21988 RVA: 0x001B6D34 File Offset: 0x001B5134
		public void OnWillRenderObject()
		{
			if (!base.enabled || !base.GetComponent<Renderer>() || !base.GetComponent<Renderer>().sharedMaterial || !base.GetComponent<Renderer>().enabled)
			{
				return;
			}
			Camera current = Camera.current;
			if (!current)
			{
				return;
			}
			if (Water.s_InsideWater)
			{
				return;
			}
			Water.s_InsideWater = true;
			this.m_HardwareWaterSupport = this.FindHardwareWaterSupport();
			Water.WaterMode waterMode = this.GetWaterMode();
			Camera camera;
			Camera camera2;
			this.CreateWaterObjects(current, out camera, out camera2);
			Vector3 position = base.transform.position;
			Vector3 up = base.transform.up;
			int pixelLightCount = QualitySettings.pixelLightCount;
			if (this.disablePixelLights)
			{
				QualitySettings.pixelLightCount = 0;
			}
			this.UpdateCameraModes(current, camera);
			this.UpdateCameraModes(current, camera2);
			if (waterMode >= Water.WaterMode.Reflective)
			{
				float w = -Vector3.Dot(up, position) - this.clipPlaneOffset;
				Vector4 plane = new Vector4(up.x, up.y, up.z, w);
				Matrix4x4 zero = Matrix4x4.zero;
				Water.CalculateReflectionMatrix(ref zero, plane);
				Vector3 position2 = current.transform.position;
				Vector3 position3 = zero.MultiplyPoint(position2);
				camera.worldToCameraMatrix = current.worldToCameraMatrix * zero;
				Vector4 clipPlane = this.CameraSpacePlane(camera, position, up, 1f);
				camera.projectionMatrix = current.CalculateObliqueMatrix(clipPlane);
				camera.cullingMatrix = current.projectionMatrix * current.worldToCameraMatrix;
				camera.cullingMask = (-17 & this.reflectLayers.value);
				camera.targetTexture = this.m_ReflectionTexture;
				bool invertCulling = GL.invertCulling;
				GL.invertCulling = !invertCulling;
				camera.transform.position = position3;
				Vector3 eulerAngles = current.transform.eulerAngles;
				camera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
				camera.Render();
				camera.transform.position = position2;
				GL.invertCulling = invertCulling;
				base.GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex", this.m_ReflectionTexture);
			}
			if (waterMode >= Water.WaterMode.Refractive)
			{
				camera2.worldToCameraMatrix = current.worldToCameraMatrix;
				Vector4 clipPlane2 = this.CameraSpacePlane(camera2, position, up, -1f);
				camera2.projectionMatrix = current.CalculateObliqueMatrix(clipPlane2);
				camera2.cullingMatrix = current.projectionMatrix * current.worldToCameraMatrix;
				camera2.cullingMask = (-17 & this.refractLayers.value);
				camera2.targetTexture = this.m_RefractionTexture;
				camera2.transform.position = current.transform.position;
				camera2.transform.rotation = current.transform.rotation;
				camera2.Render();
				base.GetComponent<Renderer>().sharedMaterial.SetTexture("_RefractionTex", this.m_RefractionTexture);
			}
			if (this.disablePixelLights)
			{
				QualitySettings.pixelLightCount = pixelLightCount;
			}
			if (waterMode != Water.WaterMode.Simple)
			{
				if (waterMode != Water.WaterMode.Reflective)
				{
					if (waterMode == Water.WaterMode.Refractive)
					{
						Shader.DisableKeyword("WATER_SIMPLE");
						Shader.DisableKeyword("WATER_REFLECTIVE");
						Shader.EnableKeyword("WATER_REFRACTIVE");
					}
				}
				else
				{
					Shader.DisableKeyword("WATER_SIMPLE");
					Shader.EnableKeyword("WATER_REFLECTIVE");
					Shader.DisableKeyword("WATER_REFRACTIVE");
				}
			}
			else
			{
				Shader.EnableKeyword("WATER_SIMPLE");
				Shader.DisableKeyword("WATER_REFLECTIVE");
				Shader.DisableKeyword("WATER_REFRACTIVE");
			}
			Water.s_InsideWater = false;
		}

		// Token: 0x060055E5 RID: 21989 RVA: 0x001B70A8 File Offset: 0x001B54A8
		private void OnDisable()
		{
			if (this.m_ReflectionTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
				this.m_ReflectionTexture = null;
			}
			if (this.m_RefractionTexture)
			{
				UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
				this.m_RefractionTexture = null;
			}
			foreach (KeyValuePair<Camera, Camera> keyValuePair in this.m_ReflectionCameras)
			{
				UnityEngine.Object.DestroyImmediate(keyValuePair.Value.gameObject);
			}
			this.m_ReflectionCameras.Clear();
			foreach (KeyValuePair<Camera, Camera> keyValuePair2 in this.m_RefractionCameras)
			{
				UnityEngine.Object.DestroyImmediate(keyValuePair2.Value.gameObject);
			}
			this.m_RefractionCameras.Clear();
		}

		// Token: 0x060055E6 RID: 21990 RVA: 0x001B71C0 File Offset: 0x001B55C0
		private void Update()
		{
			if (!base.GetComponent<Renderer>())
			{
				return;
			}
			Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			if (!sharedMaterial)
			{
				return;
			}
			Vector4 vector = sharedMaterial.GetVector("WaveSpeed");
			float @float = sharedMaterial.GetFloat("_WaveScale");
			Vector4 value = new Vector4(@float, @float, @float * 0.4f, @float * 0.45f);
			double num = (double)Time.timeSinceLevelLoad / 20.0;
			Vector4 value2 = new Vector4((float)Math.IEEERemainder((double)(vector.x * value.x) * num, 1.0), (float)Math.IEEERemainder((double)(vector.y * value.y) * num, 1.0), (float)Math.IEEERemainder((double)(vector.z * value.z) * num, 1.0), (float)Math.IEEERemainder((double)(vector.w * value.w) * num, 1.0));
			sharedMaterial.SetVector("_WaveOffset", value2);
			sharedMaterial.SetVector("_WaveScale4", value);
		}

		// Token: 0x060055E7 RID: 21991 RVA: 0x001B72E0 File Offset: 0x001B56E0
		private void UpdateCameraModes(Camera src, Camera dest)
		{
			if (dest == null)
			{
				return;
			}
			dest.clearFlags = src.clearFlags;
			dest.backgroundColor = src.backgroundColor;
			if (src.clearFlags == CameraClearFlags.Skybox)
			{
				Skybox component = src.GetComponent<Skybox>();
				Skybox component2 = dest.GetComponent<Skybox>();
				if (!component || !component.material)
				{
					component2.enabled = false;
				}
				else
				{
					component2.enabled = true;
					component2.material = component.material;
				}
			}
			dest.farClipPlane = src.farClipPlane;
			dest.nearClipPlane = src.nearClipPlane;
			dest.orthographic = src.orthographic;
			dest.fieldOfView = src.fieldOfView;
			dest.aspect = src.aspect;
			dest.orthographicSize = src.orthographicSize;
		}

		// Token: 0x060055E8 RID: 21992 RVA: 0x001B73B0 File Offset: 0x001B57B0
		private void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
		{
			Water.WaterMode waterMode = this.GetWaterMode();
			reflectionCamera = null;
			refractionCamera = null;
			if (waterMode >= Water.WaterMode.Reflective)
			{
				if (!this.m_ReflectionTexture || this.m_OldReflectionTextureSize != this.textureSize)
				{
					if (this.m_ReflectionTexture)
					{
						UnityEngine.Object.DestroyImmediate(this.m_ReflectionTexture);
					}
					this.m_ReflectionTexture = new RenderTexture(this.textureSize, this.textureSize, 16);
					this.m_ReflectionTexture.name = "__WaterReflection" + base.GetInstanceID();
					this.m_ReflectionTexture.isPowerOfTwo = true;
					this.m_ReflectionTexture.hideFlags = HideFlags.DontSave;
					this.m_OldReflectionTextureSize = this.textureSize;
				}
				this.m_ReflectionCameras.TryGetValue(currentCamera, out reflectionCamera);
				if (!reflectionCamera)
				{
					GameObject gameObject = new GameObject(string.Concat(new object[]
					{
						"Water Refl Camera id",
						base.GetInstanceID(),
						" for ",
						currentCamera.GetInstanceID()
					}), new Type[]
					{
						typeof(Camera),
						typeof(Skybox)
					});
					reflectionCamera = gameObject.GetComponent<Camera>();
					reflectionCamera.enabled = false;
					reflectionCamera.transform.position = base.transform.position;
					reflectionCamera.transform.rotation = base.transform.rotation;
					reflectionCamera.gameObject.AddComponent<FlareLayer>();
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					this.m_ReflectionCameras[currentCamera] = reflectionCamera;
				}
			}
			if (waterMode >= Water.WaterMode.Refractive)
			{
				if (!this.m_RefractionTexture || this.m_OldRefractionTextureSize != this.textureSize)
				{
					if (this.m_RefractionTexture)
					{
						UnityEngine.Object.DestroyImmediate(this.m_RefractionTexture);
					}
					this.m_RefractionTexture = new RenderTexture(this.textureSize, this.textureSize, 16);
					this.m_RefractionTexture.name = "__WaterRefraction" + base.GetInstanceID();
					this.m_RefractionTexture.isPowerOfTwo = true;
					this.m_RefractionTexture.hideFlags = HideFlags.DontSave;
					this.m_OldRefractionTextureSize = this.textureSize;
				}
				this.m_RefractionCameras.TryGetValue(currentCamera, out refractionCamera);
				if (!refractionCamera)
				{
					GameObject gameObject2 = new GameObject(string.Concat(new object[]
					{
						"Water Refr Camera id",
						base.GetInstanceID(),
						" for ",
						currentCamera.GetInstanceID()
					}), new Type[]
					{
						typeof(Camera),
						typeof(Skybox)
					});
					refractionCamera = gameObject2.GetComponent<Camera>();
					refractionCamera.enabled = false;
					refractionCamera.transform.position = base.transform.position;
					refractionCamera.transform.rotation = base.transform.rotation;
					refractionCamera.gameObject.AddComponent<FlareLayer>();
					gameObject2.hideFlags = HideFlags.HideAndDontSave;
					this.m_RefractionCameras[currentCamera] = refractionCamera;
				}
			}
		}

		// Token: 0x060055E9 RID: 21993 RVA: 0x001B76BC File Offset: 0x001B5ABC
		private Water.WaterMode GetWaterMode()
		{
			if (this.m_HardwareWaterSupport < this.waterMode)
			{
				return this.m_HardwareWaterSupport;
			}
			return this.waterMode;
		}

		// Token: 0x060055EA RID: 21994 RVA: 0x001B76DC File Offset: 0x001B5ADC
		private Water.WaterMode FindHardwareWaterSupport()
		{
			if (!base.GetComponent<Renderer>())
			{
				return Water.WaterMode.Simple;
			}
			Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			if (!sharedMaterial)
			{
				return Water.WaterMode.Simple;
			}
			string tag = sharedMaterial.GetTag("WATERMODE", false);
			if (tag == "Refractive")
			{
				return Water.WaterMode.Refractive;
			}
			if (tag == "Reflective")
			{
				return Water.WaterMode.Reflective;
			}
			return Water.WaterMode.Simple;
		}

		// Token: 0x060055EB RID: 21995 RVA: 0x001B7748 File Offset: 0x001B5B48
		private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
		{
			Vector3 point = pos + normal * this.clipPlaneOffset;
			Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
			Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
			Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
			return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
		}

		// Token: 0x060055EC RID: 21996 RVA: 0x001B77B4 File Offset: 0x001B5BB4
		private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
		{
			reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
			reflectionMat.m01 = -2f * plane[0] * plane[1];
			reflectionMat.m02 = -2f * plane[0] * plane[2];
			reflectionMat.m03 = -2f * plane[3] * plane[0];
			reflectionMat.m10 = -2f * plane[1] * plane[0];
			reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
			reflectionMat.m12 = -2f * plane[1] * plane[2];
			reflectionMat.m13 = -2f * plane[3] * plane[1];
			reflectionMat.m20 = -2f * plane[2] * plane[0];
			reflectionMat.m21 = -2f * plane[2] * plane[1];
			reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
			reflectionMat.m23 = -2f * plane[3] * plane[2];
			reflectionMat.m30 = 0f;
			reflectionMat.m31 = 0f;
			reflectionMat.m32 = 0f;
			reflectionMat.m33 = 1f;
		}

		// Token: 0x0400368A RID: 13962
		public Water.WaterMode waterMode = Water.WaterMode.Refractive;

		// Token: 0x0400368B RID: 13963
		public bool disablePixelLights = true;

		// Token: 0x0400368C RID: 13964
		public int textureSize = 256;

		// Token: 0x0400368D RID: 13965
		public float clipPlaneOffset = 0.07f;

		// Token: 0x0400368E RID: 13966
		public LayerMask reflectLayers = -1;

		// Token: 0x0400368F RID: 13967
		public LayerMask refractLayers = -1;

		// Token: 0x04003690 RID: 13968
		private Dictionary<Camera, Camera> m_ReflectionCameras = new Dictionary<Camera, Camera>();

		// Token: 0x04003691 RID: 13969
		private Dictionary<Camera, Camera> m_RefractionCameras = new Dictionary<Camera, Camera>();

		// Token: 0x04003692 RID: 13970
		private RenderTexture m_ReflectionTexture;

		// Token: 0x04003693 RID: 13971
		private RenderTexture m_RefractionTexture;

		// Token: 0x04003694 RID: 13972
		private Water.WaterMode m_HardwareWaterSupport = Water.WaterMode.Refractive;

		// Token: 0x04003695 RID: 13973
		private int m_OldReflectionTextureSize;

		// Token: 0x04003696 RID: 13974
		private int m_OldRefractionTextureSize;

		// Token: 0x04003697 RID: 13975
		private static bool s_InsideWater;

		// Token: 0x02000B9E RID: 2974
		public enum WaterMode
		{
			// Token: 0x04003699 RID: 13977
			Simple,
			// Token: 0x0400369A RID: 13978
			Reflective,
			// Token: 0x0400369B RID: 13979
			Refractive
		}
	}
}
