using System;
using System.IO;
using System.Xml.Serialization;

namespace Utils
{
	// Token: 0x0200071D RID: 1821
	public static class XmlSerialization
	{
		// Token: 0x06002D03 RID: 11523 RVA: 0x000E7A3C File Offset: 0x000E5E3C
		public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
		{
			TextWriter textWriter = null;
			try
			{
				XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
				xmlSerializerNamespaces.Add(string.Empty, string.Empty);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				textWriter = new StreamWriter(filePath, append);
				xmlSerializer.Serialize(textWriter, objectToWrite, xmlSerializerNamespaces);
			}
			finally
			{
				if (textWriter != null)
				{
					textWriter.Close();
				}
			}
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x000E7AAC File Offset: 0x000E5EAC
		public static T ReadFromXmlFile<T>(string filePath) where T : new()
		{
			TextReader textReader = null;
			T result;
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				textReader = new StreamReader(filePath);
				result = (T)((object)xmlSerializer.Deserialize(textReader));
			}
			finally
			{
				if (textReader != null)
				{
					textReader.Close();
				}
			}
			return result;
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x000E7B04 File Offset: 0x000E5F04
		public static T ReadFromXmlText<T>(string xmlText) where T : new()
		{
			TextReader textReader = null;
			T result;
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				textReader = new StringReader(xmlText);
				result = (T)((object)xmlSerializer.Deserialize(textReader));
			}
			finally
			{
				if (textReader != null)
				{
					textReader.Close();
				}
			}
			return result;
		}
	}
}
