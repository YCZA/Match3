using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

namespace Match3.Scripts1.UnityEngine.EventSystems
{
	// Token: 0x02000B0A RID: 2826
	public static class MultiTouchModuleEvents
	{
		// Token: 0x060042A5 RID: 17061 RVA: 0x00155C3A File Offset: 0x0015403A
		private static void Execute(IPinchHandler handler, BaseEventData eventData)
		{
			handler.OnPinch(ExecuteEvents.ValidateEventData<PinchEventData>(eventData));
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x00155C48 File Offset: 0x00154048
		private static void Execute(IRotateHandler handler, BaseEventData eventData)
		{
			handler.OnRotate(ExecuteEvents.ValidateEventData<RotateEventData>(eventData));
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x00155C56 File Offset: 0x00154056
		private static void Execute(IBeginPinchHandler handler, BaseEventData eventData)
		{
			handler.OnBeginPinch(ExecuteEvents.ValidateEventData<PinchEventData>(eventData));
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x060042A8 RID: 17064 RVA: 0x00155C64 File Offset: 0x00154064
		public static ExecuteEvents.EventFunction<IPinchHandler> pinchHandler
		{
			get
			{
				if (MultiTouchModuleEvents._003C_003Ef__mg_0024cache0 == null)
				{
					MultiTouchModuleEvents._003C_003Ef__mg_0024cache0 = new ExecuteEvents.EventFunction<IPinchHandler>(MultiTouchModuleEvents.Execute);
				}
				return MultiTouchModuleEvents._003C_003Ef__mg_0024cache0;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x060042A9 RID: 17065 RVA: 0x00155C83 File Offset: 0x00154083
		public static ExecuteEvents.EventFunction<IRotateHandler> rotateHandler
		{
			get
			{
				if (MultiTouchModuleEvents._003C_003Ef__mg_0024cache1 == null)
				{
					MultiTouchModuleEvents._003C_003Ef__mg_0024cache1 = new ExecuteEvents.EventFunction<IRotateHandler>(MultiTouchModuleEvents.Execute);
				}
				return MultiTouchModuleEvents._003C_003Ef__mg_0024cache1;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x060042AA RID: 17066 RVA: 0x00155CA2 File Offset: 0x001540A2
		public static ExecuteEvents.EventFunction<IBeginPinchHandler> beginPinchHandler
		{
			get
			{
				if (MultiTouchModuleEvents._003C_003Ef__mg_0024cache2 == null)
				{
					MultiTouchModuleEvents._003C_003Ef__mg_0024cache2 = new ExecuteEvents.EventFunction<IBeginPinchHandler>(MultiTouchModuleEvents.Execute);
				}
				return MultiTouchModuleEvents._003C_003Ef__mg_0024cache2;
			}
		}

		// Token: 0x04006B87 RID: 27527
		[CompilerGenerated]
		private static ExecuteEvents.EventFunction<IPinchHandler> _003C_003Ef__mg_0024cache0;

		// Token: 0x04006B88 RID: 27528
		[CompilerGenerated]
		private static ExecuteEvents.EventFunction<IRotateHandler> _003C_003Ef__mg_0024cache1;

		// Token: 0x04006B89 RID: 27529
		[CompilerGenerated]
		private static ExecuteEvents.EventFunction<IBeginPinchHandler> _003C_003Ef__mg_0024cache2;
	}
}
