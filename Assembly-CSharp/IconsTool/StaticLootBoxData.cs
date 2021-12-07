using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Commerce;

namespace IconsTool
{
	// Token: 0x020006FC RID: 1788
	public class StaticLootBoxData : StaticDataBase
	{
		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x000E599E File Offset: 0x000E3D9E
		// (set) Token: 0x06002CA3 RID: 11427 RVA: 0x000E59A6 File Offset: 0x000E3DA6
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

		// Token: 0x04001FAE RID: 8110
		[XmlElement("LootBoxType")]
		public uint m_lootBoxTypeAsInt;

		// Token: 0x04001FAF RID: 8111
		[XmlElement("StaticLootBoxItems")]
		public List<StaticLootBoxItemData> m_lootBoxItemList = new List<StaticLootBoxItemData>();
	}
}
