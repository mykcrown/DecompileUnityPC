// Decompile from assembly: Assembly-CSharp.dll

using System;

public class RollbackDebugLayer : IRollbackDebugLayer
{
	private int markedFrame = -1;

	private RollbackStateContainer markedState;

	[Inject]
	public IDevConsole devConsole
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	private GameManager gameManager
	{
		get
		{
			return this.gameController.currentGame;
		}
	}

	private FrameController frameController
	{
		get
		{
			return this.gameManager.FrameController;
		}
	}

	private IRollbackLayer rollbackLayer
	{
		get
		{
			return this.frameController.rollbackLayer;
		}
	}

	[PostConstruct]
	public void Init()
	{
		this.devConsole.AddCommand(new Action(this.exportState), "rollback", "export", null);
		this.devConsole.AddCommand(new Action(this.loadState), "rollback", "load", null);
	}

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
}
