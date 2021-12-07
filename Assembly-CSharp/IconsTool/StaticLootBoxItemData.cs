using System;
using System.Xml.Serialization;
using Commerce;

namespace IconsTool
{
	// Token: 0x020006FB RID: 1787
	public class StaticLootBoxItemData
	{
		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06002C9F RID: 11423 RVA: 0x000E597A File Offset: 0x000E3D7A
		// (set) Token: 0x06002CA0 RID: 11424 RVA: 0x000E5982 File Offset: 0x000E3D82
		[XmlIgnore]
		public ECurrencyType m_creditCurrencyType
		{
			get
			{
				return (ECurrencyType)this.m_currencyTypeAsInt;
			}
			set
			{
				this.m_currencyTypeAsInt = (uint)value;
			}
		}

		// Token: 0x04001FAA RID: 8106
		[XmlElement("ItemId")]
		public ulong m_itemId;

		// Token: 0x04001FAB RID: 8107
		[XmlElement("CurrencyType")]
		public uint m_currencyTypeAsInt;

		// Token: 0x04001FAC RID: 8108
		[XmlElement("CreditValue")]
		public ulong m_creditCurrencyValue;

		// Token: 0x04001FAD RID: 8109
		[XmlElement("Flags")]
		public ulong m_flags;
	}
}
