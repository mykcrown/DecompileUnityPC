using System;
using System.Collections.Generic;
using IconsServer;
using network;
using P2P;

namespace BattleServer
{
	// Token: 0x0200079A RID: 1946
	public class BufferedBattleMsgs : IBufferedBattleMsgs
	{
		// Token: 0x06002FF0 RID: 12272 RVA: 0x000EEF74 File Offset: 0x000ED374
		public void Init()
		{
			this.RegisterMsg<BatchMsg>(new BatchMsg(), 2000U);
			this.RegisterMsg<RelayMsg>(new RelayMsg(), 500U);
			this.RegisterMsg<InputMsg>(new InputMsg(), 500U);
			this.RegisterMsg<InputAckMsg>(new InputAckMsg(), 100U);
			this.RegisterMsg<RequestMissingInputMsg>(new RequestMissingInputMsg(), 50U);
			this.RegisterMsg<DisconnectMsg>(new DisconnectMsg(), 500U);
			this.RegisterMsg<DisconnectAckMsg>(new DisconnectAckMsg(), 100U);
			this.RegisterMsg<HashCodeMsg>(new HashCodeMsg(), 50U);
			this.RegisterMsg<P2PPingMsg>(new P2PPingMsg(), 16U);
			this.RegisterMsg<P2PPongMsg>(new P2PPongMsg(), 16U);
			this.RegisterServerEvent<InputEvent>(new InputEvent());
			this.RegisterServerEvent<InputAckEvent>(new InputAckEvent());
			this.RegisterServerEvent<RelayDataEvent>(new RelayDataEvent());
			this.RegisterServerEvent<HashCodeEvent>(new HashCodeEvent());
			this.RegisterServerEvent<RequestMissingInputEvent>(new RequestMissingInputEvent());
			this.RegisterServerEvent<DisconnectEvent>(new DisconnectEvent());
			this.RegisterServerEvent<DisconnectAckEvent>(new DisconnectAckEvent());
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x000EF05C File Offset: 0x000ED45C
		private void RegisterMsg<T>(T newMsg, uint bufferSize) where T : INetMsg, IBufferable
		{
			newMsg.SetAsBufferable(bufferSize);
			this.staticMessageDictionary[typeof(T)] = newMsg;
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000EF087 File Offset: 0x000ED487
		private void RegisterServerEvent<T>(T newEvent) where T : BatchEvent
		{
			this.staticEventDictionary[typeof(T)] = newEvent;
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000EF0A4 File Offset: 0x000ED4A4
		public T GetBufferedNetMessage<T>() where T : INetMsg
		{
			Type typeFromHandle = typeof(T);
			return (T)((object)this.staticMessageDictionary[typeFromHandle]);
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000EF0D0 File Offset: 0x000ED4D0
		public T GetBufferedServerEvent<T>() where T : BatchEvent
		{
			Type typeFromHandle = typeof(T);
			return this.staticEventDictionary[typeFromHandle] as T;
		}

		// Token: 0x0400218D RID: 8589
		private Dictionary<Type, INetMsg> staticMessageDictionary = new Dictionary<Type, INetMsg>();

		// Token: 0x0400218E RID: 8590
		private Dictionary<Type, ServerEvent> staticEventDictionary = new Dictionary<Type, ServerEvent>();
	}
}
