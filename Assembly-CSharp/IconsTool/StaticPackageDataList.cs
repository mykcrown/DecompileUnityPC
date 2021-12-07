using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IconsTool
{
	// Token: 0x020006FA RID: 1786
	public class StaticPackageDataList
	{
		// Token: 0x04001FA9 RID: 8105
		[XmlElement("StaticPackage")]
		public List<StaticPackageData> m_packageList = new List<StaticPackageData>();
	}
}
