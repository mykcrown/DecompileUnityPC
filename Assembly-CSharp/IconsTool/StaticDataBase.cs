using System;
using System.Xml.Serialization;

namespace IconsTool
{
	// Token: 0x020006E6 RID: 1766
	public class StaticDataBase
	{
		// Token: 0x06002C75 RID: 11381 RVA: 0x000E5679 File Offset: 0x000E3A79
		public override string ToString()
		{
			return this.m_friendlyName;
		}

		// Token: 0x04001F84 RID: 8068
		[XmlElement("ID")]
		public ulong m_ID;

		// Token: 0x04001F85 RID: 8069
		[XmlElement("Version")]
		public uint m_version;

		// Token: 0x04001F86 RID: 8070
		[XmlElement("FriendlyName")]
		public string m_friendlyName = "StaticItemBase";

		// Token: 0x04001F87 RID: 8071
		[XmlElement("Description")]
		public string m_description = string.Empty;
	}
}
