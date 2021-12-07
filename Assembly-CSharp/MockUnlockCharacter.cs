using System;

// Token: 0x020006D7 RID: 1751
public class MockUnlockCharacter : IUnlockCharacter
{
	// Token: 0x17000ACA RID: 2762
	// (get) Token: 0x06002BF9 RID: 11257 RVA: 0x000E4B06 File Offset: 0x000E2F06
	// (set) Token: 0x06002BFA RID: 11258 RVA: 0x000E4B0E File Offset: 0x000E2F0E
	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel { get; set; }

	// Token: 0x17000ACB RID: 2763
	// (get) Token: 0x06002BFB RID: 11259 RVA: 0x000E4B17 File Offset: 0x000E2F17
	// (set) Token: 0x06002BFC RID: 11260 RVA: 0x000E4B1F File Offset: 0x000E2F1F
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000ACC RID: 2764
	// (get) Token: 0x06002BFD RID: 11261 RVA: 0x000E4B28 File Offset: 0x000E2F28
	// (set) Token: 0x06002BFE RID: 11262 RVA: 0x000E4B30 File Offset: 0x000E2F30
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x17000ACD RID: 2765
	// (get) Token: 0x06002BFF RID: 11263 RVA: 0x000E4B39 File Offset: 0x000E2F39
	// (set) Token: 0x06002C00 RID: 11264 RVA: 0x000E4B41 File Offset: 0x000E2F41
	[Inject]
	public IUserCurrencyModel userCurrencyModel { get; set; }

	// Token: 0x06002C01 RID: 11265 RVA: 0x000E4B4C File Offset: 0x000E2F4C
	public void PurchaseWithHard(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		this.timer.SetTimeout(1000, delegate
		{
			this.userCharacterUnlockModel.SetUnlocked(characterId, false);
			callback(UserPurchaseResult.SUCCESS);
		});
	}

	// Token: 0x06002C02 RID: 11266 RVA: 0x000E4B90 File Offset: 0x000E2F90
	public void PurchaseWithSoft(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		this.timer.SetTimeout(2000, delegate
		{
			this.userCharacterUnlockModel.SetUnlocked(characterId, false);
			this.userCurrencyModel.Spectra -= this.GetSoftPrice(characterId);
			callback(UserPurchaseResult.SUCCESS);
		});
	}

	// Token: 0x06002C03 RID: 11267 RVA: 0x000E4BD4 File Offset: 0x000E2FD4
	public void PurchaseWithToken(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		this.timer.SetTimeout(2000, delegate
		{
			this.userCharacterUnlockModel.SetUnlocked(characterId, false);
			this.userCurrencyModel.CharacterUnlockTokens--;
			callback(UserPurchaseResult.SUCCESS);
		});
	}

	// Token: 0x06002C04 RID: 11268 RVA: 0x000E4C18 File Offset: 0x000E3018
	public float GetHardPrice(CharacterID characterId)
	{
		return 8.72f;
	}

	// Token: 0x06002C05 RID: 11269 RVA: 0x000E4C1F File Offset: 0x000E301F
	public int GetSoftPrice(CharacterID characterId)
	{
		return 300;
	}

	// Token: 0x06002C06 RID: 11270 RVA: 0x000E4C28 File Offset: 0x000E3028
	public string GetHardPriceString(CharacterID characterId)
	{
		float hardPrice = this.GetHardPrice(characterId);
		return this.localization.GetHardPriceString(hardPrice);
	}

	// Token: 0x06002C07 RID: 11271 RVA: 0x000E4C4C File Offset: 0x000E304C
	public string GetSoftPriceString(CharacterID characterId)
	{
		int softPrice = this.GetSoftPrice(characterId);
		return this.localization.GetSoftPriceString(softPrice);
	}

	// Token: 0x06002C08 RID: 11272 RVA: 0x000E4C6D File Offset: 0x000E306D
	public void SetSoftPrice(CharacterID id, ulong packageInfo, ulong price)
	{
	}

	// Token: 0x06002C09 RID: 11273 RVA: 0x000E4C6F File Offset: 0x000E306F
	public void SetHardPrice(CharacterID id, ulong packageInfo, ulong price)
	{
	}
}
