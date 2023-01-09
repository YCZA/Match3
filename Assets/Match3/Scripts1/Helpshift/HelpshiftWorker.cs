using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Match3.Scripts1.Helpshift
{
	// Token: 0x020001D0 RID: 464
	public class HelpshiftWorker
	{
		// Token: 0x06000D51 RID: 3409 RVA: 0x0001FD10 File Offset: 0x0001E110
		private HelpshiftWorker()
		{
			this.callerQueue = new Queue<APICallInfo>();
			this.workerThread = new Thread(delegate()
			{
				try
				{
					AndroidJNI.AttachCurrentThread();
					while (!this.shouldStop)
					{
						while (!this.shouldStop && this.callerQueue.Count > 0)
						{
							APICallInfo apicallInfo = this.callerQueue.Dequeue();
							try
							{
								this.resolveHsApiCall(apicallInfo);
							}
							catch (Exception ex)
							{
								global::UnityEngine.Debug.Log(string.Concat(new string[]
								{
									"Error in : ",
									apicallInfo.apiName,
									". Exception : ",
									ex.Message,
									ex.StackTrace
								}));
							}
						}
						if (!this.shouldStop)
						{
							this.waitHandle.WaitOne();
							this.waitHandle.Reset();
						}
					}
				}
				finally
				{
					AndroidJNI.DetachCurrentThread();
				}
			});
			this.workerThread.Name = "HSAsyncThread";
			this.workerThread.Start();
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0001FD77 File Offset: 0x0001E177
		public static HelpshiftWorker getInstance()
		{
			if (HelpshiftWorker.hsWorker == null)
			{
				HelpshiftWorker.hsWorker = new HelpshiftWorker();
			}
			return HelpshiftWorker.hsWorker;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0001FD92 File Offset: 0x0001E192
		public void registerClient(string identifier, IWorkerMethodDispatcher instance)
		{
			if (this.listeners.ContainsKey(identifier))
			{
				this.listeners[identifier] = instance;
			}
			else
			{
				this.listeners.Add(identifier, instance);
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0001FDC4 File Offset: 0x0001E1C4
		public void enqueueApiCall(string instanceIdentifier, string methodIdentifier, string api, object[] args)
		{
			this.callerQueue.Enqueue(new APICallInfo(instanceIdentifier, methodIdentifier, api, args));
			this.waitHandle.Set();
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0001FDE8 File Offset: 0x0001E1E8
		public void synchronousWaitForApiCallQueue()
		{
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);
			this.callerQueue.Enqueue(new APICallInfo(manualResetEvent));
			this.waitHandle.Set();
			manualResetEvent.WaitOne();
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0001FE20 File Offset: 0x0001E220
		public void resolveHsApiCall(APICallInfo apiInfo)
		{
			if (apiInfo.resetEvent != null)
			{
				apiInfo.resetEvent.Set();
				return;
			}
			this.listeners[apiInfo.instanceIdentifier].resolveAndCallApi(apiInfo.methodIdentifier, apiInfo.apiName, apiInfo.args);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0001FE6D File Offset: 0x0001E26D
		public void onApplicationQuit()
		{
			this.shouldStop = true;
			this.waitHandle.Set();
			this.workerThread.Join();
		}

		// Token: 0x04003F80 RID: 16256
		private static HelpshiftWorker hsWorker;

		// Token: 0x04003F81 RID: 16257
		private Queue<APICallInfo> callerQueue;

		// Token: 0x04003F82 RID: 16258
		private ManualResetEvent waitHandle = new ManualResetEvent(false);

		// Token: 0x04003F83 RID: 16259
		private Dictionary<string, IWorkerMethodDispatcher> listeners = new Dictionary<string, IWorkerMethodDispatcher>();

		// Token: 0x04003F84 RID: 16260
		private Thread workerThread;

		// Token: 0x04003F85 RID: 16261
		private bool shouldStop;
	}
}
