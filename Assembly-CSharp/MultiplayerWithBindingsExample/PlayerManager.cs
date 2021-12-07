using System;
using System.Collections.Generic;
using InControl;
using UnityEngine;

namespace MultiplayerWithBindingsExample
{
	// Token: 0x0200004A RID: 74
	public class PlayerManager : MonoBehaviour
	{
		// Token: 0x0600026E RID: 622 RVA: 0x000111E0 File Offset: 0x0000F5E0
		private void OnEnable()
		{
			InputManager.OnDeviceDetached += this.OnDeviceDetached;
			this.keyboardListener = PlayerActions.CreateWithKeyboardBindings();
			this.joystickListener = PlayerActions.CreateWithJoystickBindings();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00011209 File Offset: 0x0000F609
		private void OnDisable()
		{
			InputManager.OnDeviceDetached -= this.OnDeviceDetached;
			this.joystickListener.Destroy();
			this.keyboardListener.Destroy();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00011234 File Offset: 0x0000F634
		private void Update()
		{
			if (this.JoinButtonWasPressedOnListener(this.joystickListener))
			{
				InputDevice activeDevice = InputManager.ActiveDevice;
				if (this.ThereIsNoPlayerUsingJoystick(activeDevice))
				{
					this.CreatePlayer(activeDevice);
				}
			}
			if (this.JoinButtonWasPressedOnListener(this.keyboardListener) && this.ThereIsNoPlayerUsingKeyboard())
			{
				this.CreatePlayer(null);
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00011290 File Offset: 0x0000F690
		private bool JoinButtonWasPressedOnListener(PlayerActions actions)
		{
			return actions.Green.WasPressed || actions.Red.WasPressed || actions.Blue.WasPressed || actions.Yellow.WasPressed;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x000112D0 File Offset: 0x0000F6D0
		private Player FindPlayerUsingJoystick(InputDevice inputDevice)
		{
			int count = this.players.Count;
			for (int i = 0; i < count; i++)
			{
				Player player = this.players[i];
				if (player.Actions.Device == inputDevice)
				{
					return player;
				}
			}
			return null;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0001131C File Offset: 0x0000F71C
		private bool ThereIsNoPlayerUsingJoystick(InputDevice inputDevice)
		{
			return this.FindPlayerUsingJoystick(inputDevice) == null;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0001132C File Offset: 0x0000F72C
		private Player FindPlayerUsingKeyboard()
		{
			int count = this.players.Count;
			for (int i = 0; i < count; i++)
			{
				Player player = this.players[i];
				if (player.Actions == this.keyboardListener)
				{
					return player;
				}
			}
			return null;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00011378 File Offset: 0x0000F778
		private bool ThereIsNoPlayerUsingKeyboard()
		{
			return this.FindPlayerUsingKeyboard() == null;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00011388 File Offset: 0x0000F788
		private void OnDeviceDetached(InputDevice inputDevice)
		{
			Player player = this.FindPlayerUsingJoystick(inputDevice);
			if (player != null)
			{
				this.RemovePlayer(player);
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000113B0 File Offset: 0x0000F7B0
		private Player CreatePlayer(InputDevice inputDevice)
		{
			if (this.players.Count < 4)
			{
				Vector3 position = this.playerPositions[0];
				this.playerPositions.RemoveAt(0);
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, position, Quaternion.identity);
				Player component = gameObject.GetComponent<Player>();
				if (inputDevice == null)
				{
					component.Actions = this.keyboardListener;
				}
				else
				{
					PlayerActions playerActions = PlayerActions.CreateWithJoystickBindings();
					playerActions.Device = inputDevice;
					component.Actions = playerActions;
				}
				this.players.Add(component);
				return component;
			}
			return null;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0001143A File Offset: 0x0000F83A
		private void RemovePlayer(Player player)
		{
			this.playerPositions.Insert(0, player.transform.position);
			this.players.Remove(player);
			player.Actions = null;
			UnityEngine.Object.Destroy(player.gameObject);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00011474 File Offset: 0x0000F874
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
				GUI.Label(new Rect(10f, num, 300f, num + 22f), "Press a button or a/s/d/f key to join!");
				num += 22f;
			}
		}

		// Token: 0x040001D1 RID: 465
		public GameObject playerPrefab;

		// Token: 0x040001D2 RID: 466
		private const int maxPlayers = 4;

		// Token: 0x040001D3 RID: 467
		private List<Vector3> playerPositions = new List<Vector3>
		{
			new Vector3(-1f, 1f, -10f),
			new Vector3(1f, 1f, -10f),
			new Vector3(-1f, -1f, -10f),
			new Vector3(1f, -1f, -10f)
		};

		// Token: 0x040001D4 RID: 468
		private List<Player> players = new List<Player>(4);

		// Token: 0x040001D5 RID: 469
		private PlayerActions keyboardListener;

		// Token: 0x040001D6 RID: 470
		private PlayerActions joystickListener;
	}
}
