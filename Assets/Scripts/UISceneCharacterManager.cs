// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UISceneCharacterManager : IUISceneCharacterManager
{
	public struct CharacterWithSkin
	{
		public CharacterDefinition characterDef;

		public SkinDefinition skinDefinition;

		public override bool Equals(object other)
		{
			return other is UISceneCharacterManager.CharacterWithSkin && (UISceneCharacterManager.CharacterWithSkin)other == this;
		}

		public override int GetHashCode()
		{
			int num = 17;
			num = num * 7 + ((!(this.characterDef == null)) ? this.characterDef.GetHashCode() : 0);
			return num * 7 + ((!(this.skinDefinition == null)) ? this.skinDefinition.GetHashCode() : 0);
		}

		public static bool operator ==(UISceneCharacterManager.CharacterWithSkin a, UISceneCharacterManager.CharacterWithSkin b)
		{
			return a.characterDef == b.characterDef && a.skinDefinition == b.skinDefinition;
		}

		public static bool operator !=(UISceneCharacterManager.CharacterWithSkin a, UISceneCharacterManager.CharacterWithSkin b)
		{
			return !(a == b);
		}
	}

	private sealed class _recursivePreloadSkins_c__AnonStorey0
	{
		internal List<UISceneCharacterManager.CharacterWithSkin> characters;

		internal int i;

		internal Action callback;

		internal UISceneCharacterManager _this;

		internal void __m__0(SkinData skinData)
		{
			this._this.recursivePreloadSkins(this.characters, this.i + 1, this.callback);
		}
	}

	private sealed class _VRAMPreload_c__AnonStorey1
	{
		private sealed class _VRAMPreload_c__AnonStorey2
		{
			internal List<IUISceneCharacter> releaseList;

			internal UISceneCharacterManager._VRAMPreload_c__AnonStorey1 __f__ref_1;

			internal void __m__0()
			{
				foreach (IUISceneCharacter current in this.releaseList)
				{
					this.__f__ref_1._this.ReleaseCharacter(current);
				}
				if (this.__f__ref_1.callback != null)
				{
					this.__f__ref_1.callback();
				}
			}
		}

		internal List<UISceneCharacterManager.CharacterWithSkin> characters;

		internal Action callback;

		internal UISceneCharacterManager _this;

		internal void __m__0()
		{
			UISceneCharacterManager._VRAMPreload_c__AnonStorey1._VRAMPreload_c__AnonStorey2 _VRAMPreload_c__AnonStorey = new UISceneCharacterManager._VRAMPreload_c__AnonStorey1._VRAMPreload_c__AnonStorey2();
			_VRAMPreload_c__AnonStorey.__f__ref_1 = this;
			List<GameObject> list = new List<GameObject>();
			List<IUISceneCharacter> list2 = new List<IUISceneCharacter>();
			_VRAMPreload_c__AnonStorey.releaseList = new List<IUISceneCharacter>();
			foreach (UISceneCharacterManager.CharacterWithSkin current in this.characters)
			{
				if (!this._this.vramPreloaded.Contains(current))
				{
					this._this.vramPreloaded.Add(current);
					IUISceneCharacter character = this._this.GetCharacter(current.characterDef.characterID, current.skinDefinition);
					_VRAMPreload_c__AnonStorey.releaseList.Add(character);
					list2.Add(character);
					list.Add((character as MonoBehaviour).gameObject);
				}
			}
			int count = list2.Count;
			this._this.vramPreloader.Preload(Camera.main, list, new Action(_VRAMPreload_c__AnonStorey.__m__0));
		}
	}

	private Dictionary<CharacterID, List<IUISceneCharacter>> pool = new Dictionary<CharacterID, List<IUISceneCharacter>>();

	private GameObject container;

	private HashSet<UISceneCharacterManager.CharacterWithSkin> vramPreloaded = new HashSet<UISceneCharacterManager.CharacterWithSkin>();

	[Inject]
	public IDependencyInjection injector
	{
		private get;
		set;
	}

	[Inject]
	public IVRAMPreloader vramPreloader
	{
		private get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader
	{
		private get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.container = new GameObject("UICharacterManager");
		this.container.SetActive(false);
		UnityEngine.Object.DontDestroyOnLoad(this.container);
	}

	public void ClearPreload()
	{
		this.vramPreloaded.Clear();
	}

	private IUISceneCharacter getFromPool(CharacterID id)
	{
		if (this.pool.ContainsKey(id) && this.pool[id].Count > 0)
		{
			IUISceneCharacter iUISceneCharacter = this.pool[id][0];
			this.pool[id].RemoveAt(0);
			iUISceneCharacter.Reinitialize();
			return iUISceneCharacter;
		}
		return null;
	}

	private void addToPool(IUISceneCharacter display)
	{
		CharacterID characterID = display.CharacterID;
		if (!this.pool.ContainsKey(characterID))
		{
			this.pool[characterID] = new List<IUISceneCharacter>();
		}
		this.pool[characterID].Add(display);
	}

	public IUISceneCharacter GetCharacter(CharacterID characterId, SkinDefinition initWithSkin)
	{
		IUISceneCharacter iUISceneCharacter = this.getFromPool(characterId);
		if (iUISceneCharacter == null)
		{
			CharacterMenusData data = this.characterMenusDataLoader.GetData(characterId);
			iUISceneCharacter = new GameObject("UIChar-" + data.characterName).AddComponent<UISceneCharacterGroup>();
			UnityEngine.Object.DontDestroyOnLoad((iUISceneCharacter as Component).gameObject);
			(iUISceneCharacter as Component).transform.SetParent(this.container.transform, false);
			this.injector.Inject(iUISceneCharacter);
			iUISceneCharacter.Init(data, initWithSkin);
		}
		(iUISceneCharacter as MonoBehaviour).gameObject.SetActive(true);
		return iUISceneCharacter;
	}

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

	private void recursivePreloadSkins(List<UISceneCharacterManager.CharacterWithSkin> characters, int i, Action callback)
	{
		UISceneCharacterManager._recursivePreloadSkins_c__AnonStorey0 _recursivePreloadSkins_c__AnonStorey = new UISceneCharacterManager._recursivePreloadSkins_c__AnonStorey0();
		_recursivePreloadSkins_c__AnonStorey.characters = characters;
		_recursivePreloadSkins_c__AnonStorey.i = i;
		_recursivePreloadSkins_c__AnonStorey.callback = callback;
		_recursivePreloadSkins_c__AnonStorey._this = this;
		if (_recursivePreloadSkins_c__AnonStorey.i >= _recursivePreloadSkins_c__AnonStorey.characters.Count)
		{
			_recursivePreloadSkins_c__AnonStorey.callback();
		}
		else
		{
			UISceneCharacterManager.CharacterWithSkin characterWithSkin = _recursivePreloadSkins_c__AnonStorey.characters[_recursivePreloadSkins_c__AnonStorey.i];
			this.skinDataManager.GetSkinData(characterWithSkin.skinDefinition, new Action<SkinData>(_recursivePreloadSkins_c__AnonStorey.__m__0));
		}
	}

	public void VRAMPreload(List<UISceneCharacterManager.CharacterWithSkin> characters, Action callback)
	{
		UISceneCharacterManager._VRAMPreload_c__AnonStorey1 _VRAMPreload_c__AnonStorey = new UISceneCharacterManager._VRAMPreload_c__AnonStorey1();
		_VRAMPreload_c__AnonStorey.characters = characters;
		_VRAMPreload_c__AnonStorey.callback = callback;
		_VRAMPreload_c__AnonStorey._this = this;
		this.recursivePreloadSkins(_VRAMPreload_c__AnonStorey.characters, 0, new Action(_VRAMPreload_c__AnonStorey.__m__0));
	}
}
