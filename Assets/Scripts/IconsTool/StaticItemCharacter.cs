// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticItemCharacter : StaticItemBase
	{
		[XmlElement("CharacterType")]
		public uint m_characterTypeAsInt;

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

		internal StaticItemCharacter()
		{
			base.m_itemType = EItemType.Character;
		}
	}
}
