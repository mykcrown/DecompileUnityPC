using System;
using System.Xml.Serialization;
using Commerce;
using IconsServer;

namespace IconsTool
{
	// Token: 0x020006F6 RID: 1782
	public class StaticItemCurrency : StaticItemBase
	{
		// Token: 0x06002C90 RID: 11408 RVA: 0x000E57C0 File Offset: 0x000E3BC0
		internal StaticItemCurrency()
		{
			base.m_itemType = EItemType.Currency;
			this.m_currencyType = ECurrencyType.Soft;
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06002C91 RID: 11409 RVA: 0x000E57D7 File Offset: 0x000E3BD7
		// (set) Token: 0x06002C92 RID: 11410 RVA: 0x000E57DF File Offset: 0x000E3BDF
		[XmlIgnore]
		public ECurrencyType m_currencyType
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

		// Token: 0x04001F91 RID: 8081
		[XmlElement("CurrencyType")]
		public uint m_currencyTypeAsInt;

		// Token: 0x04001F92 RID: 8082
		[XmlElement("CurrencyAmount")]
		public ulong m_currencyAmount;
	}
}
