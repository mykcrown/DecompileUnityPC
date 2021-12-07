// Decompile from assembly: Assembly-CSharp.dll

using Commerce;
using IconsServer;
using System;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticItemCurrency : StaticItemBase
	{
		[XmlElement("CurrencyType")]
		public uint m_currencyTypeAsInt;

		[XmlElement("CurrencyAmount")]
		public ulong m_currencyAmount;

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

		internal StaticItemCurrency()
		{
			base.m_itemType = EItemType.Currency;
			this.m_currencyType = ECurrencyType.Soft;
		}
	}
}
