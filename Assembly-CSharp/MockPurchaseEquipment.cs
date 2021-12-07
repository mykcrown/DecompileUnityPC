using System;
using UnityEngine;

// Token: 0x0200073C RID: 1852
public class MockPurchaseEquipment : IUserPurchaseEquipment
{
	// Token: 0x17000B38 RID: 2872
	// (get) Token: 0x06002DD5 RID: 11733 RVA: 0x000E9C92 File Offset: 0x000E8092
	// (set) Token: 0x06002DD6 RID: 11734 RVA: 0x000E9C9A File Offset: 0x000E809A
	[Inject]
	public IUserInventory userInventory { get; set; }

	// Token: 0x17000B39 RID: 2873
	// (get) Token: 0x06002DD7 RID: 11735 RVA: 0x000E9CA3 File Offset: 0x000E80A3
	// (set) Token: 0x06002DD8 RID: 11736 RVA: 0x000E9CAB File Offset: 0x000E80AB
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000B3A RID: 2874
	// (get) Token: 0x06002DD9 RID: 11737 RVA: 0x000E9CB4 File Offset: 0x000E80B4
	// (set) Token: 0x06002DDA RID: 11738 RVA: 0x000E9CBC File Offset: 0x000E80BC
	[Inject]
	public ILootBoxesModel lootBoxesModel { get; set; }

	// Token: 0x17000B3B RID: 2875
	// (get) Token: 0x06002DDB RID: 11739 RVA: 0x000E9CC5 File Offset: 0x000E80C5
	// (set) Token: 0x06002DDC RID: 11740 RVA: 0x000E9CCD File Offset: 0x000E80CD
	[Inject]
	public IUserLootboxesModel userLootboxesModel { get; set; }

	// Token: 0x17000B3C RID: 2876
	// (get) Token: 0x06002DDD RID: 11741 RVA: 0x000E9CD6 File Offset: 0x000E80D6
	// (set) Token: 0x06002DDE RID: 11742 RVA: 0x000E9CDE File Offset: 0x000E80DE
	[Inject]
	public IUserCurrencyModel userCurrencyModel { get; set; }

	// Token: 0x06002DDF RID: 11743 RVA: 0x000E9CE8 File Offset: 0x000E80E8
	public void Purchase(EquipmentID itemId, Action<UserPurchaseResult> callback)
	{
		this.timer.SetTimeout(100, delegate
		{
			this.userInventory.AddItem(itemId, true);
			callback(UserPurchaseResult.SUCCESS);
		});
	}

	// Token: 0x06002DE0 RID: 11744 RVA: 0x000E9D2C File Offset: 0x000E812C
	public void PurchaseManual(ulong packageId, CurrencyType currencyType, ulong price, Action<UserPurchaseResult> callback)
	{
		this.timer.SetTimeout(100, delegate
		{
			LootBoxPackage boxToBuy = this.lootBoxesModel.GetBoxToBuy(packageId);
			if (boxToBuy != null)
			{
				this.userLootboxesModel.Add((int)boxToBuy.lootBoxId, (int)boxToBuy.quantity);
			}
			else
			{
				Debug.LogWarning("Offline manual purchase does not grant item, but will clear success");
			}
			if (currencyType == CurrencyType.Soft)
			{
				this.userCurrencyModel.Spectra -= (int)price;
			}
			callback(UserPurchaseResult.SUCCESS);
		});
	}

	// Token: 0x06002DE1 RID: 11745 RVA: 0x000E9D7C File Offset: 0x000E817C
	public void UnlockTokenPurchase(CharacterID character, Action<UserPurchaseResult> callback)
	{
		this.timer.SetTimeout(100, delegate
		{
			this.userCurrencyModel.CharacterUnlockTokens--;
			callback(UserPurchaseResult.SUCCESS);
		});
	}
}
