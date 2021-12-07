using System;
using System.Collections.Generic;
using System.IO;
using IconsServer;
using UnityEngine;

// Token: 0x0200020D RID: 525
public class SaveFileData : ISaveFileData
{
	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x060009D8 RID: 2520 RVA: 0x00050E19 File Offset: 0x0004F219
	// (set) Token: 0x060009D9 RID: 2521 RVA: 0x00050E21 File Offset: 0x0004F221
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x060009DA RID: 2522 RVA: 0x00050E2A File Offset: 0x0004F22A
	// (set) Token: 0x060009DB RID: 2523 RVA: 0x00050E32 File Offset: 0x0004F232
	[Inject]
	public IOfflineModeDetector offlineMode { get; set; }

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x060009DC RID: 2524 RVA: 0x00050E3B File Offset: 0x0004F23B
	// (set) Token: 0x060009DD RID: 2525 RVA: 0x00050E43 File Offset: 0x0004F243
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x060009DE RID: 2526 RVA: 0x00050E4C File Offset: 0x0004F24C
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

	// Token: 0x060009DF RID: 2527 RVA: 0x00050EC0 File Offset: 0x0004F2C0
	private bool testWriteDirectory(string path)
	{
		if (this.gameController.currentGame != null)
		{
			Debug.LogWarning("WARNING accessing file system during gameplay!");
		}
		bool result;
		try
		{
			string test_WRITE_FILE_NAME = SaveFileData.TEST_WRITE_FILE_NAME;
			string path2 = path + test_WRITE_FILE_NAME;
			string test_WRITE_VALUE = SaveFileData.TEST_WRITE_VALUE;
			StreamWriter streamWriter = File.CreateText(path2);
			streamWriter.Write(test_WRITE_VALUE);
			streamWriter.Close();
			StreamReader streamReader = File.OpenText(path2);
			string a = streamReader.ReadToEnd();
			streamReader.Close();
			result = (a == test_WRITE_VALUE);
		}
		catch (Exception ex)
		{
			Debug.LogWarning("FILE WRITE FAILURE: " + ex.Message);
			result = false;
		}
		return result;
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x00050F70 File Offset: 0x0004F370
	private string getFullPath(string fileName)
	{
		return this.getStoragePath() + fileName;
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x00050F80 File Offset: 0x0004F380
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

	// Token: 0x060009E2 RID: 2530 RVA: 0x00050FF4 File Offset: 0x0004F3F4
	public void DeleteFile(string fileName)
	{
		string fullPath = this.getFullPath(fileName);
		if (File.Exists(fullPath))
		{
			File.Delete(fullPath);
		}
		else
		{
			Debug.LogError("FILE DELETION FAILED: " + fullPath);
		}
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x00051030 File Offset: 0x0004F430
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

	// Token: 0x060009E4 RID: 2532 RVA: 0x00051098 File Offset: 0x0004F498
	public bool HasFile(string fileName)
	{
		string fullPath = this.getFullPath(fileName);
		return File.Exists(fullPath);
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x000510B4 File Offset: 0x0004F4B4
	public bool SaveToFile(string fileName, string value)
	{
		string fullPath = this.getFullPath(fileName);
		this.saveTextFile(fullPath, value);
		return true;
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x000510D4 File Offset: 0x0004F4D4
	public bool SaveToFile<T>(string fileName, T data) where T : new()
	{
		string fullPath = this.getFullPath(fileName);
		this.saveBinaryFile<T>(fullPath, data);
		return true;
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x000510F4 File Offset: 0x0004F4F4
	public bool SaveToXmlFile<T>(string fileName, T data) where T : new()
	{
		string fullPath = this.getFullPath(fileName);
		this.prepareDirectory(fullPath);
		Serialization.WriteToXmlFile<T>(fullPath, data, false);
		return true;
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x0005111C File Offset: 0x0004F51C
	public T GetFromXmlFile<T>(string fileName) where T : new()
	{
		string fullPath = this.getFullPath(fileName);
		if (File.Exists(fullPath))
		{
			return this.readXmlFile<T>(fullPath);
		}
		return default(T);
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00051150 File Offset: 0x0004F550
	public string GetFromFile(string fileName)
	{
		string fullPath = this.getFullPath(fileName);
		return this.readTextFile(fullPath);
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x0005116C File Offset: 0x0004F56C
	public T GetFromFile<T>(string fileName) where T : new()
	{
		string fullPath = this.getFullPath(fileName);
		if (File.Exists(fullPath))
		{
			return this.readBinaryFile<T>(fullPath);
		}
		return default(T);
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x000511A0 File Offset: 0x0004F5A0
	private void saveTextFile(string fullPath, string value)
	{
		this.prepareDirectory(fullPath);
		Debug.Log("Save text file " + fullPath);
		try
		{
			StreamWriter streamWriter = File.CreateText(fullPath);
			streamWriter.Write(value);
			streamWriter.Close();
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception saving file " + ex.Message);
		}
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x00051208 File Offset: 0x0004F608
	private string readTextFile(string fullPath)
	{
		this.prepareDirectory(fullPath);
		if (File.Exists(fullPath))
		{
			return File.ReadAllText(fullPath);
		}
		return null;
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x00051224 File Offset: 0x0004F624
	private void saveBinaryFile<T>(string fullPath, T objectToWrite) where T : new()
	{
		this.prepareDirectory(fullPath);
		Serialization.WriteToBinaryFile<T>(fullPath, objectToWrite, false, false);
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x00051238 File Offset: 0x0004F638
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
			Debug.LogWarning("FILE READ ERROR: " + ex.Message);
			result = default(T);
		}
		return result;
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00051290 File Offset: 0x0004F690
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
			Debug.LogWarning("FILE READ ERROR: " + ex.Message);
			result = default(T);
		}
		return result;
	}

	// Token: 0x040006EF RID: 1775
	private static string TEST_WRITE_FILE_NAME = "test-write";

	// Token: 0x040006F0 RID: 1776
	private static string TEST_WRITE_VALUE = "test123-//5";

	// Token: 0x040006F4 RID: 1780
	private string storagePath;

	// Token: 0x040006F5 RID: 1781
	private string testPath;

	// Token: 0x040006F6 RID: 1782
	private string savedUserName;
}
