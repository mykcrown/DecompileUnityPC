// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticLootBoxDataList
	{
		[XmlElement("StaticLootBox")]
		public List<StaticLootBoxData> m_lootBoxList = new List<StaticLootBoxData>();
	}
}
