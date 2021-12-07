using System;

// Token: 0x0200020E RID: 526
public interface ISaveFileData
{
	// Token: 0x060009F1 RID: 2545
	bool HasFile(string fileName);

	// Token: 0x060009F2 RID: 2546
	bool SaveToFile(string fileName, string value);

	// Token: 0x060009F3 RID: 2547
	string GetFromFile(string fileName);

	// Token: 0x060009F4 RID: 2548
	bool SaveToFile<T>(string fileName, T data) where T : new();

	// Token: 0x060009F5 RID: 2549
	T GetFromFile<T>(string fileName) where T : new();

	// Token: 0x060009F6 RID: 2550
	bool SaveToXmlFile<T>(string fileName, T data) where T : new();

	// Token: 0x060009F7 RID: 2551
	T GetFromXmlFile<T>(string fileName) where T : new();

	// Token: 0x060009F8 RID: 2552
	void DeleteFile(string fileName);

	// Token: 0x060009F9 RID: 2553
	string[] GetDirectoryContents(string dirName);
}
