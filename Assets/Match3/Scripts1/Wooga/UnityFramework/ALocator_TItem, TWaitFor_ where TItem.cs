using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wooga.Coroutines;
using UnityEngine;

namespace Wooga.UnityFramework
{
	// Token: 0x02000B6C RID: 2924
	public class ALocator<TItem, TWaitFor> where TItem : IInitializable where TWaitFor : WaitFor
	{
		// Token: 0x06004457 RID: 17495 RVA: 0x0015ACFB File Offset: 0x001590FB
		public virtual T Register<T>(TItem service) where T : TItem
		{
			this.items[typeof(T)] = service;
			return (T)((object)service);
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x0015AD20 File Offset: 0x00159120
		public virtual void Unregister(IInitializable service)
		{
			Type type = service.GetType();
			if (this.Has(type, true))
			{
				TItem titem = this.items[type];
				titem.DeInit();
			}
			this.items.Remove(service.GetType());
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x0015AD70 File Offset: 0x00159170
		public virtual void UnregisterAll()
		{
			foreach (TItem titem in this.items.Values)
			{
				if (titem.OnInitialized.WasDispatched)
				{
					titem.DeInit();
				}
			}
			this.items.Clear();
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x0015ADFC File Offset: 0x001591FC
		protected T Get<T>(bool hasToBeInitialized = true) where T : IInitializable
		{
			return (T)((object)this.Get(typeof(T), hasToBeInitialized));
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x0015AE14 File Offset: 0x00159214
		protected IInitializable Get(Type type, bool hasToBeInitialized)
		{
			if (this.Has(type, hasToBeInitialized))
			{
				return this.items[type];
			}
			return null;
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x0015AE38 File Offset: 0x00159238
		public bool Has(Type type, bool hasToBeInitialized)
		{
			try
			{
				bool result;
				if (this.items.ContainsKey(type))
				{
					if (hasToBeInitialized)
					{
						TItem titem = this.items[type];
						result = titem.OnInitialized.WasDispatched;
					}
					else
					{
						result = true;
					}
				}
				else
				{
					result = false;
				}
				return result;
			}
			catch (Exception message)
			{
				global::UnityEngine.Debug.LogError(message);
			}
			return false;
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x0015AEA8 File Offset: 0x001592A8
		public virtual Coroutine Inject(object obj)
		{
			return WooroutineRunner.StartCoroutine(this.InjectRoutine(obj), null);
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x0015AEB7 File Offset: 0x001592B7
		public Wooroutine<T> Await<T>(bool hasToBeInitialized = true)
		{
			return WooroutineRunner.StartWooroutine<T>(this.AwaitServiceRoutine(typeof(T), hasToBeInitialized));
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x0015AED0 File Offset: 0x001592D0
		protected void InjectImmediateInternal(object injectInto)
		{
			FieldInfo[] fieldsForInjection = this.GetFieldsForInjection(injectInto);
			foreach (FieldInfo fieldInfo in fieldsForInjection)
			{
				WaitFor waitForAttribute = this.GetWaitForAttribute(fieldInfo, typeof(TWaitFor));
				if (!this.Has(fieldInfo.FieldType, waitForAttribute.hasToBeInitialized))
				{
					string error = string.Concat(new object[]
					{
						"Could not inject ",
						fieldInfo.FieldType,
						" into ",
						injectInto.GetType(),
						".",
						fieldInfo.Name
					});
					Debug.LogError(error);
					throw new InvalidOperationException(error);
				}
				fieldInfo.SetValue(injectInto, this.Get(fieldInfo.FieldType, waitForAttribute.hasToBeInitialized));
			}
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x0015AF8C File Offset: 0x0015938C
		protected IEnumerator InjectRoutine(object injectInto)
		{
			FieldInfo[] waitForFields = this.GetFieldsForInjection(injectInto);
			float injectTimer = 0f;
			int numMessages = 0;
			foreach (FieldInfo field in waitForFields)
			{
				WaitFor waitFor = this.GetWaitForAttribute(field, typeof(TWaitFor));
				while (!this.Has(field.FieldType, waitFor.hasToBeInitialized))
				{
					injectTimer += Time.deltaTime;
					if (injectTimer >= 2f)
					{
						numMessages++;
						injectTimer -= 2f;
						WoogaDebug.LogFormatted("{0} Waiting for: {1}", new object[]
						{
							injectInto.GetType(),
							field.Name
						});
					}
					yield return null;
				}
				field.SetValue(injectInto, this.Get(field.FieldType, waitFor.hasToBeInitialized));
			}
			yield break;
		}

		// Token: 0x06004461 RID: 17505 RVA: 0x0015AFB0 File Offset: 0x001593B0
		protected FieldInfo[] GetFieldsForInjection(object injectInto)
		{
			Type type = injectInto.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			return (from f in fields
			where typeof(IInitializable).IsAssignableFrom(f.FieldType) && Attribute.IsDefined(f, typeof(TWaitFor))
			select f).ToArray<FieldInfo>();
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x0015AFF8 File Offset: 0x001593F8
		private IEnumerator AwaitServiceRoutine(Type serviceType, bool hasToBeInitialized)
		{
			while (!this.Has(serviceType, false))
			{
				yield return null;
			}
			IInitializable service = this.Get(serviceType, false);
			if (hasToBeInitialized)
			{
				yield return service.OnInitialized;
			}
			yield return service;
			yield break;
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x0015B021 File Offset: 0x00159421
		private WaitFor GetWaitForAttribute(FieldInfo fieldInfo, Type waitForType)
		{
			return (WaitFor)fieldInfo.GetCustomAttributes(waitForType, true)[0];
		}

		// Token: 0x04006C75 RID: 27765
		protected readonly Dictionary<Type, TItem> items = new Dictionary<Type, TItem>();
	}
}
