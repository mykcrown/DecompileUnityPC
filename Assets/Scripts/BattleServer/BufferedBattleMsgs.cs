// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using network;
using P2P;
using System;
using System.Collections.Generic;

namespace BattleServer
{
	public class BufferedBattleMsgs : IBufferedBattleMsgs
	{
		private Dictionary<Type, INetMsg> staticMessageDictionary = new Dictionary<Type, INetMsg>();

		private Dictionary<Type, ServerEvent> staticEventDictionary = new Dictionary<Type, ServerEvent>();

		public void Init()
		{
			this.RegisterMsg<BatchMsg>(new BatchMsg(), 2000u);
			this.RegisterMsg<RelayMsg>(new RelayMsg(), 500u);
			this.RegisterMsg<InputMsg>(new InputMsg(), 500u);
			this.RegisterMsg<InputAckMsg>(new InputAckMsg(), 100u);
			this.RegisterMsg<RequestMissingInputMsg>(new RequestMissingInputMsg(), 50u);
			this.RegisterMsg<DisconnectMsg>(new DisconnectMsg(), 500u);
			this.RegisterMsg<DisconnectAckMsg>(new DisconnectAckMsg(), 100u);
			this.RegisterMsg<HashCodeMsg>(new HashCodeMsg(), 50u);
			this.RegisterMsg<P2PPingMsg>(new P2PPingMsg(), 16u);
			this.RegisterMsg<P2PPongMsg>(new P2PPongMsg(), 16u);
			this.RegisterServerEvent<InputEvent>(new InputEvent());
			this.RegisterServerEvent<InputAckEvent>(new InputAckEvent());
			this.RegisterServerEvent<RelayDataEvent>(new RelayDataEvent());
			this.RegisterServerEvent<HashCodeEvent>(new HashCodeEvent());
			this.RegisterServerEvent<RequestMissingInputEvent>(new RequestMissingInputEvent());
			this.RegisterServerEvent<DisconnectEvent>(new DisconnectEvent());
			this.RegisterServerEvent<DisconnectAckEvent>(new DisconnectAckEvent());
		}

		private void RegisterMsg<T>(T newMsg, uint bufferSize) where T : INetMsg, IBufferable
		{
			newMsg.SetAsBufferable(bufferSize);
			this.staticMessageDictionary[typeof(T)] = newMsg;
		}

		private void RegisterServerEvent<T>(T newEvent) where T : BatchEvent
		{
			this.staticEventDictionary[typeof(T)] = newEvent;
		}

		public T GetBufferedNetMessage<T>() where T : INetMsg
		{
			Type typeFromHandle = typeof(T);
			return (T)((object)this.staticMessageDictionary[typeFromHandle]);
		}

		public T GetBufferedServerEvent<T>() where T : BatchEvent
		{
			Type typeFromHandle = typeof(T);
			return this.staticEventDictionary[typeFromHandle] as T;
		}
	}
}
