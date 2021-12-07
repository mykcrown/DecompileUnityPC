using System;
using Steamworks;
using TMPro;
using UnityEngine;

// Token: 0x020009A6 RID: 2470
public class LobbyGameButton : MonoBehaviour
{
	// Token: 0x060043C5 RID: 17349 RVA: 0x0012B968 File Offset: 0x00129D68
	public void Initialize(ulong lobbyId, string name, int players, int maxPlayers, JoinLobbyDialog dialogOwner)
	{
		this.lobbyId = lobbyId;
		this.dialogOwner = dialogOwner;
		this.gameName.text = name;
		this.playerCounts.text = players.ToString() + "/" + maxPlayers.ToString();
		dialogOwner.inputButtons.AddButton(this.button, new Action(this.onClick));
	}

	// Token: 0x060043C6 RID: 17350 RVA: 0x0012B9DD File Offset: 0x00129DDD
	public void SetButtonActive(bool active)
	{
		this.dialogOwner.inputButtons.SetButtonEnabled(this.button, active);
		base.gameObject.SetActive(active);
	}

	// Token: 0x060043C7 RID: 17351 RVA: 0x0012BA02 File Offset: 0x00129E02
	public void onClick()
	{
		this.dialogOwner.steamManager.JoinLobby(this.lobbyId, delegate(EChatRoomEnterResponse response)
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
		});
	}

	// Token: 0x060043C8 RID: 17352 RVA: 0x0012BA26 File Offset: 0x00129E26
	public void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002D21 RID: 11553
	public TextMeshProUGUI gameName;

	// Token: 0x04002D22 RID: 11554
	public TextMeshProUGUI playerCounts;

	// Token: 0x04002D23 RID: 11555
	public MenuItemButton button;

	// Token: 0x04002D24 RID: 11556
	private JoinLobbyDialog dialogOwner;

	// Token: 0x04002D25 RID: 11557
	private ulong lobbyId;
}
