// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using network;
using P2P;
using System;

public class ContextConfigOnline : ContextConfig
{
	public ContextConfigOnline(MasterContext.InitContext initContext) : base(initContext)
	{
		bool flag = initContext.offlineModeDetector.IsOfflineMode();
		this.injectionBinder.Bind<P2PServerMgr>().To<P2PServerMgr>().ToSingleton();
		this.injectionBinder.Bind<P2PHost>().To<P2PHost>().ToSingleton();
		this.injectionBinder.Bind<P2PClient>().To<P2PClient>().ToSingleton();
		this.injectionBinder.Bind<IIconsServerAPI>().To<IconsServerManager>().ToSingleton();
		this.injectionBinder.Bind<IPingManager>().To<PingManager>().ToSingleton();
		this.injectionBinder.Bind<ITimeSynchronizer>().To<TimeSynchronizer>().ToSingleton();
		this.injectionBinder.Bind<IUDPWarming>().To<UDPWarming>().ToSingleton();
		if (!flag)
		{
			this.injectionBinder.Bind<IUserCurrencyReceiver>().To<ServerCurrencyReceiver>().ToSingleton();
			this.injectionBinder.Bind<IUnlockCharacter>().To<ServerUnlockCharacter>().ToSingleton();
			this.injectionBinder.Bind<IUnlockProAccount>().To<ServerUnlockProAccount>().ToSingleton();
		}
		else
		{
			this.injectionBinder.Bind<IUserCurrencyReceiver>().To<MockCurrencyReceiver>().ToSingleton();
			this.injectionBinder.Bind<IUserCharacterUnlockSource>().To<MockUserCharacterUnlockSource>().ToSingleton();
			this.injectionBinder.Bind<IUserProAccountSource>().To<MockProAccountSource>().ToSingleton();
			this.injectionBinder.Bind<IInventorySource>().To<MockInventorySource>().ToSingleton();
			this.injectionBinder.Bind<IUserLootboxesSource>().To<MockUserLootboxSource>().ToSingleton();
			this.injectionBinder.Bind<IEquipmentSource>().To<MockEquipmentSource>().ToSingleton();
			this.injectionBinder.Bind<ILootBoxesSource>().To<MockLootBoxesSource>().ToSingleton();
			this.injectionBinder.Bind<IUserPurchaseEquipment>().To<MockPurchaseEquipment>().ToSingleton();
			this.injectionBinder.Bind<IUnlockCharacter>().To<MockUnlockCharacter>().ToSingleton();
			this.injectionBinder.Bind<IUnlockProAccount>().To<MockUnlockProAccount>().ToSingleton();
		}
		this.injectionBinder.Bind<IGetEquippedItemsFromServer>().To<GetEquippedItemsFromServer>().ToSingleton();
		this.injectionBinder.Bind<IBuyLootBox>().To<ServerBuyLootBox>().ToSingleton();
		this.injectionBinder.Bind<IUserTauntsSource>().To<MockUserTauntsSource>().ToSingleton();
		this.injectionBinder.Bind<PackageApplier>().To<PackageApplier>().ToSingleton();
		this.injectionBinder.Bind<IServerDataConverter>().To<ServerDataConverter>().ToSingleton();
		this.injectionBinder.Bind<IServerConnectionManager>().To<ServerConnectionManager>().ToSingleton();
		this.injectionBinder.Bind<ConnectionController>().To<ConnectionController>().ToSingleton();
		this.injectionBinder.Bind<SteamManager>().ToSingleton();
		this.injectionBinder.Bind<DiscordManager>().ToSingleton();
		this.injectionBinder.Bind<EnterCustomLobbyController>().ToSingleton();
		this.injectionBinder.Bind<LobbyNameValidator>().To<LobbyNameValidator>();
		this.injectionBinder.Bind<LobbyPasswordValidator>().To<LobbyPasswordValidator>();
		this.injectionBinder.Bind<IUserEnteredNameValidator>().To<UserEnteredNameValidator>().ToSingleton();
		this.injectionBinder.Bind<ILoginValidator>().To<LoginValidator>().ToSingleton();
		this.injectionBinder.Bind<ISendFeedback>().To<SendFeedback>().ToSingleton();
		this.injectionBinder.Bind<IBattleServerStagingAPI>().To<BattleServerStaging>().ToSingleton();
		this.injectionBinder.Bind<IAccountAPI>().To<AccountAPI>().ToSingleton();
		this.injectionBinder.Bind<ICustomLobbyController>().To<CustomLobbyController>().ToSingleton();
		this.injectionBinder.Bind<IBattleServerAPI>().To<BattleServerController>().ToSingleton();
		this.injectionBinder.Bind<IConnectionStatusHandler>().To<UIConnectionStatusHandler>().ToSingleton();
		this.injectionBinder.Bind<IOnlineSubsystem>().Bind<IRichPresence>().To<OnlineSubsystem>().ToSingleton();
		this.injectionBinder.Bind<NetGraphVisualizer>().To<NetGraphVisualizer>().ToSingleton();
		this.injectionBinder.Bind<INetworkHealthReport>().To<NetworkHealthReport>().ToSingleton();
		this.injectionBinder.Bind<IRollbackQuitGame>().To<RollbackQuitGame>();
	}
}
