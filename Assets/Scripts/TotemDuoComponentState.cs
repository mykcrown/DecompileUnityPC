// Decompile from assembly: Assembly-CSharp.dll

using System;

public class TotemDuoComponentState : IComponentState
{
	public bool isClone;

	public bool isActive;

	public bool ignoreNextSwap;

	public IPlayerDelegate partner
	{
		get;
		set;
	}

	public TotemDuoComponent partnerComponent
	{
		get;
		set;
	}
}
