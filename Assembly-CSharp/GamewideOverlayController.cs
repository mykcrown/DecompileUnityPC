using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000939 RID: 2361
public class GamewideOverlayController : IGamewideOverlayController
{
	// Token: 0x17000ED4 RID: 3796
	// (get) Token: 0x06003E5B RID: 15963 RVA: 0x0011C9FD File Offset: 0x0011ADFD
	// (set) Token: 0x06003E5C RID: 15964 RVA: 0x0011CA05 File Offset: 0x0011AE05
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x17000ED5 RID: 3797
	// (get) Token: 0x06003E5D RID: 15965 RVA: 0x0011CA0E File Offset: 0x0011AE0E
	// (set) Token: 0x06003E5E RID: 15966 RVA: 0x0011CA16 File Offset: 0x0011AE16
	[Inject]
	public IGamewideOverlayDisplay overlayDisplay { get; set; }

	// Token: 0x06003E5F RID: 15967 RVA: 0x0011CA20 File Offset: 0x0011AE20
	public void Init(GameClient client)
	{
		Canvas componentInChildren = client.gameObject.GetComponentInChildren<Canvas>();
		CanvasContainer component = componentInChildren.gameObject.GetComponent<CanvasContainer>();
		this.mapClass<MatchmakingQueueOverlay>(component.MatchmakingOverlayPrefab);
		this.mapClass<UserFacingPerformanceDisplay>(component.PerformanceOverlayPrefab);
		this.mapClass<BetaWatermarkDisplay>(component.BetaWatermarkOverlayPrefab);
		this.mapClass<AlphaWatermarkDisplay>(component.AlphaWatermarkOverlayPrefab);
		this.mapClass<PlayerCard>(component.PlayerCardOverlayPrefab);
		this.mapClass<UserFacingSystemClock>(component.SystemClockPrefab);
		this.setUpDeveloperFutureOverlay();
	}

	// Token: 0x06003E60 RID: 15968 RVA: 0x0011CA94 File Offset: 0x0011AE94
	private void setUpDeveloperFutureOverlay()
	{
		GameObject prefab = Resources.Load("Future/Watermark") as GameObject;
		this.mapClass<FutureWatermarkDisplay>(prefab);
	}

	// Token: 0x06003E61 RID: 15969 RVA: 0x0011CAB8 File Offset: 0x0011AEB8
	private void mapClass<T>(GameObject prefab) where T : BaseGamewideOverlay
	{
		this.prefabMap[typeof(T)] = prefab;
	}

	// Token: 0x06003E62 RID: 15970 RVA: 0x0011CAD0 File Offset: 0x0011AED0
	private GameObject getPrefab<T>() where T : BaseGamewideOverlay
	{
		return this.prefabMap[typeof(T)];
	}

	// Token: 0x06003E63 RID: 15971 RVA: 0x0011CAE7 File Offset: 0x0011AEE7
	public T ShowOverlay<T>(WindowTransition transition = WindowTransition.STANDARD_FADE) where T : BaseGamewideOverlay
	{
		return this.overlayDisplay.AddGamewideOverlay<T>(this.getPrefab<T>(), transition);
	}

	// Token: 0x04002A61 RID: 10849
	private Dictionary<Type, GameObject> prefabMap = new Dictionary<Type, GameObject>();
}
