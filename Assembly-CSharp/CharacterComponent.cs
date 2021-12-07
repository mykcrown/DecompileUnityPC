using System;
using UnityEngine;

// Token: 0x020005A1 RID: 1441
public class CharacterComponent : ScriptableObject, ICharacterComponent, ISerializationCallbackReceiver, IPreloadedGameAsset, IMoveRequirementValidator
{
	// Token: 0x1700072A RID: 1834
	// (get) Token: 0x06002088 RID: 8328 RVA: 0x00086C9A File Offset: 0x0008509A
	// (set) Token: 0x06002089 RID: 8329 RVA: 0x00086CA2 File Offset: 0x000850A2
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x0600208A RID: 8330 RVA: 0x00086CAB File Offset: 0x000850AB
	// (set) Token: 0x0600208B RID: 8331 RVA: 0x00086CB3 File Offset: 0x000850B3
	[Inject]
	public IDependencyInjection dependencyInjection { get; set; }

	// Token: 0x1700072C RID: 1836
	// (get) Token: 0x0600208C RID: 8332 RVA: 0x00086CBC File Offset: 0x000850BC
	// (set) Token: 0x0600208D RID: 8333 RVA: 0x00086CC4 File Offset: 0x000850C4
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x0600208E RID: 8334 RVA: 0x00086CCD File Offset: 0x000850CD
	// (set) Token: 0x0600208F RID: 8335 RVA: 0x00086CD5 File Offset: 0x000850D5
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x06002090 RID: 8336 RVA: 0x00086CDE File Offset: 0x000850DE
	public virtual void Init(IPlayerDelegate playerDelegate)
	{
		this.inject();
		this.playerDelegate = playerDelegate;
	}

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x06002091 RID: 8337 RVA: 0x00086CED File Offset: 0x000850ED
	protected GameManager gameManager
	{
		get
		{
			return (this.gameController != null) ? this.gameController.currentGame : null;
		}
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x00086D0B File Offset: 0x0008510B
	private void inject()
	{
		StaticInject.Inject(this);
	}

	// Token: 0x06002093 RID: 8339 RVA: 0x00086D13 File Offset: 0x00085113
	public virtual void LoadState(IComponentState state)
	{
	}

	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x06002094 RID: 8340 RVA: 0x00086D15 File Offset: 0x00085115
	public virtual IComponentState State
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06002095 RID: 8341 RVA: 0x00086D18 File Offset: 0x00085118
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06002096 RID: 8342 RVA: 0x00086D1A File Offset: 0x0008511A
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06002097 RID: 8343 RVA: 0x00086D1C File Offset: 0x0008511C
	public virtual void Destroy()
	{
		this.gameController = null;
		this.playerDelegate = null;
	}

	// Token: 0x06002098 RID: 8344 RVA: 0x00086D2C File Offset: 0x0008512C
	public virtual bool ValidateRequirements(MoveData move, IPlayerDelegate player, InputButtonsData input)
	{
		return true;
	}

	// Token: 0x06002099 RID: 8345 RVA: 0x00086D2F File Offset: 0x0008512F
	public virtual void RegisterPreload(PreloadContext context)
	{
	}

	// Token: 0x040019ED RID: 6637
	protected IPlayerDelegate playerDelegate;
}
