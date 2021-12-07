// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IGamewideOverlayController
{
	void Init(GameClient client);

	T ShowOverlay<T>(WindowTransition transition = WindowTransition.STANDARD_FADE) where T : BaseGamewideOverlay;
}
