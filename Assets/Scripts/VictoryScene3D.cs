// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScene3D : UIScene
{
	private Dictionary<int, PlayerNetsukeDisplay> displays = new Dictionary<int, PlayerNetsukeDisplay>();

	public GameObject NetsukeTest;

	public void AddNetsuke(Dictionary<int, Netsuke> allSlots, Transform attachTo, int index, float scale = 1f)
	{
		PlayerNetsukeDisplay playerNetsukeDisplay = UnityEngine.Object.Instantiate<PlayerNetsukeDisplay>(base.config.netsukeDisplayPrefab);
		foreach (KeyValuePair<int, Netsuke> current in allSlots)
		{
			playerNetsukeDisplay.AddNetsuke(current.Value, current.Key);
		}
		playerNetsukeDisplay.transform.SetParent(this.NetsukeTest.transform, false);
		playerNetsukeDisplay.Attach(attachTo, this.myCamera);
		playerNetsukeDisplay.Scale = new Vector3(scale, scale, scale);
		this.displays[index] = playerNetsukeDisplay;
	}

	public void OnRightTriggerPressed()
	{
		this.attemptSpinNetsukeAtIndex(1, 1);
	}

	public void OnLeftTriggerPressed()
	{
		this.attemptSpinNetsukeAtIndex(0, 1);
	}

	public void OnXPressed()
	{
		this.attemptSpinNetsukeAtIndex(1, -1);
	}

	public void OnYPressed()
	{
		this.attemptSpinNetsukeAtIndex(0, -1);
	}

	public void OnZPressed()
	{
		this.attemptSpinNetsukeAtIndex(0, 1);
		this.attemptSpinNetsukeAtIndex(1, -1);
	}

	private void attemptSpinNetsukeAtIndex(int index, int dir)
	{
		if (this.displays.ContainsKey(index))
		{
			PlayerNetsukeDisplay playerNetsukeDisplay = this.displays[index];
			if (playerNetsukeDisplay != null)
			{
				playerNetsukeDisplay.SpinDirection(dir);
			}
		}
	}
}
