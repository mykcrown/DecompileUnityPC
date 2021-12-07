using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IconsTool
{
	// Token: 0x020006FD RID: 1789
	public class StaticLootBoxDataList
	{
		// Token: 0x04001FB0 RID: 8112
		[XmlElement("StaticLootBox")]
		public List<StaticLootBoxData> m_lootBoxList = new List<StaticLootBoxData>();
	}
}
