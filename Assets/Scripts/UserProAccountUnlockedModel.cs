// Decompile from assembly: Assembly-CSharp.dll

using System;

public class UserProAccountUnlockedModel : IUserProAccountUnlockedModel
{
	public static string UPDATED = "UserProAccountUnlockedModel.UPDATED";

	public static string SOURCE_UPDATED = "UserProAccountUnlockedModel.SOURCE_UPDATED";

	private bool isUnlocked;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IUserProAccountSource source
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserProAccountUnlockedModel.SOURCE_UPDATED, new Action(this.update));
		this.update();
	}

	private void update()
	{
		this.isUnlocked = this.source.IsUnlocked();
		this.signalBus.Dispatch(UserProAccountUnlockedModel.UPDATED);
	}

	public bool IsUnlocked()
	{
		return this.isUnlocked;
	}

	public void SetUnlocked()
	{
		this.isUnlocked = true;
		this.signalBus.Dispatch(UserProAccountUnlockedModel.UPDATED);
	}
}
