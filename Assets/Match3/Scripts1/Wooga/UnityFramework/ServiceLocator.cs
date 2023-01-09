using System;
using System.Collections.Generic;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B6B RID: 2923
	public class ServiceLocator : ALocator<IService, WaitForService>, IServiceLocator
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06004450 RID: 17488 RVA: 0x0015C30A File Offset: 0x0015A70A
		public static ServiceLocator Instance
		{
			get
			{
				ServiceLocator result;
				if ((result = ServiceLocator._instance) == null)
				{
					result = (ServiceLocator._instance = new ServiceLocator());
				}
				return result;
			}
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x0015C324 File Offset: 0x0015A724
		public virtual void OnSuspend()
		{
			foreach (IService service in this.items.Values)
			{
				if (service.OnInitialized.WasDispatched)
				{
					service.OnSuspend();
				}
			}
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x0015C394 File Offset: 0x0015A794
		public virtual void OnResume()
		{
			foreach (IService service in this.items.Values)
			{
				if (service.OnInitialized.WasDispatched)
				{
					service.OnResume();
				}
			}
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x0015C404 File Offset: 0x0015A804
		public IEnumerable<string> InitializedServices()
		{
			foreach (KeyValuePair<Type, IService> item in this.items)
			{
				if (item.Value.OnInitialized.WasDispatched)
				{
					yield return item.Value.GetType().Name;
				}
			}
			yield break;
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0015C428 File Offset: 0x0015A828
		public IEnumerable<string> NotInitalizedServices()
		{
			foreach (KeyValuePair<Type, IService> item in this.items)
			{
				if (!item.Value.OnInitialized.WasDispatched)
				{
					yield return item.Value.GetType().Name;
				}
			}
			yield break;
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0015C44B File Offset: 0x0015A84B
		public void InjectImmediate(object obj)
		{
			base.InjectImmediateInternal(obj);
		}

		// Token: 0x04006C74 RID: 27764
		private static ServiceLocator _instance;
	}
}
