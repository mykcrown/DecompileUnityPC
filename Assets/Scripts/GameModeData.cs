// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class GameModeData : ScriptableObject, IGameDataElement
{
	public string ModeTitle;

	public GameMode Type;

	public LocalizationData localization;

	public bool enabled = true;

	public bool demoEnabled = true;

	public bool selectableInFreePlay = true;

	public bool debugOnly;

	public GameModeSettings settings;

	public CustomGameModeData customData;

	LocalizationData IGameDataElement.Localization
	{
		get
		{
			return this.localization;
		}
	}

	bool IGameDataElement.Enabled
	{
		get
		{
			return this.enabled;
		}
	}

	public string Key
	{
		get
		{
			return this.ModeTitle;
		}
	}

	public int ID
	{
		get
		{
			return (int)this.Type;
		}
	}

	public virtual void RegisterPreload(PreloadContext context)
	{
		if (this.customData != null)
		{
			this.customData.RegisterPreload(context);
		}
	}
}
