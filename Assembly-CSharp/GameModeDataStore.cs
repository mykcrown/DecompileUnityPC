using System;
using System.Collections.Generic;

// Token: 0x02000419 RID: 1049
public class GameModeDataStore : GameDataStore<GameModeData>
{
	// Token: 0x060015E7 RID: 5607 RVA: 0x00077AA4 File Offset: 0x00075EA4
	public GameModeDataStore(ILocalization localization) : base(localization)
	{
	}

	// Token: 0x060015E8 RID: 5608 RVA: 0x00077AB8 File Offset: 0x00075EB8
	protected override void addDataElement(GameModeData dataElement)
	{
		base.addDataElement(dataElement);
		if (dataElement == null)
		{
			return;
		}
		this.mapByType.Add(dataElement.Type, dataElement);
	}

	// Token: 0x060015E9 RID: 5609 RVA: 0x00077AE0 File Offset: 0x00075EE0
	public GameModeData GetDataByType(GameMode type)
	{
		if (this.mapByType.ContainsKey(type))
		{
			return this.mapByType[type];
		}
		return null;
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x00077B04 File Offset: 0x00075F04
	public void GetAllTypes(List<GameMode> types)
	{
		types.Clear();
		foreach (GameMode item in this.mapByType.Keys)
		{
			types.Add(item);
		}
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x00077B6C File Offset: 0x00075F6C
	public List<GameModeData> GetDataByTypes(List<GameMode> types)
	{
		List<GameModeData> list = new List<GameModeData>();
		this.GetDataByTypes(types, list);
		return list;
	}

	// Token: 0x060015EC RID: 5612 RVA: 0x00077B88 File Offset: 0x00075F88
	public void GetDataByTypes(List<GameMode> types, List<GameModeData> elements)
	{
		elements.Clear();
		for (int i = 0; i < types.Count; i++)
		{
			elements.Add(this.GetDataByType(types[i]));
		}
	}

	// Token: 0x040010B5 RID: 4277
	private Dictionary<GameMode, GameModeData> mapByType = new Dictionary<GameMode, GameModeData>();
}
