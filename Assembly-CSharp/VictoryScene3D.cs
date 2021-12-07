using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A83 RID: 2691
public class VictoryScene3D : UIScene
{
	// Token: 0x06004EB6 RID: 20150 RVA: 0x0014A2DC File Offset: 0x001486DC
	public void AddNetsuke(Dictionary<int, Netsuke> allSlots, Transform attachTo, int index, float scale = 1f)
	{
		PlayerNetsukeDisplay playerNetsukeDisplay = UnityEngine.Object.Instantiate<PlayerNetsukeDisplay>(base.config.netsukeDisplayPrefab);
		foreach (KeyValuePair<int, Netsuke> keyValuePair in allSlots)
		{
			playerNetsukeDisplay.AddNetsuke(keyValuePair.Value, keyValuePair.Key);
		}
		playerNetsukeDisplay.transform.SetParent(this.NetsukeTest.transform, false);
		playerNetsukeDisplay.Attach(attachTo, this.myCamera);
		playerNetsukeDisplay.Scale = new Vector3(scale, scale, scale);
		this.displays[index] = playerNetsukeDisplay;
	}

	// Token: 0x06004EB7 RID: 20151 RVA: 0x0014A394 File Offset: 0x00148794
	public void OnRightTriggerPressed()
	{
		this.attemptSpinNetsukeAtIndex(1, 1);
	}

	// Token: 0x06004EB8 RID: 20152 RVA: 0x0014A39E File Offset: 0x0014879E
	public void OnLeftTriggerPressed()
	{
		this.attemptSpinNetsukeAtIndex(0, 1);
	}

	// Token: 0x06004EB9 RID: 20153 RVA: 0x0014A3A8 File Offset: 0x001487A8
	public void OnXPressed()
	{
		this.attemptSpinNetsukeAtIndex(1, -1);
	}

	// Token: 0x06004EBA RID: 20154 RVA: 0x0014A3B2 File Offset: 0x001487B2
	public void OnYPressed()
	{
		this.attemptSpinNetsukeAtIndex(0, -1);
	}

	// Token: 0x06004EBB RID: 20155 RVA: 0x0014A3BC File Offset: 0x001487BC
	public void OnZPressed()
	{
		this.attemptSpinNetsukeAtIndex(0, 1);
		this.attemptSpinNetsukeAtIndex(1, -1);
	}

	// Token: 0x06004EBC RID: 20156 RVA: 0x0014A3D0 File Offset: 0x001487D0
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

	// Token: 0x04003364 RID: 13156
	private Dictionary<int, PlayerNetsukeDisplay> displays = new Dictionary<int, PlayerNetsukeDisplay>();

	// Token: 0x04003365 RID: 13157
	public GameObject NetsukeTest;
}
