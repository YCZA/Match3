using System;
using System.Collections.Generic;

namespace Wooga.Coroutines
{
	// Token: 0x020003D1 RID: 977
	public class ObjectPool
	{
		// Token: 0x06001DA2 RID: 7586 RVA: 0x0007EFC0 File Offset: 0x0007D3C0
		public static ObjectPool Get()
		{
			return ObjectPool.s_instance;
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x0007EFC8 File Offset: 0x0007D3C8
		public void Release(IPoolable obj)
		{
			obj.Release();
			List<IPoolable> list;
			if (!this._instances.TryGetValue(obj.GetType(), out list))
			{
				list = new List<IPoolable>();
				this._instances[obj.GetType()] = list;
			}
			list.Add(obj);
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0007F014 File Offset: 0x0007D414
		public bool TryGet<T>(out T result)
		{
			object obj;
			if (this.TryGet(typeof(T), out obj))
			{
				result = (T)((object)obj);
				return true;
			}
			result = default(T);
			return false;
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x0007F058 File Offset: 0x0007D458
		public bool TryGet(Type type, out object result)
		{
			List<IPoolable> list;
			if (this._instances.TryGetValue(type, out list) && list.Count > 0)
			{
				result = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
				return true;
			}
			result = null;
			return false;
		}

		// Token: 0x040049DA RID: 18906
		private static ObjectPool s_instance = new ObjectPool();

		// Token: 0x040049DB RID: 18907
		private readonly Dictionary<Type, List<IPoolable>> _instances = new Dictionary<Type, List<IPoolable>>();
	}
}
