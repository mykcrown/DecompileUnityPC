using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000061 RID: 97
	public abstract class PlayerActionSet
	{
		// Token: 0x0600034A RID: 842 RVA: 0x0000FB4C File Offset: 0x0000DF4C
		protected PlayerActionSet()
		{
			this.Enabled = true;
			this.PreventInputWhileListeningForBinding = true;
			this.Device = null;
			this.IncludeDevices = new List<InputDevice>();
			this.ExcludeDevices = new List<InputDevice>();
			this.listenOptions.IncludeMouseButtons = true;
			this.Actions = new ReadOnlyCollection<PlayerAction>(this.actions);
			InputManager.AttachPlayerActionSet(this);
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000FBE4 File Offset: 0x0000DFE4
		// (set) Token: 0x0600034C RID: 844 RVA: 0x0000FBEC File Offset: 0x0000DFEC
		public InputDevice Device { get; set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000FBF5 File Offset: 0x0000DFF5
		// (set) Token: 0x0600034E RID: 846 RVA: 0x0000FBFD File Offset: 0x0000DFFD
		public List<InputDevice> IncludeDevices { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000FC06 File Offset: 0x0000E006
		// (set) Token: 0x06000350 RID: 848 RVA: 0x0000FC0E File Offset: 0x0000E00E
		public List<InputDevice> ExcludeDevices { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000FC17 File Offset: 0x0000E017
		// (set) Token: 0x06000352 RID: 850 RVA: 0x0000FC1F File Offset: 0x0000E01F
		public ReadOnlyCollection<PlayerAction> Actions { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000FC28 File Offset: 0x0000E028
		// (set) Token: 0x06000354 RID: 852 RVA: 0x0000FC30 File Offset: 0x0000E030
		public ulong UpdateTick { get; protected set; }

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000355 RID: 853 RVA: 0x0000FC3C File Offset: 0x0000E03C
		// (remove) Token: 0x06000356 RID: 854 RVA: 0x0000FC74 File Offset: 0x0000E074
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000FCAA File Offset: 0x0000E0AA
		// (set) Token: 0x06000358 RID: 856 RVA: 0x0000FCB2 File Offset: 0x0000E0B2
		public bool Enabled { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000FCBB File Offset: 0x0000E0BB
		// (set) Token: 0x0600035A RID: 858 RVA: 0x0000FCC3 File Offset: 0x0000E0C3
		public bool PreventInputWhileListeningForBinding { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000FCCC File Offset: 0x0000E0CC
		// (set) Token: 0x0600035C RID: 860 RVA: 0x0000FCD4 File Offset: 0x0000E0D4
		public object UserData { get; set; }

		// Token: 0x0600035D RID: 861 RVA: 0x0000FCDD File Offset: 0x0000E0DD
		public void Destroy()
		{
			this.OnLastInputTypeChanged = null;
			InputManager.DetachPlayerActionSet(this);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000FCEC File Offset: 0x0000E0EC
		protected PlayerAction CreatePlayerAction(string name)
		{
			return new PlayerAction(name, this);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000FCF8 File Offset: 0x0000E0F8
		internal void AddPlayerAction(PlayerAction action)
		{
			action.Device = this.FindActiveDevice();
			if (this.actionsByName.ContainsKey(action.Name))
			{
				throw new InControlException("Action '" + action.Name + "' already exists in this set.");
			}
			this.actions.Add(action);
			this.actionsByName.Add(action.Name, action);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000FD60 File Offset: 0x0000E160
		protected PlayerOneAxisAction CreateOneAxisPlayerAction(PlayerAction negativeAction, PlayerAction positiveAction)
		{
			PlayerOneAxisAction playerOneAxisAction = new PlayerOneAxisAction(negativeAction, positiveAction);
			this.oneAxisActions.Add(playerOneAxisAction);
			return playerOneAxisAction;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000FD84 File Offset: 0x0000E184
		protected PlayerTwoAxisAction CreateTwoAxisPlayerAction(PlayerAction negativeXAction, PlayerAction positiveXAction, PlayerAction negativeYAction, PlayerAction positiveYAction)
		{
			PlayerTwoAxisAction playerTwoAxisAction = new PlayerTwoAxisAction(negativeXAction, positiveXAction, negativeYAction, positiveYAction);
			this.twoAxisActions.Add(playerTwoAxisAction);
			return playerTwoAxisAction;
		}

		// Token: 0x1700005D RID: 93
		public PlayerAction this[string actionName]
		{
			get
			{
				PlayerAction result;
				if (this.actionsByName.TryGetValue(actionName, out result))
				{
					return result;
				}
				throw new KeyNotFoundException("Action '" + actionName + "' does not exist in this action set.");
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000FDE4 File Offset: 0x0000E1E4
		public PlayerAction GetPlayerActionByName(string actionName)
		{
			PlayerAction result;
			if (this.actionsByName.TryGetValue(actionName, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000FE08 File Offset: 0x0000E208
		internal void Update(ulong updateTick, float deltaTime)
		{
			InputDevice device = this.Device ?? this.FindActiveDevice();
			BindingSourceType lastInputType = this.LastInputType;
			ulong lastInputTypeChangedTick = this.LastInputTypeChangedTick;
			InputDeviceClass lastDeviceClass = this.LastDeviceClass;
			InputDeviceStyle lastDeviceStyle = this.LastDeviceStyle;
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				PlayerAction playerAction = this.actions[i];
				playerAction.Update(updateTick, deltaTime, device);
				if (playerAction.UpdateTick > this.UpdateTick)
				{
					this.UpdateTick = playerAction.UpdateTick;
					this.activeDevice = playerAction.ActiveDevice;
				}
				if (playerAction.LastInputTypeChangedTick > lastInputTypeChangedTick)
				{
					lastInputType = playerAction.LastInputType;
					lastInputTypeChangedTick = playerAction.LastInputTypeChangedTick;
					lastDeviceClass = playerAction.LastDeviceClass;
					lastDeviceStyle = playerAction.LastDeviceStyle;
				}
			}
			int count2 = this.oneAxisActions.Count;
			for (int j = 0; j < count2; j++)
			{
				this.oneAxisActions[j].Update(updateTick, deltaTime);
			}
			int count3 = this.twoAxisActions.Count;
			for (int k = 0; k < count3; k++)
			{
				this.twoAxisActions[k].Update(updateTick, deltaTime);
			}
			if (lastInputTypeChangedTick > this.LastInputTypeChangedTick)
			{
				bool flag = lastInputType != this.LastInputType;
				this.LastInputType = lastInputType;
				this.LastInputTypeChangedTick = lastInputTypeChangedTick;
				this.LastDeviceClass = lastDeviceClass;
				this.LastDeviceStyle = lastDeviceStyle;
				if (this.OnLastInputTypeChanged != null && flag)
				{
					this.OnLastInputTypeChanged(lastInputType);
				}
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000FFA4 File Offset: 0x0000E3A4
		public void Reset()
		{
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].ResetBindings();
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000FFE0 File Offset: 0x0000E3E0
		private InputDevice FindActiveDevice()
		{
			bool flag = this.IncludeDevices.Count > 0;
			bool flag2 = this.ExcludeDevices.Count > 0;
			if (flag || flag2)
			{
				InputDevice inputDevice = InputDevice.Null;
				int count = InputManager.Devices.Count;
				for (int i = 0; i < count; i++)
				{
					InputDevice inputDevice2 = InputManager.Devices[i];
					if (inputDevice2 != inputDevice && inputDevice2.LastChangedAfter(inputDevice))
					{
						if (!flag2 || !this.ExcludeDevices.Contains(inputDevice2))
						{
							if (!flag || this.IncludeDevices.Contains(inputDevice2))
							{
								inputDevice = inputDevice2;
							}
						}
					}
				}
				return inputDevice;
			}
			return InputManager.ActiveDevice;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x000100A0 File Offset: 0x0000E4A0
		public void ClearInputState()
		{
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].ClearInputState();
			}
			int count2 = this.oneAxisActions.Count;
			for (int j = 0; j < count2; j++)
			{
				this.oneAxisActions[j].ClearInputState();
			}
			int count3 = this.twoAxisActions.Count;
			for (int k = 0; k < count3; k++)
			{
				this.twoAxisActions[k].ClearInputState();
			}
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00010144 File Offset: 0x0000E544
		public bool HasBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return false;
			}
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.actions[i].HasBinding(binding))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00010198 File Offset: 0x0000E598
		public void RemoveBinding(BindingSource binding)
		{
			if (binding == null)
			{
				return;
			}
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				this.actions[i].RemoveBinding(binding);
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600036A RID: 874 RVA: 0x000101E2 File Offset: 0x0000E5E2
		public bool IsListeningForBinding
		{
			get
			{
				return this.listenWithAction != null;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600036B RID: 875 RVA: 0x000101F0 File Offset: 0x0000E5F0
		// (set) Token: 0x0600036C RID: 876 RVA: 0x000101F8 File Offset: 0x0000E5F8
		public BindingListenOptions ListenOptions
		{
			get
			{
				return this.listenOptions;
			}
			set
			{
				this.listenOptions = (value ?? new BindingListenOptions());
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600036D RID: 877 RVA: 0x0001020D File Offset: 0x0000E60D
		public InputDevice ActiveDevice
		{
			get
			{
				return (this.activeDevice != null) ? this.activeDevice : InputDevice.Null;
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0001022C File Offset: 0x0000E62C
		public string Save()
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8))
				{
					binaryWriter.Write(66);
					binaryWriter.Write(73);
					binaryWriter.Write(78);
					binaryWriter.Write(68);
					binaryWriter.Write(2);
					int count = this.actions.Count;
					binaryWriter.Write(count);
					for (int i = 0; i < count; i++)
					{
						this.actions[i].Save(binaryWriter);
					}
				}
				result = Convert.ToBase64String(memoryStream.ToArray());
			}
			return result;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000102F8 File Offset: 0x0000E6F8
		public void Load(string data)
		{
			if (data == null)
			{
				return;
			}
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data)))
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						if (binaryReader.ReadUInt32() != 1145981250U)
						{
							throw new Exception("Unknown data format.");
						}
						ushort num = binaryReader.ReadUInt16();
						if (num < 1 || num > 2)
						{
							throw new Exception("Unknown data format version: " + num);
						}
						int num2 = binaryReader.ReadInt32();
						for (int i = 0; i < num2; i++)
						{
							PlayerAction playerAction;
							if (this.actionsByName.TryGetValue(binaryReader.ReadString(), out playerAction))
							{
								playerAction.Load(binaryReader, num);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Provided state could not be loaded:\n" + ex.Message);
				this.Reset();
			}
		}

		// Token: 0x040002B2 RID: 690
		public BindingSourceType LastInputType;

		// Token: 0x040002B4 RID: 692
		public ulong LastInputTypeChangedTick;

		// Token: 0x040002B5 RID: 693
		public InputDeviceClass LastDeviceClass;

		// Token: 0x040002B6 RID: 694
		public InputDeviceStyle LastDeviceStyle;

		// Token: 0x040002BA RID: 698
		private List<PlayerAction> actions = new List<PlayerAction>();

		// Token: 0x040002BB RID: 699
		private List<PlayerOneAxisAction> oneAxisActions = new List<PlayerOneAxisAction>();

		// Token: 0x040002BC RID: 700
		private List<PlayerTwoAxisAction> twoAxisActions = new List<PlayerTwoAxisAction>();

		// Token: 0x040002BD RID: 701
		private Dictionary<string, PlayerAction> actionsByName = new Dictionary<string, PlayerAction>();

		// Token: 0x040002BE RID: 702
		private BindingListenOptions listenOptions = new BindingListenOptions();

		// Token: 0x040002BF RID: 703
		internal PlayerAction listenWithAction;

		// Token: 0x040002C0 RID: 704
		private InputDevice activeDevice;

		// Token: 0x040002C1 RID: 705
		private const ushort currentDataFormatVersion = 2;
	}
}
