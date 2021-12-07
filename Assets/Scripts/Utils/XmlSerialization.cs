// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.IO;
using System.Xml.Serialization;

namespace Utils
{
	public static class XmlSerialization
	{
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
