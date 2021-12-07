using System;

// Token: 0x02000897 RID: 2199
public class ContextConfigUI : ContextConfig
{
	// Token: 0x0600373F RID: 14143 RVA: 0x00101520 File Offset: 0x000FF920
	public ContextConfigUI(MasterContext.InitContext initContext) : base(initContext)
	{
		this.injectionBinder.Bind<UIManager>().Bind<IScreenDisplay>().Bind<IWindowDisplay>().Bind<IGamewideOverlayDisplay>().ToSingleton();
		this.injectionBinder.Bind<IUIAdapter>().To<UIScreenAdapter>().ToSingleton();
		this.injectionBinder.Bind<ISelectionManager>().To<UISelectionManager>().ToSingleton();
		this.injectionBinder.Bind<ICharacterSelectModel>().Bind<CharacterSelectModel>().To<CharacterSelectModel>().ToSingleton();
		this.injectionBinder.Bind<IGamewideOverlayController>().To<GamewideOverlayController>().ToSingleton();
		this.injectionBinder.Bind<IDialogController>().To<DialogController>().ToSingleton();
		this.injectionBinder.Bind<IUIComponentLocator>().To<UIComponentLocator>().ToSingleton();
		this.injectionBinder.Bind<IUITextHelper>().To<UITextHelper>().ToSingleton();
		this.injectionBinder.Bind<IScreenNavigationHelper>().To<ScreenNavigationHelper>().ToSingleton();
		this.injectionBinder.Bind<MenuItemList>().To<MenuItemList>();
		this.injectionBinder.Bind<GenericWindowFlow>().To<GenericWindowFlow>();
		this.injectionBinder.Bind<ITokenManager>().To<TokenManager>().ToSingleton();
		this.injectionBinder.Bind<IMainOptionsCalculator>().To<MainOptionsCalculator>().ToSingleton();
		this.injectionBinder.Bind<OptionValueDisplay>().To<OptionValueDisplay>().ToSingleton();
		this.injectionBinder.Bind<UIPreload3DAssets>().ToSingleton();
		this.injectionBinder.Bind<IInputBlocker>().To<InputBlocker>().ToSingleton();
		this.injectionBinder.Bind<GamewideOverlayTransitionManager>().ToSingleton();
		this.injectionBinder.Bind<WindowTransitionManager>().ToSingleton();
		this.injectionBinder.Bind<IScreenTransitionMap>().To<ScreenTransitionMap>().ToSingleton();
		this.injectionBinder.Bind<IStoreTabsModel>().To<StoreTabsModel>().ToSingleton();
		this.injectionBinder.Bind<ICharactersTabAPI>().To<CharactersTabAPI>().ToSingleton();
		this.injectionBinder.Bind<ICharacterEquipViewAPI>().To<CharacterEquipViewAPI>().ToSingleton();
		this.injectionBinder.Bind<ICollectiblesTabAPI>().To<CollectiblesTabAPI>().ToSingleton();
		this.injectionBinder.Bind<ICollectiblesEquipViewAPI>().To<CollectiblesEquipViewAPI>().ToSingleton();
		this.injectionBinder.Bind<IUnlockEquipmentFlow>().To<UnlockEquipmentFlow>().ToSingleton();
		this.injectionBinder.Bind<INetsukeEquipViewAPI>().To<NetsukeEquipViewAPI>().ToSingleton();
		this.injectionBinder.Bind<IEquipFlow>().To<EquipFlow>().ToSingleton();
		this.injectionBinder.Bind<IFeaturedTabAPI>().To<FeaturedTabAPI>().ToSingleton();
		this.injectionBinder.Bind<IMainMenuAPI>().To<MainMenuAPI>().ToSingleton();
		this.injectionBinder.Bind<IEquipModuleAPI>().To<EquipmentSelectorAPI>().ToName("CharacterEquipView").ToSingleton();
		this.injectionBinder.Bind<IEquipModuleAPI>().To<EquipmentSelectorAPI>().ToName("CollectiblesEquipView").ToSingleton();
		this.injectionBinder.Bind<EquipTauntDialogAPI>().To<EquipTauntDialogAPI>();
		this.injectionBinder.Bind<IOnlineBlindPickScreenAPI>().To<OnlineBlindPickScreenAPI>().ToSingleton();
		this.injectionBinder.Bind<ILoginScreenAPI>().To<LoginScreenAPI>().ToSingleton();
		this.injectionBinder.Bind<INewAccountWindowAPI>().To<NewAccountWindowAPI>().ToSingleton();
		this.injectionBinder.Bind<ICustomLobbyScreenAPI>().To<CustomLobbyScreenAPI>().ToSingleton();
		this.injectionBinder.Bind<ISettingsTabsModel>().To<SettingsTabsModel>().ToSingleton();
		this.injectionBinder.Bind<ISettingsScreenAPI>().To<SettingsScreenAPI>().ToSingleton();
		this.injectionBinder.Bind<IInputSettingsScreenAPI>().To<InputSettingsScreenAPI>().ToSingleton();
		this.injectionBinder.Bind<IAudioTabAPI>().To<AudioTabAPI>().ToSingleton();
		this.injectionBinder.Bind<IVideoTabAPI>().To<VideoTabAPI>().ToSingleton();
		this.injectionBinder.Bind<IGameplayTabAPI>().To<GameplayTabAPI>().ToSingleton();
		this.injectionBinder.Bind<IVictoryScreenAPI>().To<VictoryScreenAPI>().ToSingleton();
		this.injectionBinder.Bind<DirectionalBindingHelper>().To<DirectionalBindingHelper>().ToSingleton();
		this.injectionBinder.Bind<ICursorManager>().To<CursorManager>().ToSingleton();
		this.injectionBinder.Bind<IPlayerJoinGameController>().To<PlayerJoinGameController>().ToSingleton();
		this.injectionBinder.Bind<IScreenPermissions>().To<ScreenPermissions>().ToSingleton();
		this.injectionBinder.Bind<IUISceneCharacterManager>().To<UISceneCharacterManager>().ToSingleton();
		this.injectionBinder.Bind<IUICSSSceneCharacterManager>().To<UICSSSceneCharacterManager>().ToSingleton();
		this.injectionBinder.Bind<UISceneLoader>().To<UISceneLoader>();
		this.uiScreenBind<CharacterSelectScreenController>(ScreenType.CharacterSelect);
		this.uiScreenBind<StageSelectScreenController>(ScreenType.SelectStage);
		this.uiScreenBind<StoreScreenController>(ScreenType.StoreScreen);
		this.uiScreenBind<SettingsScreenController>(ScreenType.SettingsScreen);
		this.uiScreenBind<CustomLobbyScreenController>(ScreenType.CustomLobbyScreen);
		this.uiScreenBind<CreditsScreenController>(ScreenType.CreditsScreen);
		this.uiLoadBinders();
	}

	// Token: 0x06003740 RID: 14144 RVA: 0x001019D0 File Offset: 0x000FFDD0
	private void uiScreenBind<T>(object name) where T : UIScreenController
	{
		this.injectionBinder.Bind<UIScreenController>().To<T>().ToName(name);
	}

	// Token: 0x06003741 RID: 14145 RVA: 0x001019EC File Offset: 0x000FFDEC
	private void uiLoadBinders()
	{
		UILoadBindings uiloadBindings = new UILoadBindings();
		this.injectionBinder.Bind<IUILoaderBindings>().To<UILoadBindings>().ToValue(uiloadBindings);
		uiloadBindings.AddBinding<StoreScreen, IStoreAPI>(DataRequirementLevel.REQUIRED);
		uiloadBindings.AddBinding<CharacterSelectScreen, CharacterSelectModel>(DataRequirementLevel.PREFERRED);
		uiloadBindings.AddBinding<CharacterSelectScreen, OptionsProfileAPI>(DataRequirementLevel.PREFERRED);
		uiloadBindings.AddBinding<MainMenuScreen>(SoundBundleKey.mainMenu, true);
		uiloadBindings.AddBinding<LoginScreen>(SoundBundleKey.login, true);
		uiloadBindings.AddBinding<InputSettingsScreen>(SoundBundleKey.settings, true);
		uiloadBindings.AddBinding<CharacterSelectScreen>(SoundBundleKey.characterSelect, true);
		uiloadBindings.AddBinding<OnlineBlindPickScreen>(SoundBundleKey.characterSelect, true);
		uiloadBindings.AddBinding<StageSelectScreen>(SoundBundleKey.stageSelect, true);
		uiloadBindings.AddBinding<LoadBattleScreen>(SoundBundleKey.loadingBattle, true);
		uiloadBindings.AddBinding<VictoryScreen>(SoundBundleKey.victoryScreen, true);
		uiloadBindings.AddBinding<CustomLobbyScreen>(SoundBundleKey.customLobby, true);
		uiloadBindings.AddBinding<StoreScreen>(SoundBundleKey.store, true);
		uiloadBindings.AddBinding<StoreScreen>(SoundBundleKey.unboxing, true);
	}
}
