using System;
using System.Xml.Serialization;
using IconsServer;

namespace IconsTool
{
	// Token: 0x020006E7 RID: 1767
	public class StaticItemBase : StaticDataBase
	{
		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06002C77 RID: 11383 RVA: 0x000E5694 File Offset: 0x000E3A94
		// (set) Token: 0x06002C78 RID: 11384 RVA: 0x000E569C File Offset: 0x000E3A9C
		[XmlIgnore]
		public EItemType m_itemType
		{
			get
			{
				return (EItemType)this.m_itemTypeAsInt;
			}
			set
			{
				this.m_rarityAsInt = (uint)value;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06002C79 RID: 11385 RVA: 0x000E56A5 File Offset: 0x000E3AA5
		// (set) Token: 0x06002C7A RID: 11386 RVA: 0x000E56AD File Offset: 0x000E3AAD
		[XmlIgnore]
		public EItemRarity m_rarity
		{
			get
			{
				return (EItemRarity)this.m_rarityAsInt;
			}
			set
			{
				this.m_rarityAsInt = (uint)value;
			}
		}

		// Token: 0x06002C7B RID: 11387 RVA: 0x000E56B6 File Offset: 0x000E3AB6
		public override string ToString()
		{
			return this.m_friendlyName;
		}

		// Token: 0x04001F88 RID: 8072
		[XmlElement("Type")]
		public uint m_itemTypeAsInt;

		// Token: 0x04001F89 RID: 8073
		[XmlElement("Rarity")]
		public uint m_rarityAsInt;

		// Token: 0x04001F8A RID: 8074
		[XmlElement("Flags")]
		public ulong m_flags;

		// Token: 0x04001F8B RID: 8075
		[XmlElement("Asset")]
		public string m_asset = string.Empty;

		// Token: 0x04001F8C RID: 8076
		[XmlElement("Enabled")]
		public bool m_enabled;
	}
}
