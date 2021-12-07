// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IconsTool
{
	public class StaticPackageDataList
	{
		[XmlElement("StaticPackage")]
		public List<StaticPackageData> m_packageList = new List<StaticPackageData>();
	}
}
