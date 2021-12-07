// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

public class ExecuteEventsUtil
{
	private static readonly ExecuteEvents.EventFunction<IAltSubmitHandler> s_FaceButton3Handler;

	private static readonly ExecuteEvents.EventFunction<IAltSubmitHandler> s_AltSubmitHandler;

	private static readonly ExecuteEvents.EventFunction<IAltCancelHandler> s_AltCancelHandler;

	private static readonly ExecuteEvents.EventFunction<IAdvanceHandler> s_AdvanceHandler;

	private static readonly ExecuteEvents.EventFunction<IPreviousHandler> s_PreviousHandler;

	private static ExecuteEvents.EventFunction<IAltSubmitHandler> __f__mg_cache0;

	private static ExecuteEvents.EventFunction<IAltSubmitHandler> __f__mg_cache1;

	private static ExecuteEvents.EventFunction<IAltCancelHandler> __f__mg_cache2;

	private static ExecuteEvents.EventFunction<IAdvanceHandler> __f__mg_cache3;

	private static ExecuteEvents.EventFunction<IPreviousHandler> __f__mg_cache4;

	public static ExecuteEvents.EventFunction<IAltSubmitHandler> faceButton3Handler
	{
		get
		{
			return ExecuteEventsUtil.s_FaceButton3Handler;
		}
	}

	public static ExecuteEvents.EventFunction<IAltSubmitHandler> altSubmitHandler
	{
		get
		{
			return ExecuteEventsUtil.s_AltSubmitHandler;
		}
	}

	public static ExecuteEvents.EventFunction<IAltCancelHandler> altCancelHandler
	{
		get
		{
			return ExecuteEventsUtil.s_AltCancelHandler;
		}
	}

	public static ExecuteEvents.EventFunction<IAdvanceHandler> advanceHandler
	{
		get
		{
			return ExecuteEventsUtil.s_AdvanceHandler;
		}
	}

	public static ExecuteEvents.EventFunction<IPreviousHandler> previousHandler
	{
		get
		{
			return ExecuteEventsUtil.s_PreviousHandler;
		}
	}

	private static void Execute(IFaceButton3Handler handler, BaseEventData eventData)
	{
		handler.OnFaceButton3(eventData);
	}

	private static void Execute(IAltSubmitHandler handler, BaseEventData eventData)
	{
		handler.OnAltSubmit(eventData);
	}

	private static void Execute(IAltCancelHandler handler, BaseEventData eventData)
	{
		handler.OnAltCancel(eventData);
	}

	private static void Execute(IAdvanceHandler handler, BaseEventData eventData)
	{
		handler.OnAdvance(eventData);
	}

	private static void Execute(IPreviousHandler handler, BaseEventData eventData)
	{
		handler.OnPrevious(eventData);
	}

	static ExecuteEventsUtil()
	{
		// Note: this type is marked as 'beforefieldinit'.
		if (ExecuteEventsUtil.__f__mg_cache0 == null)
		{
			ExecuteEventsUtil.__f__mg_cache0 = new ExecuteEvents.EventFunction<IAltSubmitHandler>(ExecuteEventsUtil.Execute);
		}
		ExecuteEventsUtil.s_FaceButton3Handler = ExecuteEventsUtil.__f__mg_cache0;
		if (ExecuteEventsUtil.__f__mg_cache1 == null)
		{
			ExecuteEventsUtil.__f__mg_cache1 = new ExecuteEvents.EventFunction<IAltSubmitHandler>(ExecuteEventsUtil.Execute);
		}
		ExecuteEventsUtil.s_AltSubmitHandler = ExecuteEventsUtil.__f__mg_cache1;
		if (ExecuteEventsUtil.__f__mg_cache2 == null)
		{
			ExecuteEventsUtil.__f__mg_cache2 = new ExecuteEvents.EventFunction<IAltCancelHandler>(ExecuteEventsUtil.Execute);
		}
		ExecuteEventsUtil.s_AltCancelHandler = ExecuteEventsUtil.__f__mg_cache2;
		if (ExecuteEventsUtil.__f__mg_cache3 == null)
		{
			ExecuteEventsUtil.__f__mg_cache3 = new ExecuteEvents.EventFunction<IAdvanceHandler>(ExecuteEventsUtil.Execute);
		}
		ExecuteEventsUtil.s_AdvanceHandler = ExecuteEventsUtil.__f__mg_cache3;
		if (ExecuteEventsUtil.__f__mg_cache4 == null)
		{
			ExecuteEventsUtil.__f__mg_cache4 = new ExecuteEvents.EventFunction<IPreviousHandler>(ExecuteEventsUtil.Execute);
		}
		ExecuteEventsUtil.s_PreviousHandler = ExecuteEventsUtil.__f__mg_cache4;
	}
}
