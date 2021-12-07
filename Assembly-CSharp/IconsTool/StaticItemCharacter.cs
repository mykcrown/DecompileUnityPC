using System;
using System.Xml.Serialization;
using IconsServer;

namespace IconsTool
{
	// Token: 0x020006F3 RID: 1779
	public class StaticItemCharacter : StaticItemBase
	{
		// Token: 0x06002C87 RID: 11399 RVA: 0x000E575D File Offset: 0x000E3B5D
		internal StaticItemCharacter()
		{
			base.m_itemType = EItemType.Character;
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06002C88 RID: 11400 RVA: 0x000E576D File Offset: 0x000E3B6D
		// (set) Token: 0x06002C89 RID: 11401 RVA: 0x000E5775 File Offset: 0x000E3B75
		[XmlIgnore]
		public ECharacterType m_characterType
		{
			get
			{
				return (ECharacterType)this.m_characterTypeAsInt;
			}
			set
			{
				this.m_characterTypeAsInt = (uint)value;
			}
		}

		// Token: 0x04001F8E RID: 8078
		[XmlElement("CharacterType")]
		public uint m_characterTypeAsInt;
	}
}
