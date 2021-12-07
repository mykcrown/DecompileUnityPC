using System;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

// Token: 0x0200092D RID: 2349
public class ExecuteEventsUtil
{
	// Token: 0x17000EA3 RID: 3747
	// (get) Token: 0x06003D3D RID: 15677 RVA: 0x0011AE00 File Offset: 0x00119200
	public static ExecuteEvents.EventFunction<IAltSubmitHandler> faceButton3Handler
	{
		get
		{
			return ExecuteEventsUtil.s_FaceButton3Handler;
		}
	}

	// Token: 0x06003D3E RID: 15678 RVA: 0x0011AE07 File Offset: 0x00119207
	private static void Execute(IFaceButton3Handler handler, BaseEventData eventData)
	{
		handler.OnFaceButton3(eventData);
	}

	// Token: 0x17000EA4 RID: 3748
	// (get) Token: 0x06003D3F RID: 15679 RVA: 0x0011AE10 File Offset: 0x00119210
	public static ExecuteEvents.EventFunction<IAltSubmitHandler> altSubmitHandler
	{
		get
		{
			return ExecuteEventsUtil.s_AltSubmitHandler;
		}
	}

	// Token: 0x06003D40 RID: 15680 RVA: 0x0011AE17 File Offset: 0x00119217
	private static void Execute(IAltSubmitHandler handler, BaseEventData eventData)
	{
		handler.OnAltSubmit(eventData);
	}

	// Token: 0x17000EA5 RID: 3749
	// (get) Token: 0x06003D41 RID: 15681 RVA: 0x0011AE20 File Offset: 0x00119220
	public static ExecuteEvents.EventFunction<IAltCancelHandler> altCancelHandler
	{
		get
		{
			return ExecuteEventsUtil.s_AltCancelHandler;
		}
	}

	// Token: 0x06003D42 RID: 15682 RVA: 0x0011AE27 File Offset: 0x00119227
	private static void Execute(IAltCancelHandler handler, BaseEventData eventData)
	{
		handler.OnAltCancel(eventData);
	}

	// Token: 0x17000EA6 RID: 3750
	// (get) Token: 0x06003D43 RID: 15683 RVA: 0x0011AE30 File Offset: 0x00119230
	public static ExecuteEvents.EventFunction<IAdvanceHandler> advanceHandler
	{
		get
		{
			return ExecuteEventsUtil.s_AdvanceHandler;
		}
	}

	// Token: 0x06003D44 RID: 15684 RVA: 0x0011AE37 File Offset: 0x00119237
	private static void Execute(IAdvanceHandler handler, BaseEventData eventData)
	{
		handler.OnAdvance(eventData);
	}

	// Token: 0x17000EA7 RID: 3751
	// (get) Token: 0x06003D45 RID: 15685 RVA: 0x0011AE40 File Offset: 0x00119240
	public static ExecuteEvents.EventFunction<IPreviousHandler> previousHandler
	{
		get
		{
			return ExecuteEventsUtil.s_PreviousHandler;
		}
	}

	// Token: 0x06003D46 RID: 15686 RVA: 0x0011AE47 File Offset: 0x00119247
	private static void Execute(IPreviousHandler handler, BaseEventData eventData)
	{
		handler.OnPrevious(eventData);
	}

	// Token: 0x06003D47 RID: 15687 RVA: 0x0011AE50 File Offset: 0x00119250
	// Note: this type is marked as 'beforefieldinit'.
	static ExecuteEventsUtil()
	{
		if (ExecuteEventsUtil.f__mg_cache0 == null)
		{
			ExecuteEventsUtil.f__mg_cache0 = new ExecuteEvents.EventFunction<IAltSubmitHandler>(ExecuteEventsUtil.Execute);
		}
		ExecuteEventsUtil.s_FaceButton3Handler = ExecuteEventsUtil.f__mg_cache0;
		if (ExecuteEventsUtil.f__mg_cache1 == null)
		{
			ExecuteEventsUtil.f__mg_cache1 = new ExecuteEvents.EventFunction<IAltSubmitHandler>(ExecuteEventsUtil.Execute);
		}
		ExecuteEventsUtil.s_AltSubmitHandler = ExecuteEventsUtil.f__mg_cache1;
		if (ExecuteEventsUtil.f__mg_cache2 == null)
		{
			ExecuteEventsUtil.f__mg_cache2 = new ExecuteEvents.EventFunction<IAltCancelHandler>(ExecuteEventsUtil.Execute);
		}
		ExecuteEventsUtil.s_AltCancelHandler = ExecuteEventsUtil.f__mg_cache2;
		if (ExecuteEventsUtil.f__mg_cache3 == null)
		{
			ExecuteEventsUtil.f__mg_cache3 = new ExecuteEvents.EventFunction<IAdvanceHandler>(ExecuteEventsUtil.Execute);
		}
		ExecuteEventsUtil.s_AdvanceHandler = ExecuteEventsUtil.f__mg_cache3;
		if (ExecuteEventsUtil.f__mg_cache4 == null)
		{
			ExecuteEventsUtil.f__mg_cache4 = new ExecuteEvents.EventFunction<IPreviousHandler>(ExecuteEventsUtil.Execute);
		}
		ExecuteEventsUtil.s_PreviousHandler = ExecuteEventsUtil.f__mg_cache4;
	}

	// Token: 0x040029D0 RID: 10704
	private static readonly ExecuteEvents.EventFunction<IAltSubmitHandler> s_FaceButton3Handler;

	// Token: 0x040029D1 RID: 10705
	private static readonly ExecuteEvents.EventFunction<IAltSubmitHandler> s_AltSubmitHandler;

	// Token: 0x040029D2 RID: 10706
	private static readonly ExecuteEvents.EventFunction<IAltCancelHandler> s_AltCancelHandler;

	// Token: 0x040029D3 RID: 10707
	private static readonly ExecuteEvents.EventFunction<IAdvanceHandler> s_AdvanceHandler;

	// Token: 0x040029D4 RID: 10708
	private static readonly ExecuteEvents.EventFunction<IPreviousHandler> s_PreviousHandler;

	// Token: 0x040029D5 RID: 10709
	[CompilerGenerated]
	private static ExecuteEvents.EventFunction<IAltSubmitHandler> f__mg_cache0;

	// Token: 0x040029D6 RID: 10710
	[CompilerGenerated]
	private static ExecuteEvents.EventFunction<IAltSubmitHandler> f__mg_cache1;

	// Token: 0x040029D7 RID: 10711
	[CompilerGenerated]
	private static ExecuteEvents.EventFunction<IAltCancelHandler> f__mg_cache2;

	// Token: 0x040029D8 RID: 10712
	[CompilerGenerated]
	private static ExecuteEvents.EventFunction<IAdvanceHandler> f__mg_cache3;

	// Token: 0x040029D9 RID: 10713
	[CompilerGenerated]
	private static ExecuteEvents.EventFunction<IPreviousHandler> f__mg_cache4;
}
