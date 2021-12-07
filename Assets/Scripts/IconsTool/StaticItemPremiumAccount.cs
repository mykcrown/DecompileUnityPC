// Decompile from assembly: Assembly-CSharp.dll

using Commerce;
using IconsServer;
using System;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticItemPremiumAccount : StaticItemBase
	{
		[XmlElement("PremiumType")]
		public uint m_premiumTypeAsInt;

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

		internal StaticItemPremiumAccount()
		{
			base.m_itemType = EItemType.PremiumAccount;
		}
	}
}
