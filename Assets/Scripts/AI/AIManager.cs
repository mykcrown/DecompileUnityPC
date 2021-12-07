// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class AIManager : IAIManager
	{
		private CompositeNodeData passiveRootNodeData;

		private CompositeNodeData defaultRootNodeData;

		[Inject]
		public GameController gameController
		{
			get;
			set;
		}

		public CompositeNodeData GetPassiveRootNode()
		{
			if (this.passiveRootNodeData == null)
			{
				this.passiveRootNodeData = Resources.Load<CompositeNodeData>("BehaviorTree/PassiveRoot");
			}
			return this.passiveRootNodeData;
		}

		public CompositeNodeData GetDefaultRootNode()
		{
			if (this.defaultRootNodeData == null)
			{
				this.defaultRootNodeData = Resources.Load<CompositeNodeData>("BehaviorTree/DefaultRoot");
			}
			return this.defaultRootNodeData;
		}

		public bool IsAnyAIActive()
		{
			if (this.gameController.currentGame == null)
			{
				return false;
			}
			List<PlayerReference> playerReferences = this.gameController.currentGame.PlayerReferences;
			foreach (PlayerReference current in playerReferences)
			{
				AIInput x = current.InputController as AIInput;
				if (x != null)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsAnyPassiveAIActive()
		{
			if (this.gameController.currentGame == null)
			{
				return false;
			}
			List<PlayerReference> playerReferences = this.gameController.currentGame.PlayerReferences;
			foreach (PlayerReference current in playerReferences)
			{
				if (current.IsPassiveAI)
				{
					return true;
				}
			}
			return false;
		}
	}
}
