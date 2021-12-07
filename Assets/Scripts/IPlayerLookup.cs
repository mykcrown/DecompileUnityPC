// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IPlayerLookup
{
	PlayerReference GetPlayerReference(PlayerNum player);

	PlayerController GetPlayerController(PlayerNum player);

	List<PlayerController> GetPlayers();
}
