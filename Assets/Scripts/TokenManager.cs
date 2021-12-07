// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class TokenManager : ITokenManager
{
	private Dictionary<PlayerNum, PlayerToken> tokens = new Dictionary<PlayerNum, PlayerToken>();

	private Dictionary<PlayerNum, PlayerToken> currentlyGrabbing = new Dictionary<PlayerNum, PlayerToken>();

	private Func<PlayerNum, IPlayerCursor> findPlayerCursor;

	public void Init(Func<PlayerNum, IPlayerCursor> findPlayerCursor)
	{
		this.findPlayerCursor = findPlayerCursor;
	}

	public void Reset()
	{
		this.tokens.Clear();
		this.currentlyGrabbing.Clear();
	}

	public void ReleaseFunctions()
	{
		this.findPlayerCursor = null;
	}

	public PlayerToken[] GetAll()
	{
		PlayerToken[] array = new PlayerToken[this.tokens.Values.Count];
		this.tokens.Values.CopyTo(array, 0);
		return array;
	}

	public PlayerToken GetCurrentlyGrabbing(PlayerNum playerNum)
	{
		if (this.currentlyGrabbing.ContainsKey(playerNum))
		{
			return this.currentlyGrabbing[playerNum];
		}
		return null;
	}

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

	public void ReleaseAnyGrabbers(PlayerToken token)
	{
		List<PlayerNum> list = new List<PlayerNum>();
		foreach (PlayerNum current in this.currentlyGrabbing.Keys)
		{
			if (this.currentlyGrabbing[current] == token)
			{
				list.Add(current);
			}
		}
		foreach (PlayerNum current2 in list)
		{
			this.ReleaseToken(current2);
		}
	}

	public PlayerToken GetPlayerToken(PlayerNum playerNum)
	{
		if (this.tokens.ContainsKey(playerNum))
		{
			return this.tokens[playerNum];
		}
		return null;
	}

	public PlayerNum IsBeingGrabbedByPlayer(PlayerToken token)
	{
		if (token != null && this.currentlyGrabbing.ContainsValue(token))
		{
			foreach (PlayerNum current in this.currentlyGrabbing.Keys)
			{
				if (this.currentlyGrabbing[current] == token)
				{
					return current;
				}
			}
			return PlayerNum.None;
		}
		return PlayerNum.None;
	}

	public void ReleaseToken(PlayerNum player)
	{
		PlayerToken playerToken = this.GetCurrentlyGrabbing(player);
		if (playerToken != null)
		{
			this.setGrabbing(player, null);
			playerToken.Attach(null, false);
		}
	}

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

	public void AddToken(PlayerNum owner, PlayerToken token)
	{
		this.tokens[owner] = token;
	}

	public void RemoveToken(PlayerNum owner, PlayerToken token)
	{
		if (this.tokens.ContainsKey(owner))
		{
			this.tokens.Remove(owner);
		}
	}
}
