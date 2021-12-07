using System;
using System.IO;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InControl
{
	// Token: 0x020001D9 RID: 473
	public static class Utility
	{
		// Token: 0x06000833 RID: 2099 RVA: 0x0004AEAC File Offset: 0x000492AC
		public static void DrawCircleGizmo(Vector2 center, float radius)
		{
			Vector2 v = Utility.circleVertexList[0] * radius + center;
			int num = Utility.circleVertexList.Length;
			for (int i = 1; i < num; i++)
			{
				Gizmos.DrawLine(v, v = Utility.circleVertexList[i] * radius + center);
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0004AF1E File Offset: 0x0004931E
		public static void DrawCircleGizmo(Vector2 center, float radius, Color color)
		{
			Gizmos.color = color;
			Utility.DrawCircleGizmo(center, radius);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0004AF30 File Offset: 0x00049330
		public static void DrawOvalGizmo(Vector2 center, Vector2 size)
		{
			Vector2 b = size / 2f;
			Vector2 v = Vector2.Scale(Utility.circleVertexList[0], b) + center;
			int num = Utility.circleVertexList.Length;
			for (int i = 1; i < num; i++)
			{
				Gizmos.DrawLine(v, v = Vector2.Scale(Utility.circleVertexList[i], b) + center);
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0004AFAE File Offset: 0x000493AE
		public static void DrawOvalGizmo(Vector2 center, Vector2 size, Color color)
		{
			Gizmos.color = color;
			Utility.DrawOvalGizmo(center, size);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0004AFC0 File Offset: 0x000493C0
		public static void DrawRectGizmo(Rect rect)
		{
			Vector3 vector = new Vector3(rect.xMin, rect.yMin);
			Vector3 vector2 = new Vector3(rect.xMax, rect.yMin);
			Vector3 vector3 = new Vector3(rect.xMax, rect.yMax);
			Vector3 vector4 = new Vector3(rect.xMin, rect.yMax);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector4);
			Gizmos.DrawLine(vector4, vector);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0004B03D File Offset: 0x0004943D
		public static void DrawRectGizmo(Rect rect, Color color)
		{
			Gizmos.color = color;
			Utility.DrawRectGizmo(rect);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0004B04C File Offset: 0x0004944C
		public static void DrawRectGizmo(Vector2 center, Vector2 size)
		{
			float num = size.x / 2f;
			float num2 = size.y / 2f;
			Vector3 vector = new Vector3(center.x - num, center.y - num2);
			Vector3 vector2 = new Vector3(center.x + num, center.y - num2);
			Vector3 vector3 = new Vector3(center.x + num, center.y + num2);
			Vector3 vector4 = new Vector3(center.x - num, center.y + num2);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector4);
			Gizmos.DrawLine(vector4, vector);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0004B0F9 File Offset: 0x000494F9
		public static void DrawRectGizmo(Vector2 center, Vector2 size, Color color)
		{
			Gizmos.color = color;
			Utility.DrawRectGizmo(center, size);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0004B108 File Offset: 0x00049508
		public static bool GameObjectIsCulledOnCurrentCamera(GameObject gameObject)
		{
			return (Camera.current.cullingMask & 1 << gameObject.layer) == 0;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0004B124 File Offset: 0x00049524
		public static Color MoveColorTowards(Color color0, Color color1, float maxDelta)
		{
			float r = Mathf.MoveTowards(color0.r, color1.r, maxDelta);
			float g = Mathf.MoveTowards(color0.g, color1.g, maxDelta);
			float b = Mathf.MoveTowards(color0.b, color1.b, maxDelta);
			float a = Mathf.MoveTowards(color0.a, color1.a, maxDelta);
			return new Color(r, g, b, a);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0004B190 File Offset: 0x00049590
		public static float ApplyDeadZone(float value, float lowerDeadZone, float upperDeadZone)
		{
			if (value < 0f)
			{
				if (value > -lowerDeadZone)
				{
					return 0f;
				}
				if (value < -upperDeadZone)
				{
					return -1f;
				}
				return (value + lowerDeadZone) / (upperDeadZone - lowerDeadZone);
			}
			else
			{
				if (value < lowerDeadZone)
				{
					return 0f;
				}
				if (value > upperDeadZone)
				{
					return 1f;
				}
				return (value - lowerDeadZone) / (upperDeadZone - lowerDeadZone);
			}
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0004B1ED File Offset: 0x000495ED
		public static Vector2 ApplySeparateDeadZone(float x, float y, float lowerDeadZone, float upperDeadZone)
		{
			return new Vector2(Utility.ApplyDeadZone(x, lowerDeadZone, upperDeadZone), Utility.ApplyDeadZone(y, lowerDeadZone, upperDeadZone));
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0004B204 File Offset: 0x00049604
		public static Vector2 ApplyCircularDeadZone(Vector2 v, float lowerDeadZone, float upperDeadZone)
		{
			float magnitude = v.magnitude;
			if (magnitude < lowerDeadZone)
			{
				return Vector2.zero;
			}
			if (magnitude > upperDeadZone)
			{
				return v.normalized;
			}
			return v.normalized * ((magnitude - lowerDeadZone) / (upperDeadZone - lowerDeadZone));
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0004B248 File Offset: 0x00049648
		public static Vector2 ApplyCircularDeadZone(float x, float y, float lowerDeadZone, float upperDeadZone)
		{
			return Utility.ApplyCircularDeadZone(new Vector2(x, y), lowerDeadZone, upperDeadZone);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0004B258 File Offset: 0x00049658
		public static float ApplySmoothing(float thisValue, float lastValue, float deltaTime, float sensitivity)
		{
			if (Utility.Approximately(sensitivity, 1f))
			{
				return thisValue;
			}
			float maxDelta = deltaTime * sensitivity * 100f;
			if (Utility.IsNotZero(thisValue) && Mathf.Sign(lastValue) != Mathf.Sign(thisValue))
			{
				lastValue = 0f;
			}
			return Mathf.MoveTowards(lastValue, thisValue, maxDelta);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0004B2AC File Offset: 0x000496AC
		public static float ApplySnapping(float value, float threshold)
		{
			if (value < -threshold)
			{
				return -1f;
			}
			if (value > threshold)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0004B2CE File Offset: 0x000496CE
		internal static bool TargetIsButton(InputControlType target)
		{
			return (target >= InputControlType.Action1 && target <= InputControlType.Action12) || (target >= InputControlType.Button0 && target <= InputControlType.Button19);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0004B2FC File Offset: 0x000496FC
		internal static bool TargetIsStandard(InputControlType target)
		{
			return (target >= InputControlType.LeftStickUp && target <= InputControlType.Action12) || (target >= InputControlType.Command && target <= InputControlType.DPadY);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0004B329 File Offset: 0x00049729
		internal static bool TargetIsAlias(InputControlType target)
		{
			return target >= InputControlType.Command && target <= InputControlType.DPadY;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0004B344 File Offset: 0x00049744
		public static string ReadFromFile(string path)
		{
			StreamReader streamReader = new StreamReader(path);
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			return result;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0004B368 File Offset: 0x00049768
		public static void WriteToFile(string path, string data)
		{
			StreamWriter streamWriter = new StreamWriter(path);
			streamWriter.Write(data);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0004B38F File Offset: 0x0004978F
		public static float Abs(float value)
		{
			return (value >= 0f) ? value : (-value);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0004B3A4 File Offset: 0x000497A4
		public static bool Approximately(float v1, float v2)
		{
			float num = v1 - v2;
			return num >= -1E-07f && num <= 1E-07f;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0004B3CE File Offset: 0x000497CE
		public static bool Approximately(Vector2 v1, Vector2 v2)
		{
			return Utility.Approximately(v1.x, v2.x) && Utility.Approximately(v1.y, v2.y);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0004B3FE File Offset: 0x000497FE
		public static bool IsNotZero(float value)
		{
			return value < -1E-07f || value > 1E-07f;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0004B416 File Offset: 0x00049816
		public static bool IsZero(float value)
		{
			return value >= -1E-07f && value <= 1E-07f;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0004B431 File Offset: 0x00049831
		public static bool AbsoluteIsOverThreshold(float value, float threshold)
		{
			return value < -threshold || value > threshold;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0004B442 File Offset: 0x00049842
		public static float NormalizeAngle(float angle)
		{
			while (angle < 0f)
			{
				angle += 360f;
			}
			while (angle > 360f)
			{
				angle -= 360f;
			}
			return angle;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0004B478 File Offset: 0x00049878
		public static float VectorToAngle(Vector2 vector)
		{
			if (Utility.IsZero(vector.x) && Utility.IsZero(vector.y))
			{
				return 0f;
			}
			return Utility.NormalizeAngle(Mathf.Atan2(vector.x, vector.y) * 57.29578f);
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0004B4CB File Offset: 0x000498CB
		public static float Min(float v0, float v1)
		{
			return (v0 < v1) ? v0 : v1;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0004B4DB File Offset: 0x000498DB
		public static float Max(float v0, float v1)
		{
			return (v0 > v1) ? v0 : v1;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0004B4EC File Offset: 0x000498EC
		public static float Min(float v0, float v1, float v2, float v3)
		{
			float num = (v0 < v1) ? v0 : v1;
			float num2 = (v2 < v3) ? v2 : v3;
			return (num < num2) ? num : num2;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0004B528 File Offset: 0x00049928
		public static float Max(float v0, float v1, float v2, float v3)
		{
			float num = (v0 > v1) ? v0 : v1;
			float num2 = (v2 > v3) ? v2 : v3;
			return (num > num2) ? num : num2;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0004B564 File Offset: 0x00049964
		internal static float ValueFromSides(float negativeSide, float positiveSide)
		{
			float num = Utility.Abs(negativeSide);
			float num2 = Utility.Abs(positiveSide);
			if (Utility.Approximately(num, num2))
			{
				return 0f;
			}
			return (num <= num2) ? num2 : (-num);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0004B5A0 File Offset: 0x000499A0
		internal static float ValueFromSides(float negativeSide, float positiveSide, bool invertSides)
		{
			if (invertSides)
			{
				return Utility.ValueFromSides(positiveSide, negativeSide);
			}
			return Utility.ValueFromSides(negativeSide, positiveSide);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0004B5B7 File Offset: 0x000499B7
		public static void ArrayResize<T>(ref T[] array, int capacity)
		{
			if (array == null || capacity > array.Length)
			{
				Array.Resize<T>(ref array, Utility.NextPowerOfTwo(capacity));
			}
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0004B5D6 File Offset: 0x000499D6
		public static void ArrayExpand<T>(ref T[] array, int capacity)
		{
			if (array == null || capacity > array.Length)
			{
				array = new T[Utility.NextPowerOfTwo(capacity)];
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0004B5F6 File Offset: 0x000499F6
		public static int NextPowerOfTwo(int value)
		{
			if (value > 0)
			{
				value--;
				value |= value >> 1;
				value |= value >> 2;
				value |= value >> 4;
				value |= value >> 8;
				value |= value >> 16;
				value++;
				return value;
			}
			return 0;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0004B630 File Offset: 0x00049A30
		internal static bool Is32Bit
		{
			get
			{
				return IntPtr.Size == 4;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x0004B63A File Offset: 0x00049A3A
		internal static bool Is64Bit
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0004B644 File Offset: 0x00049A44
		public static string HKLM_GetString(string path, string key)
		{
			string result;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(path);
				if (registryKey == null)
				{
					result = string.Empty;
				}
				else
				{
					result = (string)registryKey.GetValue(key);
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0004B69C File Offset: 0x00049A9C
		public static string GetWindowsVersion()
		{
			string text = Utility.HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName");
			if (text != null)
			{
				string text2 = Utility.HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CSDVersion");
				string text3 = (!Utility.Is32Bit) ? "64Bit" : "32Bit";
				int systemBuildNumber = Utility.GetSystemBuildNumber();
				return string.Concat(new object[]
				{
					text,
					(text2 == null) ? string.Empty : (" " + text2),
					" ",
					text3,
					" Build ",
					systemBuildNumber
				});
			}
			return SystemInfo.operatingSystem;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0004B73F File Offset: 0x00049B3F
		public static int GetSystemBuildNumber()
		{
			return Environment.OSVersion.Version.Build;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0004B750 File Offset: 0x00049B50
		internal static void LoadScene(string sceneName)
		{
			SceneManager.LoadScene(sceneName);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0004B758 File Offset: 0x00049B58
		internal static string PluginFileExtension()
		{
			return ".dll";
		}

		// Token: 0x040005E8 RID: 1512
		public const float Epsilon = 1E-07f;

		// Token: 0x040005E9 RID: 1513
		private static Vector2[] circleVertexList = new Vector2[]
		{
			new Vector2(0f, 1f),
			new Vector2(0.2588f, 0.9659f),
			new Vector2(0.5f, 0.866f),
			new Vector2(0.7071f, 0.7071f),
			new Vector2(0.866f, 0.5f),
			new Vector2(0.9659f, 0.2588f),
			new Vector2(1f, 0f),
			new Vector2(0.9659f, -0.2588f),
			new Vector2(0.866f, -0.5f),
			new Vector2(0.7071f, -0.7071f),
			new Vector2(0.5f, -0.866f),
			new Vector2(0.2588f, -0.9659f),
			new Vector2(0f, -1f),
			new Vector2(-0.2588f, -0.9659f),
			new Vector2(-0.5f, -0.866f),
			new Vector2(-0.7071f, -0.7071f),
			new Vector2(-0.866f, -0.5f),
			new Vector2(-0.9659f, -0.2588f),
			new Vector2(-1f, --0f),
			new Vector2(-0.9659f, 0.2588f),
			new Vector2(-0.866f, 0.5f),
			new Vector2(-0.7071f, 0.7071f),
			new Vector2(-0.5f, 0.866f),
			new Vector2(-0.2588f, 0.9659f),
			new Vector2(0f, 1f)
		};
	}
}
