// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticDataBase
	{
		[XmlElement("ID")]
		public ulong m_ID;

		[XmlElement("Version")]
		public uint m_version;

		[XmlElement("FriendlyName")]
		public string m_friendlyName = "StaticItemBase";

		[XmlElement("Description")]
		public string m_description = string.Empty;

		public override string ToString()
		{
			return this.m_friendlyName;
		}
	}
}
