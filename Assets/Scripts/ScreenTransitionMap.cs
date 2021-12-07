// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class ScreenTransitionMap : IScreenTransitionMap
{
	private Dictionary<Type, Dictionary<Type, ScreenTransition>> map;

	[PostConstruct]
	public void Init()
	{
		this.map = new Dictionary<Type, Dictionary<Type, ScreenTransition>>();
		this.transitionBind<MainMenuScreen, CharacterSelectScreen>(new ScreenTransition(ScreenTransitionType.CROSSFADE));
		this.transitionBind<CharacterSelectScreen, MainMenuScreen>(new ScreenTransition(ScreenTransitionType.CROSSFADE));
		this.transitionBind<BattleUI, VictoryScreen>(new ScreenTransition(ScreenTransitionType.CROSSFADE));
		this.transitionBind<BattleUI, StageSelectScreen>(new ScreenTransition(ScreenTransitionType.CROSSFADE));
		this.transitionBind<BattleUI, CharacterSelectScreen>(new ScreenTransition(ScreenTransitionType.CROSSFADE));
		this.transitionBind<MainMenuScreen, StoreScreen>(new ScreenTransition(ScreenTransitionType.CROSSFADE));
		this.transitionBind<StoreScreen, MainMenuScreen>(new ScreenTransition(ScreenTransitionType.CROSSFADE));
		this.transitionBind<LoginScreen, MainMenuScreen>(new ScreenTransition(ScreenTransitionType.CROSSFADE));
		this.transitionBind<MainMenuScreen, InputSettingsScreen>(new ScreenTransition(ScreenTransitionType.CROSSFADE));
		this.transitionBind<InputSettingsScreen, MainMenuScreen>(new ScreenTransition(ScreenTransitionType.CROSSFADE));
	}

	private void transitionBind<T, U>(ScreenTransition transition)
	{
		Type typeFromHandle = typeof(T);
		Type typeFromHandle2 = typeof(U);
		if (!this.map.ContainsKey(typeFromHandle))
		{
			this.map[typeFromHandle] = new Dictionary<Type, ScreenTransition>();
		}
		this.map[typeFromHandle][typeFromHandle2] = transition;
	}

	public ScreenTransition Get(GameScreen fromScreen, GameScreen toScreen)
	{
		if (fromScreen == null || toScreen == null)
		{
			return null;
		}
		Type type = fromScreen.GetType();
		Type type2 = toScreen.GetType();
		if (!this.map.ContainsKey(type))
		{
			return null;
		}
		if (!this.map[type].ContainsKey(type2))
		{
			return null;
		}
		return this.map[type][type2];
	}
}
