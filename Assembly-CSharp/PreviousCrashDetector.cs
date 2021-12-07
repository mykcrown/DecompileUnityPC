using System;
using System.IO;
using System.Text;
using Beebyte.Obfuscator;

// Token: 0x0200089D RID: 2205
public class PreviousCrashDetector : IPreviousCrashDetector
{
	// Token: 0x17000D73 RID: 3443
	// (get) Token: 0x0600374F RID: 14159 RVA: 0x0010265E File Offset: 0x00100A5E
	// (set) Token: 0x06003750 RID: 14160 RVA: 0x00102666 File Offset: 0x00100A66
	[Inject]
	public IUIAdapter uIAdapter { get; set; }

	// Token: 0x06003751 RID: 14161 RVA: 0x00102670 File Offset: 0x00100A70
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

	// Token: 0x06003752 RID: 14162 RVA: 0x00102708 File Offset: 0x00100B08
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

	// Token: 0x0400258C RID: 9612
	private static readonly string statusFileName = "exitStatus.txt";

	// Token: 0x0400258D RID: 9613
	private bool connectedToNexus;

	// Token: 0x0200089E RID: 2206
	[Skip]
	public enum GameStatus
	{
		// Token: 0x0400258F RID: 9615
		Menu,
		// Token: 0x04002590 RID: 9616
		GameLoad,
		// Token: 0x04002591 RID: 9617
		LocalGame,
		// Token: 0x04002592 RID: 9618
		OnlineGame,
		// Token: 0x04002593 RID: 9619
		SafeShutdown
	}
}
