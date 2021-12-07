using System;

// Token: 0x0200093A RID: 2362
public interface IGamewideOverlayController
{
	// Token: 0x06003E64 RID: 15972
	void Init(GameClient client);

	// Token: 0x06003E65 RID: 15973
	T ShowOverlay<T>(WindowTransition transition = WindowTransition.STANDARD_FADE) where T : BaseGamewideOverlay;
}
