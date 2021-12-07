using System;
using System.Xml.Serialization;
using IconsServer;

namespace IconsTool
{
	// Token: 0x020006F5 RID: 1781
	public class StaticItemUnlockToken : StaticItemBase
	{
		// Token: 0x06002C8D RID: 11405 RVA: 0x000E579F File Offset: 0x000E3B9F
		internal StaticItemUnlockToken()
		{
			base.m_itemType = EItemType.UnlockToken;
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06002C8E RID: 11406 RVA: 0x000E57AF File Offset: 0x000E3BAF
		// (set) Token: 0x06002C8F RID: 11407 RVA: 0x000E57B7 File Offset: 0x000E3BB7
		[XmlIgnore]
		public EItemType m_grantedItemType
		{
			get
			{
				return (EItemType)this.m_grantedItemTypeAsInt;
			}
			set
			{
				this.m_grantedItemTypeAsInt = (uint)value;
			}
		}

		// Token: 0x04001F90 RID: 8080
		[XmlElement("GrantedItemType")]
		public uint m_grantedItemTypeAsInt;
	}
}
