using System;
using IconsServer;

// Token: 0x02000739 RID: 1849
public class GetEquippedItemsFromServer : IGetEquippedItemsFromServer
{
	// Token: 0x17000B29 RID: 2857
	// (get) Token: 0x06002DAE RID: 11694 RVA: 0x000E970D File Offset: 0x000E7B0D
	// (set) Token: 0x06002DAF RID: 11695 RVA: 0x000E9715 File Offset: 0x000E7B15
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000B2A RID: 2858
	// (get) Token: 0x06002DB0 RID: 11696 RVA: 0x000E971E File Offset: 0x000E7B1E
	// (set) Token: 0x06002DB1 RID: 11697 RVA: 0x000E9726 File Offset: 0x000E7B26
	[Inject]
	public IIconsServerAPI iconsServerAPI { get; set; }

	// Token: 0x17000B2B RID: 2859
	// (get) Token: 0x06002DB2 RID: 11698 RVA: 0x000E972F File Offset: 0x000E7B2F
	// (set) Token: 0x06002DB3 RID: 11699 RVA: 0x000E9737 File Offset: 0x000E7B37
	[Inject]
	public IUserTauntsModel userTauntsModel { get; set; }

	// Token: 0x17000B2C RID: 2860
	// (get) Token: 0x06002DB4 RID: 11700 RVA: 0x000E9740 File Offset: 0x000E7B40
	// (set) Token: 0x06002DB5 RID: 11701 RVA: 0x000E9748 File Offset: 0x000E7B48
	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel { get; set; }

	// Token: 0x17000B2D RID: 2861
	// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x000E9751 File Offset: 0x000E7B51
	// (set) Token: 0x06002DB7 RID: 11703 RVA: 0x000E9759 File Offset: 0x000E7B59
	[Inject]
	public IUserGlobalEquippedModel userGlobalEquippedModel { get; set; }

	// Token: 0x17000B2E RID: 2862
	// (get) Token: 0x06002DB8 RID: 11704 RVA: 0x000E9762 File Offset: 0x000E7B62
	// (set) Token: 0x06002DB9 RID: 11705 RVA: 0x000E976A File Offset: 0x000E7B6A
	[Inject]
	public IStaticDataSource staticDataSource { get; set; }

	// Token: 0x17000B2F RID: 2863
	// (get) Token: 0x06002DBA RID: 11706 RVA: 0x000E9773 File Offset: 0x000E7B73
	// (set) Token: 0x06002DBB RID: 11707 RVA: 0x000E977B File Offset: 0x000E7B7B
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x06002DBC RID: 11708 RVA: 0x000E9784 File Offset: 0x000E7B84
	// (set) Token: 0x06002DBD RID: 11709 RVA: 0x000E978C File Offset: 0x000E7B8C
	[Inject]
	public IEquipMethodMap equipMethodMap { get; set; }

	// Token: 0x17000B31 RID: 2865
	// (get) Token: 0x06002DBE RID: 11710 RVA: 0x000E9795 File Offset: 0x000E7B95
	public bool IsComplete
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002DBF RID: 11711 RVA: 0x000E9798 File Offset: 0x000E7B98
	[PostConstruct]
	public void Init()
	{
	}

	// Token: 0x06002DC0 RID: 11712 RVA: 0x000E979A File Offset: 0x000E7B9A
	public void MakeRequest()
	{
	}

	// Token: 0x04002059 RID: 8281
	public static string UPDATED = "GetEquippedItemsFromServer.UPDATE";
}
