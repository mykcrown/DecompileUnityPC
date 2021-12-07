using System;
using UnityEngine;

// Token: 0x02000A66 RID: 2662
public class UIScene : MonoBehaviour
{
	// Token: 0x17001247 RID: 4679
	// (get) Token: 0x06004D3A RID: 19770 RVA: 0x00114D6A File Offset: 0x0011316A
	// (set) Token: 0x06004D3B RID: 19771 RVA: 0x00114D72 File Offset: 0x00113172
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17001248 RID: 4680
	// (get) Token: 0x06004D3C RID: 19772 RVA: 0x00114D7B File Offset: 0x0011317B
	// (set) Token: 0x06004D3D RID: 19773 RVA: 0x00114D83 File Offset: 0x00113183
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17001249 RID: 4681
	// (get) Token: 0x06004D3E RID: 19774 RVA: 0x00114D8C File Offset: 0x0011318C
	// (set) Token: 0x06004D3F RID: 19775 RVA: 0x00114D94 File Offset: 0x00113194
	[Inject]
	public IVideoSettingsUtility videoSettingsUtility { get; set; }

	// Token: 0x06004D40 RID: 19776 RVA: 0x00114D9D File Offset: 0x0011319D
	protected virtual void Awake()
	{
		this.myCamera = base.GetComponentInChildren<Camera>();
	}

	// Token: 0x06004D41 RID: 19777 RVA: 0x00114DAB File Offset: 0x001131AB
	private void OnEnable()
	{
		if (this.videoSettingsUtility != null)
		{
			this.videoSettingsUtility.ApplyToCamera(this.myCamera);
		}
		this.onEnable();
	}

	// Token: 0x06004D42 RID: 19778 RVA: 0x00114DCF File Offset: 0x001131CF
	protected virtual void onEnable()
	{
	}

	// Token: 0x06004D43 RID: 19779 RVA: 0x00114DD1 File Offset: 0x001131D1
	private void OnDisable()
	{
		this.onDisable();
	}

	// Token: 0x06004D44 RID: 19780 RVA: 0x00114DD9 File Offset: 0x001131D9
	protected virtual void onDisable()
	{
	}

	// Token: 0x040032B0 RID: 12976
	public GameObject VisualContainer;

	// Token: 0x040032B1 RID: 12977
	protected Camera myCamera;
}
