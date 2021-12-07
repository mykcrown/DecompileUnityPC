using System;
using System.Collections.Generic;

// Token: 0x020009CB RID: 2507
public class ScreenTransitionMap : IScreenTransitionMap
{
	// Token: 0x0600463D RID: 17981 RVA: 0x0013294C File Offset: 0x00130D4C
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

	// Token: 0x0600463E RID: 17982 RVA: 0x001329DC File Offset: 0x00130DDC
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

	// Token: 0x0600463F RID: 17983 RVA: 0x00132A34 File Offset: 0x00130E34
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

	// Token: 0x04002E6E RID: 11886
	private Dictionary<Type, Dictionary<Type, ScreenTransition>> map;
}
