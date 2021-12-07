using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using Ionic.Zip;
using UnityEngine;

// Token: 0x0200084C RID: 2124
public class Serialization
{
	// Token: 0x0600351F RID: 13599 RVA: 0x000F9600 File Offset: 0x000F7A00
	public static T DeepClone<T>(T obj)
	{
		T result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = Serialization.getBinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			memoryStream.Position = 0L;
			result = (T)((object)binaryFormatter.Deserialize(memoryStream));
		}
		return result;
	}

	// Token: 0x06003520 RID: 13600 RVA: 0x000F9660 File Offset: 0x000F7A60
	public static void WriteString(string filePath, string data)
	{
		File.WriteAllText(filePath, data);
	}

	// Token: 0x06003521 RID: 13601 RVA: 0x000F9669 File Offset: 0x000F7A69
	public static void AppendLine(string filePath, string line)
	{
		File.AppendAllText(filePath, line);
	}

	// Token: 0x06003522 RID: 13602 RVA: 0x000F9674 File Offset: 0x000F7A74
	private static BinaryFormatter getBinaryFormatter()
	{
		if (Serialization.binaryFormatter == null)
		{
			Serialization.binaryFormatter = new BinaryFormatter();
			SurrogateSelector surrogateSelector = new SurrogateSelector();
			Vector3SerializationSurrogate surrogate = new Vector3SerializationSurrogate();
			surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), surrogate);
			Vector2SerializationSurrogate surrogate2 = new Vector2SerializationSurrogate();
			surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), surrogate2);
			Serialization.binaryFormatter.SurrogateSelector = surrogateSelector;
		}
		return Serialization.binaryFormatter;
	}

	// Token: 0x06003523 RID: 13603 RVA: 0x000F96F0 File Offset: 0x000F7AF0
	public static byte[] WriteBytes(object objectToWrite)
	{
		IFormatter formatter = Serialization.getBinaryFormatter();
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			formatter.Serialize(memoryStream, objectToWrite);
			result = memoryStream.ToArray();
		}
		return result;
	}

	// Token: 0x06003524 RID: 13604 RVA: 0x000F973C File Offset: 0x000F7B3C
	public static object ReadBytes(byte[] bytes)
	{
		IFormatter formatter = Serialization.getBinaryFormatter();
		object result;
		using (MemoryStream memoryStream = new MemoryStream(bytes))
		{
			memoryStream.Seek(0L, SeekOrigin.Begin);
			result = formatter.Deserialize(memoryStream);
		}
		return result;
	}

	// Token: 0x06003525 RID: 13605 RVA: 0x000F978C File Offset: 0x000F7B8C
	public static void Write<T>(string filePath, T objectToWrite, Serialization.SerializeType type, bool appendToFile, bool compress = false) where T : new()
	{
		if (type != Serialization.SerializeType.Binary)
		{
			if (type != Serialization.SerializeType.XML)
			{
				throw new Exception("SerializeType " + type + " not supported");
			}
			Serialization.WriteToXmlFile<T>(filePath, objectToWrite, appendToFile);
		}
		else
		{
			Serialization.WriteToBinaryFile<T>(filePath, objectToWrite, appendToFile, compress);
		}
	}

	// Token: 0x06003526 RID: 13606 RVA: 0x000F97E2 File Offset: 0x000F7BE2
	public static T Read<T>(string filePath, Serialization.SerializeType type, bool decompress = false) where T : new()
	{
		if (type == Serialization.SerializeType.Binary)
		{
			return Serialization.ReadFromBinaryFile<T>(filePath, decompress);
		}
		if (type != Serialization.SerializeType.XML)
		{
			throw new Exception("SerializeType " + type + " not supported");
		}
		return Serialization.ReadFromXmlFile<T>(filePath);
	}

	// Token: 0x06003527 RID: 13607 RVA: 0x000F9820 File Offset: 0x000F7C20
	public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
	{
		TextWriter textWriter = null;
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			textWriter = new StreamWriter(filePath, append);
			xmlSerializer.Serialize(textWriter, objectToWrite);
		}
		finally
		{
			if (textWriter != null)
			{
				textWriter.Close();
			}
		}
	}

	// Token: 0x06003528 RID: 13608 RVA: 0x000F9878 File Offset: 0x000F7C78
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

	// Token: 0x06003529 RID: 13609 RVA: 0x000F98D0 File Offset: 0x000F7CD0
	public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false, bool compress = false)
	{
		if (compress)
		{
			using (ZipFile zipFile = new ZipFile())
			{
				zipFile.AlternateEncoding = Encoding.Unicode;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					IFormatter formatter = Serialization.getBinaryFormatter();
					formatter.Serialize(memoryStream, objectToWrite);
					memoryStream.Position = 0L;
					ZipEntry zipEntry = zipFile.AddEntry("entry", memoryStream);
					zipEntry.AlternateEncoding = Encoding.Unicode;
					zipFile.Save(filePath);
				}
			}
		}
		else
		{
			using (FileStream fileStream = File.Open(filePath, (!append) ? FileMode.Create : FileMode.Append))
			{
				BinaryFormatter binaryFormatter = Serialization.getBinaryFormatter();
				binaryFormatter.Serialize(fileStream, objectToWrite);
			}
		}
	}

	// Token: 0x0600352A RID: 13610 RVA: 0x000F99C4 File Offset: 0x000F7DC4
	public static T ReadFromBinaryFile<T>(string filePath, bool decompress = false)
	{
		if (File.Exists(filePath))
		{
			try
			{
				if (decompress)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (ZipFile zipFile = new ZipFile(filePath))
						{
							zipFile.AlternateEncoding = Encoding.Unicode;
							ZipEntry zipEntry = zipFile["entry"];
							zipEntry.AlternateEncoding = Encoding.Unicode;
							zipEntry.Extract(memoryStream);
							if (zipEntry.UncompressedSize > 5000000L)
							{
								throw new Exception("Replay Too Large to Deserialize");
							}
							memoryStream.Position = 0L;
							IFormatter formatter = Serialization.getBinaryFormatter();
							return (T)((object)formatter.Deserialize(memoryStream));
						}
					}
				}
				using (FileStream fileStream = File.Open(filePath, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = Serialization.getBinaryFormatter();
					return (T)((object)binaryFormatter.Deserialize(fileStream));
				}
			}
			catch (Exception ex)
			{
				if (Application.isEditor)
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						"Failed to load file ",
						filePath,
						": ",
						ex
					}));
				}
				File.Delete(filePath);
				return default(T);
			}
		}
		return default(T);
	}

	// Token: 0x04002493 RID: 9363
	private static BinaryFormatter binaryFormatter;

	// Token: 0x0200084D RID: 2125
	public enum SerializeType
	{
		// Token: 0x04002495 RID: 9365
		Binary,
		// Token: 0x04002496 RID: 9366
		XML,
		// Token: 0x04002497 RID: 9367
		JSON
	}
}
