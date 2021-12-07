using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Commerce;

namespace IconsTool
{
	// Token: 0x020006F9 RID: 1785
	public class StaticPackageData : StaticDataBase
	{
		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06002C99 RID: 11417 RVA: 0x000E593D File Offset: 0x000E3D3D
		// (set) Token: 0x06002C9A RID: 11418 RVA: 0x000E5945 File Offset: 0x000E3D45
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

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06002C9B RID: 11419 RVA: 0x000E594E File Offset: 0x000E3D4E
		// (set) Token: 0x06002C9C RID: 11420 RVA: 0x000E5956 File Offset: 0x000E3D56
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

		// Token: 0x04001FA5 RID: 8101
		[XmlElement("CurrencyType")]
		public uint m_currencyTypeAsInt;

		// Token: 0x04001FA6 RID: 8102
		[XmlElement("State")]
		public uint m_stateAsInt;

		// Token: 0x04001FA7 RID: 8103
		[XmlElement("StateTimeStamp")]
		public DateTime m_stateTimeStamp = default(DateTime);

		// Token: 0x04001FA8 RID: 8104
		[XmlElement("StaticPackageItems")]
		public List<StaticPackageItemData> m_packageItemList = new List<StaticPackageItemData>();
	}
}
