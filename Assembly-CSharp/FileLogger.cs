using System;
using System.IO;

// Token: 0x02000B18 RID: 2840
public class FileLogger : BaseLogger
{
	// Token: 0x06005171 RID: 20849 RVA: 0x001524E3 File Offset: 0x001508E3
	public FileLogger()
	{
		this.writer = File.CreateText("log.txt");
		this.writer.AutoFlush = false;
	}

	// Token: 0x06005172 RID: 20850 RVA: 0x00152508 File Offset: 0x00150908
	public FileLogger(string DirectoryPath, string logFile)
	{
		if (!Directory.Exists(DirectoryPath))
		{
			Directory.CreateDirectory(DirectoryPath);
		}
		this.writer = File.CreateText(DirectoryPath + Path.DirectorySeparatorChar + logFile);
		this.writer.AutoFlush = true;
	}

	// Token: 0x06005173 RID: 20851 RVA: 0x00152558 File Offset: 0x00150958
	~FileLogger()
	{
		this.writer.Close();
	}

	// Token: 0x06005174 RID: 20852 RVA: 0x0015258C File Offset: 0x0015098C
	public override void LogMessage(LogLevel logLevel, string message)
	{
		this.writer.WriteLine(message);
	}

	// Token: 0x0400346E RID: 13422
	private StreamWriter writer;
}
