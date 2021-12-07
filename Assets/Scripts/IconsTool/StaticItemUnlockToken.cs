// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticItemUnlockToken : StaticItemBase
	{
		[XmlElement("GrantedItemType")]
		public uint m_grantedItemTypeAsInt;

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

		internal StaticItemUnlockToken()
		{
			base.m_itemType = EItemType.UnlockToken;
		}
	}
}
