// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class GameModeDataStore : GameDataStore<GameModeData>
{
	private Dictionary<GameMode, GameModeData> mapByType = new Dictionary<GameMode, GameModeData>();

	public GameModeDataStore(ILocalization localization) : base(localization)
	{
	}

	protected override void addDataElement(GameModeData dataElement)
	{
		base.addDataElement(dataElement);
		if (dataElement == null)
		{
			return;
		}
		this.mapByType.Add(dataElement.Type, dataElement);
	}

	public GameModeData GetDataByType(GameMode type)
	{
		if (this.mapByType.ContainsKey(type))
		{
			return this.mapByType[type];
		}
		return null;
	}

	public void GetAllTypes(List<GameMode> types)
	{
		types.Clear();
		foreach (GameMode current in this.mapByType.Keys)
		{
			types.Add(current);
		}
	}

	public List<GameModeData> GetDataByTypes(List<GameMode> types)
	{
		List<GameModeData> list = new List<GameModeData>();
		this.GetDataByTypes(types, list);
		return list;
	}

	public void GetDataByTypes(List<GameMode> types, List<GameModeData> elements)
	{
		elements.Clear();
		for (int i = 0; i < types.Count; i++)
		{
			elements.Add(this.GetDataByType(types[i]));
		}
	}
}
