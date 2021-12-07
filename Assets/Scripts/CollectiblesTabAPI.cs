// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CollectiblesTabAPI : ICollectiblesTabAPI
{
	public const string UPDATED = "CollectiblesTabAPI.UPDATED";

	private CollectiblesTabState state;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IStoreTabsModel storeTabsModel
	{
		get;
		set;
	}

	[Inject]
	public IUIAdapter uiScreenAdapter
	{
		get;
		set;
	}

	public bool SkipAnimation
	{
		get;
		private set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UIManager.SCREEN_CLOSED, new Action(this.resetOnLeave));
	}

	private void storeTabUpdated()
	{
		if (this.storeTabsModel.Current == StoreTab.COLLECTIBLES)
		{
			this.resetState();
		}
	}

	private void resetState()
	{
		this.SetState(CollectiblesTabState.IntroView, false);
	}

	private void resetOnLeave()
	{
		if (this.uiScreenAdapter.PreviousScreen == ScreenType.StoreScreen)
		{
			this.resetState();
		}
	}

	public void SetState(CollectiblesTabState state, bool skipAnimation = false)
	{
		if (this.state != state)
		{
			this.state = state;
			this.SkipAnimation = skipAnimation;
			this.signalBus.Dispatch("CollectiblesTabAPI.UPDATED");
			this.SkipAnimation = false;
		}
	}

	public CollectiblesTabState GetState()
	{
		return this.state;
	}
}
