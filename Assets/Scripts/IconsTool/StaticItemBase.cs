// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticItemBase : StaticDataBase
	{
		[XmlElement("Type")]
		public uint m_itemTypeAsInt;

		[XmlElement("Rarity")]
		public uint m_rarityAsInt;

		[XmlElement("Flags")]
		public ulong m_flags;

		[XmlElement("Asset")]
		public string m_asset = string.Empty;

		[XmlElement("Enabled")]
		public bool m_enabled;

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

		public override string ToString()
		{
			return this.m_friendlyName;
		}
	}
}
