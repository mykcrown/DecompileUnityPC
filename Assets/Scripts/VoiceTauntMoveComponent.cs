// Decompile from assembly: Assembly-CSharp.dll

using System;

public class VoiceTauntMoveComponent : MoveComponent, IMoveTauntComponent, IMoveTickMoveFrameComponent
{
	public int tauntFrame;

	private VoiceTauntData voiceTauntData;

	[Inject]
	public IPlayerTauntsFinder tauntFinder
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public IItemLoader itemLoader
	{
		get;
		set;
	}

	public TauntSlot TauntSlot
	{
		get;
		set;
	}

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

	public void TickMoveFrame(InputButtonsData input)
	{
		if (this.voiceTauntData != null && this.moveDelegate.Model.internalFrame == this.tauntFrame)
		{
			this.playerDelegate.PlayVoiceTaunt(this.voiceTauntData);
		}
	}
}
