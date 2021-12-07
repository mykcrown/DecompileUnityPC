using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using XInputDotNetPure;

namespace InControl
{
	// Token: 0x020001DE RID: 478
	public class XInputDeviceManager : InputDeviceManager
	{
		// Token: 0x06000898 RID: 2200 RVA: 0x0004C734 File Offset: 0x0004AB34
		public XInputDeviceManager()
		{
			if (InputManager.XInputUpdateRate == 0U)
			{
				this.timeStep = Mathf.FloorToInt(Time.fixedDeltaTime * 1000f);
			}
			else
			{
				this.timeStep = Mathf.FloorToInt(1f / InputManager.XInputUpdateRate * 1000f);
			}
			this.bufferSize = (int)Math.Max(InputManager.XInputBufferSize, 1U);
			for (int i = 0; i < 4; i++)
			{
				this.gamePadState[i] = new RingBuffer<GamePadState>(this.bufferSize);
			}
			this.StartWorker();
			for (int j = 0; j < 4; j++)
			{
				this.devices.Add(new XInputDevice(j, this));
			}
			this.Update(0UL, 0f);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0004C80F File Offset: 0x0004AC0F
		private void StartWorker()
		{
			if (this.thread == null)
			{
				this.thread = new Thread(new ThreadStart(this.Worker));
				this.thread.IsBackground = true;
				this.thread.Start();
			}
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0004C84A File Offset: 0x0004AC4A
		private void StopWorker()
		{
			if (this.thread != null)
			{
				this.thread.Abort();
				this.thread.Join();
				this.thread = null;
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0004C874 File Offset: 0x0004AC74
		private void Worker()
		{
			for (;;)
			{
				for (int i = 0; i < 4; i++)
				{
					this.gamePadState[i].Enqueue(GamePad.GetState((PlayerIndex)i));
				}
				Thread.Sleep(this.timeStep);
			}
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0004C8B5 File Offset: 0x0004ACB5
		internal GamePadState GetState(int deviceIndex)
		{
			return this.gamePadState[deviceIndex].Dequeue();
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0004C8C4 File Offset: 0x0004ACC4
		public override void Update(ulong updateTick, float deltaTime)
		{
			for (int i = 0; i < 4; i++)
			{
				XInputDevice xinputDevice = this.devices[i] as XInputDevice;
				if (!xinputDevice.IsConnected)
				{
					xinputDevice.GetState();
				}
				if (xinputDevice.IsConnected != this.deviceConnected[i])
				{
					if (xinputDevice.IsConnected)
					{
						InputManager.AttachDevice(xinputDevice);
					}
					else
					{
						InputManager.DetachDevice(xinputDevice);
					}
					this.deviceConnected[i] = xinputDevice.IsConnected;
				}
			}
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0004C943 File Offset: 0x0004AD43
		public override void Destroy()
		{
			this.StopWorker();
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0004C94C File Offset: 0x0004AD4C
		public static bool CheckPlatformSupport(ICollection<string> errors)
		{
			if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
			{
				return false;
			}
			try
			{
				GamePad.GetState(PlayerIndex.One);
			}
			catch (DllNotFoundException ex)
			{
				if (errors != null)
				{
					errors.Add(ex.Message + ".dll could not be found or is missing a dependency.");
				}
				return false;
			}
			return true;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0004C9B4 File Offset: 0x0004ADB4
		internal static void Enable()
		{
			List<string> list = new List<string>();
			if (XInputDeviceManager.CheckPlatformSupport(list))
			{
				InputManager.HideDevicesWithProfile(typeof(Xbox360WinProfile));
				InputManager.HideDevicesWithProfile(typeof(XboxOneWinProfile));
				InputManager.HideDevicesWithProfile(typeof(XboxOneWin10Profile));
				InputManager.HideDevicesWithProfile(typeof(XboxOneWin10AEProfile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF310ModeXWinProfile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF510ModeXWinProfile));
				InputManager.HideDevicesWithProfile(typeof(LogitechF710ModeXWinProfile));
				InputManager.AddDeviceManager<XInputDeviceManager>();
			}
			else
			{
				foreach (string text in list)
				{
					Logger.LogError(text);
				}
			}
		}

		// Token: 0x04000606 RID: 1542
		private bool[] deviceConnected = new bool[4];

		// Token: 0x04000607 RID: 1543
		private const int maxDevices = 4;

		// Token: 0x04000608 RID: 1544
		private RingBuffer<GamePadState>[] gamePadState = new RingBuffer<GamePadState>[4];

		// Token: 0x04000609 RID: 1545
		private Thread thread;

		// Token: 0x0400060A RID: 1546
		private int timeStep;

		// Token: 0x0400060B RID: 1547
		private int bufferSize;
	}
}
