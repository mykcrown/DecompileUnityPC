// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticData
	{
		[XmlElement("Version")]
		public ulong m_version = StaticDataManager.skConfigVersion;

		[XmlElement("Checksum")]
		public ulong m_checksum;

		[XmlElement("Items")]
		public StaticDataItemList m_items = new StaticDataItemList();

		[XmlElement("Packages")]
		public StaticPackageDataList m_packages = new StaticPackageDataList();

		[XmlElement("LootBoxes")]
		public StaticLootBoxDataList m_lootBoxes = new StaticLootBoxDataList();
	}
}
