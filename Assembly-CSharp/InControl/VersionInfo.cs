using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InControl
{
	// Token: 0x020001DA RID: 474
	public struct VersionInfo : IComparable<VersionInfo>
	{
		// Token: 0x06000861 RID: 2145 RVA: 0x0004BA2C File Offset: 0x00049E2C
		public VersionInfo(int major, int minor, int patch, int build)
		{
			this.Major = major;
			this.Minor = minor;
			this.Patch = patch;
			this.Build = build;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0004BA4C File Offset: 0x00049E4C
		public static VersionInfo InControlVersion()
		{
			return new VersionInfo
			{
				Major = 1,
				Minor = 6,
				Patch = 16,
				Build = 9119
			};
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0004BA88 File Offset: 0x00049E88
		public static VersionInfo UnityVersion()
		{
			Match match = Regex.Match(Application.unityVersion, "^(\\d+)\\.(\\d+)\\.(\\d+)");
			int build = 0;
			return new VersionInfo
			{
				Major = Convert.ToInt32(match.Groups[1].Value),
				Minor = Convert.ToInt32(match.Groups[2].Value),
				Patch = Convert.ToInt32(match.Groups[3].Value),
				Build = build
			};
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x0004BB0F File Offset: 0x00049F0F
		public static VersionInfo Min
		{
			get
			{
				return new VersionInfo(int.MinValue, int.MinValue, int.MinValue, int.MinValue);
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0004BB2A File Offset: 0x00049F2A
		public static VersionInfo Max
		{
			get
			{
				return new VersionInfo(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0004BB48 File Offset: 0x00049F48
		public int CompareTo(VersionInfo other)
		{
			if (this.Major < other.Major)
			{
				return -1;
			}
			if (this.Major > other.Major)
			{
				return 1;
			}
			if (this.Minor < other.Minor)
			{
				return -1;
			}
			if (this.Minor > other.Minor)
			{
				return 1;
			}
			if (this.Patch < other.Patch)
			{
				return -1;
			}
			if (this.Patch > other.Patch)
			{
				return 1;
			}
			if (this.Build < other.Build)
			{
				return -1;
			}
			if (this.Build > other.Build)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0004BBF6 File Offset: 0x00049FF6
		public static bool operator ==(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) == 0;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0004BC03 File Offset: 0x0004A003
		public static bool operator !=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) != 0;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0004BC13 File Offset: 0x0004A013
		public static bool operator <=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) <= 0;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0004BC23 File Offset: 0x0004A023
		public static bool operator >=(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) >= 0;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0004BC33 File Offset: 0x0004A033
		public static bool operator <(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) < 0;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0004BC40 File Offset: 0x0004A040
		public static bool operator >(VersionInfo a, VersionInfo b)
		{
			return a.CompareTo(b) > 0;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0004BC4D File Offset: 0x0004A04D
		public override bool Equals(object other)
		{
			return other is VersionInfo && this == (VersionInfo)other;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0004BC70 File Offset: 0x0004A070
		public override int GetHashCode()
		{
			return this.Major.GetHashCode() ^ this.Minor.GetHashCode() ^ this.Patch.GetHashCode() ^ this.Build.GetHashCode();
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0004BCC4 File Offset: 0x0004A0C4
		public override string ToString()
		{
			if (this.Build == 0)
			{
				return string.Format("{0}.{1}.{2}", this.Major, this.Minor, this.Patch);
			}
			return string.Format("{0}.{1}.{2} build {3}", new object[]
			{
				this.Major,
				this.Minor,
				this.Patch,
				this.Build
			});
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0004BD50 File Offset: 0x0004A150
		public string ToShortString()
		{
			if (this.Build == 0)
			{
				return string.Format("{0}.{1}.{2}", this.Major, this.Minor, this.Patch);
			}
			return string.Format("{0}.{1}.{2}b{3}", new object[]
			{
				this.Major,
				this.Minor,
				this.Patch,
				this.Build
			});
		}

		// Token: 0x040005EA RID: 1514
		public int Major;

		// Token: 0x040005EB RID: 1515
		public int Minor;

		// Token: 0x040005EC RID: 1516
		public int Patch;

		// Token: 0x040005ED RID: 1517
		public int Build;
	}
}
