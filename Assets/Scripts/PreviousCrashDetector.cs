// Decompile from assembly: Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using System.IO;
using System.Text;

public class PreviousCrashDetector : IPreviousCrashDetector
{
	[Skip]
	public enum GameStatus
	{
		Menu,
		GameLoad,
		LocalGame,
		OnlineGame,
		SafeShutdown
	}

	private static readonly string statusFileName = "exitStatus.txt";

	private bool connectedToNexus;

	[Inject]
	public IUIAdapter uIAdapter
	{
		get;
		set;
	}

	public void ReportPreviousCrash()
	{
		if (File.Exists(PreviousCrashDetector.statusFileName))
		{
			using (FileStream fileStream = File.Open(PreviousCrashDetector.statusFileName, FileMode.Open))
			{
				byte[] array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
				string @string = Encoding.ASCII.GetString(array);
				if (!string.Equals(@string, PreviousCrashDetector.GameStatus.SafeShutdown.ToString()))
				{
				}
			}
		}
		this.connectedToNexus = true;
		this.UpdateStatus(PreviousCrashDetector.GameStatus.Menu);
	}

	public void UpdateStatus(PreviousCrashDetector.GameStatus gameStatus)
	{
		if (this.connectedToNexus)
		{
			byte[] bytes;
			if (gameStatus == PreviousCrashDetector.GameStatus.Menu)
			{
				bytes = Encoding.ASCII.GetBytes(gameStatus.ToString() + "." + this.uIAdapter.CurrentScreen.ToString());
			}
			else
			{
				bytes = Encoding.ASCII.GetBytes(gameStatus.ToString());
			}
			using (FileStream fileStream = new FileStream(PreviousCrashDetector.statusFileName, FileMode.Create))
			{
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}
	}
}
