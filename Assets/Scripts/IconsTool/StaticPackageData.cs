// Decompile from assembly: Assembly-CSharp.dll

using Commerce;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticPackageData : StaticDataBase
	{
		[XmlElement("CurrencyType")]
		public uint m_currencyTypeAsInt;

		[XmlElement("State")]
		public uint m_stateAsInt;

		[XmlElement("StateTimeStamp")]
		public DateTime m_stateTimeStamp = default(DateTime);

		[XmlElement("StaticPackageItems")]
		public List<StaticPackageItemData> m_packageItemList = new List<StaticPackageItemData>();

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

		[XmlIgnore]
		public EPackageState m_state
		{
			get
			{
				return (EPackageState)this.m_stateAsInt;
			}
			set
			{
				this.m_stateAsInt = (uint)value;
			}
		}
	}
}
