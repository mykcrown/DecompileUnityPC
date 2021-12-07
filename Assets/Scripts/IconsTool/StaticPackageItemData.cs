// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticPackageItemData
	{
		[XmlElement("ClassificationType")]
		public uint m_classificationTypeAsInt;

		[XmlElement("StaticId")]
		public ulong m_staticId;

		[XmlElement("CurrencyCost")]
		public uint m_currencyCost;

		[XmlElement("CreditCurrencyValue")]
		public uint m_creditCurrencyValue;

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
	}
}
