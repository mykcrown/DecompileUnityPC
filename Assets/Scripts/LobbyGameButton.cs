// Decompile from assembly: Assembly-CSharp.dll

using Steamworks;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class LobbyGameButton : MonoBehaviour
{
	public TextMeshProUGUI gameName;

	public TextMeshProUGUI playerCounts;

	public MenuItemButton button;

	private JoinLobbyDialog dialogOwner;

	private ulong lobbyId;

	public void Initialize(ulong lobbyId, string name, int players, int maxPlayers, JoinLobbyDialog dialogOwner)
	{
		this.lobbyId = lobbyId;
		this.dialogOwner = dialogOwner;
		this.gameName.text = name;
		this.playerCounts.text = players.ToString() + "/" + maxPlayers.ToString();
		dialogOwner.inputButtons.AddButton(this.button, new Action(this.onClick));
	}

	public void SetButtonActive(bool active)
	{
		this.dialogOwner.inputButtons.SetButtonEnabled(this.button, active);
		base.gameObject.SetActive(active);
	}

	public void onClick()
	{
		this.dialogOwner.steamManager.JoinLobby(this.lobbyId, new Action<EChatRoomEnterResponse>(this._onClick_m__0));
	}

	public void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void _onClick_m__0(EChatRoomEnterResponse response)
	{
		switch (response)
		{
		case EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess:
			break;
		case EChatRoomEnterResponse.k_EChatRoomEnterResponseDoesntExist:
			this.dialogOwner.errorTitle = "ui.lobbyDialog.join.title." + response.ToString();
			this.dialogOwner.errorBody = "ui.lobbyDialog.join.body." + response.ToString();
			this.dialogOwner.OnUpdate();
			break;
		case EChatRoomEnterResponse.k_EChatRoomEnterResponseNotAllowed:
			this.dialogOwner.errorTitle = "ui.lobbyDialog.join.title." + response.ToString();
			this.dialogOwner.errorBody = "ui.lobbyDialog.join.body." + response.ToString();
			this.dialogOwner.OnUpdate();
			break;
		case EChatRoomEnterResponse.k_EChatRoomEnterResponseFull:
			this.dialogOwner.errorTitle = "ui.lobbyDialog.join.title." + response.ToString();
			this.dialogOwner.errorBody = "ui.lobbyDialog.join.body." + response.ToString();
			this.dialogOwner.OnUpdate();
			break;
		default:
			this.dialogOwner.errorTitle = "ui.lobbyDialog.join.title.unknown";
			this.dialogOwner.errorBody = "ui.lobbyDialog.join.body.unknown";
			this.dialogOwner.OnUpdate();
			break;
		}
	}
}
