// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFileData : ISaveFileData
{
	private static string TEST_WRITE_FILE_NAME = "test-write";

	private static string TEST_WRITE_VALUE = "test123-//5";

	private string storagePath;

	private string testPath;

	private string savedUserName;

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public IOfflineModeDetector offlineMode
	{
		get;
		set;
	}

	[Inject]
	public IIconsServerAPI iconsServerAPI
	{
		get;
		set;
	}

	private string getStoragePath()
	{
		if (this.storagePath == null)
		{
			this.testPath = Application.persistentDataPath + "/userData_legacy/";
			this.prepareDirectory(this.testPath);
			if (this.testWriteDirectory(this.testPath))
			{
				this.storagePath = this.testPath;
			}
		}
		if (this.storagePath == null)
		{
			this.storagePath = string.Empty;
		}
		return this.storagePath;
	}

	private bool testWriteDirectory(string path)
	{
		if (this.gameController.currentGame != null)
		{
			UnityEngine.Debug.LogWarning("WARNING accessing file system during gameplay!");
		}
		bool result;
		try
		{
			string tEST_WRITE_FILE_NAME = SaveFileData.TEST_WRITE_FILE_NAME;
			string path2 = path + tEST_WRITE_FILE_NAME;
			string tEST_WRITE_VALUE = SaveFileData.TEST_WRITE_VALUE;
			StreamWriter streamWriter = File.CreateText(path2);
			streamWriter.Write(tEST_WRITE_VALUE);
			streamWriter.Close();
			StreamReader streamReader = File.OpenText(path2);
			string a = streamReader.ReadToEnd();
			streamReader.Close();
			result = (a == tEST_WRITE_VALUE);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning("FILE WRITE FAILURE: " + ex.Message);
			result = false;
		}
		return result;
	}

	private string getFullPath(string fileName)
	{
		return this.getStoragePath() + fileName;
	}

	public string[] GetDirectoryContents(string dirName)
	{
		string fullPath = this.getFullPath(dirName);
		string oldValue = this.getStoragePath();
		if (Directory.Exists(fullPath))
		{
			List<string> list = new List<string>();
			string[] files = Directory.GetFiles(fullPath);
			int num = files.Length;
			for (int i = 0; i < num; i++)
			{
				string item = files[i].Replace(oldValue, string.Empty);
				list.Add(item);
			}
			return list.ToArray();
		}
		return null;
	}

	public void DeleteFile(string fileName)
	{
		string fullPath = this.getFullPath(fileName);
		if (File.Exists(fullPath))
		{
			File.Delete(fullPath);
		}
		else
		{
			UnityEngine.Debug.LogError("FILE DELETION FAILED: " + fullPath);
		}
	}

	private void prepareDirectory(string fullPath)
	{
		string[] array = fullPath.Split(new char[]
		{
			'/'
		});
		if (array.Length > 0)
		{
			string text = string.Empty;
			for (int i = 0; i < array.Length - 1; i++)
			{
				text = text + array[i] + "/";
			}
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
		}
	}

	public bool HasFile(string fileName)
	{
		string fullPath = this.getFullPath(fileName);
		return File.Exists(fullPath);
	}

	public bool SaveToFile(string fileName, string value)
	{
		string fullPath = this.getFullPath(fileName);
		this.saveTextFile(fullPath, value);
		return true;
	}

	public bool SaveToFile<T>(string fileName, T data) where T : new()
	{
		string fullPath = this.getFullPath(fileName);
		this.saveBinaryFile<T>(fullPath, data);
		return true;
	}

	public bool SaveToXmlFile<T>(string fileName, T data) where T : new()
	{
		string fullPath = this.getFullPath(fileName);
		this.prepareDirectory(fullPath);
		Serialization.WriteToXmlFile<T>(fullPath, data, false);
		return true;
	}

	public T GetFromXmlFile<T>(string fileName) where T : new()
	{
		string fullPath = this.getFullPath(fileName);
		if (File.Exists(fullPath))
		{
			return this.readXmlFile<T>(fullPath);
		}
		return default(T);
	}

	public string GetFromFile(string fileName)
	{
		string fullPath = this.getFullPath(fileName);
		return this.readTextFile(fullPath);
	}

	public T GetFromFile<T>(string fileName) where T : new()
	{
		string fullPath = this.getFullPath(fileName);
		if (File.Exists(fullPath))
		{
			return this.readBinaryFile<T>(fullPath);
		}
		return default(T);
	}

	private void saveTextFile(string fullPath, string value)
	{
		this.prepareDirectory(fullPath);
		UnityEngine.Debug.Log("Save text file " + fullPath);
		try
		{
			StreamWriter streamWriter = File.CreateText(fullPath);
			streamWriter.Write(value);
			streamWriter.Close();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception saving file " + ex.Message);
		}
	}

	private string readTextFile(string fullPath)
	{
		this.prepareDirectory(fullPath);
		if (File.Exists(fullPath))
		{
			return File.ReadAllText(fullPath);
		}
		return null;
	}

	private void saveBinaryFile<T>(string fullPath, T objectToWrite) where T : new()
	{
		this.prepareDirectory(fullPath);
		Serialization.WriteToBinaryFile<T>(fullPath, objectToWrite, false, false);
	}

	private T readBinaryFile<T>(string fullPath) where T : new()
	{
		this.prepareDirectory(fullPath);
		T result;
		try
		{
			result = Serialization.ReadFromBinaryFile<T>(fullPath, false);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning("FILE READ ERROR: " + ex.Message);
			result = default(T);
		}
		return result;
	}

	private T readXmlFile<T>(string fullPath) where T : new()
	{
		this.prepareDirectory(fullPath);
		T result;
		try
		{
			result = Serialization.ReadFromXmlFile<T>(fullPath);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning("FILE READ ERROR: " + ex.Message);
			result = default(T);
		}
		return result;
	}
}
