// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticItemCharMask : StaticItemBase
	{
		[XmlElement("CharacterTypeBitMask")]
		public ulong m_characterTypeBitMask;
	}
}
