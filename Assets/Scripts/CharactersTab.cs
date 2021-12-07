// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CharactersTab : StoreTabElement
{
	public CharacterIntroView IntroViewPrefab;

	public CharacterEquipView EquipViewPrefab;

	private CharacterIntroView introView;

	private CharacterEquipView equipView;

	private CharactersTabState currentState;

	[Inject]
	public ICharactersTabAPI api
	{
		get;
		set;
	}

	[Inject]
	public ICharactersTabAPI charactersTabAPI
	{
		get;
		set;
	}

	[Inject]
	public ICharacterEquipViewAPI equipViewAPI
	{
		get;
		set;
	}

	public override void Awake()
	{
		base.Awake();
		this.introView = UnityEngine.Object.Instantiate<CharacterIntroView>(this.IntroViewPrefab, base.transform);
		this.equipView = UnityEngine.Object.Instantiate<CharacterEquipView>(this.EquipViewPrefab, base.transform);
		this.introView.AllowInteraction = new Func<bool>(this.isIntroViewFocused);
		this.equipView.AllowInteraction = new Func<bool>(this.isEquipViewFocused);
		CharacterIntroView expr_68 = this.introView;
		expr_68.CharacterSelected = (Action<CharacterID>)Delegate.Combine(expr_68.CharacterSelected, new Action<CharacterID>(this._Awake_m__0));
	}

	[PostConstruct]
	public void Init()
	{
		base.listen("CharactersTabAPI.UPDATED", new Action(this.onTabStateUpdated));
	}

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

	private bool isIntroViewFocused()
	{
		return base.allowInteraction() && this.api.GetState() == CharactersTabState.IntroView;
	}

	private bool isEquipViewFocused()
	{
		return base.allowInteraction() && this.api.GetState() == CharactersTabState.EquipView;
	}

	public override void OnDrawComplete()
	{
		this.introView.OnDrawComplete();
		this.equipView.OnDrawComplete();
	}

	public override bool OnCancelPressed()
	{
		bool flag = this.equipView.OnCancelPressed();
		if (!flag)
		{
			flag = this.tryReturnToIntroView();
		}
		return flag;
	}

	public override bool OnBackButtonClicked()
	{
		return this.tryReturnToIntroView();
	}

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

	public override void OnLeft()
	{
		this.equipView.OnLeft();
	}

	public override void OnZPressed()
	{
		this.equipView.OnZPressed();
	}

	public override void OnLeftBumperPressed()
	{
		this.equipView.OnLeftBumperPressed();
	}

	public override void OnActivate()
	{
		base.OnActivate();
		this.introView.OnActivate();
		this.equipView.OnActivate();
	}

	public override void UpdateMouseMode()
	{
		base.UpdateMouseMode();
		this.introView.UpdateMouseMode();
		this.equipView.UpdateMouseMode();
	}

	public override void OnRight()
	{
		this.equipView.OnRight();
	}

	public override void OnYButtonPressed()
	{
		this.equipView.OnYButtonPressed();
	}

	public override void OnRightStickUp()
	{
		this.equipView.OnRightStickUp();
	}

	public override void OnRightStickDown()
	{
		this.equipView.OnRightStickDown();
	}

	public override void UpdateRightStick(float x, float y)
	{
		this.equipView.UpdateRightStick(x, y);
	}

	private void _Awake_m__0(CharacterID characterID)
	{
		this.equipViewAPI.SelectedCharacter = characterID;
		this.charactersTabAPI.SetState(CharactersTabState.EquipView, false);
		this.introView.PlayTransition();
		this.equipView.PlayTransition();
		base.audioManager.PlayMenuSound(SoundKey.store_characterEquipViewOpen, 0f);
	}
}
