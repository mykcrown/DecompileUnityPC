using System;
using UnityEngine;

// Token: 0x020008F0 RID: 2288
public class OptionsProfileAPI : IDataDependency
{
	// Token: 0x17000E17 RID: 3607
	// (get) Token: 0x06003AAE RID: 15022 RVA: 0x001129A0 File Offset: 0x00110DA0
	// (set) Token: 0x06003AAF RID: 15023 RVA: 0x001129A8 File Offset: 0x00110DA8
	[Inject]
	public IOptionsProfileManager optionsProfileManager { get; set; }

	// Token: 0x06003AB0 RID: 15024 RVA: 0x001129B4 File Offset: 0x00110DB4
	public void Load(Action<DataLoadResult> callback)
	{
		this.optionsProfileManager.LoadAll(delegate(LoadOptionsProfileListResult result)
		{
			DataLoadResult dataLoadResult = new DataLoadResult();
			dataLoadResult.status = ((!result.success) ? DataLoadStatus.FAILURE : DataLoadStatus.SUCCESS);
			callback(dataLoadResult);
		});
	}

	// Token: 0x06003AB1 RID: 15025 RVA: 0x001129E5 File Offset: 0x00110DE5
	public void DeleteDefaultSettings(Action<SaveOptionsProfileResult> callback)
	{
		this.optionsProfileManager.DeleteDefaultState(callback);
	}

	// Token: 0x06003AB2 RID: 15026 RVA: 0x001129F3 File Offset: 0x00110DF3
	public void SetDefaultGameMode(GameMode mode)
	{
		this.optionsProfileManager.SetDefaultGameMode(mode);
	}

	// Token: 0x06003AB3 RID: 15027 RVA: 0x00112A01 File Offset: 0x00110E01
	public BattleSettings GetInitialSettings()
	{
		if (this.optionsProfileManager.CurrentProfile != null)
		{
			return this.optionsProfileManager.CurrentProfile.settings.Clone();
		}
		return null;
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x00112A2A File Offset: 0x00110E2A
	public void UpdatePayload(GameLoadPayload payload)
	{
		this.currentPayload = payload;
	}

	// Token: 0x06003AB5 RID: 15029 RVA: 0x00112A33 File Offset: 0x00110E33
	public void SaveAndSwitchToDefault(Action<SaveOptionsProfileResult> callback = null)
	{
		if (this.currentPayload == null)
		{
			Debug.LogError("We should have a payload");
		}
		else
		{
			this.optionsProfileManager.SaveAndSwitchToDefault(this.currentPayload.battleConfig, callback);
		}
	}

	// Token: 0x06003AB6 RID: 15030 RVA: 0x00112A66 File Offset: 0x00110E66
	public void SaveCurrent(Action<SaveOptionsProfileResult> callback = null)
	{
		if (this.currentPayload == null)
		{
			Debug.LogError("We should have a payload");
		}
		else
		{
			this.optionsProfileManager.SaveToCurrent(this.currentPayload.battleConfig, callback);
		}
	}

	// Token: 0x06003AB7 RID: 15031 RVA: 0x00112A99 File Offset: 0x00110E99
	public OptionsProfile[] GetAll()
	{
		return this.optionsProfileManager.GetAll();
	}

	// Token: 0x06003AB8 RID: 15032 RVA: 0x00112AA6 File Offset: 0x00110EA6
	public OptionsProfileSet GetStateClone()
	{
		return this.optionsProfileManager.GetStateCopy();
	}

	// Token: 0x06003AB9 RID: 15033 RVA: 0x00112AB3 File Offset: 0x00110EB3
	public void LoadStateClone(OptionsProfileSet copy)
	{
		this.optionsProfileManager.RevertState(copy);
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x00112AC1 File Offset: 0x00110EC1
	public bool IsCurrentlySelected(string id)
	{
		return this.optionsProfileManager.CurrentProfile != null && this.optionsProfileManager.CurrentProfile.id == id;
	}

	// Token: 0x04002872 RID: 10354
	private GameLoadPayload currentPayload;
}
