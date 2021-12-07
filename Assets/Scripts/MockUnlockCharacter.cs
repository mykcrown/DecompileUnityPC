// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

public class MockUnlockCharacter : IUnlockCharacter
{
	private sealed class _PurchaseWithHard_c__AnonStorey0
	{
		internal CharacterID characterId;

		internal Action<UserPurchaseResult> callback;

		internal MockUnlockCharacter _this;

		internal void __m__0()
		{
			this._this.userCharacterUnlockModel.SetUnlocked(this.characterId, false);
			this.callback(UserPurchaseResult.SUCCESS);
		}
	}

	private sealed class _PurchaseWithSoft_c__AnonStorey1
	{
		internal CharacterID characterId;

		internal Action<UserPurchaseResult> callback;

		internal MockUnlockCharacter _this;

		internal void __m__0()
		{
			this._this.userCharacterUnlockModel.SetUnlocked(this.characterId, false);
			this._this.userCurrencyModel.Spectra -= this._this.GetSoftPrice(this.characterId);
			this.callback(UserPurchaseResult.SUCCESS);
		}
	}

	private sealed class _PurchaseWithToken_c__AnonStorey2
	{
		internal CharacterID characterId;

		internal Action<UserPurchaseResult> callback;

		internal MockUnlockCharacter _this;

		internal void __m__0()
		{
			this._this.userCharacterUnlockModel.SetUnlocked(this.characterId, false);
			this._this.userCurrencyModel.CharacterUnlockTokens--;
			this.callback(UserPurchaseResult.SUCCESS);
		}
	}

	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IUserCurrencyModel userCurrencyModel
	{
		get;
		set;
	}

	public void PurchaseWithHard(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		MockUnlockCharacter._PurchaseWithHard_c__AnonStorey0 _PurchaseWithHard_c__AnonStorey = new MockUnlockCharacter._PurchaseWithHard_c__AnonStorey0();
		_PurchaseWithHard_c__AnonStorey.characterId = characterId;
		_PurchaseWithHard_c__AnonStorey.callback = callback;
		_PurchaseWithHard_c__AnonStorey._this = this;
		this.timer.SetTimeout(1000, new Action(_PurchaseWithHard_c__AnonStorey.__m__0));
	}

	public void PurchaseWithSoft(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		MockUnlockCharacter._PurchaseWithSoft_c__AnonStorey1 _PurchaseWithSoft_c__AnonStorey = new MockUnlockCharacter._PurchaseWithSoft_c__AnonStorey1();
		_PurchaseWithSoft_c__AnonStorey.characterId = characterId;
		_PurchaseWithSoft_c__AnonStorey.callback = callback;
		_PurchaseWithSoft_c__AnonStorey._this = this;
		this.timer.SetTimeout(2000, new Action(_PurchaseWithSoft_c__AnonStorey.__m__0));
	}

	public void PurchaseWithToken(CharacterID characterId, Action<UserPurchaseResult> callback)
	{
		MockUnlockCharacter._PurchaseWithToken_c__AnonStorey2 _PurchaseWithToken_c__AnonStorey = new MockUnlockCharacter._PurchaseWithToken_c__AnonStorey2();
		_PurchaseWithToken_c__AnonStorey.characterId = characterId;
		_PurchaseWithToken_c__AnonStorey.callback = callback;
		_PurchaseWithToken_c__AnonStorey._this = this;
		this.timer.SetTimeout(2000, new Action(_PurchaseWithToken_c__AnonStorey.__m__0));
	}

	public float GetHardPrice(CharacterID characterId)
	{
		return 8.72f;
	}

	public int GetSoftPrice(CharacterID characterId)
	{
		return 300;
	}

	public string GetHardPriceString(CharacterID characterId)
	{
		float hardPrice = this.GetHardPrice(characterId);
		return this.localization.GetHardPriceString(hardPrice);
	}

	public string GetSoftPriceString(CharacterID characterId)
	{
		int softPrice = this.GetSoftPrice(characterId);
		return this.localization.GetSoftPriceString(softPrice);
	}

	public void SetSoftPrice(CharacterID id, ulong packageInfo, ulong price)
	{
	}

	public void SetHardPrice(CharacterID id, ulong packageInfo, ulong price)
	{
	}
}
