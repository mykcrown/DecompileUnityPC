using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000060 RID: 96
	public class PlayerAction : OneAxisInputControl
	{
		// Token: 0x0600031D RID: 797 RVA: 0x00016344 File Offset: 0x00014744
		public PlayerAction(string name, PlayerActionSet owner)
		{
			this.Raw = true;
			this.Name = name;
			this.Owner = owner;
			this.bindings = new ReadOnlyCollection<BindingSource>(this.visibleBindings);
			this.unfilteredBindings = new ReadOnlyCollection<BindingSource>(this.regularBindings);
			owner.AddPlayerAction(this);
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600031E RID: 798 RVA: 0x000163B6 File Offset: 0x000147B6
		// (set) Token: 0x0600031F RID: 799 RVA: 0x000163BE File Offset: 0x000147BE
		public string Name { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000320 RID: 800 RVA: 0x000163C7 File Offset: 0x000147C7
		// (set) Token: 0x06000321 RID: 801 RVA: 0x000163CF File Offset: 0x000147CF
		public PlayerActionSet Owner { get; private set; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000322 RID: 802 RVA: 0x000163D8 File Offset: 0x000147D8
		// (remove) Token: 0x06000323 RID: 803 RVA: 0x00016410 File Offset: 0x00014810
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000324 RID: 804 RVA: 0x00016446 File Offset: 0x00014846
		// (set) Token: 0x06000325 RID: 805 RVA: 0x0001644E File Offset: 0x0001484E
		public object UserData { get; set; }

		// Token: 0x06000326 RID: 806 RVA: 0x00016458 File Offset: 0x00014858
		public void AddDefaultBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			if (binding.BoundTo != null)
			{
				throw new InControlException("Binding source is already bound to action " + binding.BoundTo.Name);
			}
			if (!this.defaultBindings.Contains(binding))
			{
				this.defaultBindings.Add(binding);
				binding.BoundTo = this;
			}
			if (!this.regularBindings.Contains(binding))
			{
				this.regularBindings.Add(binding);
				binding.BoundTo = this;
				if (binding.IsValid)
				{
					this.visibleBindings.Add(binding);
				}
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000164F7 File Offset: 0x000148F7
		public void AddDefaultBinding(params Key[] keys)
		{
			this.AddDefaultBinding(new KeyBindingSource(keys));
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00016505 File Offset: 0x00014905
		public void AddDefaultBinding(KeyCombo keyCombo)
		{
			this.AddDefaultBinding(new KeyBindingSource(keyCombo));
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00016513 File Offset: 0x00014913
		public void AddDefaultBinding(Mouse control)
		{
			this.AddDefaultBinding(new MouseBindingSource(control));
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00016521 File Offset: 0x00014921
		public void AddDefaultBinding(InputControlType control)
		{
			this.AddDefaultBinding(new DeviceBindingSource(control));
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00016530 File Offset: 0x00014930
		public bool AddBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			if (binding.BoundTo != null)
			{
				UnityEngine.Debug.LogWarning("Binding source is already bound to action " + binding.BoundTo.Name);
				return false;
			}
			if (this.regularBindings.Contains(binding))
			{
				return false;
			}
			this.regularBindings.Add(binding);
			binding.BoundTo = this;
			if (binding.IsValid)
			{
				this.visibleBindings.Add(binding);
			}
			return true;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x000165B0 File Offset: 0x000149B0
		public bool InsertBindingAt(int index, BindingSource binding)
		{
			if (index < 0 || index > this.visibleBindings.Count)
			{
				throw new InControlException("Index is out of range for bindings on this action.");
			}
			if (index == this.visibleBindings.Count)
			{
				return this.AddBinding(binding);
			}
			if (binding == null)
			{
				return false;
			}
			if (binding.BoundTo != null)
			{
				UnityEngine.Debug.LogWarning("Binding source is already bound to action " + binding.BoundTo.Name);
				return false;
			}
			if (this.regularBindings.Contains(binding))
			{
				return false;
			}
			int index2 = (index != 0) ? this.regularBindings.IndexOf(this.visibleBindings[index]) : 0;
			this.regularBindings.Insert(index2, binding);
			binding.BoundTo = this;
			if (binding.IsValid)
			{
				this.visibleBindings.Insert(index, binding);
			}
			return true;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00016694 File Offset: 0x00014A94
		public bool ReplaceBinding(BindingSource findBinding, BindingSource withBinding)
		{
			if (findBinding == null || withBinding == null)
			{
				return false;
			}
			if (withBinding.BoundTo != null)
			{
				UnityEngine.Debug.LogWarning("Binding source is already bound to action " + withBinding.BoundTo.Name);
				return false;
			}
			int num = this.regularBindings.IndexOf(findBinding);
			if (num < 0)
			{
				UnityEngine.Debug.LogWarning("Binding source to replace is not present in this action.");
				return false;
			}
			findBinding.BoundTo = null;
			this.regularBindings[num] = withBinding;
			withBinding.BoundTo = this;
			num = this.visibleBindings.IndexOf(findBinding);
			if (num >= 0)
			{
				this.visibleBindings[num] = withBinding;
			}
			return true;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00016740 File Offset: 0x00014B40
		public bool HasBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			BindingSource bindingSource = this.FindBinding(binding);
			return !(bindingSource == null) && bindingSource.BoundTo == this;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0001677C File Offset: 0x00014B7C
		public BindingSource FindBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return null;
			}
			int num = this.regularBindings.IndexOf(binding);
			if (num >= 0)
			{
				return this.regularBindings[num];
			}
			return null;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000167BC File Offset: 0x00014BBC
		public void HardRemoveBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			int num = this.regularBindings.IndexOf(binding);
			if (num >= 0)
			{
				BindingSource bindingSource = this.regularBindings[num];
				if (bindingSource.BoundTo == this)
				{
					bindingSource.BoundTo = null;
					this.regularBindings.RemoveAt(num);
					this.UpdateVisibleBindings();
				}
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0001681C File Offset: 0x00014C1C
		public void RemoveBinding(BindingSource binding)
		{
			BindingSource bindingSource = this.FindBinding(binding);
			if (bindingSource != null && bindingSource.BoundTo == this)
			{
				bindingSource.BoundTo = null;
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00016850 File Offset: 0x00014C50
		public void RemoveBindingAt(int index)
		{
			if (index < 0 || index >= this.regularBindings.Count)
			{
				throw new InControlException("Index is out of range for bindings on this action.");
			}
			this.regularBindings[index].BoundTo = null;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00016888 File Offset: 0x00014C88
		private int CountBindingsOfType(BindingSourceType bindingSourceType)
		{
			int num = 0;
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.BoundTo == this && bindingSource.BindingSourceType == bindingSourceType)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x000168E0 File Offset: 0x00014CE0
		private void RemoveFirstBindingOfType(BindingSourceType bindingSourceType)
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.BoundTo == this && bindingSource.BindingSourceType == bindingSourceType)
				{
					bindingSource.BoundTo = null;
					this.regularBindings.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00016944 File Offset: 0x00014D44
		private int IndexOfFirstInvalidBinding()
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				if (!this.regularBindings[i].IsValid)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00016988 File Offset: 0x00014D88
		public void ClearBindings()
		{
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				this.regularBindings[i].BoundTo = null;
			}
			this.regularBindings.Clear();
			this.visibleBindings.Clear();
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000169DC File Offset: 0x00014DDC
		public void ResetBindings()
		{
			this.ClearBindings();
			this.regularBindings.AddRange(this.defaultBindings);
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				bindingSource.BoundTo = this;
				if (bindingSource.IsValid)
				{
					this.visibleBindings.Add(bindingSource);
				}
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00016A49 File Offset: 0x00014E49
		public void ListenForBinding()
		{
			this.ListenForBindingReplacing(null);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00016A54 File Offset: 0x00014E54
		public void ListenForBindingReplacing(BindingSource binding)
		{
			BindingListenOptions bindingListenOptions = this.ListenOptions ?? this.Owner.ListenOptions;
			bindingListenOptions.ReplaceBinding = binding;
			this.Owner.listenWithAction = this;
			int num = PlayerAction.bindingSourceListeners.Length;
			for (int i = 0; i < num; i++)
			{
				PlayerAction.bindingSourceListeners[i].Reset();
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00016AB3 File Offset: 0x00014EB3
		public void StopListeningForBinding()
		{
			if (this.IsListeningForBinding)
			{
				this.Owner.listenWithAction = null;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00016ACC File Offset: 0x00014ECC
		public bool IsListeningForBinding
		{
			get
			{
				return this.Owner.listenWithAction == this;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600033C RID: 828 RVA: 0x00016ADC File Offset: 0x00014EDC
		public ReadOnlyCollection<BindingSource> Bindings
		{
			get
			{
				return this.bindings;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00016AE4 File Offset: 0x00014EE4
		public ReadOnlyCollection<BindingSource> UnfilteredBindings
		{
			get
			{
				return this.unfilteredBindings;
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00016AEC File Offset: 0x00014EEC
		private void RemoveOrphanedBindings()
		{
			int count = this.regularBindings.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				if (this.regularBindings[i].BoundTo != this)
				{
					this.regularBindings.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00016B3C File Offset: 0x00014F3C
		internal void Update(ulong updateTick, float deltaTime, InputDevice device)
		{
			this.Device = device;
			this.UpdateBindings(updateTick, deltaTime);
			this.DetectBindings();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00016B54 File Offset: 0x00014F54
		private void UpdateBindings(ulong updateTick, float deltaTime)
		{
			bool flag = this.IsListeningForBinding || (this.Owner.IsListeningForBinding && this.Owner.PreventInputWhileListeningForBinding);
			BindingSourceType bindingSourceType = this.LastInputType;
			ulong num = this.LastInputTypeChangedTick;
			ulong updateTick2 = base.UpdateTick;
			InputDeviceClass lastDeviceClass = this.LastDeviceClass;
			InputDeviceStyle lastDeviceStyle = this.LastDeviceStyle;
			int count = this.regularBindings.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.BoundTo != this)
				{
					this.regularBindings.RemoveAt(i);
					this.visibleBindings.Remove(bindingSource);
				}
				else if (!flag)
				{
					float value = bindingSource.GetValue(this.Device);
					if (base.UpdateWithValue(value, updateTick, deltaTime))
					{
						bindingSourceType = bindingSource.BindingSourceType;
						num = updateTick;
						lastDeviceClass = bindingSource.DeviceClass;
						lastDeviceStyle = bindingSource.DeviceStyle;
					}
				}
			}
			if (flag || count == 0)
			{
				base.UpdateWithValue(0f, updateTick, deltaTime);
			}
			base.Commit();
			this.Enabled = this.Owner.Enabled;
			if (num > this.LastInputTypeChangedTick && (bindingSourceType != BindingSourceType.MouseBindingSource || Utility.Abs(base.LastValue - base.Value) >= MouseBindingSource.JitterThreshold))
			{
				bool flag2 = bindingSourceType != this.LastInputType;
				this.LastInputType = bindingSourceType;
				this.LastInputTypeChangedTick = num;
				this.LastDeviceClass = lastDeviceClass;
				this.LastDeviceStyle = lastDeviceStyle;
				if (this.OnLastInputTypeChanged != null && flag2)
				{
					this.OnLastInputTypeChanged(bindingSourceType);
				}
			}
			if (base.UpdateTick > updateTick2)
			{
				this.activeDevice = ((!this.LastInputTypeIsDevice) ? null : this.Device);
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00016D2C File Offset: 0x0001512C
		private void DetectBindings()
		{
			if (this.IsListeningForBinding)
			{
				BindingSource bindingSource = null;
				BindingListenOptions bindingListenOptions = this.ListenOptions ?? this.Owner.ListenOptions;
				int num = PlayerAction.bindingSourceListeners.Length;
				for (int i = 0; i < num; i++)
				{
					bindingSource = PlayerAction.bindingSourceListeners[i].Listen(bindingListenOptions, this.device);
					if (bindingSource != null)
					{
						break;
					}
				}
				if (bindingSource == null)
				{
					return;
				}
				if (!bindingListenOptions.CallOnBindingFound(this, bindingSource))
				{
					return;
				}
				if (this.HasBinding(bindingSource))
				{
					if (bindingListenOptions.RejectRedundantBindings)
					{
						bindingListenOptions.CallOnBindingRejected(this, bindingSource, BindingSourceRejectionType.DuplicateBindingOnActionSet);
						return;
					}
					this.StopListeningForBinding();
					bindingListenOptions.CallOnBindingAdded(this, bindingSource);
					return;
				}
				else
				{
					if (bindingListenOptions.UnsetDuplicateBindingsOnSet)
					{
						int count = this.Owner.Actions.Count;
						for (int j = 0; j < count; j++)
						{
							this.Owner.Actions[j].HardRemoveBinding(bindingSource);
						}
					}
					if (!bindingListenOptions.AllowDuplicateBindingsPerSet && this.Owner.HasBinding(bindingSource))
					{
						bindingListenOptions.CallOnBindingRejected(this, bindingSource, BindingSourceRejectionType.DuplicateBindingOnActionSet);
						return;
					}
					this.StopListeningForBinding();
					if (bindingListenOptions.ReplaceBinding == null)
					{
						if (bindingListenOptions.MaxAllowedBindingsPerType > 0U)
						{
							while ((long)this.CountBindingsOfType(bindingSource.BindingSourceType) >= (long)((ulong)bindingListenOptions.MaxAllowedBindingsPerType))
							{
								this.RemoveFirstBindingOfType(bindingSource.BindingSourceType);
							}
						}
						else if (bindingListenOptions.MaxAllowedBindings > 0U)
						{
							while ((long)this.regularBindings.Count >= (long)((ulong)bindingListenOptions.MaxAllowedBindings))
							{
								int index = Mathf.Max(0, this.IndexOfFirstInvalidBinding());
								this.regularBindings.RemoveAt(index);
							}
						}
						this.AddBinding(bindingSource);
					}
					else
					{
						this.ReplaceBinding(bindingListenOptions.ReplaceBinding, bindingSource);
					}
					this.UpdateVisibleBindings();
					bindingListenOptions.CallOnBindingAdded(this, bindingSource);
				}
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00016F1C File Offset: 0x0001531C
		private void UpdateVisibleBindings()
		{
			this.visibleBindings.Clear();
			int count = this.regularBindings.Count;
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				if (bindingSource.IsValid)
				{
					this.visibleBindings.Add(bindingSource);
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00016F76 File Offset: 0x00015376
		// (set) Token: 0x06000344 RID: 836 RVA: 0x00016FA0 File Offset: 0x000153A0
		internal InputDevice Device
		{
			get
			{
				if (this.device == null)
				{
					this.device = this.Owner.Device;
					this.UpdateVisibleBindings();
				}
				return this.device;
			}
			set
			{
				if (this.device != value)
				{
					this.device = value;
					this.UpdateVisibleBindings();
				}
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00016FBB File Offset: 0x000153BB
		public InputDevice ActiveDevice
		{
			get
			{
				return (this.activeDevice != null) ? this.activeDevice : InputDevice.Null;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000346 RID: 838 RVA: 0x00016FD8 File Offset: 0x000153D8
		private bool LastInputTypeIsDevice
		{
			get
			{
				return this.LastInputType == BindingSourceType.DeviceBindingSource || this.LastInputType == BindingSourceType.UnknownDeviceBindingSource;
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00016FF4 File Offset: 0x000153F4
		internal void Load(BinaryReader reader, ushort dataFormatVersion)
		{
			this.ClearBindings();
			int num = reader.ReadInt32();
			int i = 0;
			while (i < num)
			{
				BindingSourceType bindingSourceType = (BindingSourceType)reader.ReadInt32();
				BindingSource bindingSource;
				switch (bindingSourceType)
				{
				case BindingSourceType.None:
					break;
				case BindingSourceType.DeviceBindingSource:
					bindingSource = new DeviceBindingSource();
					goto IL_81;
				case BindingSourceType.KeyBindingSource:
					bindingSource = new KeyBindingSource();
					goto IL_81;
				case BindingSourceType.MouseBindingSource:
					bindingSource = new MouseBindingSource();
					goto IL_81;
				case BindingSourceType.UnknownDeviceBindingSource:
					bindingSource = new UnknownDeviceBindingSource();
					goto IL_81;
				default:
					throw new InControlException("Don't know how to load BindingSourceType: " + bindingSourceType);
				}
				IL_91:
				i++;
				continue;
				IL_81:
				bindingSource.Load(reader, dataFormatVersion);
				this.AddBinding(bindingSource);
				goto IL_91;
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000170A0 File Offset: 0x000154A0
		internal void Save(BinaryWriter writer)
		{
			this.RemoveOrphanedBindings();
			writer.Write(this.Name);
			int count = this.regularBindings.Count;
			writer.Write(count);
			for (int i = 0; i < count; i++)
			{
				BindingSource bindingSource = this.regularBindings[i];
				writer.Write((int)bindingSource.BindingSourceType);
				bindingSource.Save(writer);
			}
		}

		// Token: 0x0400029E RID: 670
		public BindingListenOptions ListenOptions;

		// Token: 0x0400029F RID: 671
		public BindingSourceType LastInputType;

		// Token: 0x040002A1 RID: 673
		public ulong LastInputTypeChangedTick;

		// Token: 0x040002A2 RID: 674
		public InputDeviceClass LastDeviceClass;

		// Token: 0x040002A3 RID: 675
		public InputDeviceStyle LastDeviceStyle;

		// Token: 0x040002A5 RID: 677
		private List<BindingSource> defaultBindings = new List<BindingSource>();

		// Token: 0x040002A6 RID: 678
		private List<BindingSource> regularBindings = new List<BindingSource>();

		// Token: 0x040002A7 RID: 679
		private List<BindingSource> visibleBindings = new List<BindingSource>();

		// Token: 0x040002A8 RID: 680
		private readonly ReadOnlyCollection<BindingSource> bindings;

		// Token: 0x040002A9 RID: 681
		private readonly ReadOnlyCollection<BindingSource> unfilteredBindings;

		// Token: 0x040002AA RID: 682
		private static readonly BindingSourceListener[] bindingSourceListeners = new BindingSourceListener[]
		{
			new DeviceBindingSourceListener(),
			new UnknownDeviceBindingSourceListener(),
			new KeyBindingSourceListener(),
			new MouseBindingSourceListener()
		};

		// Token: 0x040002AB RID: 683
		private InputDevice device;

		// Token: 0x040002AC RID: 684
		private InputDevice activeDevice;
	}
}
