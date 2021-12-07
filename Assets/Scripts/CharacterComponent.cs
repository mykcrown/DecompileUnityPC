// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class CharacterComponent : ScriptableObject, ICharacterComponent, ISerializationCallbackReceiver, IPreloadedGameAsset, IMoveRequirementValidator
{
	protected IPlayerDelegate playerDelegate;

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection dependencyInjection
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
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	protected GameManager gameManager
	{
		get
		{
			return (this.gameController != null) ? this.gameController.currentGame : null;
		}
	}

	public virtual IComponentState State
	{
		get
		{
			return null;
		}
	}

	public virtual void Init(IPlayerDelegate playerDelegate)
	{
		this.inject();
		this.playerDelegate = playerDelegate;
	}

	private void inject()
	{
		StaticInject.Inject(this);
	}

	public virtual void LoadState(IComponentState state)
	{
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
	}

	public virtual void Destroy()
	{
		this.gameController = null;
		this.playerDelegate = null;
	}

	public virtual bool ValidateRequirements(MoveData move, IPlayerDelegate player, InputButtonsData input)
	{
		return true;
	}

	public virtual void RegisterPreload(PreloadContext context)
	{
	}
}
