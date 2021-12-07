// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;
using System.Collections.Generic;

namespace strange.framework.impl
{
	public class Binder : IBinder
	{
		public delegate void BindingResolver(IBinding binding);

		protected Dictionary<object, Dictionary<object, IBinding>> bindings;

		protected Dictionary<object, Dictionary<IBinding, object>> conflicts;

		public Binder()
		{
			this.bindings = new Dictionary<object, Dictionary<object, IBinding>>();
			this.conflicts = new Dictionary<object, Dictionary<IBinding, object>>();
		}

		public virtual IBinding Bind<T>()
		{
			return this.Bind(typeof(T));
		}

		public virtual IBinding Bind(object key)
		{
			IBinding rawBinding = this.GetRawBinding();
			rawBinding.Bind(key);
			return rawBinding;
		}

		public virtual IBinding GetBinding<T>()
		{
			return this.GetBinding(typeof(T), null);
		}

		public virtual IBinding GetBinding(object key)
		{
			return this.GetBinding(key, null);
		}

		public virtual IBinding GetBinding<T>(object name)
		{
			return this.GetBinding(typeof(T), name);
		}

		public virtual IBinding GetBinding(object key, object name)
		{
			if (this.conflicts.Count > 0)
			{
				string text = string.Empty;
				Dictionary<object, Dictionary<IBinding, object>>.KeyCollection keys = this.conflicts.Keys;
				foreach (object current in keys)
				{
					if (text.Length > 0)
					{
						text += ", ";
					}
					text += current.ToString();
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

		public virtual void Unbind<T>()
		{
			this.Unbind(typeof(T), null);
		}

		public virtual void Unbind(object key)
		{
			this.Unbind(key, null);
		}

		public virtual void Unbind<T>(object name)
		{
			this.Unbind(typeof(T), name);
		}

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

		public virtual void Unbind(IBinding binding)
		{
			if (binding == null)
			{
				return;
			}
			this.Unbind(binding.key, binding.name);
		}

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

		public virtual IBinding GetRawBinding()
		{
			return new Binding(new Binder.BindingResolver(this.resolver));
		}

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

		protected bool isConflictCleared(Dictionary<IBinding, object> dict, IBinding binding)
		{
			foreach (KeyValuePair<IBinding, object> current in dict)
			{
				if (current.Key != binding && current.Key.name == binding.name)
				{
					return false;
				}
			}
			return true;
		}

		protected void clearConflict(object key, object name, Dictionary<IBinding, object> dict)
		{
			List<IBinding> list = new List<IBinding>();
			foreach (KeyValuePair<IBinding, object> current in dict)
			{
				object value = current.Value;
				if (value.Equals(name))
				{
					list.Add(current.Key);
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

		protected object[] spliceValueAt(int splicePos, object[] objectValue)
		{
			return this.spliceValueAt<object>(splicePos, objectValue);
		}

		public virtual void OnRemove()
		{
		}
	}
}
