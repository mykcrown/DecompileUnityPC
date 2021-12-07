using System;
using System.Xml.Serialization;
using Commerce;
using IconsServer;

namespace IconsTool
{
	// Token: 0x020006F4 RID: 1780
	public class StaticItemPremiumAccount : StaticItemBase
	{
		// Token: 0x06002C8A RID: 11402 RVA: 0x000E577E File Offset: 0x000E3B7E
		internal StaticItemPremiumAccount()
		{
			base.m_itemType = EItemType.PremiumAccount;
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06002C8B RID: 11403 RVA: 0x000E578E File Offset: 0x000E3B8E
		// (set) Token: 0x06002C8C RID: 11404 RVA: 0x000E5796 File Offset: 0x000E3B96
		[XmlIgnore]
		public EPremiumAccount m_characterType
		{
			get
			{
				return (EPremiumAccount)this.m_premiumTypeAsInt;
			}
			set
			{
				this.m_premiumTypeAsInt = (uint)value;
			}
		}

		// Token: 0x04001F8F RID: 8079
		[XmlElement("PremiumType")]
		public uint m_premiumTypeAsInt;
	}
}
