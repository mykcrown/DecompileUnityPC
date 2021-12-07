// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GamewideOverlayController : IGamewideOverlayController
{
	private Dictionary<Type, GameObject> prefabMap = new Dictionary<Type, GameObject>();

	[Inject]
	public IWindowDisplay windowDisplay
	{
		get;
		set;
	}

	[Inject]
	public IGamewideOverlayDisplay overlayDisplay
	{
		get;
		set;
	}

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

	private void setUpDeveloperFutureOverlay()
	{
		GameObject prefab = Resources.Load("Future/Watermark") as GameObject;
		this.mapClass<FutureWatermarkDisplay>(prefab);
	}

	private void mapClass<T>(GameObject prefab) where T : BaseGamewideOverlay
	{
		this.prefabMap[typeof(T)] = prefab;
	}

	private GameObject getPrefab<T>() where T : BaseGamewideOverlay
	{
		return this.prefabMap[typeof(T)];
	}

	public T ShowOverlay<T>(WindowTransition transition = WindowTransition.STANDARD_FADE) where T : BaseGamewideOverlay
	{
		return this.overlayDisplay.AddGamewideOverlay<T>(this.getPrefab<T>(), transition);
	}
}
