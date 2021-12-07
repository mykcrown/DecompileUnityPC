// Decompile from assembly: Assembly-CSharp.dll

using Ionic.Zip;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class Serialization
{
	public enum SerializeType
	{
		Binary,
		XML,
		JSON
	}

	private static BinaryFormatter binaryFormatter;

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

	public static void WriteString(string filePath, string data)
	{
		File.WriteAllText(filePath, data);
	}

	public static void AppendLine(string filePath, string line)
	{
		File.AppendAllText(filePath, line);
	}

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
							T t = (T)((object)formatter.Deserialize(memoryStream));
							T result = t;
							return result;
						}
					}
				}
				using (FileStream fileStream = File.Open(filePath, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = Serialization.getBinaryFormatter();
					T result = (T)((object)binaryFormatter.Deserialize(fileStream));
					return result;
				}
			}
			catch (Exception ex)
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.LogWarning(string.Concat(new object[]
					{
						"Failed to load file ",
						filePath,
						": ",
						ex
					}));
				}
				File.Delete(filePath);
				T result = default(T);
				return result;
			}
		}
		return default(T);
	}
}
