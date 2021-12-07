using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A6E RID: 2670
public class UISceneCharacterManager : IUISceneCharacterManager
{
	// Token: 0x1700125F RID: 4703
	// (get) Token: 0x06004DB4 RID: 19892 RVA: 0x00147AEE File Offset: 0x00145EEE
	// (set) Token: 0x06004DB5 RID: 19893 RVA: 0x00147AF6 File Offset: 0x00145EF6
	[Inject]
	public IDependencyInjection injector { private get; set; }

	// Token: 0x17001260 RID: 4704
	// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x00147AFF File Offset: 0x00145EFF
	// (set) Token: 0x06004DB7 RID: 19895 RVA: 0x00147B07 File Offset: 0x00145F07
	[Inject]
	public IVRAMPreloader vramPreloader { private get; set; }

	// Token: 0x17001261 RID: 4705
	// (get) Token: 0x06004DB8 RID: 19896 RVA: 0x00147B10 File Offset: 0x00145F10
	// (set) Token: 0x06004DB9 RID: 19897 RVA: 0x00147B18 File Offset: 0x00145F18
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17001262 RID: 4706
	// (get) Token: 0x06004DBA RID: 19898 RVA: 0x00147B21 File Offset: 0x00145F21
	// (set) Token: 0x06004DBB RID: 19899 RVA: 0x00147B29 File Offset: 0x00145F29
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { private get; set; }

	// Token: 0x06004DBC RID: 19900 RVA: 0x00147B32 File Offset: 0x00145F32
	[PostConstruct]
	public void Init()
	{
		this.container = new GameObject("UICharacterManager");
		this.container.SetActive(false);
		UnityEngine.Object.DontDestroyOnLoad(this.container);
	}

	// Token: 0x06004DBD RID: 19901 RVA: 0x00147B5B File Offset: 0x00145F5B
	public void ClearPreload()
	{
		this.vramPreloaded.Clear();
	}

	// Token: 0x06004DBE RID: 19902 RVA: 0x00147B68 File Offset: 0x00145F68
	private IUISceneCharacter getFromPool(CharacterID id)
	{
		if (this.pool.ContainsKey(id) && this.pool[id].Count > 0)
		{
			IUISceneCharacter iuisceneCharacter = this.pool[id][0];
			this.pool[id].RemoveAt(0);
			iuisceneCharacter.Reinitialize();
			return iuisceneCharacter;
		}
		return null;
	}

	// Token: 0x06004DBF RID: 19903 RVA: 0x00147BCC File Offset: 0x00145FCC
	private void addToPool(IUISceneCharacter display)
	{
		CharacterID characterID = display.CharacterID;
		if (!this.pool.ContainsKey(characterID))
		{
			this.pool[characterID] = new List<IUISceneCharacter>();
		}
		this.pool[characterID].Add(display);
	}

	// Token: 0x06004DC0 RID: 19904 RVA: 0x00147C14 File Offset: 0x00146014
	public IUISceneCharacter GetCharacter(CharacterID characterId, SkinDefinition initWithSkin)
	{
		IUISceneCharacter iuisceneCharacter = this.getFromPool(characterId);
		if (iuisceneCharacter == null)
		{
			CharacterMenusData data = this.characterMenusDataLoader.GetData(characterId);
			iuisceneCharacter = new GameObject("UIChar-" + data.characterName).AddComponent<UISceneCharacterGroup>();
			UnityEngine.Object.DontDestroyOnLoad((iuisceneCharacter as Component).gameObject);
			(iuisceneCharacter as Component).transform.SetParent(this.container.transform, false);
			this.injector.Inject(iuisceneCharacter);
			iuisceneCharacter.Init(data, initWithSkin);
		}
		(iuisceneCharacter as MonoBehaviour).gameObject.SetActive(true);
		return iuisceneCharacter;
	}

	// Token: 0x06004DC1 RID: 19905 RVA: 0x00147CAC File Offset: 0x001460AC
	public void ReleaseCharacter(IUISceneCharacter characterDisplay)
	{
		if (characterDisplay != null && this.container != null)
		{
			Component component = characterDisplay as Component;
			if (component != null && component.transform != null)
			{
				component.transform.SetParent(this.container.transform, false);
				characterDisplay.RemoveAligners();
				component.transform.localScale = Vector3.one;
				component.transform.localPosition = Vector3.zero;
				this.addToPool(characterDisplay);
			}
		}
	}

	// Token: 0x06004DC2 RID: 19906 RVA: 0x00147D38 File Offset: 0x00146138
	private void recursivePreloadSkins(List<UISceneCharacterManager.CharacterWithSkin> characters, int i, Action callback)
	{
		if (i >= characters.Count)
		{
			callback();
		}
		else
		{
			UISceneCharacterManager.CharacterWithSkin characterWithSkin = characters[i];
			this.skinDataManager.GetSkinData(characterWithSkin.skinDefinition, delegate(SkinData skinData)
			{
				this.recursivePreloadSkins(characters, i + 1, callback);
			});
		}
	}

	// Token: 0x06004DC3 RID: 19907 RVA: 0x00147DC0 File Offset: 0x001461C0
	public void VRAMPreload(List<UISceneCharacterManager.CharacterWithSkin> characters, Action callback)
	{
		UISceneCharacterManager.<VRAMPreload>c__AnonStorey1 <VRAMPreload>c__AnonStorey = new UISceneCharacterManager.<VRAMPreload>c__AnonStorey1();
		<VRAMPreload>c__AnonStorey.characters = characters;
		<VRAMPreload>c__AnonStorey.callback = callback;
		<VRAMPreload>c__AnonStorey.$this = this;
		this.recursivePreloadSkins(<VRAMPreload>c__AnonStorey.characters, 0, delegate
		{
			List<GameObject> list = new List<GameObject>();
			List<IUISceneCharacter> list2 = new List<IUISceneCharacter>();
			List<IUISceneCharacter> releaseList = new List<IUISceneCharacter>();
			foreach (UISceneCharacterManager.CharacterWithSkin item in <VRAMPreload>c__AnonStorey.characters)
			{
				if (!<VRAMPreload>c__AnonStorey.$this.vramPreloaded.Contains(item))
				{
					<VRAMPreload>c__AnonStorey.$this.vramPreloaded.Add(item);
					IUISceneCharacter character = <VRAMPreload>c__AnonStorey.$this.GetCharacter(item.characterDef.characterID, item.skinDefinition);
					releaseList.Add(character);
					list2.Add(character);
					list.Add((character as MonoBehaviour).gameObject);
				}
			}
			int count = list2.Count;
			<VRAMPreload>c__AnonStorey.$this.vramPreloader.Preload(Camera.main, list, delegate
			{
				foreach (IUISceneCharacter characterDisplay in releaseList)
				{
					<VRAMPreload>c__AnonStorey.ReleaseCharacter(characterDisplay);
				}
				if (<VRAMPreload>c__AnonStorey.callback != null)
				{
					<VRAMPreload>c__AnonStorey.callback();
				}
			});
		});
	}

	// Token: 0x040032E9 RID: 13033
	private Dictionary<CharacterID, List<IUISceneCharacter>> pool = new Dictionary<CharacterID, List<IUISceneCharacter>>();

	// Token: 0x040032EA RID: 13034
	private GameObject container;

	// Token: 0x040032EB RID: 13035
	private HashSet<UISceneCharacterManager.CharacterWithSkin> vramPreloaded = new HashSet<UISceneCharacterManager.CharacterWithSkin>();

	// Token: 0x02000A6F RID: 2671
	public struct CharacterWithSkin
	{
		// Token: 0x06004DC4 RID: 19908 RVA: 0x00147E01 File Offset: 0x00146201
		public override bool Equals(object other)
		{
			return other is UISceneCharacterManager.CharacterWithSkin && (UISceneCharacterManager.CharacterWithSkin)other == this;
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x00147E24 File Offset: 0x00146224
		public override int GetHashCode()
		{
			int num = 17;
			num = num * 7 + ((!(this.characterDef == null)) ? this.characterDef.GetHashCode() : 0);
			return num * 7 + ((!(this.skinDefinition == null)) ? this.skinDefinition.GetHashCode() : 0);
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x00147E83 File Offset: 0x00146283
		public static bool operator ==(UISceneCharacterManager.CharacterWithSkin a, UISceneCharacterManager.CharacterWithSkin b)
		{
			return a.characterDef == b.characterDef && a.skinDefinition == b.skinDefinition;
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x00147EB3 File Offset: 0x001462B3
		public static bool operator !=(UISceneCharacterManager.CharacterWithSkin a, UISceneCharacterManager.CharacterWithSkin b)
		{
			return !(a == b);
		}

		// Token: 0x040032EC RID: 13036
		public CharacterDefinition characterDef;

		// Token: 0x040032ED RID: 13037
		public SkinDefinition skinDefinition;
	}
}
