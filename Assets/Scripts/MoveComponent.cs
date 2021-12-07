// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class MoveComponent : ScriptableObject, IMoveComponent, IMoveRequirementValidator, IPreloadedGameAsset
{
	protected IPlayerDelegate playerDelegate;

	protected IMoveDelegate moveDelegate;

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	public MoveData moveInfo
	{
		get;
		set;
	}

	public virtual void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		this.moveDelegate = moveDelegate;
		this.playerDelegate = playerDelegate;
	}

	public virtual void RegisterPreload(PreloadContext context)
	{
		if (this.moveInfo != null)
		{
			this.moveInfo.RegisterPreload(context);
		}
	}

	public virtual bool ValidateRequirements(MoveData data, IPlayerDelegate player, InputButtonsData input)
	{
		return true;
	}
}
