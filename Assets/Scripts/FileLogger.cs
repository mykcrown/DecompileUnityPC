// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.IO;

public class FileLogger : BaseLogger
{
	private StreamWriter writer;

	public FileLogger()
	{
		this.writer = File.CreateText("log.txt");
		this.writer.AutoFlush = false;
	}

	public FileLogger(string DirectoryPath, string logFile)
	{
		if (!Directory.Exists(DirectoryPath))
		{
			Directory.CreateDirectory(DirectoryPath);
		}
		this.writer = File.CreateText(DirectoryPath + Path.DirectorySeparatorChar + logFile);
		this.writer.AutoFlush = true;
	}

	~FileLogger()
	{
		this.writer.Close();
	}

	public override void LogMessage(LogLevel logLevel, string message)
	{
		this.writer.WriteLine(message);
	}
}
