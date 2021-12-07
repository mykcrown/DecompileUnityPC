using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000B9B RID: 2971
	[ExecuteInEditMode]
	[RequireComponent(typeof(WaterBase))]
	public class PlanarReflection : MonoBehaviour
	{
		// Token: 0x060055D1 RID: 21969 RVA: 0x001B63C5 File Offset: 0x001B47C5
		public void Start()
		{
			this.m_SharedMaterial = ((WaterBase)base.gameObject.GetComponent(typeof(WaterBase))).sharedMaterial;
		}

		// Token: 0x060055D2 RID: 21970 RVA: 0x001B63EC File Offset: 0x001B47EC
		private Camera CreateReflectionCameraFor(Camera cam)
		{
			string name = base.gameObject.name + "Reflection" + cam.name;
			GameObject gameObject = GameObject.Find(name);
			if (!gameObject)
			{
				gameObject = new GameObject(name, new Type[]
				{
					typeof(Camera)
				});
			}
			if (!gameObject.GetComponent(typeof(Camera)))
			{
				gameObject.AddComponent(typeof(Camera));
			}
			Camera component = gameObject.GetComponent<Camera>();
			component.backgroundColor = this.clearColor;
			component.clearFlags = ((!this.reflectSkybox) ? CameraClearFlags.Color : CameraClearFlags.Skybox);
			this.SetStandardCameraParameter(component, this.reflectionMask);
			if (!component.targetTexture)
			{
				component.targetTexture = this.CreateTextureFor(cam);
			}
			return component;
		}

		// Token: 0x060055D3 RID: 21971 RVA: 0x001B64C2 File Offset: 0x001B48C2
		private void SetStandardCameraParameter(Camera cam, LayerMask mask)
		{
			cam.cullingMask = (mask & ~(1 << LayerMask.NameToLayer("Water")));
			cam.backgroundColor = Color.black;
			cam.enabled = false;
		}

		// Token: 0x060055D4 RID: 21972 RVA: 0x001B64F4 File Offset: 0x001B48F4
		private RenderTexture CreateTextureFor(Camera cam)
		{
			return new RenderTexture(Mathf.FloorToInt((float)cam.pixelWidth * 0.5f), Mathf.FloorToInt((float)cam.pixelHeight * 0.5f), 24)
			{
				hideFlags = HideFlags.DontSave
			};
		}

		// Token: 0x060055D5 RID: 21973 RVA: 0x001B6538 File Offset: 0x001B4938
		public void RenderHelpCameras(Camera currentCam)
		{
			if (this.m_HelperCameras == null)
			{
				this.m_HelperCameras = new Dictionary<Camera, bool>();
			}
			if (!this.m_HelperCameras.ContainsKey(currentCam))
			{
				this.m_HelperCameras.Add(currentCam, false);
			}
			if (this.m_HelperCameras[currentCam])
			{
				return;
			}
			if (!this.m_ReflectionCamera)
			{
				this.m_ReflectionCamera = this.CreateReflectionCameraFor(currentCam);
			}
			this.RenderReflectionFor(currentCam, this.m_ReflectionCamera);
			this.m_HelperCameras[currentCam] = true;
		}

		// Token: 0x060055D6 RID: 21974 RVA: 0x001B65C2 File Offset: 0x001B49C2
		public void LateUpdate()
		{
			if (this.m_HelperCameras != null)
			{
				this.m_HelperCameras.Clear();
			}
		}

		// Token: 0x060055D7 RID: 21975 RVA: 0x001B65DC File Offset: 0x001B49DC
		public void WaterTileBeingRendered(Transform tr, Camera currentCam)
		{
			this.RenderHelpCameras(currentCam);
			if (this.m_ReflectionCamera && this.m_SharedMaterial)
			{
				this.m_SharedMaterial.SetTexture(this.reflectionSampler, this.m_ReflectionCamera.targetTexture);
			}
		}

		// Token: 0x060055D8 RID: 21976 RVA: 0x001B662C File Offset: 0x001B4A2C
		public void OnEnable()
		{
			Shader.EnableKeyword("WATER_REFLECTIVE");
			Shader.DisableKeyword("WATER_SIMPLE");
		}

		// Token: 0x060055D9 RID: 21977 RVA: 0x001B6642 File Offset: 0x001B4A42
		public void OnDisable()
		{
			Shader.EnableKeyword("WATER_SIMPLE");
			Shader.DisableKeyword("WATER_REFLECTIVE");
		}

		// Token: 0x060055DA RID: 21978 RVA: 0x001B6658 File Offset: 0x001B4A58
		private void RenderReflectionFor(Camera cam, Camera reflectCamera)
		{
			if (!reflectCamera)
			{
				return;
			}
			if (this.m_SharedMaterial && !this.m_SharedMaterial.HasProperty(this.reflectionSampler))
			{
				return;
			}
			reflectCamera.cullingMask = (this.reflectionMask & ~(1 << LayerMask.NameToLayer("Water")));
			this.SaneCameraSettings(reflectCamera);
			reflectCamera.backgroundColor = this.clearColor;
			reflectCamera.clearFlags = ((!this.reflectSkybox) ? CameraClearFlags.Color : CameraClearFlags.Skybox);
			if (this.reflectSkybox && cam.gameObject.GetComponent(typeof(Skybox)))
			{
				Skybox skybox = (Skybox)reflectCamera.gameObject.GetComponent(typeof(Skybox));
				if (!skybox)
				{
					skybox = (Skybox)reflectCamera.gameObject.AddComponent(typeof(Skybox));
				}
				skybox.material = ((Skybox)cam.GetComponent(typeof(Skybox))).material;
			}
			GL.invertCulling = true;
			Transform transform = base.transform;
			Vector3 eulerAngles = cam.transform.eulerAngles;
			reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
			reflectCamera.transform.position = cam.transform.position;
			Vector3 position = transform.transform.position;
			position.y = transform.position.y;
			Vector3 up = transform.transform.up;
			float w = -Vector3.Dot(up, position) - this.clipPlaneOffset;
			Vector4 plane = new Vector4(up.x, up.y, up.z, w);
			Matrix4x4 matrix4x = Matrix4x4.zero;
			matrix4x = PlanarReflection.CalculateReflectionMatrix(matrix4x, plane);
			this.m_Oldpos = cam.transform.position;
			Vector3 position2 = matrix4x.MultiplyPoint(this.m_Oldpos);
			reflectCamera.worldToCameraMatrix = cam.worldToCameraMatrix * matrix4x;
			Vector4 clipPlane = this.CameraSpacePlane(reflectCamera, position, up, 1f);
			Matrix4x4 matrix4x2 = cam.projectionMatrix;
			matrix4x2 = PlanarReflection.CalculateObliqueMatrix(matrix4x2, clipPlane);
			reflectCamera.projectionMatrix = matrix4x2;
			reflectCamera.transform.position = position2;
			Vector3 eulerAngles2 = cam.transform.eulerAngles;
			reflectCamera.transform.eulerAngles = new Vector3(-eulerAngles2.x, eulerAngles2.y, eulerAngles2.z);
			reflectCamera.Render();
			GL.invertCulling = false;
		}

		// Token: 0x060055DB RID: 21979 RVA: 0x001B68E1 File Offset: 0x001B4CE1
		private void SaneCameraSettings(Camera helperCam)
		{
			helperCam.depthTextureMode = DepthTextureMode.None;
			helperCam.backgroundColor = Color.black;
			helperCam.clearFlags = CameraClearFlags.Color;
			helperCam.renderingPath = RenderingPath.Forward;
		}

		// Token: 0x060055DC RID: 21980 RVA: 0x001B6904 File Offset: 0x001B4D04
		private static Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
		{
			Vector4 b = projection.inverse * new Vector4(PlanarReflection.Sgn(clipPlane.x), PlanarReflection.Sgn(clipPlane.y), 1f, 1f);
			Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
			projection[2] = vector.x - projection[3];
			projection[6] = vector.y - projection[7];
			projection[10] = vector.z - projection[11];
			projection[14] = vector.w - projection[15];
			return projection;
		}

		// Token: 0x060055DD RID: 21981 RVA: 0x001B69C0 File Offset: 0x001B4DC0
		private static Matrix4x4 CalculateReflectionMatrix(Matrix4x4 reflectionMat, Vector4 plane)
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
			return reflectionMat;
		}

		// Token: 0x060055DE RID: 21982 RVA: 0x001B6B78 File Offset: 0x001B4F78
		private static float Sgn(float a)
		{
			if (a > 0f)
			{
				return 1f;
			}
			if (a < 0f)
			{
				return -1f;
			}
			return 0f;
		}

		// Token: 0x060055DF RID: 21983 RVA: 0x001B6BA4 File Offset: 0x001B4FA4
		private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
		{
			Vector3 point = pos + normal * this.clipPlaneOffset;
			Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
			Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
			Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
			return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
		}

		// Token: 0x0400367F RID: 13951
		public LayerMask reflectionMask;

		// Token: 0x04003680 RID: 13952
		public bool reflectSkybox;

		// Token: 0x04003681 RID: 13953
		public Color clearColor = Color.grey;

		// Token: 0x04003682 RID: 13954
		public string reflectionSampler = "_ReflectionTex";

		// Token: 0x04003683 RID: 13955
		public float clipPlaneOffset = 0.07f;

		// Token: 0x04003684 RID: 13956
		private Vector3 m_Oldpos;

		// Token: 0x04003685 RID: 13957
		private Camera m_ReflectionCamera;

		// Token: 0x04003686 RID: 13958
		private Material m_SharedMaterial;

		// Token: 0x04003687 RID: 13959
		private Dictionary<Camera, bool> m_HelperCameras;
	}
}
