using System;
using System.Collections.Generic;

// Token: 0x02000903 RID: 2307
public class TokenManager : ITokenManager
{
	// Token: 0x06003BF2 RID: 15346 RVA: 0x001165E7 File Offset: 0x001149E7
	public void Init(Func<PlayerNum, IPlayerCursor> findPlayerCursor)
	{
		this.findPlayerCursor = findPlayerCursor;
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x001165F0 File Offset: 0x001149F0
	public void Reset()
	{
		this.tokens.Clear();
		this.currentlyGrabbing.Clear();
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x00116608 File Offset: 0x00114A08
	public void ReleaseFunctions()
	{
		this.findPlayerCursor = null;
	}

	// Token: 0x06003BF5 RID: 15349 RVA: 0x00116614 File Offset: 0x00114A14
	public PlayerToken[] GetAll()
	{
		PlayerToken[] array = new PlayerToken[this.tokens.Values.Count];
		this.tokens.Values.CopyTo(array, 0);
		return array;
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x0011664A File Offset: 0x00114A4A
	public PlayerToken GetCurrentlyGrabbing(PlayerNum playerNum)
	{
		if (this.currentlyGrabbing.ContainsKey(playerNum))
		{
			return this.currentlyGrabbing[playerNum];
		}
		return null;
	}

	// Token: 0x06003BF7 RID: 15351 RVA: 0x0011666C File Offset: 0x00114A6C
	private void setGrabbing(PlayerNum playerNum, PlayerToken token)
	{
		if (playerNum != PlayerNum.None)
		{
			if (token == null)
			{
				if (this.currentlyGrabbing.ContainsKey(playerNum))
				{
					this.currentlyGrabbing.Remove(playerNum);
				}
			}
			else
			{
				this.currentlyGrabbing[playerNum] = token;
			}
		}
	}

	// Token: 0x06003BF8 RID: 15352 RVA: 0x001166C0 File Offset: 0x00114AC0
	public void ReleaseAnyGrabbers(PlayerToken token)
	{
		List<PlayerNum> list = new List<PlayerNum>();
		foreach (PlayerNum playerNum in this.currentlyGrabbing.Keys)
		{
			if (this.currentlyGrabbing[playerNum] == token)
			{
				list.Add(playerNum);
			}
		}
		foreach (PlayerNum player in list)
		{
			this.ReleaseToken(player);
		}
	}

	// Token: 0x06003BF9 RID: 15353 RVA: 0x00116788 File Offset: 0x00114B88
	public PlayerToken GetPlayerToken(PlayerNum playerNum)
	{
		if (this.tokens.ContainsKey(playerNum))
		{
			return this.tokens[playerNum];
		}
		return null;
	}

	// Token: 0x06003BFA RID: 15354 RVA: 0x001167AC File Offset: 0x00114BAC
	public PlayerNum IsBeingGrabbedByPlayer(PlayerToken token)
	{
		if (token != null && this.currentlyGrabbing.ContainsValue(token))
		{
			foreach (PlayerNum playerNum in this.currentlyGrabbing.Keys)
			{
				if (this.currentlyGrabbing[playerNum] == token)
				{
					return playerNum;
				}
			}
			return PlayerNum.None;
		}
		return PlayerNum.None;
	}

	// Token: 0x06003BFB RID: 15355 RVA: 0x00116848 File Offset: 0x00114C48
	public void ReleaseToken(PlayerNum player)
	{
		PlayerToken playerToken = this.GetCurrentlyGrabbing(player);
		if (playerToken != null)
		{
			this.setGrabbing(player, null);
			playerToken.Attach(null, false);
		}
	}

	// Token: 0x06003BFC RID: 15356 RVA: 0x0011687C File Offset: 0x00114C7C
	public void GrabToken(PlayerNum player, PlayerToken token, float clickTime = 0f)
	{
		this.ReleaseAnyGrabbers(token);
		this.ReleaseToken(player);
		IPlayerCursor playerCursor = this.findPlayerCursor(player);
		this.setGrabbing(player, token);
		if (token != null)
		{
			token.GrabbedByTime = clickTime;
			if (playerCursor != null)
			{
				token.Attach(playerCursor, false);
			}
		}
	}

	// Token: 0x06003BFD RID: 15357 RVA: 0x001168CD File Offset: 0x00114CCD
	public void AddToken(PlayerNum owner, PlayerToken token)
	{
		this.tokens[owner] = token;
	}

	// Token: 0x06003BFE RID: 15358 RVA: 0x001168DC File Offset: 0x00114CDC
	public void RemoveToken(PlayerNum owner, PlayerToken token)
	{
		if (this.tokens.ContainsKey(owner))
		{
			this.tokens.Remove(owner);
		}
	}

	// Token: 0x04002925 RID: 10533
	private Dictionary<PlayerNum, PlayerToken> tokens = new Dictionary<PlayerNum, PlayerToken>();

	// Token: 0x04002926 RID: 10534
	private Dictionary<PlayerNum, PlayerToken> currentlyGrabbing = new Dictionary<PlayerNum, PlayerToken>();

	// Token: 0x04002927 RID: 10535
	private Func<PlayerNum, IPlayerCursor> findPlayerCursor;
}
