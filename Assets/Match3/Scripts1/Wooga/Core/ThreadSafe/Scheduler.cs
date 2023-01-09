using System;
using System.Threading;

namespace Match3.Scripts1.Wooga.Core.ThreadSafe
{
	// Token: 0x020003A2 RID: 930
	public static class Scheduler
	{
		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06001C10 RID: 7184 RVA: 0x0007BA8C File Offset: 0x00079E8C
		// (remove) Token: 0x06001C11 RID: 7185 RVA: 0x0007BAC0 File Offset: 0x00079EC0
		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Scheduler.AsyncComputationExceptionEventHandler AsyncComputationExceptionEvent;

		// Token: 0x06001C12 RID: 7186 RVA: 0x0007BAF4 File Offset: 0x00079EF4
		public static void DispatchException(Exception e)
		{
			if (Scheduler.AsyncComputationExceptionEvent != null)
			{
				Scheduler.AsyncComputationExceptionEvent(e);
				return;
			}
			throw e;
		}

		private class ExecuteOnMainThread_Action<T>
		{
			internal Func<T> m;
		}
		// Token: 0x06001C13 RID: 7187 RVA: 0x0007BB14 File Offset: 0x00079F14
		public static T ExecuteOnMainThread<T>(this Func<T> m)
		{
			ExecuteOnMainThread_Action<T> c__AnonStorey = new ExecuteOnMainThread_Action<T>();
			c__AnonStorey.m = m;
			if (Unity3D.Threads.OnMainThread())
			{
				return c__AnonStorey.m();
			}
			ManualResetEvent handle = new ManualResetEvent(false);
			T result = default(T);
			MainThreadComputationQueue.Enqueue(delegate
			{
				result = c__AnonStorey.m();
				handle.Set();
			});
			handle.WaitOne();
			return result;
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x0007BB90 File Offset: 0x00079F90
		public static void ExecuteOnMainThread(this Action m)
		{
			if (Unity3D.Threads.OnMainThread())
			{
				m();
			}
			else
			{
				ManualResetEvent handle = new ManualResetEvent(false);
				MainThreadComputationQueue.Enqueue(delegate
				{
					m();
					handle.Set();
				});
				handle.WaitOne();
			}
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x0007BBFA File Offset: 0x00079FFA
		public static void StartOnMainThread(this Action m)
		{
			MainThreadComputationQueue.Enqueue(m);
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x0007BC04 File Offset: 0x0007A004
		public static void StartOnWorkerThread(this Action m)
		{
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				try
				{
					m();
				}
				catch (Exception e)
				{
					Scheduler.DispatchException(e);
				}
			});
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x0007BC30 File Offset: 0x0007A030
		public static void StartOnWorkerThread<T>(this Func<T> m)
		{
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				try
				{
					m();
				}
				catch (Exception e)
				{
					Scheduler.DispatchException(e);
				}
			});
		}

		// Token: 0x020003A3 RID: 931
		// (Invoke) Token: 0x06001C19 RID: 7193
		public delegate void AsyncComputationExceptionEventHandler(Exception e);
	}
}
