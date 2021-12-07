using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	// Token: 0x020002FD RID: 765
	public class AIManager : IAIManager
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x00062379 File Offset: 0x00060779
		// (set) Token: 0x060010C4 RID: 4292 RVA: 0x00062381 File Offset: 0x00060781
		[Inject]
		public GameController gameController { get; set; }

		// Token: 0x060010C5 RID: 4293 RVA: 0x0006238A File Offset: 0x0006078A
		public CompositeNodeData GetPassiveRootNode()
		{
			if (this.passiveRootNodeData == null)
			{
				this.passiveRootNodeData = Resources.Load<CompositeNodeData>("BehaviorTree/PassiveRoot");
			}
			return this.passiveRootNodeData;
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x000623B3 File Offset: 0x000607B3
		public CompositeNodeData GetDefaultRootNode()
		{
			if (this.defaultRootNodeData == null)
			{
				this.defaultRootNodeData = Resources.Load<CompositeNodeData>("BehaviorTree/DefaultRoot");
			}
			return this.defaultRootNodeData;
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x000623DC File Offset: 0x000607DC
		public bool IsAnyAIActive()
		{
			if (this.gameController.currentGame == null)
			{
				return false;
			}
			List<PlayerReference> playerReferences = this.gameController.currentGame.PlayerReferences;
			foreach (PlayerReference playerReference in playerReferences)
			{
				AIInput x = playerReference.InputController as AIInput;
				if (x != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0006247C File Offset: 0x0006087C
		public bool IsAnyPassiveAIActive()
		{
			if (this.gameController.currentGame == null)
			{
				return false;
			}
			List<PlayerReference> playerReferences = this.gameController.currentGame.PlayerReferences;
			foreach (PlayerReference playerReference in playerReferences)
			{
				if (playerReference.IsPassiveAI)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000A91 RID: 2705
		private CompositeNodeData passiveRootNodeData;

		// Token: 0x04000A92 RID: 2706
		private CompositeNodeData defaultRootNodeData;
	}
}
