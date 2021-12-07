using System;
using System.Xml.Serialization;
using IconsServer;

namespace IconsTool
{
	// Token: 0x020006F8 RID: 1784
	public class StaticPackageItemData
	{
		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06002C95 RID: 11413 RVA: 0x000E589D File Offset: 0x000E3C9D
		// (set) Token: 0x06002C96 RID: 11414 RVA: 0x000E58A5 File Offset: 0x000E3CA5
		[XmlIgnore]
		public EClassification m_classificationType
		{
			get
			{
				return (EClassification)this.m_classificationTypeAsInt;
			}
			set
			{
				this.m_classificationTypeAsInt = (uint)value;
			}
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x000E58B0 File Offset: 0x000E3CB0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Type:",
				this.m_classificationType,
				"StaticId:",
				this.m_staticId,
				" Cost:",
				this.m_currencyCost.ToString()
			});
		}

		// Token: 0x04001FA1 RID: 8097
		[XmlElement("ClassificationType")]
		public uint m_classificationTypeAsInt;

		// Token: 0x04001FA2 RID: 8098
		[XmlElement("StaticId")]
		public ulong m_staticId;

		// Token: 0x04001FA3 RID: 8099
		[XmlElement("CurrencyCost")]
		public uint m_currencyCost;

		// Token: 0x04001FA4 RID: 8100
		[XmlElement("CreditCurrencyValue")]
		public uint m_creditCurrencyValue;
	}
}
