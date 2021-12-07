using System;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
public class MoveComponent : ScriptableObject, IMoveComponent, IMoveRequirementValidator, IPreloadedGameAsset
{
	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x06001B45 RID: 6981 RVA: 0x00089CF0 File Offset: 0x000880F0
	// (set) Token: 0x06001B46 RID: 6982 RVA: 0x00089CF8 File Offset: 0x000880F8
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x06001B47 RID: 6983 RVA: 0x00089D01 File Offset: 0x00088101
	// (set) Token: 0x06001B48 RID: 6984 RVA: 0x00089D09 File Offset: 0x00088109
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x06001B49 RID: 6985 RVA: 0x00089D12 File Offset: 0x00088112
	// (set) Token: 0x06001B4A RID: 6986 RVA: 0x00089D1A File Offset: 0x0008811A
	public MoveData moveInfo { get; set; }

	// Token: 0x06001B4B RID: 6987 RVA: 0x00089D23 File Offset: 0x00088123
	public virtual void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		this.moveDelegate = moveDelegate;
		this.playerDelegate = playerDelegate;
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x00089D33 File Offset: 0x00088133
	public virtual void RegisterPreload(PreloadContext context)
	{
		if (this.moveInfo != null)
		{
			this.moveInfo.RegisterPreload(context);
		}
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x00089D52 File Offset: 0x00088152
	public virtual bool ValidateRequirements(MoveData data, IPlayerDelegate player, InputButtonsData input)
	{
		return true;
	}

	// Token: 0x040014B0 RID: 5296
	protected IPlayerDelegate playerDelegate;

	// Token: 0x040014B1 RID: 5297
	protected IMoveDelegate moveDelegate;
}
