using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200007B RID: 123
	public abstract class InputDeviceProfile
	{
		// Token: 0x060004C0 RID: 1216 RVA: 0x00018D20 File Offset: 0x00017120
		public InputDeviceProfile()
		{
			this.Name = string.Empty;
			this.Meta = string.Empty;
			this.AnalogMappings = new InputControlMapping[0];
			this.ButtonMappings = new InputControlMapping[0];
			this.IncludePlatforms = new string[0];
			this.ExcludePlatforms = new string[0];
			this.MinSystemBuildNumber = 0;
			this.MaxSystemBuildNumber = 0;
			this.DeviceClass = InputDeviceClass.Unknown;
			this.DeviceStyle = InputDeviceStyle.Unknown;
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00018DAB File Offset: 0x000171AB
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x00018DB3 File Offset: 0x000171B3
		[SerializeField]
		public string Name { get; protected set; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00018DBC File Offset: 0x000171BC
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x00018DC4 File Offset: 0x000171C4
		[SerializeField]
		public string Meta { get; protected set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00018DCD File Offset: 0x000171CD
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x00018DD5 File Offset: 0x000171D5
		[SerializeField]
		public InputControlMapping[] AnalogMappings { get; protected set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00018DDE File Offset: 0x000171DE
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x00018DE6 File Offset: 0x000171E6
		[SerializeField]
		public InputControlMapping[] ButtonMappings { get; protected set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00018DEF File Offset: 0x000171EF
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x00018DF7 File Offset: 0x000171F7
		[SerializeField]
		public string[] IncludePlatforms { get; protected set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00018E00 File Offset: 0x00017200
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x00018E08 File Offset: 0x00017208
		[SerializeField]
		public string[] ExcludePlatforms { get; protected set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00018E11 File Offset: 0x00017211
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x00018E19 File Offset: 0x00017219
		[SerializeField]
		public int MaxSystemBuildNumber { get; protected set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00018E22 File Offset: 0x00017222
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x00018E2A File Offset: 0x0001722A
		[SerializeField]
		public int MinSystemBuildNumber { get; protected set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00018E33 File Offset: 0x00017233
		// (set) Token: 0x060004D2 RID: 1234 RVA: 0x00018E3B File Offset: 0x0001723B
		[SerializeField]
		public InputDeviceClass DeviceClass { get; protected set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00018E44 File Offset: 0x00017244
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00018E4C File Offset: 0x0001724C
		[SerializeField]
		public InputDeviceStyle DeviceStyle { get; protected set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00018E55 File Offset: 0x00017255
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00018E5D File Offset: 0x0001725D
		[SerializeField]
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			protected set
			{
				this.sensitivity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00018E6B File Offset: 0x0001726B
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00018E73 File Offset: 0x00017273
		[SerializeField]
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			protected set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00018E81 File Offset: 0x00017281
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x00018E89 File Offset: 0x00017289
		[SerializeField]
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			protected set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00018E97 File Offset: 0x00017297
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00018E9F File Offset: 0x0001729F
		[Obsolete("This property has been renamed to IncludePlatforms.", false)]
		public string[] SupportedPlatforms
		{
			get
			{
				return this.IncludePlatforms;
			}
			protected set
			{
				this.IncludePlatforms = value;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00018EA8 File Offset: 0x000172A8
		public virtual bool IsSupportedOnThisPlatform
		{
			get
			{
				int systemBuildNumber = Utility.GetSystemBuildNumber();
				if (this.MaxSystemBuildNumber > 0 && systemBuildNumber > this.MaxSystemBuildNumber)
				{
					return false;
				}
				if (this.MinSystemBuildNumber > 0 && systemBuildNumber < this.MinSystemBuildNumber)
				{
					return false;
				}
				if (this.ExcludePlatforms != null)
				{
					int num = this.ExcludePlatforms.Length;
					for (int i = 0; i < num; i++)
					{
						if (InputManager.Platform.Contains(this.ExcludePlatforms[i].ToUpper()))
						{
							return false;
						}
					}
				}
				if (this.IncludePlatforms == null || this.IncludePlatforms.Length == 0)
				{
					return true;
				}
				if (this.IncludePlatforms != null)
				{
					int num2 = this.IncludePlatforms.Length;
					for (int j = 0; j < num2; j++)
					{
						if (InputManager.Platform.Contains(this.IncludePlatforms[j].ToUpper()))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00018F97 File Offset: 0x00017397
		internal static void Hide(Type type)
		{
			InputDeviceProfile.hideList.Add(type);
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00018FA5 File Offset: 0x000173A5
		internal bool IsHidden
		{
			get
			{
				return InputDeviceProfile.hideList.Contains(base.GetType());
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00018FB7 File Offset: 0x000173B7
		public int AnalogCount
		{
			get
			{
				return this.AnalogMappings.Length;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00018FC1 File Offset: 0x000173C1
		public int ButtonCount
		{
			get
			{
				return this.ButtonMappings.Length;
			}
		}

		// Token: 0x04000407 RID: 1031
		private static HashSet<Type> hideList = new HashSet<Type>();

		// Token: 0x04000408 RID: 1032
		private float sensitivity = 1f;

		// Token: 0x04000409 RID: 1033
		private float lowerDeadZone;

		// Token: 0x0400040A RID: 1034
		private float upperDeadZone = 1f;
	}
}
