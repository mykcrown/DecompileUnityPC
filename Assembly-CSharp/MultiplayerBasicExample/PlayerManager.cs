using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;

namespace MultiplayerBasicExample
{
	// Token: 0x02000047 RID: 71
	public class PlayerManager : MonoBehaviour
	{
		// Token: 0x0600025A RID: 602 RVA: 0x00010B54 File Offset: 0x0000EF54
		private void Start()
		{
			InputManager.OnDeviceDetached += this.OnDeviceDetached;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00010B68 File Offset: 0x0000EF68
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			if (this.JoinButtonWasPressedOnDevice(activeDevice) && this.ThereIsNoPlayerUsingDevice(activeDevice))
			{
				this.CreatePlayer(activeDevice);
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00010B9B File Offset: 0x0000EF9B
		private bool JoinButtonWasPressedOnDevice(InputDevice inputDevice)
		{
			return inputDevice.Action1.WasPressed || inputDevice.Action2.WasPressed || inputDevice.Action3.WasPressed || inputDevice.Action4.WasPressed;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00010BDC File Offset: 0x0000EFDC
		private Player FindPlayerUsingDevice(InputDevice inputDevice)
		{
			int count = this.players.Count;
			for (int i = 0; i < count; i++)
			{
				Player player = this.players[i];
				if (player.Device == inputDevice)
				{
					return player;
				}
			}
			return null;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00010C23 File Offset: 0x0000F023
		private bool ThereIsNoPlayerUsingDevice(InputDevice inputDevice)
		{
			return this.FindPlayerUsingDevice(inputDevice) == null;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00010C34 File Offset: 0x0000F034
		private void OnDeviceDetached(InputDevice inputDevice)
		{
			Player player = this.FindPlayerUsingDevice(inputDevice);
			if (player != null)
			{
				this.RemovePlayer(player);
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00010C5C File Offset: 0x0000F05C
		private Player CreatePlayer(InputDevice inputDevice)
		{
			if (this.players.Count < 4)
			{
				Vector3 position = this.playerPositions[0];
				this.playerPositions.RemoveAt(0);
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, position, Quaternion.identity);
				Player component = gameObject.GetComponent<Player>();
				component.Device = inputDevice;
				this.players.Add(component);
				return component;
			}
			return null;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00010CC2 File Offset: 0x0000F0C2
		private void RemovePlayer(Player player)
		{
			this.playerPositions.Insert(0, player.transform.position);
			this.players.Remove(player);
			player.Device = null;
			UnityEngine.Object.Destroy(player.gameObject);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00010CFC File Offset: 0x0000F0FC
		private void OnGUI()
		{
			float num = 10f;
			GUI.Label(new Rect(10f, num, 300f, num + 22f), string.Concat(new object[]
			{
				"Active players: ",
				this.players.Count,
				"/",
				4
			}));
			num += 22f;
			if (this.players.Count < 4)
			{
				GUI.Label(new Rect(10f, num, 300f, num + 22f), "Press a button to join!");
				num += 22f;
			}
		}

		// Token: 0x040001C2 RID: 450
		public GameObject playerPrefab;

		// Token: 0x040001C3 RID: 451
		private const int maxPlayers = 4;

		// Token: 0x040001C4 RID: 452
		private List<Vector3> playerPositions = new List<Vector3>
		{
			new Vector3(-1f, 1f, -10f),
			new Vector3(1f, 1f, -10f),
			new Vector3(-1f, -1f, -10f),
			new Vector3(1f, -1f, -10f)
		};

		// Token: 0x040001C5 RID: 453
		private List<Player> players = new List<Player>(4);
	}
}
