// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ISaveFileData
{
	bool HasFile(string fileName);

	bool SaveToFile(string fileName, string value);

	string GetFromFile(string fileName);

	bool SaveToFile<T>(string fileName, T data) where T : new();

	T GetFromFile<T>(string fileName) where T : new();

	bool SaveToXmlFile<T>(string fileName, T data) where T : new();

	T GetFromXmlFile<T>(string fileName) where T : new();

	void DeleteFile(string fileName);

	string[] GetDirectoryContents(string dirName);
}
