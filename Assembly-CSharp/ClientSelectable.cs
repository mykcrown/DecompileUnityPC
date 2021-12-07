using System;
using UnityEngine.UI;

// Token: 0x02000905 RID: 2309
public class ClientSelectable : Selectable
{
	// Token: 0x17000E5B RID: 3675
	// (get) Token: 0x06003C0C RID: 15372 RVA: 0x00116904 File Offset: 0x00114D04
	// (set) Token: 0x06003C0D RID: 15373 RVA: 0x0011690C File Offset: 0x00114D0C
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000E5C RID: 3676
	// (get) Token: 0x06003C0E RID: 15374 RVA: 0x00116915 File Offset: 0x00114D15
	// (set) Token: 0x06003C0F RID: 15375 RVA: 0x0011691D File Offset: 0x00114D1D
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000E5D RID: 3677
	// (get) Token: 0x06003C10 RID: 15376 RVA: 0x00116926 File Offset: 0x00114D26
	// (set) Token: 0x06003C11 RID: 15377 RVA: 0x0011692E File Offset: 0x00114D2E
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000E5E RID: 3678
	// (get) Token: 0x06003C12 RID: 15378 RVA: 0x00116937 File Offset: 0x00114D37
	// (set) Token: 0x06003C13 RID: 15379 RVA: 0x0011693F File Offset: 0x00114D3F
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x06003C14 RID: 15380 RVA: 0x00116948 File Offset: 0x00114D48
	protected override void Awake()
	{
		base.Awake();
		this.inject();
	}

	// Token: 0x06003C15 RID: 15381 RVA: 0x00116956 File Offset: 0x00114D56
	private void inject()
	{
		if (this.injector == null)
		{
			StaticInject.Inject(this);
		}
	}
}
