using System;

// Token: 0x020002F2 RID: 754
public class RollbackDebugLayer : IRollbackDebugLayer
{
	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06001061 RID: 4193 RVA: 0x000607A4 File Offset: 0x0005EBA4
	// (set) Token: 0x06001062 RID: 4194 RVA: 0x000607AC File Offset: 0x0005EBAC
	[Inject]
	public IDevConsole devConsole { get; set; }

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06001063 RID: 4195 RVA: 0x000607B5 File Offset: 0x0005EBB5
	// (set) Token: 0x06001064 RID: 4196 RVA: 0x000607BD File Offset: 0x0005EBBD
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06001065 RID: 4197 RVA: 0x000607C6 File Offset: 0x0005EBC6
	// (set) Token: 0x06001066 RID: 4198 RVA: 0x000607CE File Offset: 0x0005EBCE
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06001067 RID: 4199 RVA: 0x000607D7 File Offset: 0x0005EBD7
	private GameManager gameManager
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06001068 RID: 4200 RVA: 0x000607E4 File Offset: 0x0005EBE4
	private FrameController frameController
	{
		get
		{
			return this.gameManager.FrameController;
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06001069 RID: 4201 RVA: 0x000607F1 File Offset: 0x0005EBF1
	private IRollbackLayer rollbackLayer
	{
		get
		{
			return this.frameController.rollbackLayer;
		}
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x00060800 File Offset: 0x0005EC00
	[PostConstruct]
	public void Init()
	{
		this.devConsole.AddCommand(new Action(this.exportState), "rollback", "export", null);
		this.devConsole.AddCommand(new Action(this.loadState), "rollback", "load", null);
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x00060854 File Offset: 0x0005EC54
	private void exportState()
	{
		if (this.gameManager == null)
		{
			this.devConsole.PrintLn("Unable to export state because there is no active game.");
			return;
		}
		this.markedFrame = this.frameController.Frame;
		this.markedState = new RollbackStateContainer(true);
		this.gameManager.ExportState(ref this.markedState);
		this.devConsole.PrintLn("Exporting frame " + this.markedFrame + ".");
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x000608D8 File Offset: 0x0005ECD8
	private void loadState()
	{
		if (this.gameManager == null)
		{
			this.devConsole.PrintLn("Unable to load state because there is no active game.");
			return;
		}
		if (this.markedState == null)
		{
			this.devConsole.PrintLn("Unable to load state because there is no stored state.");
			return;
		}
		this.gameManager.LoadState(this.markedState);
		this.markedState.ResetIndex();
		this.rollbackLayer.ResetToFrame(this.markedFrame);
		this.devConsole.PrintLn(string.Format("Loaded frame {0}.", this.markedFrame));
	}

	// Token: 0x04000A83 RID: 2691
	private int markedFrame = -1;

	// Token: 0x04000A84 RID: 2692
	private RollbackStateContainer markedState;
}
