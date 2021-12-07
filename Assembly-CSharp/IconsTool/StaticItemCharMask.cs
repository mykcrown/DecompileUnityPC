using System;
using System.Xml.Serialization;

namespace IconsTool
{
	// Token: 0x020006E8 RID: 1768
	public class StaticItemCharMask : StaticItemBase
	{
		// Token: 0x04001F8D RID: 8077
		[XmlElement("CharacterTypeBitMask")]
		public ulong m_characterTypeBitMask;
	}
}
