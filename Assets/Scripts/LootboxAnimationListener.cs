// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class LootboxAnimationListener : MonoBehaviour
{
	public ClickToOpenLoot director;

	public void OnOpenFinished()
	{
		this.director.OnOpenFinished();
	}
}
