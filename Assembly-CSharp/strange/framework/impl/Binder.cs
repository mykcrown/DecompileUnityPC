using System;
using System.Collections.Generic;
using strange.framework.api;

namespace strange.framework.impl
{
	// Token: 0x0200028E RID: 654
	public class Binder : IBinder
	{
		// Token: 0x06000D8E RID: 3470 RVA: 0x00051516 File Offset: 0x0004F916
		public Binder()
		{
			this.bindings = new Dictionary<object, Dictionary<object, IBinding>>();
			this.conflicts = new Dictionary<object, Dictionary<IBinding, object>>();
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00051534 File Offset: 0x0004F934
		public virtual IBinding Bind<T>()
		{
			return this.Bind(typeof(T));
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x00051548 File Offset: 0x0004F948
		public virtual IBinding Bind(object key)
		{
			IBinding rawBinding = this.GetRawBinding();
			rawBinding.Bind(key);
			return rawBinding;
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00051565 File Offset: 0x0004F965
		public virtual IBinding GetBinding<T>()
		{
			return this.GetBinding(typeof(T), null);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00051578 File Offset: 0x0004F978
		public virtual IBinding GetBinding(object key)
		{
			return this.GetBinding(key, null);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00051582 File Offset: 0x0004F982
		public virtual IBinding GetBinding<T>(object name)
		{
			return this.GetBinding(typeof(T), name);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00051598 File Offset: 0x0004F998
		public virtual IBinding GetBinding(object key, object name)
		{
			if (this.conflicts.Count > 0)
			{
				string text = string.Empty;
				Dictionary<object, Dictionary<IBinding, object>>.KeyCollection keys = this.conflicts.Keys;
				foreach (object obj in keys)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += obj.ToString();
				}
				throw new BinderException("Binder cannot fetch Bindings when the binder is in a conflicted state.\nConflicts: " + text, BinderExceptionType.CONFLICT_IN_BINDER);
			}
			if (this.bindings.ContainsKey(key))
			{
				Dictionary<object, IBinding> dictionary = this.bindings[key];
				name = ((name != null) ? name : BindingConst.NULLOID);
				if (dictionary.ContainsKey(name))
				{
					return dictionary[name];
				}
			}
			return null;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0005168C File Offset: 0x0004FA8C
		public virtual void Unbind<T>()
		{
			this.Unbind(typeof(T), null);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0005169F File Offset: 0x0004FA9F
		public virtual void Unbind(object key)
		{
			this.Unbind(key, null);
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x000516A9 File Offset: 0x0004FAA9
		public virtual void Unbind<T>(object name)
		{
			this.Unbind(typeof(T), name);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x000516BC File Offset: 0x0004FABC
		public virtual void Unbind(object key, object name)
		{
			if (this.bindings.ContainsKey(key))
			{
				Dictionary<object, IBinding> dictionary = this.bindings[key];
				object key2 = (name != null) ? name : BindingConst.NULLOID;
				if (dictionary.ContainsKey(key2))
				{
					dictionary.Remove(key2);
				}
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0005170E File Offset: 0x0004FB0E
		public virtual void Unbind(IBinding binding)
		{
			if (binding == null)
			{
				return;
			}
			this.Unbind(binding.key, binding.name);
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0005172C File Offset: 0x0004FB2C
		public virtual void RemoveValue(IBinding binding, object value)
		{
			if (binding == null || value == null)
			{
				return;
			}
			object key = binding.key;
			if (this.bindings.ContainsKey(key))
			{
				Dictionary<object, IBinding> dictionary = this.bindings[key];
				if (dictionary.ContainsKey(binding.name))
				{
					IBinding binding2 = dictionary[binding.name];
					binding2.RemoveValue(value);
					object[] array = binding2.value as object[];
					if (array == null || array.Length == 0)
					{
						dictionary.Remove(binding2.name);
					}
				}
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x000517B8 File Offset: 0x0004FBB8
		public virtual void RemoveKey(IBinding binding, object key)
		{
			if (binding == null || key == null || !this.bindings.ContainsKey(key))
			{
				return;
			}
			Dictionary<object, IBinding> dictionary = this.bindings[key];
			if (dictionary.ContainsKey(binding.name))
			{
				IBinding binding2 = dictionary[binding.name];
				binding2.RemoveKey(key);
				object[] array = binding2.key as object[];
				if (array != null && array.Length == 0)
				{
					dictionary.Remove(binding.name);
				}
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0005183C File Offset: 0x0004FC3C
		public virtual void RemoveName(IBinding binding, object name)
		{
			if (binding == null || name == null)
			{
				return;
			}
			object key;
			if (binding.keyConstraint.Equals(BindingConstraintType.ONE))
			{
				key = binding.key;
			}
			else
			{
				object[] array = binding.key as object[];
				key = array[0];
			}
			Dictionary<object, IBinding> dictionary = this.bindings[key];
			if (dictionary.ContainsKey(name))
			{
				IBinding binding2 = dictionary[name];
				binding2.RemoveName(name);
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x000518B0 File Offset: 0x0004FCB0
		public virtual IBinding GetRawBinding()
		{
			return new Binding(new Binder.BindingResolver(this.resolver));
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x000518C4 File Offset: 0x0004FCC4
		protected virtual void resolver(IBinding binding)
		{
			object key = binding.key;
			if (binding.keyConstraint.Equals(BindingConstraintType.ONE))
			{
				this.ResolveBinding(binding, key);
			}
			else
			{
				object[] array = key as object[];
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					this.ResolveBinding(binding, array[i]);
				}
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00051924 File Offset: 0x0004FD24
		public virtual void ResolveBinding(IBinding binding, object key)
		{
			if (this.conflicts.ContainsKey(key))
			{
				Dictionary<IBinding, object> dictionary = this.conflicts[key];
				if (dictionary.ContainsKey(binding))
				{
					object name = dictionary[binding];
					if (!this.isConflictCleared(dictionary, binding))
					{
						return;
					}
					this.clearConflict(key, name, dictionary);
				}
			}
			object key2 = (binding.name != null) ? binding.name : BindingConst.NULLOID;
			Dictionary<object, IBinding> dictionary2;
			if (this.bindings.ContainsKey(key))
			{
				dictionary2 = this.bindings[key];
				if (dictionary2.ContainsKey(key2))
				{
					IBinding binding2 = dictionary2[key2];
					if (binding2 != binding && !binding2.isWeak && !binding.isWeak)
					{
						this.registerNameConflict(key, binding, dictionary2[key2]);
						return;
					}
					if (binding2.isWeak && binding2 != binding && binding2.isWeak && (!binding.isWeak || binding2.value == null || binding2.value is Type))
					{
						dictionary2.Remove(key2);
					}
				}
			}
			else
			{
				dictionary2 = new Dictionary<object, IBinding>();
				this.bindings[key] = dictionary2;
			}
			if (dictionary2.ContainsKey(BindingConst.NULLOID) && dictionary2[BindingConst.NULLOID] == binding)
			{
				dictionary2.Remove(BindingConst.NULLOID);
			}
			if (!dictionary2.ContainsKey(key2))
			{
				dictionary2.Add(key2, binding);
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00051AAC File Offset: 0x0004FEAC
		protected void registerNameConflict(object key, IBinding newBinding, IBinding existingBinding)
		{
			Dictionary<IBinding, object> dictionary;
			if (!this.conflicts.ContainsKey(key))
			{
				dictionary = new Dictionary<IBinding, object>();
				this.conflicts[key] = dictionary;
			}
			else
			{
				dictionary = this.conflicts[key];
			}
			dictionary[newBinding] = newBinding.name;
			dictionary[existingBinding] = newBinding.name;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00051B0C File Offset: 0x0004FF0C
		protected bool isConflictCleared(Dictionary<IBinding, object> dict, IBinding binding)
		{
			foreach (KeyValuePair<IBinding, object> keyValuePair in dict)
			{
				if (keyValuePair.Key != binding && keyValuePair.Key.name == binding.name)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00051B8C File Offset: 0x0004FF8C
		protected void clearConflict(object key, object name, Dictionary<IBinding, object> dict)
		{
			List<IBinding> list = new List<IBinding>();
			foreach (KeyValuePair<IBinding, object> keyValuePair in dict)
			{
				object value = keyValuePair.Value;
				if (value.Equals(name))
				{
					list.Add(keyValuePair.Key);
				}
			}
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				dict.Remove(list[i]);
			}
			if (dict.Count == 0)
			{
				this.conflicts.Remove(key);
			}
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00051C4C File Offset: 0x0005004C
		protected T[] spliceValueAt<T>(int splicePos, object[] objectValue)
		{
			T[] array = new T[objectValue.Length - 1];
			int num = 0;
			int num2 = objectValue.Length;
			for (int i = 0; i < num2; i++)
			{
				if (i == splicePos)
				{
					num = -1;
				}
				else
				{
					array[i + num] = (T)((object)objectValue[i]);
				}
			}
			return array;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00051C9C File Offset: 0x0005009C
		protected object[] spliceValueAt(int splicePos, object[] objectValue)
		{
			return this.spliceValueAt<object>(splicePos, objectValue);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00051CA6 File Offset: 0x000500A6
		public virtual void OnRemove()
		{
		}

		// Token: 0x040007E8 RID: 2024
		protected Dictionary<object, Dictionary<object, IBinding>> bindings;

		// Token: 0x040007E9 RID: 2025
		protected Dictionary<object, Dictionary<IBinding, object>> conflicts;

		// Token: 0x0200028F RID: 655
		// (Invoke) Token: 0x06000DA7 RID: 3495
		public delegate void BindingResolver(IBinding binding);
	}
}
