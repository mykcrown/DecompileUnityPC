using System;

// Token: 0x020004EC RID: 1260
public class VoiceTauntMoveComponent : MoveComponent, IMoveTauntComponent, IMoveTickMoveFrameComponent
{
	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x06001B89 RID: 7049 RVA: 0x0008BA47 File Offset: 0x00089E47
	// (set) Token: 0x06001B8A RID: 7050 RVA: 0x0008BA4F File Offset: 0x00089E4F
	[Inject]
	public IPlayerTauntsFinder tauntFinder { get; set; }

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x06001B8B RID: 7051 RVA: 0x0008BA58 File Offset: 0x00089E58
	// (set) Token: 0x06001B8C RID: 7052 RVA: 0x0008BA60 File Offset: 0x00089E60
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x06001B8D RID: 7053 RVA: 0x0008BA69 File Offset: 0x00089E69
	// (set) Token: 0x06001B8E RID: 7054 RVA: 0x0008BA71 File Offset: 0x00089E71
	[Inject]
	public IItemLoader itemLoader { get; set; }

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06001B8F RID: 7055 RVA: 0x0008BA7A File Offset: 0x00089E7A
	// (set) Token: 0x06001B90 RID: 7056 RVA: 0x0008BA82 File Offset: 0x00089E82
	public TauntSlot TauntSlot { get; set; }

	// Token: 0x06001B91 RID: 7057 RVA: 0x0008BA8C File Offset: 0x00089E8C
	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
		UserTaunts forPlayer = this.tauntFinder.GetForPlayer(playerDelegate.PlayerNum);
		SerializableDictionary<TauntSlot, EquipmentID> slotsForCharacter = forPlayer.GetSlotsForCharacter(playerDelegate.CharacterData.characterID);
		EquipmentID id;
		if (slotsForCharacter.TryGetValue(this.TauntSlot, out id) && !id.IsNull())
		{
			EquippableItem item = this.equipmentModel.GetItem(id);
			if (item.type == EquipmentTypes.VOICE_TAUNT)
			{
				this.voiceTauntData = this.itemLoader.LoadAsset<VoiceTauntData>(item);
			}
		}
	}

	// Token: 0x06001B92 RID: 7058 RVA: 0x0008BB10 File Offset: 0x00089F10
	public void TickMoveFrame(InputButtonsData input)
	{
		if (this.voiceTauntData != null && this.moveDelegate.Model.internalFrame == this.tauntFrame)
		{
			this.playerDelegate.PlayVoiceTaunt(this.voiceTauntData);
		}
	}

	// Token: 0x040014CC RID: 5324
	public int tauntFrame;

	// Token: 0x040014CD RID: 5325
	private VoiceTauntData voiceTauntData;
}
