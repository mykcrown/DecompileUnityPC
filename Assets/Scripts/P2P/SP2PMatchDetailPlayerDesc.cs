// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;

namespace P2P
{
	public class SP2PMatchDetailPlayerDesc
	{
		public bool stageLoaded;

		public ECharacterType characterID;

		public bool isDisconnected;

		public ushort characterIndex;

		public ushort skinID;

		public List<ushort> equippedPlayerItemIds = new List<ushort>();

		public List<ushort> equippedCharacterItemIds = new List<ushort>();
	}
}
