using System;

namespace InControl
{
	// Token: 0x0200004F RID: 79
	public class OuyaEverywhereDeviceManager : InputDeviceManager
	{
		// Token: 0x06000298 RID: 664 RVA: 0x00013954 File Offset: 0x00011D54
		public OuyaEverywhereDeviceManager()
		{
			for (int i = 0; i < 4; i++)
			{
				this.devices.Add(new OuyaEverywhereDevice(i));
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00013998 File Offset: 0x00011D98
		public override void Update(ulong updateTick, float deltaTime)
		{
			for (int i = 0; i < 4; i++)
			{
				OuyaEverywhereDevice ouyaEverywhereDevice = this.devices[i] as OuyaEverywhereDevice;
				if (ouyaEverywhereDevice.IsConnected != this.deviceConnected[i])
				{
					if (ouyaEverywhereDevice.IsConnected)
					{
						ouyaEverywhereDevice.BeforeAttach();
						InputManager.AttachDevice(ouyaEverywhereDevice);
					}
					else
					{
						InputManager.DetachDevice(ouyaEverywhereDevice);
					}
					this.deviceConnected[i] = ouyaEverywhereDevice.IsConnected;
				}
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00013A0C File Offset: 0x00011E0C
		public static void Enable()
		{
		}

		// Token: 0x040001E4 RID: 484
		private bool[] deviceConnected = new bool[4];
	}
}
