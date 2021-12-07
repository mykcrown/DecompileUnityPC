using System;
using System.Xml.Serialization;

namespace IconsTool
{
	// Token: 0x020006FE RID: 1790
	public class StaticData
	{
		// Token: 0x04001FB1 RID: 8113
		[XmlElement("Version")]
		public ulong m_version = StaticDataManager.skConfigVersion;

		// Token: 0x04001FB2 RID: 8114
		[XmlElement("Checksum")]
		public ulong m_checksum;

		// Token: 0x04001FB3 RID: 8115
		[XmlElement("Items")]
		public StaticDataItemList m_items = new StaticDataItemList();

		// Token: 0x04001FB4 RID: 8116
		[XmlElement("Packages")]
		public StaticPackageDataList m_packages = new StaticPackageDataList();

		// Token: 0x04001FB5 RID: 8117
		[XmlElement("LootBoxes")]
		public StaticLootBoxDataList m_lootBoxes = new StaticLootBoxDataList();
	}
}
