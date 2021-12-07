// Decompile from assembly: Assembly-CSharp.dll

using Commerce;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticLootBoxData : StaticDataBase
	{
		[XmlElement("LootBoxType")]
		public uint m_lootBoxTypeAsInt;

		[XmlElement("StaticLootBoxItems")]
		public List<StaticLootBoxItemData> m_lootBoxItemList = new List<StaticLootBoxItemData>();

		[XmlIgnore]
		public ELootBoxType m_lootBoxType
		{
			get
			{
				return (ELootBoxType)this.m_lootBoxTypeAsInt;
			}
			set
			{
				this.m_lootBoxTypeAsInt = (uint)value;
			}
		}
	}
}
