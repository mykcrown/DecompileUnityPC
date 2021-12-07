// Decompile from assembly: Assembly-CSharp.dll

using Commerce;
using System;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticLootBoxItemData
	{
		[XmlElement("ItemId")]
		public ulong m_itemId;

		[XmlElement("CurrencyType")]
		public uint m_currencyTypeAsInt;

		[XmlElement("CreditValue")]
		public ulong m_creditCurrencyValue;

		[XmlElement("Flags")]
		public ulong m_flags;

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
	}
}
