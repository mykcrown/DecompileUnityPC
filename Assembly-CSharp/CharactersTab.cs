using System;
using UnityEngine;

// Token: 0x020009DB RID: 2523
public class CharactersTab : StoreTabElement
{
	// Token: 0x1700111F RID: 4383
	// (get) Token: 0x06004764 RID: 18276 RVA: 0x00136298 File Offset: 0x00134698
	// (set) Token: 0x06004765 RID: 18277 RVA: 0x001362A0 File Offset: 0x001346A0
	[Inject]
	public ICharactersTabAPI api { get; set; }

	// Token: 0x17001120 RID: 4384
	// (get) Token: 0x06004766 RID: 18278 RVA: 0x001362A9 File Offset: 0x001346A9
	// (set) Token: 0x06004767 RID: 18279 RVA: 0x001362B1 File Offset: 0x001346B1
	[Inject]
	public ICharactersTabAPI charactersTabAPI { get; set; }

	// Token: 0x17001121 RID: 4385
	// (get) Token: 0x06004768 RID: 18280 RVA: 0x001362BA File Offset: 0x001346BA
	// (set) Token: 0x06004769 RID: 18281 RVA: 0x001362C2 File Offset: 0x001346C2
	[Inject]
	public ICharacterEquipViewAPI equipViewAPI { get; set; }

	// Token: 0x0600476A RID: 18282 RVA: 0x001362CC File Offset: 0x001346CC
	public override void Awake()
	{
		base.Awake();
		this.introView = UnityEngine.Object.Instantiate<CharacterIntroView>(this.IntroViewPrefab, base.transform);
		this.equipView = UnityEngine.Object.Instantiate<CharacterEquipView>(this.EquipViewPrefab, base.transform);
		this.introView.AllowInteraction = new Func<bool>(this.isIntroViewFocused);
		this.equipView.AllowInteraction = new Func<bool>(this.isEquipViewFocused);
		CharacterIntroView characterIntroView = this.introView;
		characterIntroView.CharacterSelected = (Action<CharacterID>)Delegate.Combine(characterIntroView.CharacterSelected, new Action<CharacterID>(delegate(CharacterID characterID)
		{
			this.equipViewAPI.SelectedCharacter = characterID;
			this.charactersTabAPI.SetState(CharactersTabState.EquipView, false);
			this.introView.PlayTransition();
			this.equipView.PlayTransition();
			base.audioManager.PlayMenuSound(SoundKey.store_characterEquipViewOpen, 0f);
		}));
	}

	// Token: 0x0600476B RID: 18283 RVA: 0x00136362 File Offset: 0x00134762
	[PostConstruct]
	public void Init()
	{
		base.listen("CharactersTabAPI.UPDATED", new Action(this.onTabStateUpdated));
	}

	// Token: 0x0600476C RID: 18284 RVA: 0x0013637C File Offset: 0x0013477C
	private void onTabStateUpdated()
	{
		if (this.currentState != this.charactersTabAPI.GetState())
		{
			this.currentState = this.charactersTabAPI.GetState();
			if (this.currentState == CharactersTabState.EquipView)
			{
				this.introView.ReleaseSelections();
				this.equipView.OnStateActivate();
			}
			else
			{
				this.equipView.ReleaseSelections();
				this.introView.IntroPanelView.SyncButtonSelectionToEquipView();
			}
		}
	}

	// Token: 0x0600476D RID: 18285 RVA: 0x001363F2 File Offset: 0x001347F2
	private bool isIntroViewFocused()
	{
		return base.allowInteraction() && this.api.GetState() == CharactersTabState.IntroView;
	}

	// Token: 0x0600476E RID: 18286 RVA: 0x00136410 File Offset: 0x00134810
	private bool isEquipViewFocused()
	{
		return base.allowInteraction() && this.api.GetState() == CharactersTabState.EquipView;
	}

	// Token: 0x0600476F RID: 18287 RVA: 0x0013642E File Offset: 0x0013482E
	public override void OnDrawComplete()
	{
		this.introView.OnDrawComplete();
		this.equipView.OnDrawComplete();
	}

	// Token: 0x06004770 RID: 18288 RVA: 0x00136448 File Offset: 0x00134848
	public override bool OnCancelPressed()
	{
		bool flag = this.equipView.OnCancelPressed();
		if (!flag)
		{
			flag = this.tryReturnToIntroView();
		}
		return flag;
	}

	// Token: 0x06004771 RID: 18289 RVA: 0x0013646F File Offset: 0x0013486F
	public override bool OnBackButtonClicked()
	{
		return this.tryReturnToIntroView();
	}

	// Token: 0x06004772 RID: 18290 RVA: 0x00136477 File Offset: 0x00134877
	private bool tryReturnToIntroView()
	{
		if (this.charactersTabAPI.GetState() == CharactersTabState.EquipView)
		{
			this.charactersTabAPI.SetState(CharactersTabState.IntroView, false);
			base.audioManager.PlayMenuSound(SoundKey.store_characterEquipViewClosed, 0f);
			return true;
		}
		return false;
	}

	// Token: 0x06004773 RID: 18291 RVA: 0x001364AC File Offset: 0x001348AC
	public override void OnLeft()
	{
		this.equipView.OnLeft();
	}

	// Token: 0x06004774 RID: 18292 RVA: 0x001364B9 File Offset: 0x001348B9
	public override void OnZPressed()
	{
		this.equipView.OnZPressed();
	}

	// Token: 0x06004775 RID: 18293 RVA: 0x001364C6 File Offset: 0x001348C6
	public override void OnLeftBumperPressed()
	{
		this.equipView.OnLeftBumperPressed();
	}

	// Token: 0x06004776 RID: 18294 RVA: 0x001364D3 File Offset: 0x001348D3
	public override void OnActivate()
	{
		base.OnActivate();
		this.introView.OnActivate();
		this.equipView.OnActivate();
	}

	// Token: 0x06004777 RID: 18295 RVA: 0x001364F1 File Offset: 0x001348F1
	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.introView.UpdateMouseMode();
		this.equipView.UpdateMouseMode();
	}

	// Token: 0x06004778 RID: 18296 RVA: 0x0013650F File Offset: 0x0013490F
	public override void OnRight()
	{
		this.equipView.OnRight();
	}

	// Token: 0x06004779 RID: 18297 RVA: 0x0013651C File Offset: 0x0013491C
	public override void OnYButtonPressed()
	{
		this.equipView.OnYButtonPressed();
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x00136529 File Offset: 0x00134929
	public override void OnRightStickUp()
	{
		this.equipView.OnRightStickUp();
	}

	// Token: 0x0600477B RID: 18299 RVA: 0x00136536 File Offset: 0x00134936
	public override void OnRightStickDown()
	{
		this.equipView.OnRightStickDown();
	}

	// Token: 0x0600477C RID: 18300 RVA: 0x00136543 File Offset: 0x00134943
	public override void UpdateRightStick(float x, float y)
	{
		this.equipView.UpdateRightStick(x, y);
	}

	// Token: 0x04002F2A RID: 12074
	public CharacterIntroView IntroViewPrefab;

	// Token: 0x04002F2B RID: 12075
	public CharacterEquipView EquipViewPrefab;

	// Token: 0x04002F2C RID: 12076
	private CharacterIntroView introView;

	// Token: 0x04002F2D RID: 12077
	private CharacterEquipView equipView;

	// Token: 0x04002F2E RID: 12078
	private CharactersTabState currentState;
}
