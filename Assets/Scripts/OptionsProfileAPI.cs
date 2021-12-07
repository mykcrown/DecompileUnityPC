// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OptionsProfileAPI : IDataDependency
{
	private sealed class _Load_c__AnonStorey0
	{
		internal Action<DataLoadResult> callback;

		internal void __m__0(LoadOptionsProfileListResult result)
		{
			DataLoadResult dataLoadResult = new DataLoadResult();
			dataLoadResult.status = ((!result.success) ? DataLoadStatus.FAILURE : DataLoadStatus.SUCCESS);
			this.callback(dataLoadResult);
		}
	}

	private GameLoadPayload currentPayload;

	[Inject]
	public IOptionsProfileManager optionsProfileManager
	{
		get;
		set;
	}

	public void Load(Action<DataLoadResult> callback)
	{
		OptionsProfileAPI._Load_c__AnonStorey0 _Load_c__AnonStorey = new OptionsProfileAPI._Load_c__AnonStorey0();
		_Load_c__AnonStorey.callback = callback;
		this.optionsProfileManager.LoadAll(new Action<LoadOptionsProfileListResult>(_Load_c__AnonStorey.__m__0));
	}

	public void DeleteDefaultSettings(Action<SaveOptionsProfileResult> callback)
	{
		this.optionsProfileManager.DeleteDefaultState(callback);
	}

	public void SetDefaultGameMode(GameMode mode)
	{
		this.optionsProfileManager.SetDefaultGameMode(mode);
	}

	public BattleSettings GetInitialSettings()
	{
		if (this.optionsProfileManager.CurrentProfile != null)
		{
			return this.optionsProfileManager.CurrentProfile.settings.Clone();
		}
		return null;
	}

	public void UpdatePayload(GameLoadPayload payload)
	{
		this.currentPayload = payload;
	}

	public void SaveAndSwitchToDefault(Action<SaveOptionsProfileResult> callback = null)
	{
		if (this.currentPayload == null)
		{
			UnityEngine.Debug.LogError("We should have a payload");
		}
		else
		{
			this.optionsProfileManager.SaveAndSwitchToDefault(this.currentPayload.battleConfig, callback);
		}
	}

	public void SaveCurrent(Action<SaveOptionsProfileResult> callback = null)
	{
		if (this.currentPayload == null)
		{
			UnityEngine.Debug.LogError("We should have a payload");
		}
		else
		{
			this.optionsProfileManager.SaveToCurrent(this.currentPayload.battleConfig, callback);
		}
	}

	public OptionsProfile[] GetAll()
	{
		return this.optionsProfileManager.GetAll();
	}

	public OptionsProfileSet GetStateClone()
	{
		return this.optionsProfileManager.GetStateCopy();
	}

	public void LoadStateClone(OptionsProfileSet copy)
	{
		this.optionsProfileManager.RevertState(copy);
	}

	public bool IsCurrentlySelected(string id)
	{
		return this.optionsProfileManager.CurrentProfile != null && this.optionsProfileManager.CurrentProfile.id == id;
	}
}
